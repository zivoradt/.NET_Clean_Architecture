using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.LeaveType
{
    public interface ILeaveTypeDto
    {
        string Name { get; set; }

        int DefaultDays { get; set; }
    }
}