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
using Application.Contracts.Identity;
using Domain;

namespace Application.Features.LeaveAllocations.Handlers.Commands
{
    public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, BaseCommandResponse>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public CreateLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository, IMapper mapper, IUserService userService, ILeaveTypeRepository leaveTypeRepository)
        {
            _leaveAllocationRepository = leaveAllocationRepository;
            _mapper = mapper;
            _userService = userService;
            _leaveTypeRepository = leaveTypeRepository;
        }

        public async Task<BaseCommandResponse> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveAllocationValidator(_leaveTypeRepository);

            var validatorResult = await validator.ValidateAsync(request.LeaveAllocationDto);

            foreach (var error in validatorResult.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }

            if (validatorResult.IsValid == false)
            {
                Console.Write(validatorResult.Errors.ToString());
                List<string> errors = validatorResult.Errors.Select(x => x.ErrorMessage).ToList();

                return BaseCommandResponse.Failed(errors, "Allocation Failed");
            }
            else
            {
                var leaveType = await _leaveTypeRepository.Get(request.LeaveAllocationDto.LeaveTypeId);

                var employees = await _userService.GetEmployees();

                var period = DateTime.Now.Year;

                var allocations = new List<LeaveAllocation>();

                foreach (var employee in employees)
                {
                    if (await _leaveAllocationRepository.AllocationExists(employee.Id, leaveType.Id, period))
                        continue;
                    allocations.Add(new LeaveAllocation
                    {
                        EmployeeId = employee.Id,
                        LeaveTypeId = leaveType.Id,
                        NumberOfDays = leaveType.DefaultDays,
                        Period = period,
                    });
                }

                await _leaveAllocationRepository.AddAllocations(allocations);

                return BaseCommandResponse.Successful(1);
            }
        }
    }
}