using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CaseStudy_Team2_Final.Models;

namespace CaseStudy_Team2_Final.Data
{
    public class IdeaContext : DbContext
    {
        public IdeaContext (DbContextOptions<IdeaContext> options)
            : base(options)
        {
        }

        public DbSet<CaseStudy_Team2_Final.Models.Idea> Idea { get; set; }
    }
}
