using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Core.Parameters;

namespace Tournament.Core.Repositories
{
    public interface IGameRepository
    {
        //Task<IEnumerable<Game>> GetFilteredAsync(DateTime? startDate, DateTime? endDate, string? title, string? sortBy);
        Task<IEnumerable<Game>> GetFilteredAsync(GameFilterParameters parameters);
        Task<IEnumerable<Game>> GetByTitleAsync(string title);
        Task<IEnumerable<Game>> GetAllAsync();
        Task<Game?> GetAsync(int id);
        Task<bool> AnyAsync(int id);
        void Add(Game game);
        void Update(Game game);
        void Remove(Game game);
    }
}
