using Application.DTOs.LeaveAllocation.Validators;
using Application.Exceptions;
using Application.Features.LeaveAllocations.Request.Commands;
using Application.Contracts.Persistance;
using Application.Responses;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.LeaveAllocations.Handlers.Commands
{
    public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, BaseCommandResponse>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IMapper _mapper;

        public CreateLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository, IMapper mapper)
        {
            _leaveAllocationRepository = leaveAllocationRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveAllocationValidator(_leaveAllocationRepository);

            var validatorResult = await validator.ValidateAsync(request.LeaveAllocationDto);

            if (validatorResult.IsValid == false)
            {
                List<string> errors = validatorResult.Errors.Select(x => x.ErrorMessage).ToList();

                return BaseCommandResponse.Failed(errors, "Allocation Failed");
            }
            else
            {
                var leaveAllocation = _mapper.Map<Domain.LeaveAllocation>(request.LeaveAllocationDto);

                leaveAllocation = await _leaveAllocationRepository.Add(leaveAllocation);

                return BaseCommandResponse.Successful(leaveAllocation.Id);
            }
        }
    }
}