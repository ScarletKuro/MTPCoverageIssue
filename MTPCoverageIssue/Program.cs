using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MTPCoverageIssue.Database;
using MTPCoverageIssue.Mapping;

namespace MTPCoverageIssue;

[ExcludeFromCodeCoverage]
public static class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddDbContext<InMemoryDbContext>(options =>
            options.UseInMemoryDatabase("MTPCoverage"));

        builder.Services.AddAutoMapper(typeof(MappingProfile));
        builder.Services.AddScoped<StatisticService>();

        var app = builder.Build();

        app.Run();
    }
}