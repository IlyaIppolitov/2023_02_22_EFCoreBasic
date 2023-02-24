using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EFCoreBasic
{
    using System.Windows.Controls;

    public class AppDbContext : DbContext
    {        
        private const string ConnectionString = "Data Source = StudentVisit.db";

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            // Ручная настройка ключа навигации
            modelBuilder.Entity<Visitation>()
                .HasOne(v => v.Student)
                .WithMany(s => s.Visitations);
        }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<Visitation> Visitations => Set<Visitation>();
    }



}
