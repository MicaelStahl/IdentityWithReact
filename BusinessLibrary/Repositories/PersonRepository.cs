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
    public class PersonRepository : IPersonRepository
    {
        #region D.I

        private readonly ReactDbContext _db;

        public PersonRepository(ReactDbContext db)
        {
            _db = db;
        }

        #endregion D.I

        #region Create

        public async Task<PersonWithMessage> Create(Person person)
        {
            try
            {
                // Create authorization with token generation later.

                if (string.IsNullOrWhiteSpace(person.FirstName) || string.IsNullOrWhiteSpace(person.LastName) ||
                    string.IsNullOrWhiteSpace(person.Email) || string.IsNullOrWhiteSpace(person.PhoneNumber) ||
                    string.IsNullOrWhiteSpace(person.City) || string.IsNullOrWhiteSpace(person.PostalCode) ||
                    person.Age > 110 || person.Age < 5)
                {
                    return new PersonWithMessage { Message = StatusMessages.InvalidFields };
                }

                // Gets the entity being created (with a new ID)
                var createdEntity = await _db.People.AddAsync(
                    new Person
                    {
                        Id = Guid.NewGuid(),
                        FirstName = person.FirstName,
                        LastName = person.LastName,
                        Age = person.Age,
                        City = person.City.ToUpper(),
                        Email = person.Email,
                        PhoneNumber = person.PhoneNumber,
                        PostalCode = person.PostalCode
                    });

                await _db.SaveChangesAsync();

                // Returns the created entity
                return new PersonWithMessage { Person = createdEntity.Entity, Message = ActionMessages.Created };
            }
            catch (Exception ex)
            {
                throw new NullReferenceException(ex.Message, ex.InnerException);
            }
        }

        #endregion Create

        #region Find

        public async Task<PersonWithMessage> Find(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return new PersonWithMessage { Message = StatusMessages.EmptyId };
                }

                var person = await _db.People.FirstOrDefaultAsync(x => x.Id == id);

                if (person == null)
                {
                    return new PersonWithMessage { Message = StatusMessages.NotFound };
                }

                return new PersonWithMessage { Person = person, Message = ActionMessages.Found };
            }
            catch (Exception ex)
            {
                throw new NullReferenceException(ex.Message, ex.InnerException);
            }
        }

        public async Task<PersonListWithMessage> FindAll()
        {
            try
            {
                var people = await _db.People.ToListAsync();

                if (people == null || people.Count == 0)
                {
                    return new PersonListWithMessage { Message = StatusMessages.EmptyList };
                }

                return new PersonListWithMessage { People = people, Message = ActionMessages.Found };
            }
            catch (Exception ex)
            {
                throw new NullReferenceException(ex.Message, ex.InnerException);
            }
        }

        #endregion Find

        #region Edit

        public async Task<PersonWithMessage> Edit(Person person)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(person.FirstName) || string.IsNullOrWhiteSpace(person.LastName) ||
                    string.IsNullOrWhiteSpace(person.Email) || string.IsNullOrWhiteSpace(person.PhoneNumber) ||
                    string.IsNullOrWhiteSpace(person.City) || string.IsNullOrWhiteSpace(person.PostalCode) ||
                    person.Age > 110 || person.Age < 5)
                {
                    return new PersonWithMessage { Message = StatusMessages.InvalidFields };
                }

                var original = await _db.People.FirstOrDefaultAsync(x => x.Id == person.Id);

                if (original == null)
                {
                    return new PersonWithMessage { Message = StatusMessages.NotFound };
                }

                original.FirstName = person.FirstName;
                original.LastName = person.LastName;
                original.Age = person.Age;
                original.Email = person.Email;
                original.PhoneNumber = person.PhoneNumber;
                original.City = person.City.ToUpper();
                original.PostalCode = person.PostalCode;

                await _db.SaveChangesAsync();

                return new PersonWithMessage { Person = original, Message = ActionMessages.Updated };
            }
            catch (Exception ex)
            {
                throw new NullReferenceException(ex.Message, ex.InnerException);
            }
        }

        #endregion Edit

        #region Delete

        public async Task<string> Delete(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return StatusMessages.EmptyId;
                }

                var person = await _db.People.FirstOrDefaultAsync(x => x.Id == id);

                if (person == null)
                {
                    return StatusMessages.NotFound;
                }

                _db.People.Remove(person);

                await _db.SaveChangesAsync();

                return ActionMessages.Deleted;
            }
            catch (Exception ex)
            {
                throw new NullReferenceException(ex.Message, ex.InnerException);
            }
        }

        #endregion Delete
    }
}