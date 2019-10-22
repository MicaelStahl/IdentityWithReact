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
        Task<PersonWithMessage> Create(Person person);

        Task<PersonWithMessage> Find(Guid id);

        Task<PersonListWithMessage> FindAll();

        Task<PersonWithMessage> Edit(Person person);

        Task<string> Delete(Guid id);
    }
}