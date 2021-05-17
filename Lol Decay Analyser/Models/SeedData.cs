using Lol_Decay_Analyser.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Lol_Decay_Analyser.Helper_Classes.RiotConnection;

namespace Lol_Decay_Analyser.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RiotContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<RiotContext>>()))
            {
                // Look for any movies.
                if (context.Riot.Any())
                {
                    return;   // DB has been seeded
                }
                context.SaveChanges();
            }
        }
    }
}


