﻿using Hsiaye.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.NUnitTest
{
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
            modelBuilder.Entity<Member_Role>();
            modelBuilder.Entity<Permission>();
            modelBuilder.Entity<MemberLoginAttempt>();
            modelBuilder.Entity<MemberToken>();
            modelBuilder.Entity<Demo>();

        }
        public DbSet<Member> Member { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Member_Role> Member_Role { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<MemberLoginAttempt> MemberLoginAttempt { get; set; }
        public DbSet<MemberToken> MemberToken { get; set; }
        public DbSet<Demo> Demo { get; set; }
    }
}
