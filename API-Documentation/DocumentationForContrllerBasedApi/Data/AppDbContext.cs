using DocumentationForContrllerBasedApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DocumentationForContrllerBasedApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options,IPasswordHasher<AppUser> passwordHasher) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
    protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);
    var superManager = new AppUser
    {
        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        FirstName = "Super",
        LastName = "Manager",
        Email = "supermanager@test.com",
        BirthDate = new DateOnly(1995, 1, 1),
        Roles = ["supermanager"],
        Permissions =
        [
            // Project
            "project:create",
            "project:read",
            "project:update",
            "project:delete",
            "project:assign_member",
            "project:manage_budget",

            // Task
            "task:create",
            "task:read",
            "task:update",
            "task:delete",
            "task:assign_user",
            "task:update_status",
            "task:comment"
        ]
    };

    superManager.PasswordHash ="AQAAAAIAAYagAAAAEMgowEOmfyX1Fg2pVWG9Y3g9EmziM9VtE4sFqvfIMrhOiy/RLoXtfnaNNHd1FSFSTg==";

    var manager = new AppUser
    {
        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
        FirstName = "Project",
        LastName = "Manager",
        Email = "manager@test.com",
        BirthDate = new DateOnly(1997, 1, 1),
        Roles = ["manager"],
        Permissions =
        [
            "project:create",
            "project:read",
            "project:update",
            "project:delete",
            "project:assign_member",
            "project:manage_budget"
        ]
    };

    manager.PasswordHash ="AQAAAAIAAYagAAAAENKcW6STYSBPHp/ZOs3E/hxeH1r06H3kZ0Eltq1NnY9oZ96w5UGAT0z6Fy8QNFqHpA==";

    var employee = new AppUser
    {
        Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
        FirstName = "Employee",
        LastName = "User",
        Email = "employee@test.com",
        BirthDate = new DateOnly(2000, 1, 1),
        Roles = ["employee"],
        Permissions =
        [
            "task:create",
            "task:read",
            "task:update",
            "task:delete",
            "task:assign_user",
            "task:update_status",
            "task:comment"
        ]
    };

    employee.PasswordHash ="AQAAAAIAAYagAAAAEEN611uY7ftj3llpfZo2d8zCn9kyo0Zxqxb/AswL3uWQYFNjFegvCupnkaGUjU4NJw==";

    builder.Entity<AppUser>().HasData(
        superManager,
        manager,
        employee
    );
}
}