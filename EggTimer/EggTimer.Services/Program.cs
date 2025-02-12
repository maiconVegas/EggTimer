using EggTimer.Dados.Banco;
using EggTimer.Services.Services;


//using (var contexto = new EggTimerContext())
//{
//    //tenta refatorar de acordo com o metodo exercutar do timertaskservice, onde que deixar o context
//    // no metodo e aqui so executar o metodo da classe sem usar o context, pois esta sendo usado pelo metodo
//    // O intuito disso é para nao repetir essa declaração e tambem para deixar codigo mais limpo
//    var service = new TimerTaskService(contexto);
//    service.Executar();
//}

var service = new TimerTaskService();
service.Executar();