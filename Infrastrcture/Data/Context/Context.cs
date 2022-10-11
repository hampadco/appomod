using Microsoft.EntityFrameworkCore;
public class Context:DbContext
{
       public Context(DbContextOptions<Context> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<MainQuestion> MainQuestions { get; set; }
    public DbSet<Answer> answers { get; set; }
    public DbSet<Points> Pointes { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Walet> walets { get; set; }
    public DbSet<SetDb> Sets { get; set; }
    
    // protected override void OnConfiguring(DbContextOptionsBuilder db)
    // {
    //     db.UseSqlServer("data source=.;initial catalog = OmidApp;integrated security=true");
    // }
}