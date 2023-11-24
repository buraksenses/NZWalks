﻿using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data;

public class NZWalksDbContext : DbContext
{
    public NZWalksDbContext(DbContextOptions dbContextOptions): base(dbContextOptions)
    {
        
    }

    public DbSet<Difficulty> Difficulties { get; set; }

    public DbSet<Region?> Regions { get; set; }

    public DbSet<Walk> Walks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Seed data for Difficulties
        //Easy, Medium, Hard

        var difficulties = new List<Difficulty>
        {
            new()
            {
                Id = Guid.Parse("68e1c164-b966-460c-9958-41a6666532b0"),
                Name = "Easy"
            },

            new()
            {
                Id = Guid.Parse("1739ea74-412a-4ccd-b1e2-8748d9173689"),
                Name = "Medium"
            },
            new()
            {
                Id = Guid.Parse("df3e0836-739e-4b62-bbe2-46b243413980"),
                Name = "Hard"
            }
        };

        //Seed difficulties to the database
        modelBuilder.Entity<Difficulty>().HasData(difficulties);
        
        //Seed data for regions
        var regions = new List<Region>
        {
            new()
            {
                Id = Guid.Parse("f9ac63be-ca43-47bf-af37-cfaf3cdd63ba"),
                Name = "Auckland",
                Code = "AKL",
                RegionImageUrl = "image.jpg"
            },
            new ()
            {
                Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
                Name = "Northland",
                Code = "NTL",
                RegionImageUrl = null
            },
            new ()
            {
                Id = Guid.Parse("14ceba71-4b51-4777-9b17-46602cf66153"),
                Name = "Bay Of Plenty",
                Code = "BOP",
                RegionImageUrl = null
            },
            new ()
            {
                Id = Guid.Parse("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"),
                Name = "Wellington",
                Code = "WGN",
                RegionImageUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
            },
            new ()
            {
                Id = Guid.Parse("906cb139-415a-4bbb-a174-1a1faf9fb1f6"),
                Name = "Nelson",
                Code = "NSN",
                RegionImageUrl = "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
            },
            new ()
            {
                Id = Guid.Parse("f077a22e-4248-4bf6-b564-c7cf4e250263"),
                Name = "Southland",
                Code = "STL",
                RegionImageUrl = null
            }
        };
        modelBuilder.Entity<Region>().HasData(regions);
    }
}