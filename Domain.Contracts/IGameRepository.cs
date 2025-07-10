using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tournament.Shared.Parameters;

namespace Domain.Contracts
{
    public interface IGameRepository
    {
        //Task<IEnumerable<Game>> GetFilteredAsync(DateTime? startDate, DateTime? endDate, string? title, string? sortBy);
        Task<IEnumerable<Game>> GetFilteredAsync(GameFilterParameters parameters);
        Task<IEnumerable<Game>> GetByTitleAsync(string title);
        Task<IEnumerable<Game>> GetAllAsync();
        Task<Game?> GetAsync(int id);
        //Task<bool> AnyAsync(int id);
        Task<bool> AnyAsync(Expression<Func<Game, bool>> predicate);
        Task<int> CountAsync(Expression<Func<Game, bool>> predicate);
        void Add(Game game);
        void Update(Game game);
        void Remove(Game game);
    }
}
