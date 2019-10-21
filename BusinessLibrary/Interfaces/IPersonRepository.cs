using DataAccessLibrary.Models;
using DataAccessLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLibrary.Interfaces
{
    /// <summary>
    /// Change this later for authorization purposes.
    /// </summary>
    public interface IPersonRepository
    {
        Task<Task> Create(Person person);

        Task<Person> Find(Guid id);

        Task<PersonList> FindAll();

        Task<Person> Edit(Person person);

        Task<bool> Delete(Guid id);
    }
}