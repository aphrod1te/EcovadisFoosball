using System.Collections.Generic;
using Foosball.Logic.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Foosball.Logic.Persistence
{
    public class FoosballContext : DbContext
    {
        public FoosballContext(DbContextOptions<FoosballContext> options)
            : base(options) { }
        
        public DbSet<FoosballTable> FoosballTables { get; set; }
        public DbSet<FoosballGame> FoosballGames { get; set; }
        public DbSet<FoosballGameScore> FoosballGameScores { get; set; }
        public DbSet<FoosballGameSet> FoosballGameSets { get; set; }
        public DbSet<FinishedFoosballGameSet> FoosballGameFinishedSets { get; set; }
        public DbSet<FoosballPlayerSetup> FoosballPlayerSetups { get; set; }
        public DbSet<FoosballPlayer> FoosballTeamPlayers { get; set; }
        public DbSet<FoosballTeam> FoosballTeams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<FoosballGame>()
                .HasIndex(x => x.TableId)
                .HasName("IX_Available_Table")
                .HasFilter($"[{nameof(FoosballGame.EndDate)}] IS NULL")
                .IsUnique();
            
            modelBuilder
                .Entity<FoosballGame>()
                .HasCheckConstraint("CK_EndDate_GreaterThan_StartDate", "[EndDate] > [StartDate]");
            modelBuilder
                .Entity<FoosballGame>()
                .HasCheckConstraint("CK_StartDate_In_Future", "[StartDate] > CAST((((JulianDay('now', 'localtime') - 2440587.5)*86400.0) + 62135596800) * 10000000 AS BIGINT)");
            modelBuilder
                .Entity<FoosballGame>()
                .HasIndex(x => new { x.TableId, x.StartDate })
                .IsUnique();
            modelBuilder
                .Entity<FoosballGame>()
                .HasOne(x => x.Table)
                .WithMany(x => x.FoosballGames);
            modelBuilder
                .Entity<FoosballGame>()
                .Property(x => x.EndDate)
                .HasConversion(new DateTimeOffsetToBinaryConverter());
            modelBuilder
                .Entity<FoosballGame>()
                .Property(x => x.StartDate)
                .HasConversion(new DateTimeOffsetToBinaryConverter());
            modelBuilder
                .Entity<FoosballGame>()
                .HasOne(x => x.FoosballPlayerSetup).WithOne();
            modelBuilder
                .Entity<FoosballGame>()
                .HasOne(x => x.Score)
                .WithOne(x => x.Game);

            modelBuilder
                .Entity<FoosballPlayerSetup>()
                .HasOne(x => x.Game)
                .WithOne(x => x.FoosballPlayerSetup);
            
            modelBuilder
                .Entity<FoosballTeamPlayer>()
                .HasOne(x => x.FoosballTeam)
                .WithMany(x => x.FoosballTeamPlayers)
                .HasForeignKey(x => x.FoosballTeamId);
            
            modelBuilder
                .Entity<FoosballTeamPlayer>()
                .HasOne(x => x.FoosballPlayer)
                .WithMany(x => x.FoosballTeamPlayers)
                .HasForeignKey(x => x.FoosballPlayerId);

            modelBuilder
                .Entity<FoosballTeamPlayer>()
                .HasKey(x => new {x.FoosballPlayerId, x.FoosballTeamId});

            modelBuilder
                .Entity<FoosballGameScore>()
                .HasMany(x => x.Sets)
                .WithOne(x => x.Score);

            modelBuilder
                .Entity<FoosballGameSet>()
                .HasCheckConstraint("TeamAScore_Positive", $"[{nameof(FoosballGameSet.TeamAScore)}] >= 0");
            modelBuilder
                .Entity<FoosballGameSet>()
                .HasCheckConstraint("TeamBScore_Positive", $"[{nameof(FoosballGameSet.TeamBScore)}] >= 0");
            
            modelBuilder
                .Entity<FinishedFoosballGameSet>()
                .HasOne(x => x.Set);
            
            Seed(modelBuilder);
        }

        private static void Seed(ModelBuilder modelBuilder)
        {
            AddDefaultTables(modelBuilder);
            AddDefaultPlayers(modelBuilder);
        }

        private static void AddDefaultTables(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoosballTable>().HasData(
                new FoosballTable
                {
                    Id = 1
                }
            );
            modelBuilder.Entity<FoosballTable>().HasData(
                new FoosballTable
                {
                    Id = 2
                }
            );
        }

        private static void AddDefaultPlayers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoosballPlayer>().HasData(
                new List<FoosballPlayer>
                {
                    new FoosballPlayer
                    {
                        Id = 1,
                        Name = "First"
                    },
                    new FoosballPlayer
                    {
                        Id = 2,
                        Name = "Second"
                    },
                    new FoosballPlayer
                    {
                        Id = 3,
                        Name = "Third"
                    },
                    new FoosballPlayer
                    {
                        Id = 4,
                        Name = "Fourth"
                    },
                });
        }
    }
}