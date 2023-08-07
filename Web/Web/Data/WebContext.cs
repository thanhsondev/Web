using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web.Data
{
    public class WebContext : DbContext
    {
        public WebContext (DbContextOptions<WebContext> options)
            : base(options)
        {
        }

        public DbSet<Web.Models.book> book { get; set; } = default!;

        public DbSet<Web.Models.usersaccounts> usersaccounts { get; set; } = default!;


    }
}
