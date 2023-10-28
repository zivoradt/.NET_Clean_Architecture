﻿using MVC.Models;
using MVC.Services.Base;

namespace MVC.Contracts
{
    public interface ILeaveTypeService
    {
        Task<List<LeaveTypeVM>> GetAllTypes();

        Task<LeaveTypeVM> GetLeaveTypeDetails(int id);

        Task<Response<int>> CreateLeaveType(CreateLeaveTypeVM leaveType);

        Task<Response<int>> UpdateLeaveType(int id, LeaveTypeVM leaveType);

        Task<Response<int>> DeleteLeaveType(int id);
    }
}