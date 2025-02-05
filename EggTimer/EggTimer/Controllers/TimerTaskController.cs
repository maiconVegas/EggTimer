using System.Reflection.Metadata.Ecma335;
using EggTimer.API.Requests;
using EggTimer.API.Responses;
using EggTimer.Dados.Banco;
using EggTimer.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        return new TimerTaskResponse(task.NomeTarefa, task.TempoCronometrado, task.Status);
    }
    private string AtualizarStatus(TimerTask task)
    {
        var horarioAtual = TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss"));
        return horarioAtual < task.HorarioFim ? "Em Andamento" : "Concluído";
    }

    [HttpPost]
    public IActionResult AdicionarTimerTask([FromServices] DAL<TimerTask> dal, [FromBody]TimerTaskRequest taskRequest)
    {
        var task = new TimerTask(taskRequest.Nome, taskRequest.HorarioCronometrado, taskRequest.Status);
        dal.Adicionar(task);
        return CreatedAtAction(nameof(RecuperarTimerTaskPorId), new { id = task.Id}, task);
    }

    [HttpGet]
    public IActionResult ListarTimerTask([FromServices] DAL<TimerTask> dal)
    {
        return Ok(EntityListToResponseList(dal.Listar()));
    }

    [HttpGet("id/{id}")]
    public IActionResult RecuperarTimerTaskPorId([FromServices] DAL<TimerTask> dal, int id)
    {
        var task = dal.RecuperarPor(task => task.Id == id);
        if (task is null)
        {
            return NotFound();
        }
        return Ok(EntityToResponse(task));
    }

    [HttpGet("nome/{nome}")]
    public IActionResult RecuperarTimerTaskPorNome([FromServices] DAL<TimerTask> dal, string nome)
    {
        var task = dal.RecuperarPor(t => t.NomeTarefa.ToUpper().Equals(nome.ToUpper()));
        if (task is null)
        {
            return NotFound();
        }
        return Ok(EntityToResponse(task));
    }

    [HttpDelete("{id}")]
    public IActionResult DeletarTimerTask([FromServices] DAL<TimerTask> dal, int id)
    {
        var task = dal.RecuperarPor(filme => filme.Id == id);
        if (task is null) return NotFound();
        dal.Deletar(task);
        return NoContent();
    }

    [HttpPut("{id}")]
    public IActionResult? AtualizarTimerTask([FromServices] DAL<TimerTask> dal, [FromBody] TimerTaskRequestEdit tarefaRequest, int id)
    {
        var task = dal.RecuperarPor(a => a.Id == id);
        if (task is null)
        {
            return NotFound();
        }

        //var taskUp = new TimerTask(task.NomeTarefa, task.TempoCronometrado, task.Status) { Id = id};

        task.NomeTarefa = tarefaRequest.Nome;
        task.Data = DateOnly.FromDateTime(DateTime.Now);
        task.TempoCronometrado = tarefaRequest.HorarioCronometrado;
        task.HorarioInicio = TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss"));
        task.HorarioFim = task.HorarioInicio.Add(tarefaRequest.HorarioCronometrado);
        //task.Status = tarefaRequest.Status;
        task.Status = AtualizarStatus(task);
        dal.Atualizar(task);
        return Ok();
    }
    //[HttpPut]
    //public IActionResult? AtualizarTimerTask([FromServices] DAL<TimerTask> dal, [FromBody] TimerTaskRequestEdit artistaRequest)
    //{
    //    var task = dal.RecuperarPor(a => a.Id.Equals(artistaRequest.id));
    //    if (task is null)
    //    {
    //        return NotFound();
    //    }

    //    task.NomeTarefa = artistaRequest.Nome;
    //    task.TempoCronometrado = artistaRequest.HorarioCronometrado;
    //    task.Status = artistaRequest.Status;
    //    //artistaAtualizar.FotoPerfil = artista.FotoPerfil;

    //    dal.Atualizar(task);
    //    return Ok();
    //}
}
