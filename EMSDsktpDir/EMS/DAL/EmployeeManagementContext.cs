using EMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace EMS.DAL
{
        public class EmployeeManagementContext : DbContext
        {

            public EmployeeManagementContext() : base("EmployeeManagementContext")
            {
            }

            public DbSet<Employee> Employees { get; set; }
            public DbSet<Shift> Shifts { get; set; }
            public DbSet<Leave> Leaves { get; set; }
        public DbSet<DayOff> DayOffs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            }
        }
}