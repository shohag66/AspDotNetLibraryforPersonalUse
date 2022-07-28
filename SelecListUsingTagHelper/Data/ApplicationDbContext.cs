using SelecListUsingTagHelper.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SelecListUsingTagHelper.Data
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Department> Departments { get; set; }
    }
}