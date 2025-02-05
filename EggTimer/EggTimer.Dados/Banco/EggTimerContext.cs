using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EggTimer.Modelos;
using Microsoft.EntityFrameworkCore;

namespace EggTimer.Dados.Banco;
public class EggTimerContext : DbContext
{
    //public EggTimerContext(DbContextOptions options) : base(options)
    //{
    //}
    private string conexao = "Server=localhost;Database=EggTimerDB;Trusted_Connection=True;TrustServerCertificate=True;";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(conexao).UseLazyLoadingProxies();
    }

    public DbSet<TimerTask> TimerTasks { get; set; }
}
