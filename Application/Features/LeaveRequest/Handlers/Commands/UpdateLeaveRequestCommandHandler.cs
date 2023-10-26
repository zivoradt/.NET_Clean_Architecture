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
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IMapper _mapper;

        public UpdateLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository, IMapper mapper)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateLeaveRequestDtoValidator(_leaveRequestRepository);

            var validationResult = await validator.ValidateAsync(request.UpdateLeaveRequestDto);

            if (validationResult.IsValid == false)
            {
                throw new ValidationException(validationResult);
            }
            var leaveRequest = await _leaveRequestRepository.Get(request.Id);

            if (request.UpdateLeaveRequestDto != null)
            {
                _mapper.Map(request.UpdateLeaveRequestDto, leaveRequest);

                await _leaveRequestRepository.Update(leaveRequest);
            }
            else if (request.ChangeLeaveRequestApprovalDto != null)
            {
                await _leaveRequestRepository.ChangeRequestApprovalStatus(leaveRequest, request.ChangeLeaveRequestApprovalDto.Approved);
            }

            return Unit.Value;
        }
    }
}