using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IUnitOfWork
    {
        public ITournamentRepository TournamentRepository { get; }
        public IGameRepository GameRepository { get; }

        Task CompleteAsync();
    }
}
