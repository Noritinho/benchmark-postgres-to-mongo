using Microsoft.EntityFrameworkCore;

namespace Benchmark;

public class BenchmarkDbContextPg(DbContextOptions<BenchmarkDbContextPg> options) : DbContext(options)
{
    public DbSet<PersonPg> Person { get; set; }
    public DbSet<NamePg> Names { get; set; }
}

public class BenchmarkDbContextMg(DbContextOptions<BenchmarkDbContextMg> options) : DbContext(options)
{
    public DbSet<PersonMg> Person { get; set; }
    public DbSet<NameMg> Names { get; set; }
}