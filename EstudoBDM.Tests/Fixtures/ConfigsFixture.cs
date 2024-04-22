using EstudoBDM.Configs;
using EstudoBDM.Infraestructure;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;


namespace EstudoBDM.Tests.Fixtures
{
    public interface IConfigsFixture : IDisposable
    {
        public IUnitOfWork Uof { get; }
    }

    public class ConfigsFixture : IConfigsFixture
    {
        public IUnitOfWork Uof { get; set; }
        public ConfigsFixture()
        {
            var services = new ServiceCollection();

            using (var file = File.OpenText("Properties\\launchSettings.json"))
            {
                var reader = new JsonTextReader(file);
                var jObject = JObject.Load(reader);

                var variables = jObject
                    .GetValue("profiles")!
                    .SelectMany(profiles => profiles.Children())
                    .SelectMany(profile => profile.Children<JProperty>())
                    .Where(prop => prop.Name == "environmentVariables")
                    .SelectMany(prop => prop.Value.Children<JProperty>())
                    .ToList();

                foreach (var variable in variables)
                {
                    Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
                }
            }

            services.AddDbContext<DatabaseConnection>(options =>
            {
                string connectionString = $"Server={Environment.GetEnvironmentVariable("Server")};" +
                          $"Database={Environment.GetEnvironmentVariable("Database")};" +
                          $"User={Environment.GetEnvironmentVariable("User")};" +
                          $"Password={Environment.GetEnvironmentVariable("Password")};";

                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            var serviceProvider = services.BuildServiceProvider();

            Uof = serviceProvider.GetService<IUnitOfWork>()!;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this); // https://learn.microsoft.com/pt-br/dotnet/fundamentals/code-analysis/quality-rules/ca1816
        }
    }
}
