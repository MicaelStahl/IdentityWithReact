using BusinessLibrary.Interfaces;
using DataAccessLibrary.Database;
using DataAccessLibrary.Models;
using DataAccessLibrary.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLibrary.Repositories
{
    internal class PersonRepository : IPersonRepository
    {
        #region D.I

        private readonly ILogger<PersonRepository> _logger;
        private readonly ReactDbContext _db;

        public PersonRepository(ILogger<PersonRepository> logger, ReactDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        #endregion D.I

        #region Create

        public async Task<Task> Create(Person person)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(person.FirstName) || string.IsNullOrWhiteSpace(person.LastName) ||
                    string.IsNullOrWhiteSpace(person.Email) || string.IsNullOrWhiteSpace(person.PhoneNumber) ||
                    string.IsNullOrWhiteSpace(person.City) || string.IsNullOrWhiteSpace(person.PostalCode) ||
                    person.Age > 110 || person.Age < 5)
                {
                    _logger.LogError($"Person {person} contained one or more invalid fields.",
                        new string[] {
                            person.FirstName,
                            person.LastName,
                            person.Email,
                            person.PhoneNumber,
                            person.City,
                            person.PostalCode,
                            person.Age.ToString()
                        });

                    throw new Exception("Unexpected error occurred: One or more fields were invalid.");
                }

                await _db.People.AddAsync(person);

                _logger.LogInformation($"Person {person} was successfully created.");

                return Task.FromResult(person);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion Create

        #region Find

        public async Task<Person> Find(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<PersonList> FindAll()
        {
            throw new NotImplementedException();
        }

        #endregion Find

        #region Edit

        public async Task<Person> Edit(Person person)
        {
            throw new NotImplementedException();
        }

        #endregion Edit

        #region Delete

        public async Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        #endregion Delete
    }
}