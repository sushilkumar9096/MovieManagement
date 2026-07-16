using Microsoft.EntityFrameworkCore;
using MovieManagement.Domain.Entities;
using System.Collections.Generic;

namespace MovieManagement.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Biography> Biographies { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure 1-to-1 relationship: Actor and Biography
            modelBuilder.Entity<Actor>()
                .HasOne(a => a.Biography)
                .WithOne(b => b.Actor)
                .HasForeignKey<Biography>(b => b.ActorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure 1-to-many relationship: Actor and Movie
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Actor)
                .WithMany(a => a.Movies)
                .HasForeignKey(m => m.ActorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure many-to-many relationship: Movie and Genre (mapped by property 'Genre' in Movie)
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Genre)
                .WithMany(g => g.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "MovieGenre",
                    j => j.HasOne<Genre>().WithMany().HasForeignKey("GenreId"),
                    j => j.HasOne<Movie>().WithMany().HasForeignKey("MovieId")
                );

            // Seeding Genres
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Drama" },
                new Genre { Id = 2, Name = "Sci-Fi" },
                new Genre { Id = 3, Name = "Action" }
            );

            // Seeding Actors
            modelBuilder.Entity<Actor>().HasData(
                new Actor { Id = 1, FirstName = "Tom", LastName = "Hanks" },
                new Actor { Id = 2, FirstName = "Leonardo", LastName = "DiCaprio" },
                new Actor { Id = 3, FirstName = "Morgan", LastName = "Freeman" }
            );

            // Seeding Biographies
            modelBuilder.Entity<Biography>().HasData(
                new Biography { Id = 1, Description = "Tom Hanks is an award-winning American actor known for Forrest Gump.", ActorId = 1 },
                new Biography { Id = 2, Description = "Leonardo DiCaprio is an Oscar-winning actor famous for Inception.", ActorId = 2 },
                new Biography { Id = 3, Description = "Morgan Freeman has an iconic voice and starred in The Shawshank Redemption.", ActorId = 3 }
            );

            // Seeding Movies
            modelBuilder.Entity<Movie>().HasData(
                new Movie { Id = 1, Name = "Forrest Gump", Description = "A simple man's epic journey through history.", ActorId = 1 },
                new Movie { Id = 2, Name = "Inception", Description = "A thief enters dreams to plant an idea.", ActorId = 2 },
                new Movie { Id = 3, Name = "The Shawshank Redemption", Description = "Two imprisoned men bond over a number of years.", ActorId = 3 }
            );
        }
    }
}
