using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        /*
            Explanation of the following:
            In summary, this code snippet defines a custom database context class ApplicationDBContext that extends DbContext and accepts DbContextOptions in its constructor. 
            This constructor allows the configuration of the database context's behavior, such as connection strings and database providers, at runtime. 
        */

        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        //dbset manipulates entire table
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
         
    }
}