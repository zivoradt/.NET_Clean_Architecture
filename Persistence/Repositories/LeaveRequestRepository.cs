using Application.Contracts.Persistance;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
    {
        private readonly LeaveManagementDbContext _dbContext;

        public LeaveRequestRepository(LeaveManagementDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ChangeRequestApprovalStatus(LeaveRequest leaveRequest, bool? Approved)
        {
            leaveRequest.Approved = Approved;
            _dbContext.Entry(leaveRequest).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<LeaveRequest> GetLeaveRequestWithDetails(int id)
        {
            var leaveRequest = await _dbContext.LeaveRequests.Include(x => x.LeaveType).FirstOrDefaultAsync(q => q.Id == id);

            return leaveRequest;
        }

        public async Task<List<LeaveRequest>> GetLeaveRequestWithDetails()
        {
            var leaveRequests = await _dbContext.LeaveRequests.Include(e => e.LeaveType).ToListAsync();

            return leaveRequests;
        }
    }
}