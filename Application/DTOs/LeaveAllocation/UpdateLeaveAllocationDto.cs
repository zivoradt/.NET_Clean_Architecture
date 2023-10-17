﻿using Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.LeaveAllocation
{
    internal class UpdateLeaveAllocationDto : BaseDto
    {
        public int NumberOfDays { get; set; }

        public int LeaveTypeId { get; set; }
        public int Period { get; set; }
    }
}