using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonCommandHandler.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            base.Database.EnsureCreated();
        }

        public DbSet<CommandInfo> Commands { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CommandInfo>()
                .HasKey(c => new { c.CreationDate, c.FileName });

            base.OnModelCreating(builder);
        }
    }
}
