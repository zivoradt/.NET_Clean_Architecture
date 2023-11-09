using Application.Contracts.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ILeaveTypeRepository _leaveTypeRepository;

        private ILeaveRequestRepository _leaveRequestRepository;

        private ILeaveAllocationRepository _leaveAllocationRepository;

        private readonly LeaveManagementDbContext _context;

        public UnitOfWork(LeaveManagementDbContext context)
        {
            _context = context;
        }

        public ILeaveAllocationRepository LeaveAllocationRepository =>
            _leaveAllocationRepository ??= new LeaveAllocationRepository(_context);

        public ILeaveRequestRepository LeaveRequestRepository => _leaveRequestRepository ??= new LeaveRequestRepository(_context);

        public ILeaveTypeRepository LeaveTypeRepository => _leaveTypeRepository ??= new LeaveTypeRepository(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}