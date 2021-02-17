using Lol_Decay_Analyser.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                if (context.Riots.Any())
                {
                    return;   // DB has been seeded
                }

                context.Riots.AddRange(
                    new RiotModel
                    {
                        SummonerName = "Test1",
                    },

                    new RiotModel
                    {
                        SummonerName = "Test2",
                    },

                    new RiotModel
                    {
                        SummonerName = "Test3",
                    },

                    new RiotModel
                    {
                        SummonerName = "Test4",
                    }
                );
                context.SaveChanges();
            }
        }
    }
}


