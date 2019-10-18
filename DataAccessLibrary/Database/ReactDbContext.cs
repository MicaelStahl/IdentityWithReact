using DataAccessLibrary.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Database
{
    /// <summary>
    /// The database used for this application.
    /// </summary>
    public class ReactDbContext : ApiAuthorizationDbContext<AppUser>
    {
        public ReactDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Person> People { get; set; }
    }
}