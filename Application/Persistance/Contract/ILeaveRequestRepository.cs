using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Persistance.Contract
{
    public interface ILeaveRequestRepository : IGenericRepository<LeaveRequest>
    {
    }
}