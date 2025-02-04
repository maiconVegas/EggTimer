namespace EggTimer.Modelos;
public class TimerTask
{
    public int Id { get; set; }
    public DateOnly Data { get; set; }
    public TimeSpan HorarioInicio { get; set; }
    public TimeSpan HorarioFim { get; set; }
    public string NomeTarefa { get; set; }
    public TimeSpan TempoCronometrado { get; set; }
    public string Status { get; set; }
}
