using BenchmarkDotNet.Running;
using Microsoft.EntityFrameworkCore;
using TextShareApi.Benchmarks.Benchmarks;
using TextShareApi.Benchmarks.Contexts;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PostgresContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
var app = builder.Build();

var benchmarkConfig = builder.Configuration.GetSection("BenchmarkConfig");

if (benchmarkConfig.GetValue<bool>("RunBenchmarks")) {
    BenchmarkRunner.Run<TextsLoadBenchmark>();
}

// app.Run();