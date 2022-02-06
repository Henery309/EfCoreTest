using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ParameterCache;

var sqlConnection = new SqliteConnection("DataSource=:memory:");
sqlConnection.Open();

var options = new DbContextOptionsBuilder<ProjectDbContext>().UseSqlite(sqlConnection).Options;
var db = new ProjectDbContext(options);
db.Database.EnsureCreated();

var project1 = new Project()
{
    Id = 1,
    Name = "project1"
};

var project2 = new Project()
{
    Id = 2,
    Name = "project2",
    StaffMembers = new List<StaffMember>() { new StaffMember { Id = 3, Name = "Josh" }, new StaffMember { Id = 5, Name = "Tony" } }
};

db.Projects.Add(project1);
db.Projects.Add(project2);
db.SaveChanges();

var staffIds = new int[] { 3, 5 };
var projs = db.Projects.In(staffIds, m => m.StaffMembers.Single().Id).ToList();

/*
 
 SELECT [p].[Id], [p].[Name]
      FROM [dbo].[Project] AS [p]
      WHERE ((
          SELECT TOP(1) [s].[Id]
          FROM [dbo].[StaffMember] AS [s]
          WHERE [p].[Id] = [s].[ProjectId]) = @__v_0) OR ((
          SELECT TOP(1) [s0].[Id]
          FROM [dbo].[StaffMember] AS [s0]
          WHERE [p].[Id] = [s0].[ProjectId]) = @__v_1)
 
 */

/*
 SELECT [p].[Id], [p].[Name]
      FROM [dbo].[Project] AS [p]
      WHERE EXISTS (
          SELECT 1
          FROM [dbo].[StaffMember] AS [s]
          WHERE ([p].[Id] = [s].[ProjectId]) AND ([s].[Id] = @param1 OR [s].Id == @param2))
 
 */