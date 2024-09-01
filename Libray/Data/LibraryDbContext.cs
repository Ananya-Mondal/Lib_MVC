using Library.Models;
using Libray.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    public class LibraryDbContext: DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options):base(options) 
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
    }
}
