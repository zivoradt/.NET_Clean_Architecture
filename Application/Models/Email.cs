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

        public static Email LeaveRequestCreated(CreateLeaveRequestDto leaveRequest, string emailAdress)
        {
            return new Email()
            {
                To = emailAdress,
                Body = $"Youre leave request for {leaveRequest.StartDate} to {leaveRequest.EndDate} has been submitted successfully",
                Subject = "Leave Request Submitted"
            };
        }
    }
}