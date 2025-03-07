﻿using System.ComponentModel.DataAnnotations;

namespace EggTimer.Modelos;
public class TimerTask
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required(ErrorMessage = "Nome da Tarefa é obrigatória")]
    [MaxLength(100, ErrorMessage = "O tamanho do Nome da Tarefa não pode exceder 100 caracteres")]
    public string NomeTarefa { get; set; }
    public DateOnly Data { get; set; }
    public TimeSpan HorarioInicio { get; set; }
    public TimeSpan HorarioFim { get; set; }
    [Required(ErrorMessage = "Tempo Cronometrado é obrigatório")]
    public TimeSpan TempoCronometrado { get; set; }
    [Required(ErrorMessage = "Status é requerido")]
    public string Status { get; set; }

    public TimerTask()
    {
        
    }
    public TimerTask(string nome, TimeSpan tempo, string status)
    {
        this.NomeTarefa = nome;
        this.Data = DateOnly.FromDateTime(DateTime.Now);
        this.TempoCronometrado = tempo;
        this.Status = status;
        this.HorarioInicio = TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss"));
        this.HorarioFim = HorarioInicio.Add(tempo);
    }
}
