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
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IMapper _mapper;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveAllocationRepository _leaveAllocaitionRepository;

        public UpdateLeaveRequestCommandHandler(ILeaveTypeRepository leaveTypeRepository, IMapper mapper
            , ILeaveRequestRepository leaveRequestRepository,
            ILeaveAllocationRepository leaveAllocaitionRepository)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _mapper = mapper;
            _leaveRequestRepository = leaveRequestRepository;
            _leaveAllocaitionRepository = leaveAllocaitionRepository;
        }

        public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _leaveRequestRepository.Get(request.Id);

            if (leaveRequest != null)
            {
                var validator = new UpdateLeaveRequestDtoValidator(_leaveTypeRepository);

                var validationResult = await validator.ValidateAsync(request.UpdateLeaveRequestDto);

                if (validationResult.IsValid == false)
                {
                    throw new ValidationException(validationResult);
                }
                _mapper.Map(request.UpdateLeaveRequestDto, leaveRequest);

                await _leaveRequestRepository.Update(leaveRequest);
            }
            else if (request.ChangeLeaveRequestApprovalDto != null)
            {
                await _leaveRequestRepository.ChangeRequestApprovalStatus(leaveRequest, request.ChangeLeaveRequestApprovalDto.Approved);

                if (request.ChangeLeaveRequestApprovalDto.Approved)
                {
                    var allocation = await _leaveAllocaitionRepository.GetUserLeaveAllocations(leaveRequest.RequestingEmployeeId, leaveRequest.LeaveTypeId);

                    int daysRequested = (int)(request.UpdateLeaveRequestDto.EndDate - request.UpdateLeaveRequestDto.StartDate).TotalDays;

                    allocation.NumberOfDays = daysRequested;

                    await _leaveAllocaitionRepository.Update(allocation);
                }
            }
            return Unit.Value;
        }
    }
}