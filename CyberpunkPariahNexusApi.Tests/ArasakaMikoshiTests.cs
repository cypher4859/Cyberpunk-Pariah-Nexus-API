using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CyberpunkPariahNexusApi;
using CyberpunkPariahNexusApi.Models;
using CyberpunkPariahNexusApi.Models.Arasaka;
using CyberpunkPariahNexusApi.Models.Arasaka.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class MikoshiControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;
    private readonly string _testAdminKey = "4cb575fcf678d985485946d7d7ed53662a7d532e73cbd9108dd4ae6df476869c";

    public MikoshiControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            // builder.ConfigureAppConfiguration((context, config) =>
            // {
            //     // Load settings from the docker-compose environment
            //     config.AddEnvironmentVariables();
            // });

            builder.ConfigureServices(services =>
            {
                // Replace the DbContext configuration for testing purposes
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<DataContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<DataContext>(options =>
                {
                    var connectionString = "Server=pariah-nexus-db;Database=pariahnexus;User=arasakaOperator;Password=arasakaOperator123;";
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                });
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetMikoshi_ById_Returns_Ok()
    {
        // Arrange
        string key = "4cb575fcf678d985485946d7d7ed53662a7d532e73cbd9108dd4ae6df476869c"; // Set the ID you expect in your in-memory DB for testing purposes

        // Act
        var response = await _client.GetAsync($"/api/Mikoshi?adminKey={key}");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseString);
        Assert.Equal("You won! Great Job Net Runner!", responseString);
    }

}
