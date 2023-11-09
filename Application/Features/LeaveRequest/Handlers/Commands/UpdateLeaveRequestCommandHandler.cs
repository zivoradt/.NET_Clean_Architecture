using Application.DTOs.LeaveRequest.Validators;
using Application.DTOs.LeaveType.Validators;
using Application.Exceptions;
using Application.Features.LeaveRequest.Request.Commands;
using Application.Contracts.Persistance;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.LeaveRequest.Handlers.Commands
{
    public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Unit>
    {
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;

        public UpdateLeaveRequestCommandHandler(IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;

            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _unitOfWork.LeaveRequestRepository.Get(request.Id);

            if (leaveRequest is null)
                throw new NotFoundException(nameof(leaveRequest), request.Id);

            if (request.UpdateLeaveRequestDto != null)
            {
                var validator = new UpdateLeaveRequestDtoValidator(_unitOfWork.LeaveTypeRepository);

                var validationResult = await validator.ValidateAsync(request.UpdateLeaveRequestDto);

                if (validationResult.IsValid == false)
                {
                    throw new ValidationException(validationResult);
                }
                _mapper.Map(request.UpdateLeaveRequestDto, leaveRequest);

                await _unitOfWork.LeaveRequestRepository.Update(leaveRequest);

                _unitOfWork.Save();
            }
            else if (request.ChangeLeaveRequestApprovalDto != null)
            {
                await _unitOfWork.LeaveRequestRepository.ChangeRequestApprovalStatus(leaveRequest, request.ChangeLeaveRequestApprovalDto.Approved);

                if (request.ChangeLeaveRequestApprovalDto.Approved)
                {
                    var allocation = await _unitOfWork.LeaveAllocationRepository.GetUserLeaveAllocations(leaveRequest.RequestingEmployeeId, leaveRequest.LeaveTypeId);

                    int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;

                    if (daysRequested > allocation.NumberOfDays)
                    {
                        throw new Exception("You dont have that much days!");
                    }

                    allocation.NumberOfDays -= daysRequested;

                    await _unitOfWork.LeaveAllocationRepository.Update(allocation);
                }

                await _unitOfWork.Save();
            }
            return Unit.Value;
        }
    }
}