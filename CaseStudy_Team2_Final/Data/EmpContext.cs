using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CaseStudy_Team2_Final.Models;

namespace CaseStudy_Team2_Final.Data
{
    public class EmpContext : DbContext
    {
        public EmpContext (DbContextOptions<EmpContext> options)
            : base(options)
        {
        }

        public DbSet<CaseStudy_Team2_Final.Models.Emp> Emp { get; set; }
    }
}
