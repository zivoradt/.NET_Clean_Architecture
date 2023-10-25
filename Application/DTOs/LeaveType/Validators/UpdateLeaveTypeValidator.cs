using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.LeaveType.Validators
{
    internal class UpdateLeaveTypeValidator : AbstractValidator<LeaveTypeDto>
    {
        public UpdateLeaveTypeValidator()
        {
            Include(new ILeaveTypeDtoValidator());

            RuleFor(p => p.Id)
                .NotEmpty()
                .NotNull().WithMessage("{PropertyName} is required.");
        }
    }
}