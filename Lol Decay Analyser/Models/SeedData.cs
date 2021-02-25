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
                if (context.Riots.Any())
                {
                    return;   // DB has been seeded
                }
             //   RiotUser();
                context.Riots.AddRange(
                    new RiotModel
                    {
                        SummonerName = "ImWorstNightmare",
                        LastMatch = null,
                        Rank = null,
                        TimeRemain = null,
                        Region = "Euw1"
                    },

                    new RiotModel
                    {
                        SummonerName = "A Wild Nightmare",
                        LastMatch = null,
                        Rank = null,
                        TimeRemain = null,
                        Region = "Euw1"
                    },

                    new RiotModel
                    {
                        SummonerName = "League of Inting",
                        LastMatch = null,
                        Rank = null,
                        TimeRemain = null,
                        Region = "Euw1"
                    },

                    new RiotModel
                    {
                        SummonerName = "Mr Donald Trump",
                        LastMatch = null,
                        Rank = null,
                        TimeRemain = null,
                        Region = "Euw1"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}


