using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Persistance
{
    public interface IUnitOfWork : IDisposable
    {
        ILeaveAllocationRepository LeaveAllocationRepository { get; }

        ILeaveRequestRepository LeaveRequestRepository { get; }

        ILeaveTypeRepository LeaveTypeRepository { get; }

        Task Save();
    }
}