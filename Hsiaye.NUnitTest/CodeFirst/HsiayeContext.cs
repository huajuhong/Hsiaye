using Hsiaye.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.NUnitTest
{
    //生成表结构示例：https://docs.aspnetzero.com/en/aspnet-core-angular/latest/Extending-Existing-Entities#extending-non-abstract-entities
    public class HsiayeContext : DbContext
    {
        [Column]
        private readonly string _connectionString;

        public HsiayeContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>();
            modelBuilder.Entity<Role>();
            modelBuilder.Entity<MemberRole>();
            modelBuilder.Entity<Permission>();
            modelBuilder.Entity<MemberLoginAttempt>();
            modelBuilder.Entity<MemberToken>();
            modelBuilder.Entity<Demo>();

            modelBuilder.Entity<OrganizationUnit>();
            modelBuilder.Entity<OrganizationUnitRole>();

            modelBuilder.Entity<Membership>();
            modelBuilder.Entity<MembershipFundsflow>();
            modelBuilder.Entity<Product>();
            modelBuilder.Entity<PromotionDiscounts>();
            modelBuilder.Entity<WorkTime>();
            modelBuilder.Entity<WorkTimeProject>();
            modelBuilder.Entity<WorkTimeSalary>();

            modelBuilder.Entity<Post>();
            modelBuilder.Entity<PostCategory>();
            modelBuilder.Entity<PostComment>();

            modelBuilder.Entity<Answer>();
            modelBuilder.Entity<Question>();

            modelBuilder.Entity<TodoCategory>();
            modelBuilder.Entity<Todo>();
        }
        public DbSet<Member> Member { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<MemberRole> MemberRole { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<MemberLoginAttempt> MemberLoginAttempt { get; set; }
        public DbSet<MemberToken> MemberToken { get; set; }
        public DbSet<Demo> Demo { get; set; }
    }
}
