using Application.DTOs.LeaveAllocation;
using Application.Features.LeaveAllocations.Request.Queries;
using Application.Contracts.Persistance;
using AutoMapper;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.LeaveAllocations.Handlers.Queries
{
    public class GetLeaveAllocationListRequestHandler : IRequestHandler<GetLeaveAllocationListRequest, List<LeaveAllocationDto>>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IMapper _mapper;

        public GetLeaveAllocationListRequestHandler(ILeaveAllocationRepository leaveAllocationRepository, IMapper mapper)
        {
            _leaveAllocationRepository = leaveAllocationRepository;
            _mapper = mapper;
        }

        async Task<List<LeaveAllocationDto>> IRequestHandler<GetLeaveAllocationListRequest, List<LeaveAllocationDto>>.Handle(GetLeaveAllocationListRequest request, CancellationToken cancellationToken)
        {
            var listLeaveAllocation = await _leaveAllocationRepository.GetLeaveAllocationWithDetails();

            return _mapper.Map<List<LeaveAllocationDto>>(listLeaveAllocation);
        }
    }
}