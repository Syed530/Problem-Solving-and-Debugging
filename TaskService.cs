# Problem-Solving-and-Debugging
Assessment
# Task Service Implementation and Unit Tests

## ✅ TaskService.cs

```csharp
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class TaskService
{
    private readonly DbContext _dbContext;

    public TaskService(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TaskEntity> GetTaskById(int id)
    {
        return await _dbContext.Set<TaskEntity>().FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<TaskDto>> GetTasksByUser(int userId, int page = 1, int pageSize = 10, string search = null)
    {
        var query = _dbContext.Set<TaskEntity>().AsQueryable()
            .Where(t => t.AssignedUserId == userId);

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(t => t.Title.Contains(search) || t.Description.Contains(search));
        }

        return await query
            .OrderBy(t => t.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description
            })
            .ToListAsync();
    }
}
