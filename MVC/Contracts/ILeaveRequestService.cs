﻿using MVC.Models;
using MVC.Services.Base;

namespace MVC.Contracts
{
    public interface ILeaveRequestService
    {
        Task<AdminLeaveRequestViewVM> GetAdminLeaveRequestList();

        Task<EmployeeLeaveRequestViewVM> GetUserLeaveRequest();

        Task<Response<int>> CreateLeaveRequest(CreateLeaveRequestVM createLeaveRequestVM);

        Task DeleteLeaveRequest(int id);

        Task<LeaveRequestVM> GetLeaveRequest(int id);

        Task ApproveLeaveRequest(int id, bool approved);
    }
}