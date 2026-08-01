using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.API.Data;
using TaskFlow.API.DTOs;
using TaskFlow.API.Models;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly TaskFlowDbContext _context;

    public TasksController(TaskFlowDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<IActionResult> GetTasks()
    {
        return Ok(await _context.Tasks.ToListAsync());
    }


    [HttpPost]
    public async Task<IActionResult> CreateTask(CreateTaskDto dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
           Completed = false
        };

        _context.Tasks.Add(task);

        await _context.SaveChangesAsync();

        return Ok(task);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, UpdateTaskDto dto)
    {
        var task = await _context.Tasks.FindAsync(id);

        if(task == null)
            return NotFound();


        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Completed = dto.Completed;


        await _context.SaveChangesAsync();

        return Ok(task);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if(task == null)
            return NotFound();


        _context.Tasks.Remove(task);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}