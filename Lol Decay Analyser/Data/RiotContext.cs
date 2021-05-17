using Lol_Decay_Analyser.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lol_Decay_Analyser.Data
{
    public class RiotContext : DbContext
    {
        public RiotContext (DbContextOptions<RiotContext> options)
            : base(options) { }
        public DbSet<RiotDBModel> Riot { get; set; }
    }
}
