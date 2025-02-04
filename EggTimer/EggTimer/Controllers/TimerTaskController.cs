using EggTimer.API.Responses;
using EggTimer.Modelos;
using Microsoft.AspNetCore.Mvc;
namespace EggTimer.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TimerTaskController : ControllerBase
{
    private static List<TimerTask> _tasks = new List<TimerTask>();
    private static ICollection<TimerTaskResponse> EntityListToResponseList(IEnumerable<TimerTask> listaDeTasks)
    {
        return listaDeTasks.Select(a => EntityToResponse(a)).ToList();
    }
    private static TimerTaskResponse EntityToResponse(TimerTask task)
    {
        return new TimerTaskResponse(task.NomeTarefa, task.Status);
    }
    [HttpPost]
    public void AdicionarTimerTask([FromBody]TimerTask task)
    {
        _tasks.Add(task);
        Console.WriteLine(task.Data);
    }

    [HttpGet]
    public IActionResult ListarTimerTask()
    {
        return Ok(EntityListToResponseList(_tasks));
    }
}
