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

namespace Application.Features.LeaveRequest.Handlers.Commands
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, BaseCommandResponse>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public CreateLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository, IMapper mapper, IEmailSender emailSender)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _mapper = mapper;
            _emailSender = emailSender;
        }

        public async Task<BaseCommandResponse> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveRequestDtoValidator(_leaveRequestRepository);

            var validationResult = await validator.ValidateAsync(request.LeaveRequestDto);

            if (validationResult.IsValid == false)
            {
                List<string> errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();

                return BaseCommandResponse.Failed(errors);
            }

            var leaveRequest = _mapper.Map<Domain.LeaveRequest>(request.LeaveRequestDto);

            leaveRequest = await _leaveRequestRepository.Add(leaveRequest);

            var email = Email.LeaveRequestCreated(request.LeaveRequestDto);

            try
            {
                await _emailSender.SendEmail(email);
            }
            catch (Exception ex)
            {
            }

            return BaseCommandResponse.Successful(leaveRequest.Id);
        }
    }
}