using Application.DTOs.LeaveAllocation;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Persistance
{
    public interface ILeaveAllocationRepository : IGenericRepository<LeaveAllocation>
    {
        Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id);

        Task<List<LeaveAllocation>> GetLeaveAllocationWithDetails();

        Task<bool> AllocationExists(string userId, int leaveTypeId, int period);

        Task AddAllocations(List<LeaveAllocation> allocation);

        Task<LeaveAllocation> GetUserLeaveAllocations(string userId, int leaveTypeId);

        Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails(string id);

        Task<List<LeaveAllocation>> GetAllLeaveAllocationsForUser(string userId, int leaveTypeId);
    }
}