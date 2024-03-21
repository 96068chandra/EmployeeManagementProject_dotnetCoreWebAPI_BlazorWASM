﻿using BaseLibrary.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {

        public DbSet<Employee>  Employees{ get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<GeneralDepartment> GeneralDepartments { get; set; }
        public DbSet<Town> Town { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<SystemRole> SystemRoles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }



    }
}