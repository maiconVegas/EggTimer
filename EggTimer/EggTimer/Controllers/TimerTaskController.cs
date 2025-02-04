using EggTimer.Modelos;
using Microsoft.AspNetCore.Mvc;
namespace EggTimer.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TimerTaskController : ControllerBase
{
    private static List<TimerTask> _tasks = new List<TimerTask>();

    [HttpPost]
    public void AdicionarTimerTask([FromBody]TimerTask task)
    {
        _tasks.Add(task);
        Console.WriteLine(task.Data);
    }

    [HttpGet]
    public List<TimerTask> ListarTimerTask()
    {
        return _tasks;
    }
}
