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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public CreateLeaveAllocationCommandHandler(IUnitOfWork unitOfWork,
            IMapper mapper, IUserService userService
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<BaseCommandResponse> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveAllocationValidator(_unitOfWork.LeaveTypeRepository);

            var validatorResult = await validator.ValidateAsync(request.LeaveAllocationDto);

            if (validatorResult.IsValid == false)
            {
                Console.Write(validatorResult.Errors.ToString());
                List<string> errors = validatorResult.Errors.Select(x => x.ErrorMessage).ToList();

                return BaseCommandResponse.Failed(errors, "Allocation Failed");
            }
            else
            {
                var leaveType = await _unitOfWork.LeaveTypeRepository.Get(request.LeaveAllocationDto.LeaveTypeId);

                var employees = await _userService.GetEmployees();

                var period = DateTime.Now.Year;

                var allocations = new List<LeaveAllocation>();

                foreach (var employee in employees)
                {
                    if (await _unitOfWork.LeaveAllocationRepository.AllocationExists(employee.Id, leaveType.Id, period))

                        continue;
                    allocations.Add(new LeaveAllocation
                    {
                        EmployeeId = employee.Id,
                        LeaveTypeId = leaveType.Id,
                        NumberOfDays = leaveType.DefaultDays,
                        Period = period,
                    });
                }

                await _unitOfWork.LeaveAllocationRepository.AddAllocations(allocations);

                await _unitOfWork.Save();

                return BaseCommandResponse.Successful(1);
            }
        }
    }
}