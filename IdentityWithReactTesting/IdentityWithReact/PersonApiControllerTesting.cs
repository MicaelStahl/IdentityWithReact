using BusinessLibrary.Interfaces;
using Castle.Core.Logging;
using DataAccessLibrary.Models;
using DataAccessLibrary.ViewModels;
using IdentityWithReact.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IdentityWithReactTesting.IdentityWithReact
{
    /// <summary>
    /// Class used for References for PersonApiControllerTesting.
    /// <list type="bullet"><see cref="IdGeneration"/> Method generating Id's.</list>
    /// <list type="bullet"><see cref="OneValidPerson"/> Returns one valid <see cref="Person"/>.</list>
    /// <list type="bullet"><see cref="OneInvalidPerson"/> Method returns one invalid <see cref="Person"/>.</list>
    /// <list type="bullet"><see cref="TwoValidPeople"/> Method returns <see cref="List{T}"/> where T is <see cref="Person"/>.</list>
    /// </summary>
    internal class TestReferences
    {
        #region References

        /// <summary>
        /// Used for generating Ids for this test page.
        /// </summary>
        /// <returns></returns>
        public static async Task<Guid> IdGeneration() => await Task.FromResult(Guid.NewGuid());

        /// <summary>
        /// Returns one valid <see cref="Person"/>.
        /// </summary>
        /// <returns></returns>
        public static async Task<Person> OneValidPerson()
        {
            return await Task.FromResult(
                new Person
                {
                    Id = await IdGeneration(),
                    FirstName = "Test",
                    LastName = "Testsson",
                    Age = 21,
                    Email = "Test.Testsson@context.com",
                    PhoneNumber = "123456789",
                    City = "TEST",
                    PostalCode = "12345"
                });
        }

        /// <summary>
        /// Returns one invalid <see cref="Person"/>.
        /// </summary>
        /// <returns></returns>
        public static async Task<Person> OneInvalidPerson()
        {
            return await Task.FromResult(
                new Person
                {
                    Id = await IdGeneration(),
                    FirstName = "",
                    LastName = "",
                    Age = 3,
                    Email = ".@context.com",
                    City = "",
                    PostalCode = ""
                });
        }

        /// <summary>
        /// Returns a <see cref="List{T}"/> of 2 people.
        /// <para>T is <see cref="Person"/></para>
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Person>> TwoValidPeople()
        {
            return await Task.FromResult(new List<Person>
            {
                new Person
                {
                    Id = await IdGeneration(),
                    FirstName = "Test",
                    LastName = "Testsson1",
                    Age = 21,
                    Email = "Test.Testsson1@context.com",
                    PhoneNumber = "123456789",
                    City = "TEST",
                    PostalCode = "12345"
                },
                new Person
                {
                    Id = await IdGeneration(),
                    FirstName = "Test",
                    LastName = "Testsson2",
                    Age = 21,
                    Email = "Test.Testsson2@context.com",
                    PhoneNumber = "123456789",
                    City = "TEST",
                    PostalCode = "12345"
                }
            });
        }

        #endregion References
    }

    /// <summary>
    /// Continue on this later.
    /// </summary>
    public class PersonApiControllerTesting
    {
        #region D.I

        private readonly Mock<IPersonRepository> _service;
        private readonly PersonApiController _controller;

        public PersonApiControllerTesting()
        {
            _service = new Mock<IPersonRepository>();
            _controller = new PersonApiController(_service.Object);
        }

        #endregion D.I

        #region Create

        [Fact]
        [Trait("Category", "Person-Create")]
        public async Task Create_SubmitValidData_ReturnsOkStatusWithCreatedPersonAsync()
        {
            var person = await TestReferences.OneValidPerson();
            _service.Setup(x => x.Create(person)).ReturnsAsync(new PersonWithMessage { Person = person, Message = ActionMessages.Created });

            var result = await _controller.CreateAsync(person);

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<Person>(viewResult.Value);
            Assert.Equal(person, model);
        }

        [Fact]
        [Trait("Category", "Person-Create")]
        public async Task Create_InvalidModelState_ReturnsBadRequestObjectResultAsync()
        {
            var person = await TestReferences.OneInvalidPerson();
            _controller.ModelState.AddModelError(string.Empty, "Unexpected error: Please check all fields and try again.");

            var result = await _controller.CreateAsync(person);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        [Trait("Category", "Person-Create")]
        public async Task Create_SubmitInvalidDataWithValidModelState_ReturnsBadRequestObjectResultAsync()
        {
            var person = await TestReferences.OneInvalidPerson();
            _service.Setup(x => x.Create(person)).Returns(Task.FromResult(new PersonWithMessage { Message = StatusMessages.InvalidFields }));

            var result = await _controller.CreateAsync(person);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion Create

        #region Find

        [Fact]
        [Trait("Category", "Person-Find")]
        public async Task Find_SubmitValidId_ReturnsOkStatusWithCorrectPersonAsync()
        {
            var person = await TestReferences.OneValidPerson();
            _service.Setup(x => x.Find(person.Id)).ReturnsAsync(new PersonWithMessage { Person = person, Message = ActionMessages.Found });

            var result = await _controller.GetAsync(person.Id);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<Person>(objectResult.Value);
            Assert.Equal(person, model);
        }

        [Fact]
        [Trait("Category", "Person-Find")]
        public async Task Find_SubmitInvalidId_ReturnsBadRequestObjectResultAsync()
        {
            var fakeId = await TestReferences.IdGeneration();

            var result = await _controller.GetAsync(fakeId);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        [Trait("Category", "Person-Find")]
        public async Task Find_SubmitValidId_ReturnsNotFoundObjectResultAsync()
        {
            var fakeId = Guid.NewGuid();
            _service.Setup(x => x.Find(fakeId)).Returns(Task.FromResult(new PersonWithMessage { Message = StatusMessages.NotFound }));

            var result = await _controller.GetAsync(fakeId);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        [Trait("Category", "Person-FindAll")]
        public async Task FindAll_CallMethod_ReturnsOkStatusWithTwoPeopleAsync()
        {
            var people = await TestReferences.TwoValidPeople();
            _service.Setup(x => x.FindAll()).ReturnsAsync(new PersonListWithMessage { People = people, Message = ActionMessages.Found });

            var result = await _controller.GetAllAsync();

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<PersonListWithMessage>(viewResult.Value);
            Assert.Equal(people, model.People);
        }

        [Fact]
        [Trait("Category", "Person-FindAll")]
        public async Task FindAll_CallMethod_ReturnsBadRequestObjectResultIfListIsEmptyAsync()
        {
            _service.Setup(x => x.FindAll()).Returns(Task.FromResult(new PersonListWithMessage { Message = StatusMessages.EmptyList }));

            var result = await _controller.GetAllAsync();

            Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion Find

        #region Edit

        [Fact]
        [Trait("Category", "Person-Edit")]
        public async Task Edit_SubmitValidData_ReturnsOkStatusWithUpdatedPersonAsync()
        {
            var person = await TestReferences.OneValidPerson();
            _service.Setup(x => x.Edit(person)).ReturnsAsync(new PersonWithMessage { Person = person, Message = ActionMessages.Updated });

            var result = await _controller.EditAsync(person);

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<Person>(viewResult.Value);
            Assert.Equal(person, model);
        }

        [Fact]
        [Trait("Category", "Person-Edit")]
        public async Task Edit_InvalidModelState_ReturnsBadRequestObjectResultAsync()
        {
            var person = await TestReferences.OneInvalidPerson();
            _controller.ModelState.AddModelError(string.Empty, "Unexpected error: Please check all fields and try again.");

            var result = await _controller.EditAsync(person);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        [Trait("Category", "Person-Edit")]
        public async Task Edit_SubmitValidDataInvalidId_ReturnsNotFoundObjectResultAsync()
        {
            var person = await TestReferences.OneValidPerson();
            person.Id = Guid.Empty;
            _service.Setup(x => x.Edit(person)).Returns(Task.FromResult(new PersonWithMessage { Message = StatusMessages.NotFound }));

            var result = await _controller.EditAsync(person);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion Edit

        #region Delete

        [Fact]
        [Trait("Category", "Person-Delete")]
        public async Task Delete_SubmitValidId_ReturnsOkStatusIndicatingPersonWasRemovedAsync()
        {
            var id = await TestReferences.IdGeneration();
            _service.Setup(x => x.Delete(id)).ReturnsAsync(ActionMessages.Deleted);

            var result = await _controller.DeleteAsync(id);

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<string>(viewResult.Value);
            Assert.Equal(ActionMessages.Deleted, model);
        }

        [Fact]
        [Trait("Category", "Person-Delete")]
        public async Task Delete_SubmitValidId_ReturnsNotFoundObjectResultAsync()
        {
            var id = await TestReferences.IdGeneration();
            _service.Setup(x => x.Delete(id)).Returns(Task.FromResult(StatusMessages.NotFound));

            var result = await _controller.DeleteAsync(id);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        [Trait("Category", "Person-Delete")]
        public async Task Delete_SubmitInvalidId_ReturnsBadRequestObjectResultAsync()
        {
            var id = Guid.Empty;

            var result = await _controller.DeleteAsync(id);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion Delete
    }
}