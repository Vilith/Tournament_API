using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tournament.Core.Repositories;
using Tournament.Data.Controllers;

namespace Tournament.Tests.TestHelpers
{
    public abstract class ControllerTestBase<TController> where TController : ControllerBase
    {
        protected readonly Mock<IUnitOfWork> MockUnitOfWork = new();
        protected readonly Mock<IMapper> MockMapper = new();
        protected readonly TController Controller;
        protected ControllerTestBase(Func<IUnitOfWork, IMapper, TController> controllerFactory)
        {
            Controller = controllerFactory(MockUnitOfWork.Object, MockMapper.Object);

            Controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            Controller.ObjectValidator = new FakeObjectModelValidator();
        }
    }
}
