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
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure 1-to-1 relationship: Actor and Biography
            modelBuilder.Entity<Actor>()
                .HasOne(a => a.Biography)
                .WithOne(b => b.Actor)
                .HasForeignKey<Biography>(b => b.ActorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure many-to-many relationship: Movie and Actor (mapped by property 'Actors' in Movie)
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Actors)
                .WithMany(a => a.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "MovieActor",
                    j => j.HasOne<Actor>().WithMany().HasForeignKey("ActorId"),
                    j => j.HasOne<Movie>().WithMany().HasForeignKey("MovieId")
                );

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
                new Movie { Id = 1, Name = "Forrest Gump", Description = "A simple man's epic journey through history." },
                new Movie { Id = 2, Name = "Inception", Description = "A thief enters dreams to plant an idea." },
                new Movie { Id = 3, Name = "The Shawshank Redemption", Description = "Two imprisoned men bond over a number of years." }
            );

            // Seeding many-to-many relationships (MovieGenre)
            modelBuilder.Entity("MovieGenre").HasData(
                new { MovieId = 1, GenreId = 1 }, // Forrest Gump (Drama)
                new { MovieId = 2, GenreId = 2 }, // Inception (Sci-Fi)
                new { MovieId = 2, GenreId = 3 }, // Inception (Action)
                new { MovieId = 3, GenreId = 1 }  // The Shawshank Redemption (Drama)
            );

            // Seeding many-to-many relationships (MovieActor)
            modelBuilder.Entity("MovieActor").HasData(
                new { MovieId = 1, ActorId = 1 }, // Forrest Gump (Tom Hanks)
                new { MovieId = 2, ActorId = 2 }, // Inception (Leonardo DiCaprio)
                new { MovieId = 3, ActorId = 3 }  // The Shawshank Redemption (Morgan Freeman)
            );

            // Seeding default users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", PasswordHash = User.HashPassword("password123"), Role = "Admin" },
                new User { Id = 2, Username = "user", PasswordHash = User.HashPassword("password123"), Role = "User" }
            );
        }
    }
}
