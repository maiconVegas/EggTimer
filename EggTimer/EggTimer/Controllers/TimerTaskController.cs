using System.Reflection.Metadata.Ecma335;
using EggTimer.API.Requests;
using EggTimer.API.Responses;
using EggTimer.Modelos;
using Microsoft.AspNetCore.Mvc;
namespace EggTimer.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TimerTaskController : ControllerBase
{
    private static List<TimerTask> _tasks = new List<TimerTask>();
    private static IEnumerable<TimerTaskResponse> EntityListToResponseList(IEnumerable<TimerTask> listaDeTasks)
    {
        return listaDeTasks.Select(a => EntityToResponse(a)).ToList();
    }
    private static TimerTaskResponse EntityToResponse(TimerTask task)
    {
        return new TimerTaskResponse(task.NomeTarefa, task.Status);
    }

    [HttpPost]
    public IActionResult AdicionarTimerTask([FromBody]TimerTaskRequest taskRequest)
    {
        var task = new TimerTask(taskRequest.Nome, taskRequest.HorarioCronometrado, taskRequest.Status);
        _tasks.Add(task);
        return CreatedAtAction(nameof(RecuperarTimerTaskPorId), new { id = task.Id}, task);
    }

    [HttpGet]
    public IActionResult ListarTimerTask()
    {
        return Ok(EntityListToResponseList(_tasks));
    }

    [HttpGet("id/{id}")]
    public IActionResult RecuperarTimerTaskPorId(int id)
    {
        var task = _tasks.FirstOrDefault(task => task.Id == id);
        if (task is null)
        {
            return NotFound();
        }
        return Ok(EntityToResponse(task));
    }

    [HttpGet("nome/{nome}")]
    public IActionResult RecuperarTimerTaskPorNome(string nome)
    {
        var task = _tasks.FirstOrDefault(t => t.NomeTarefa.ToUpper().Equals(nome.ToUpper()));
        if (task is null)
        {
            return NotFound();
        }
        return Ok(EntityToResponse(task));
    }

}
