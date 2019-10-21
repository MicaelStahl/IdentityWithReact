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

        private readonly ReactDbContext _db;

        public PersonRepository(ReactDbContext db)
        {
            _db = db;
        }

        #endregion D.I

        #region Create

        public async Task<Task> Create(Person person)
        {
            try
            {
                // Create authorization with token generation later.

                if (string.IsNullOrWhiteSpace(person.FirstName) || string.IsNullOrWhiteSpace(person.LastName) ||
                    string.IsNullOrWhiteSpace(person.Email) || string.IsNullOrWhiteSpace(person.PhoneNumber) ||
                    string.IsNullOrWhiteSpace(person.City) || string.IsNullOrWhiteSpace(person.PostalCode) ||
                    person.Age > 110 || person.Age < 5)
                {
                    throw new Exception("Unexpected error occurred: One or more fields were invalid.");
                }

                // Gets the entity being created (with a new ID)
                var createdEntity = await _db.People.AddAsync(
                    new Person
                    {
                        Id = Guid.NewGuid(),
                        FirstName = person.FirstName,
                        LastName = person.LastName,
                        Age = person.Age,
                        City = person.City,
                        Email = person.Email,
                        PhoneNumber = person.PhoneNumber,
                        PostalCode = person.PostalCode
                    });

                await _db.SaveChangesAsync();

                // Returns a task with the created entity.
                return Task.FromResult(createdEntity.Entity);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        #endregion Create

        #region Find

        public async Task<Person> Find(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    throw new NullReferenceException("Unexpected error occured: ID was blank.");
                }

                var person = await _db.People.FirstOrDefaultAsync(x => x.Id == id);

                if (person == null)
                {
                    throw new NullReferenceException($"Unexpected error occurred: No person exists with ID: {id}.");
                }

                return await Task.FromResult(person);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public async Task<PersonList> FindAll()
        {
            try
            {
                var people = await _db.People.ToListAsync();

                if (people == null || people.Count == 0)
                {
                    throw new NullReferenceException("Unexpected error: No people were found.");
                }

                return new PersonList { People = people };
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        #endregion Find

        #region Edit

        public async Task<Person> Edit(Person person)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(person.FirstName) || string.IsNullOrWhiteSpace(person.LastName) ||
        string.IsNullOrWhiteSpace(person.Email) || string.IsNullOrWhiteSpace(person.PhoneNumber) ||
        string.IsNullOrWhiteSpace(person.City) || string.IsNullOrWhiteSpace(person.PostalCode) ||
        person.Age > 110 || person.Age < 5)
                {
                    throw new Exception("Unexpected error occurred: One or more fields were invalid.");
                }

                var original = await _db.People.FirstOrDefaultAsync(x => x.Id == person.Id);

                if (original == null)
                {
                    throw new NullReferenceException($"Unexpected error occurred: No person was found with ID: {person.Id}");
                }

                original.FirstName = person.FirstName;
                original.LastName = person.LastName;
                original.Age = person.Age;
                original.Email = person.Email;
                original.PhoneNumber = person.PhoneNumber;
                original.City = person.City.ToUpper();
                original.PostalCode = person.PostalCode;

                await _db.SaveChangesAsync();

                return original;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        #endregion Edit

        #region Delete

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    throw new NullReferenceException("Unexpected error: ID was blank.");
                }

                var person = await _db.People.FirstOrDefaultAsync(x => x.Id == id);

                if (person == null)
                {
                    throw new NullReferenceException($"Unexpected error occurred: No person with ID: {id} was found.");
                }

                var entityState = _db.People.Remove(person);

                await _db.SaveChangesAsync();

                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        #endregion Delete
    }
}