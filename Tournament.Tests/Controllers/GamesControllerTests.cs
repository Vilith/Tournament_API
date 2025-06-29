using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Data.Controllers;
using Tournament.Tests.TestHelpers;

namespace Tournament.Tests.Controllers
{
    public class GamesControllerTests : ControllerTestBase<GamesController>
    {
        public GamesControllerTests() : base((uow, mapper) => new GamesController(uow, mapper))
        {
        }
        // Additional tests for GamesController can be added here
        // For example, testing GetGame, GetGamesByTitle, etc.
    }
}
