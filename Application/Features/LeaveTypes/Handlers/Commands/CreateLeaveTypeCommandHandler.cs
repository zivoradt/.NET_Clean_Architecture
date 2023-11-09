using Application.DTOs.LeaveType.Validators;
using Application.Exceptions;
using Application.Features.LeaveTypes.Request.Commands;
using Application.Contracts.Persistance;
using Application.Responses;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.LeaveTypes.Handlers.Commands
{
    public class CreateLeaveTypeCommandHandler : IRequestHandler<CreateLeaveTypeCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateLeaveTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveTypeDtoValidator();
            var validationResut = await validator.ValidateAsync(request.LeaveTypeDto);

            if (validationResut.IsValid == false)
            {
                return BaseCommandResponse.Failed(validationResut.Errors.Select(x => x.ErrorMessage).ToList());
            }
            else
            {
                var leaveType = _mapper.Map<LeaveType>(request.LeaveTypeDto);

                leaveType = await _unitOfWork.LeaveTypeRepository.Add(leaveType);

                await _unitOfWork.Save();

                return BaseCommandResponse.Successful(leaveType.Id);
            }
        }
    }
}