using Application.Exceptions;
using Application.Features.LeaveTypes.Request.Commands;
using Application.Contracts.Persistance;
using AutoMapper;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.LeaveTypes.Handlers.Commands
{
    public class DeleteLeaveTypeCommandHandler : IRequestHandler<DeleteLeaveTypeCommand, Unit>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IMapper _mapper;

        public DeleteLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository, IMapper mapper)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var leaveTypes = await _leaveTypeRepository.Get(request.Id);

            if (leaveTypes == null)
            {
                throw new NotFoundException(nameof(LeaveType), request.Id);
            }

            await _leaveTypeRepository.Delete(leaveTypes);

            return Unit.Value;
        }
    }
}