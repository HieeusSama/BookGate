using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using Microsoft.EntityFrameworkCore;
using BookGate.Domain.Entities;
namespace BookGate.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Auth> Auths { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
    }
}
