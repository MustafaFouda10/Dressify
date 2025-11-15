using Dressify.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dressify.Application.IRepos
{
    public interface IDressRepository
    {
        Task<Dress?> GetByIdAsync(int id);
        Task<IEnumerable<Dress>> GetAllAsync();
        Task AddAsync(Dress dress);
        Task UpdateAsync(Dress dress);
        Task DeleteAsync(int id);
    }
}
