using Dressify.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dressify.Application.IRepos
{
    public interface IReservationRepository
    {
        Task<Reservation?> GetByIdAsync(int id);
        Task<IEnumerable<Reservation>> GetAllAsync();
        Task AddAsync(Reservation res);
        Task UpdateAsync(Reservation res);
        Task<IEnumerable<Reservation>> GetByDressAndDateAsync(int dressId, DateOnly date);
    
    }
}
