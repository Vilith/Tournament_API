using AutoMapper;
using Domain.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Contracts;

namespace Tournament.Tests.TestHelpers
{    
    public abstract class ControllerTestBase<TController> where TController : ControllerBase
    {
        //protected readonly Mock<IUnitOfWork> MockUnitOfWork = new();
        //protected readonly Mock<IMapper> MockMapper = new();
        protected readonly Mock<IServiceManager> MockService = new();
        protected readonly TController Controller;
        protected ControllerTestBase(Func<IServiceManager, TController> controllerFactory)
        {
            Controller = controllerFactory(MockService.Object);

            Controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            Controller.ObjectValidator = new FakeObjectModelValidator();
        }
    }
}