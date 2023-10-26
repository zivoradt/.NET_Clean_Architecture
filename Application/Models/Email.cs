using Application.DTOs.LeaveRequest;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Email
    {
        public string To { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public static Email LeaveRequestCreated(CreateLeaveRequestDto leaveRequest)
        {
            return new Email()
            {
                To = "employee@org.com",
                Body = $"Youre leave request for {leaveRequest.StartDate} to {leaveRequest.EndDate} has been submitted successfully",
                Subject = "Leave Request Submitted"
            };
        }
    }
}