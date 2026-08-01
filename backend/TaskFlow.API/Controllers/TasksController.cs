using Microsoft.AspNetCore.Mvc;
using TaskFlow.API.Models;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private static readonly List<TaskItem> tasks = new();


    [HttpGet]
    public IActionResult GetTasks()
    {
        return Ok(tasks);
    }


    [HttpPost]
    public IActionResult CreateTask(TaskItem task)
    {
        task.Id = tasks.Count + 1;

        tasks.Add(task);

        return Ok(task);
    }


    [HttpPut("{id}")]
    public IActionResult UpdateTask(int id, TaskItem updatedTask)
    {
        var task = tasks.FirstOrDefault(x => x.Id == id);

        if(task == null)
            return NotFound();


        task.Title = updatedTask.Title;
        task.Description = updatedTask.Description;
        task.Completed = updatedTask.Completed;


        return Ok(task);
    }


    [HttpDelete("{id}")]
    public IActionResult DeleteTask(int id)
    {
        var task = tasks.FirstOrDefault(x => x.Id == id);

        if(task == null)
            return NotFound();


        tasks.Remove(task);

        return NoContent();
    }
}