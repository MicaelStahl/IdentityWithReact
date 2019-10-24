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

        public PersonApiControllerTesting(ILogger<PersonApiController> logger)
        {
            Logger = logger;
            _service = new Mock<IPersonRepository>();
            _controller = new PersonApiController(_service.Object, Logger);
        }

        public ILogger<PersonApiController> Logger { get; }

        #endregion D.I

        #region Create

        [Fact]
        [Trait("Category", "Person-Create")]
        public async Task Create_SubmitValidData_ReturnsOkStatusWithCreatedPersonAsync()
        {
            var person = await TestReferences.OneValidPerson();
            _service.Setup(x => x.Create(person)).ReturnsAsync(new PersonWithMessage { Person = person, Message = ActionMessages.Created });

            var result = await _controller.CreateAsync(person);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Person>(viewResult.Model);
            Assert.Equal(person, model);
        }

        #endregion Create
    }
}