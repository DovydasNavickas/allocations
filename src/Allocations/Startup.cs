using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Allocations
{
    class Test
    {
        public string Id { get; set; }
    }
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseIISPlatformHandler();

            app.Run(async (context) =>
            {
                Func<Test, Task> action = async x =>
                {
                    await Task.Delay(TimeSpan.FromTicks(10));
                };
                const int numberOfTestObjects = 1000000;
                var tests = Enumerable.Range(0, numberOfTestObjects).Select(x => new Test { Id = Guid.NewGuid().ToString() }).ToList();

                foreach (var test in tests)
                {
                    var controlUnit = test;
                }

                var s1 = new Stopwatch();
                s1.Start();
                foreach (var test in tests)
                {
                    await action(test);
                }
                s1.Stop();

                var s2 = new Stopwatch();
                s2.Start();

                await Task.WhenAll(tests.Select(action));

                s2.Stop();

                var s3 = new Stopwatch();
                s3.Start();
                var tasks = new List<Task>(numberOfTestObjects);
                foreach (var test in tests)
                {
                    tasks.Add(action(test));
                }
                await Task.WhenAll(tasks);
                s3.Stop();

                var nl = Environment.NewLine;
                var statistics = new Dictionary<string, Stopwatch>()
                {
                    ["No allocations. Loop through objects with constant time async function"] = s1,
                    ["Allocate list via Linq. Select tasks of constant time async function and await WhenAll"] = s2,
                    ["Allocate list manually. Add tasks to list manually and await WhenAll"] = s3
                }
                .Select(x => $"{x.Key}{nl}{x.Value.ElapsedTicks} ticks ({x.Value.ElapsedMilliseconds} ms)")
                .Aggregate((i, j) => $"{nl}{i}{nl}{nl}{j}");
                await context.Response.WriteAsync(statistics);
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
