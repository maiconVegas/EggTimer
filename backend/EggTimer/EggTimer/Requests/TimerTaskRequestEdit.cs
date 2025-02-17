namespace EggTimer.API.Requests;

//public record TimerTaskRequestEdit(int id, string Nome, TimeSpan HorarioCronometrado, string Status) :
//    TimerTaskRequest(Nome, HorarioCronometrado, Status);
//public record TimerTaskRequestEdit(string Nome, TimeSpan HorarioCronometrado, string Status) :
//    TimerTaskRequest(Nome, HorarioCronometrado, Status);

public record TimerTaskRequestEdit(string Nome, TimeSpan HorarioCronometrado);
