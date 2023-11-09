using Application.DTOs.LeaveRequest;
using Application.Features.LeaveRequest.Request.Commands;
using Application.Contracts.Persistance;
using AutoMapper;
using MediatR;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.LeaveAllocation.Validators;
using Application.DTOs.LeaveType.Validators;
using Application.Exceptions;
using Application.Responses;
using Application.Contracts.Infrastructure;
using Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.Reflection;
using Application.Constants;

namespace Application.Features.LeaveRequest.Handlers.Commands
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, BaseCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IUnitOfWork _unitOfWork;

        public CreateLeaveRequestCommandHandler(
            IMapper mapper, IEmailSender emailSender,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;

            _unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveRequestDtoValidator(_unitOfWork.LeaveTypeRepository);

            var validationResult = await validator.ValidateAsync(request.LeaveRequestDto);

            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(q => q.Type == CustomClaimTypes.Uid)?.Value;

            var allocation = await _unitOfWork.LeaveAllocationRepository.GetUserLeaveAllocations(userId, request.LeaveRequestDto.LeaveTypeId);

            if (allocation is null)
            {
                validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(nameof(request.LeaveRequestDto.LeaveTypeId),
                    "You dont have any allocations for this leave type"));
            }
            else
            {
                int daysRequested = (int)(request.LeaveRequestDto.EndDate - request.LeaveRequestDto.StartDate).TotalDays;

                if (daysRequested > allocation.NumberOfDays)
                {
                    validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(nameof(request.LeaveRequestDto.EndDate), "You dont have enoguh days" +
                        "for this reques"));
                }
            }

            if (validationResult.IsValid == false)
            {
                List<string> errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();

                return BaseCommandResponse.Failed(errors);
            }
            else
            {
                var leaveRequest = _mapper.Map<Domain.LeaveRequest>(request.LeaveRequestDto);

                leaveRequest.RequestingEmployeeId = userId;

                leaveRequest = await _unitOfWork.LeaveRequestRepository.Add(leaveRequest);

                await _unitOfWork.Save();

                try
                {
                    var emailAddress = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

                    var email = Email.LeaveRequestCreated(request.LeaveRequestDto, emailAddress);

                    await _emailSender.SendEmail(email);
                }
                catch (Exception ex)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
                return BaseCommandResponse.Successful(leaveRequest.Id);
            }
        }
    }
}