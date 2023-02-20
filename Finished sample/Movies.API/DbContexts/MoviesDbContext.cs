using Microsoft.EntityFrameworkCore;
using Movies.API.Entities;

namespace Movies.API.DbContexts;

public class MoviesDbContext : DbContext
{
    public DbSet<Movie> Movies { get; set; } = null!;

    public MoviesDbContext(DbContextOptions<MoviesDbContext> options)
        : base(options)
    {
    }

    // seed the database with data
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Director>().HasData(
            new(Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                "Quentin",
                "Tarantino"),
            new(Guid.Parse("da2fd609-d754-4feb-8acd-c4f9ff13ba96"),
                "Joel",
                "Coen"),
            new(Guid.Parse("c19099ed-94db-44ba-885b-0ad7205d5e40"),
                "Martin",
                "Scorsese"),
            new(Guid.Parse("0c4dc798-b38b-4a1c-905c-a9e76dbef17b"),
                "David",
                "Fincher"),
            new(Guid.Parse("937b1ba1-7969-4324-9ab5-afb0e4d875e6"),
                "Bryan",
                "Singer"),
            new(Guid.Parse("7a2fbc72-bb33-49de-bd23-c78fceb367fc"),
                "James",
                "Cameron"));

        modelBuilder.Entity<Movie>().HasData(
            new(Guid.Parse("5b1c2b4d-48c7-402a-80c3-cc796ad49c6b"),
                 Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                 "Pulp Fiction",
                 new DateTimeOffset(new DateTime(1994, 11, 9)),
                 "Crime, Drama",
                 "The lives of two mob hitmen, a boxer, a gangster's wife, and a pair of diner bandits intertwine in four tales of violence and redemption."),
            new(Guid.Parse("6e87f657-f2c1-4d90-9b37-cbe43cc6adb9"),
                 Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                 "Jackie Brown",
                 new DateTimeOffset(new DateTime(1997, 12, 25)),
                 "Crime, Drama",
                 "A middle-aged woman finds herself in the middle of a huge conflict that will either make her a profit or cost her life."),
            new(Guid.Parse("d8663e5e-7494-4f81-8739-6e0de1bea7ee"),
                Guid.Parse("da2fd609-d754-4feb-8acd-c4f9ff13ba96"),
                "The Big Lebowski",
                new DateTimeOffset(new DateTime(1998, 3, 6)),
                "Comedy, Crime",
                "The Dude (Lebowski), mistaken for a millionaire Lebowski, seeks restitution for his ruined rug and enlists his bowling buddies to help get it."),
            new(Guid.Parse("f9a16fee-4c49-41bb-87a1-bbaad0cd1174"),
                Guid.Parse("c19099ed-94db-44ba-885b-0ad7205d5e40"),
                "Casino",
                new DateTimeOffset(new DateTime(1995, 11, 22)),
                "Crime, Drama",
                "A tale of greed, deception, money, power, and murder occur between two best friends: a mafia enforcer and a casino executive, compete against each other over a gambling empire, and over a fast living and fast loving socialite."),
            new(Guid.Parse("bb6a100a-053f-4bf8-b271-60ce3aae6eb5"),
                Guid.Parse("0c4dc798-b38b-4a1c-905c-a9e76dbef17b"),
                "Fight Club",
                new DateTimeOffset(new DateTime(1999, 10, 15)),
                "Drama",
                "An insomniac office worker and a devil-may-care soapmaker form an underground fight club that evolves into something much, much more."),
              new(Guid.Parse("3d2880ae-5ba6-417c-845d-f4ebfd4bcac7"),
                  Guid.Parse("937b1ba1-7969-4324-9ab5-afb0e4d875e6"),
                  "The Usual Suspects",
                  new DateTimeOffset(new DateTime(1995, 9, 15)),
                  "Crime, Thriller",
                  "A sole survivor tells of the twisty events leading up to a horrific gun battle on a boat, which began when five criminals met at a seemingly random police lineup."),
              new(Guid.Parse("26fcbcc4-b7f7-47fc-9382-740c12246b59"),
                  Guid.Parse("7a2fbc72-bb33-49de-bd23-c78fceb367fc"),
                  "Terminator 2: Judgment Day",
                  new DateTimeOffset(new DateTime(1991, 7, 3)),
                  "Action, Sci-Fi",
                   "A cyborg, identical to the one who failed to kill Sarah Connor, must now protect her teenage son, John Connor, from a more advanced and powerful cyborg."));

        base.OnModelCreating(modelBuilder);
    }

}
