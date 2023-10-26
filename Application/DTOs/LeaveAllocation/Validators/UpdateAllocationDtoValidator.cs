using Application.Contracts.Persistance;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.LeaveAllocation.Validators
{
    public class UpdateAllocationDtoValidator : AbstractValidator<UpdateLeaveAllocationDto>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;

        public UpdateAllocationDtoValidator(ILeaveAllocationRepository leaveAllocationRepository)
        {
            _leaveAllocationRepository = leaveAllocationRepository;
            Include(new ILeaveAllocationDtoValidator(_leaveAllocationRepository));

            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} must be present");
        }
    }
}