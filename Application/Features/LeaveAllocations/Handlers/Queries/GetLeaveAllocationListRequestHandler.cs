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
using Microsoft.AspNetCore.Http;
using Application.Contracts.Identity;
using Application.Constants;

namespace Application.Features.LeaveAllocations.Handlers.Queries
{
    public class GetLeaveAllocationListRequestHandler : IRequestHandler<GetLeaveAllocationListRequest, List<LeaveAllocationDto>>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public GetLeaveAllocationListRequestHandler(ILeaveAllocationRepository leaveAllocationRepository,
            IMapper mapper, IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _leaveAllocationRepository = leaveAllocationRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        async Task<List<LeaveAllocationDto>> IRequestHandler<GetLeaveAllocationListRequest, List<LeaveAllocationDto>>.Handle(GetLeaveAllocationListRequest request, CancellationToken cancellationToken)
        {
            var leaveAllocations = new List<LeaveAllocation>();

            var allocations = new List<LeaveAllocationDto>();

            if (request.IsLoggedInUser)
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(q => q.Type == CustomClaimTypes.Uid)?.Value;

                leaveAllocations = await _leaveAllocationRepository.GetLeaveAllocationsWithDetails(userId);

                var employee = await _userService.GetEmployee(userId);

                allocations = _mapper.Map<List<LeaveAllocationDto>>(leaveAllocations);

                foreach (var allocation in allocations)
                {
                    allocation.Employee = employee;
                }
            }
            else
            {
                leaveAllocations = await _leaveAllocationRepository.GetLeaveAllocationWithDetails();

                allocations = _mapper.Map<List<LeaveAllocationDto>>(leaveAllocations);

                foreach (var allocation in allocations)
                {
                    allocation.Employee = await _userService.GetEmployee(allocation.EmployeeId);
                }
            }

            return allocations;
        }
    }
}