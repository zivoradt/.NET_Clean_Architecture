﻿using Application.Contracts.Persistance;
using Application.DTOs.LeaveAllocation;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class LeaveAllocationRepository : GenericRepository<LeaveAllocation>, ILeaveAllocationRepository
    {
        private readonly LeaveManagementDbContext _dbContext;

        public LeaveAllocationRepository(LeaveManagementDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAllocations(List<LeaveAllocation> allocation)
        {
            await _dbContext.AddRangeAsync(allocation);
        }

        public async Task<bool> AllocationExists(string userId, int leaveTypeId, int period)
        {
            return await _dbContext.LeaveAllocation.AnyAsync(q => q.EmployeeId == userId && q.LeaveTypeId == leaveTypeId && q.Period == period);
        }

        public Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails(string id)
        {
            var leaveAllocations = _dbContext.LeaveAllocation.Where(q => q.EmployeeId == id)
                .Include(q => q.LeaveType).ToListAsync();

            return leaveAllocations;
        }

        public async Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id)
        {
            var leaveAllocations = await _dbContext.LeaveAllocation.Include(x => x.LeaveType).FirstOrDefaultAsync(q => q.Id == id);

            return leaveAllocations;
        }

        public async Task<List<LeaveAllocation>> GetLeaveAllocationWithDetails()
        {
            var leaveAllocations = await _dbContext.LeaveAllocation.Include(x => x.LeaveType).ToListAsync();

            return leaveAllocations;
        }

        public async Task<LeaveAllocation> GetUserLeaveAllocations(string userId, int leaveTypeId)
        {
            return await _dbContext.LeaveAllocation.FirstOrDefaultAsync(q => q.EmployeeId == userId && q.LeaveTypeId == leaveTypeId);
        }

        public async Task<List<LeaveAllocation>> GetAllLeaveAllocationsForUser(string userId, int leaveTypeId)
        {
            return await _dbContext.LeaveAllocation.Where(x => x.EmployeeId == userId && x.LeaveTypeId == leaveTypeId).ToListAsync();
        }
    }
}