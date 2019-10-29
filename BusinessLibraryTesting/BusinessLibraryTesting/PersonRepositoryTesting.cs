using BusinessLibrary.Interfaces;
using DataAccessLibrary.Models;
using DataAccessLibrary.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BusinessLibraryTesting.BusinessLibraryTesting
{
    /// <summary>
    /// Class used for References for BusinessLibraryTesting.
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

    public class PersonRepositoryTesting
    {
        #region D.I

        private readonly Mock<IPersonRepository> _service;

        public PersonRepositoryTesting()
        {
            _service = new Mock<IPersonRepository>();
        }

        #endregion D.I

        #region Create

        [Fact]
        [Trait("Repository", "Person-Create")]
        public async Task Create_SubmitValidData_ReturnsCorrectlyCreatedPersonWithMessageAsync()
        {
            var person = await TestReferences.OneValidPerson();
            _service.Setup(x => x.Create(person)).ReturnsAsync(new PersonWithMessage { Person = person, Message = ActionMessages.Created });

            var result = await _service.Object.Create(person);

            Assert.Equal(person, result.Person);
            Assert.Contains("created.", result.Message);
        }

        [Fact]
        [Trait("Repository", "Person-Create")]
        public async Task Create_SubmitEmptyFields_ReturnsStatusMessageIndicatingInvalidFieldsAsync()
        {
            var person = await TestReferences.OneInvalidPerson();
            _service.Setup(x => x.Create(person)).Returns(Task.FromResult(new PersonWithMessage { Message = StatusMessages.InvalidFields }));

            var result = await _service.Object.Create(person);

            Assert.Equal(StatusMessages.InvalidFields, result.Message);
        }

        #endregion Create

        #region Find

        #region FindOne

        [Fact]
        [Trait("Repository", "Person-FindOne")]
        public async Task FindOne_SubmitValidId_ReturnsCorrectPersonWithMessageAsync()
        {
            var person = await TestReferences.OneValidPerson();
            _service.Setup(x => x.Find(person.Id)).ReturnsAsync(new PersonWithMessage { Person = person, Message = ActionMessages.Found });

            var result = await _service.Object.Find(person.Id);

            Assert.Equal(person, result.Person);
            Assert.Contains("found.", result.Message);
        }

        [Fact]
        [Trait("Repository", "Person-FindOne")]
        public async Task FindOne_SubmitValidId_ReturnsNotFoundMessageAsync()
        {
            var fakeId = await TestReferences.IdGeneration();
            _service.Setup(x => x.Find(fakeId)).Returns(Task.FromResult(new PersonWithMessage { Message = StatusMessages.NotFound }));

            var result = await _service.Object.Find(fakeId);

            Assert.Equal(StatusMessages.NotFound, result.Message);
        }

        [Fact]
        [Trait("Repository", "Person-FindOne")]
        public async Task FindOne_SubmitInvalidId_ReturnsBadIdMessageAsync()
        {
            var fakeId = Guid.Empty;
            _service.Setup(x => x.Find(fakeId)).Returns(Task.FromResult(new PersonWithMessage { Message = StatusMessages.EmptyId }));

            var result = await _service.Object.Find(fakeId);

            Assert.Equal(StatusMessages.EmptyId, result.Message);
        }

        #endregion FindOne

        #region FindAll

        [Fact]
        [Trait("Repository", "Person-FindAll")]
        public async Task FindAll_CallMethod_ReturnsListOfPeopleIfExistsAsync()
        {
            var people = await TestReferences.TwoValidPeople();
            _service.Setup(x => x.FindAll()).ReturnsAsync(new PersonListWithMessage { People = people, Message = ActionMessages.Found });

            var result = await _service.Object.FindAll();

            Assert.Equal(people, result.People);
            Assert.Equal(ActionMessages.Found, result.Message);
        }

        [Fact]
        [Trait("Reposiory", "Person-FindAll")]
        public async Task FindAll_CallMethod_ReturnsMessageIndicatingEmptyListAsync()
        {
            _service.Setup(x => x.FindAll()).Returns(Task.FromResult(new PersonListWithMessage { Message = StatusMessages.EmptyList }));

            var result = await _service.Object.FindAll();

            Assert.Empty(result.People);
            Assert.Equal(StatusMessages.EmptyList, result.Message);
        }

        #endregion FindAll

        #endregion Find

        #region Edit

        [Fact]
        [Trait("Repository", "Person-Edit")]
        public async Task Edit_SubmitValidData_ReturnsUpdatedPersonWithMessageAsync()
        {
            var person = await TestReferences.OneValidPerson();
            _service.Setup(x => x.Edit(person)).ReturnsAsync(new PersonWithMessage { Person = person, Message = ActionMessages.Updated });

            var result = await _service.Object.Edit(person);

            Assert.Equal(person, result.Person);
            Assert.Equal(ActionMessages.Updated, result.Message);
        }

        [Fact]
        [Trait("Repository", "Person-Edit")]
        public async Task Edit_SubmitInvalidData_ReturnsErrorMessageIndicatingInvalidFieldsAsync()
        {
            var person = await TestReferences.OneInvalidPerson();
            _service.Setup(x => x.Edit(person)).Returns(Task.FromResult(new PersonWithMessage { Message = StatusMessages.InvalidFields }));

            var result = await _service.Object.Edit(person);

            Assert.Equal(StatusMessages.InvalidFields, result.Message);
        }

        #endregion Edit

        #region Delete

        [Fact]
        [Trait("Repository", "Person-Delete")]
        public async Task Delete_SubmitValidId_ReturnsStringIndicatingPersonWasDeletedAsync()
        {
            var person = await TestReferences.OneValidPerson();
            _service.Setup(x => x.Delete(person.Id)).ReturnsAsync(ActionMessages.Deleted);

            Assert.Equal(ActionMessages.Deleted, await _service.Object.Delete(person.Id));
        }

        [Fact]
        [Trait("Repository", "Person-Delete")]
        public async Task Delete_SubmitValidId_ReturnsNotFoundMessageAsync()
        {
            var fakeId = await TestReferences.IdGeneration();
            _service.Setup(x => x.Delete(fakeId)).Returns(Task.FromResult(StatusMessages.NotFound));

            Assert.Equal(StatusMessages.NotFound, await _service.Object.Delete(fakeId));
        }

        [Fact]
        [Trait("Repository", "Person-Delete")]
        public async Task Delete_SubmitInvalidId_ReturnsInvalidIdMessageAsync()
        {
            var fakeId = Guid.Empty;
            _service.Setup(x => x.Delete(fakeId)).Returns(Task.FromResult(StatusMessages.EmptyId));

            Assert.Equal(StatusMessages.EmptyId, await _service.Object.Delete(fakeId));
        }

        #endregion Delete
    }
}