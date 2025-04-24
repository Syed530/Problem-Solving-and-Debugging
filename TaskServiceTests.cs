using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

public class TaskServiceTests
{
    private DbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<DbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{System.Guid.NewGuid()}")
            .Options;

        var context = new DbContext(options);
        context.Set<TaskEntity>().AddRange(GetSeedData());
        context.SaveChanges();

        return context;
    }

    private List<TaskEntity> GetSeedData() => new()
    {
        new TaskEntity { Id = 1, Title = "Fix Bug", Description = "Fix login bug", AssignedUserId = 101 },
        new TaskEntity { Id = 2, Title = "New Feature", Description = "Add dark mode", AssignedUserId = 101 },
        new TaskEntity { Id = 3, Title = "Code Review", Description = "Review John's PR", AssignedUserId = 102 }
    };

    [Fact]
    public async Task GetTaskById_Returns_Correct_Task()
    {
        var context = GetInMemoryDbContext();
        var service = new TaskService(context);

        var task = await service.GetTaskById(1);

        Assert.NotNull(task);
        Assert.Equal("Fix Bug", task.Title);
    }

    [Fact]
    public async Task GetTasksByUser_Returns_Only_That_Users_Tasks()
    {
        var context = GetInMemoryDbContext();
        var service = new TaskService(context);

        var tasks = await service.GetTasksByUser(userId: 101);

        Assert.Equal(2, tasks.Count);
        Assert.All(tasks, t => Assert.Contains("Feature", t.Title) || Assert.Contains("Bug", t.Title));
    }

    [Fact]
    public async Task GetTasksByUser_With_Search_Returns_Filtered_Result()
    {
        var context = GetInMemoryDbContext();
        var service = new TaskService(context);

        var tasks = await service.GetTasksByUser(userId: 101, search: "dark");

        Assert.Single(tasks);
        Assert.Contains("dark", tasks[0].Description.ToLower());
    }
}
