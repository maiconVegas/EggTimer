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
    public EggTimerContext(DbContextOptions<EggTimerContext> options)
            : base(options)
    {
    }
    private string conexao = "Server=localhost;Database=EggTimerDB;Trusted_Connection=True;TrustServerCertificate=True;";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) // Garante que a configuração não será sobrescrita pelo AddDbContext
        {
            optionsBuilder.UseSqlServer(conexao).UseLazyLoadingProxies();
        }
        //optionsBuilder.UseSqlServer(conexao).UseLazyLoadingProxies();
    }

    public DbSet<TimerTask> TimerTasks { get; set; }
}
