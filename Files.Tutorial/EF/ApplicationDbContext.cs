using Files.Tutorial.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Files.Tutorial.EF
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FileOnDatabaseModel> FileOnDatabase { get; set; }
        public DbSet<FileOnFileSystemModel> FileOnFileSystem { get; set; }
    }
}
