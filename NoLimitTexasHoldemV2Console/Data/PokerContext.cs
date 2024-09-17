using NoLimitTexasHoldemV2.Models;      //To use Card.cs and HandData.cs
using Microsoft.EntityFrameworkCore;    //To inherit from the DbContext class

namespace NoLimitTexasHoldemV2.Data
{
    public class PokerContext : DbContext       //Inheriting DbContext class
    {
        //Setting up PokerContext to know about these entities
        public DbSet<HandData> Hands => Set<HandData>();
        public DbSet<Card> Cards => Set<Card>();

        //Calling the constructor of its base class (DbContext) and passing the options parameter to it.
        public PokerContext(DbContextOptions<PokerContext> options) : base(options)
        {

        }

        //Method for configuring entity relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Each card has 1 related HandData entity through a PlayerHandData reference, each HandData entity has a collection of
            //Card entities through a PlayerHoleCards reference, and we link a FK with the common attribute PlayerHandDataID.
            //Same for all below but with different references and such.
            
            //For player hole cards
            modelBuilder.Entity<Card>()
                .HasOne(c => c.PlayerHandData)
                .WithMany(h => h.PlayerHoleCards)
                .HasForeignKey(c => c.PlayerHandDataId);

            //For machine hole cards
            modelBuilder.Entity<Card>()
                .HasOne(c => c.MachineHandData)
                .WithMany(h => h.MachineHoleCards)
                .HasForeignKey(c => c.MachineHandDataId);

            //For community cards
            modelBuilder.Entity<Card>()
                .HasOne(c => c.CommunityHandData)
                .WithMany(h => h.CommunityCards)
                .HasForeignKey(c => c.CommunityHandDataId);

            //Explicitly specifying primary keys
            modelBuilder.Entity<Card>()
                .HasKey(c => c.CardId);

            modelBuilder.Entity<HandData>()
                .HasKey(h => h.HandDataId);
        }
    }
}