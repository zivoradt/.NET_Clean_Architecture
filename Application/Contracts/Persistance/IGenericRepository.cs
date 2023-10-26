using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Persistance
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Get(int Id);

        Task<IReadOnlyList<T>> GetAll();

        Task<T> Add(T entity);

        Task<bool> Exist(int id);

        Task Delete(T entity);

        Task Update(T entity);
    }
}