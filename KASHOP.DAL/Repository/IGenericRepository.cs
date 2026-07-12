using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace KASHOP.DAL.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task <List<T>> GetAllAsync(string[]? includes = null);
        Task <T> CreateAsync(T entity);
        Task <T> GetOne(Expression<Func<T,bool>> filter, string[]? includes = null);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}
