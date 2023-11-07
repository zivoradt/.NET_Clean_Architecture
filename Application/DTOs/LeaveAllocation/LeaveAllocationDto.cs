using Application.DTOs.Common;
using Application.DTOs.LeaveType;
using Application.Models.Identity;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.LeaveAllocation
{
    public class LeaveAllocationDto : BaseDto
    {
        public int NumberOfDays { get; set; }

        public LeaveTypeDto LeaveType { get; set; }

        public Employee Employee { get; set; }

        public string EmployeeId { get; set; }

        public int LeaveTypeId { get; set; }

        public int Period { get; set; }
    }
}