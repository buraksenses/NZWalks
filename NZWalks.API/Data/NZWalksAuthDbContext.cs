using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data;

public class NZWalksAuthDbContext : IdentityDbContext
{
    public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        const string readerRoleId = "5da5bb3b-615e-45e8-a9e9-36ceeaf6300b";
        const string writerRoleId = "7c9d47dc-ef87-48e0-baf5-78566801d790";

        var roles = new List<IdentityRole>
        {
            new ()
            {
                Id = readerRoleId,
                ConcurrencyStamp = readerRoleId,
                Name = "Reader",
                NormalizedName = "Reader".ToUpper()
            },
            new ()
            {
                Id = writerRoleId,
                ConcurrencyStamp =  writerRoleId,
                Name = "Writer",
                NormalizedName = "Writer".ToUpper()
            }
        };

        builder.Entity<IdentityRole>().HasData(roles);
    }
}