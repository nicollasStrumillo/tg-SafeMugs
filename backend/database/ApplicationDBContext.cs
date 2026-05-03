using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.models;
using Microsoft.EntityFrameworkCore;

namespace backend.database
{
    public class ApplicationDBContext:DbContext
    {
        public DbSet<Produto> produtos {get; set;}
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options)
        {
            
        }
        
    }
}