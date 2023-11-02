using MVC.Services.Base;

namespace MVC.Contracts
{
    public interface ILeaveAllocationService
    {
        Task<Response<int>> CreateLeaveAllocation(int leaveTypeId);
    }
}