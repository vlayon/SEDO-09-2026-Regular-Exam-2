using Homies.Controllers;
using Homies.Data;
using Homies.Data.Models;
using Homies.Models.Event;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Type = Homies.Data.Models.Type;

namespace Homies.Tests
{
    [TestFixture]
    public class EventControllerUnitTests
    {
        private HomiesDbContext _dbContext;
        private EventController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<HomiesDbContext>()
                .UseInMemoryDatabase(databaseName: "HomiesDbTest")
                .Options;

            _dbContext = new HomiesDbContext(options);

            _dbContext.Types.AddRange(new List<Type>
            {
                new Type { Id = 1, Name = "Animals" },
                new Type { Id = 2, Name = "Fun" }
            });
            // second comment
            _dbContext.SaveChanges();

            var mockUser = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "testUserId")
            }));
            //comment
            _controller = new EventController(_dbContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = mockUser }
                }
            };
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task Add_Get_ShouldReturnViewWithEventFormModel()
        {
            var result = await _controller.Add();

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as EventFormModel;
            Assert.IsNotNull(model);
            Assert.IsAssignableFrom<EventFormModel>(model);
        }

        [Test]
        public async Task Add_Post_ValidModel_ShouldRedirectToAll()
        {
            var eventModel = new EventFormModel
            {
                Name = "Test Event",
                Description = "Test Description",
                TypeId = 1,
                Start = System.DateTime.Now.AddDays(1),
                End = System.DateTime.Now.AddDays(2)
            };

            var result = await _controller.Add(eventModel);

            var redirectToActionResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectToActionResult);
            Assert.AreEqual("All", redirectToActionResult.ActionName);
        }

        [Test]
        public async Task Add_Post_InvalidModel_ShouldReturnViewWithModel()
        {
            _controller.ModelState.AddModelError("TypeId", "Invalid Type");

            var eventModel = new EventFormModel
            {
                Name = "Test Event",
                Description = "Test Description",
                TypeId = 999,
                Start = System.DateTime.Now.AddDays(1),
                End = System.DateTime.Now.AddDays(2)
            };

            var result = await _controller.Add(eventModel);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(eventModel, viewResult.Model);
        }

        [Test]
        public async Task Edit_Post_ValidModel_ShouldRedirectToAll()
        {
            var eventEntity = new Event
            {
                Id = 1,
                Name = "Original Event",
                Description = "Original Description",
                TypeId = 1,
                Start = System.DateTime.Now.AddDays(1),
                End = System.DateTime.Now.AddDays(2),
                OrganiserId = "testUserId"
            };

            _dbContext.Events.Add(eventEntity);
            await _dbContext.SaveChangesAsync();

            var eventModel = new EventFormModel
            {
                Name = "Updated Event",
                Description = "Updated Description",
                TypeId = 1,
                Start = System.DateTime.Now.AddDays(2),
                End = System.DateTime.Now.AddDays(3)
            };

            var result = await _controller.Edit(1, eventModel);

            var redirectToActionResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectToActionResult);
            Assert.AreEqual("All", redirectToActionResult.ActionName);
        }
    }
}
