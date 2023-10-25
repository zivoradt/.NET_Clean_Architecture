using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Persistance.Contract
{
    public interface ILeaveTypeRepository : IGenericRepository<LeaveType>
    {
        Task<LeaveType> Update(LeaveType enitity);
    }
}