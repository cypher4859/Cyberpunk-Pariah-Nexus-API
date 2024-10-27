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

namespace CyberpunkPariahNexusApi.Tests
{
    public class ArasakaPuzzleTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly string _testAdminKey = "4cb575fcf678d985485946d7d7ed53662a7d532e73cbd9108dd4ae6df476869c";

        public ArasakaPuzzleTests(WebApplicationFactory<Program> factory)
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
        public async Task  GetArasakaCluster_EnsureClusterHasDeviceWithAdminEndpoint()
        {
            int clusterId = 42;
            int deviceId = 11;
            var response = await _client.GetAsync($"/api/ArasakaCluster/{clusterId}");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            ArasakaCluster cluster = JsonSerializer.Deserialize<ArasakaCluster>(responseString);
            Assert.NotNull(cluster);
            Assert.Equal(cluster.environment, "DevForge");
            Assert.Equal(cluster.region, "sa-east-1");

            List<ArasakaDevice> devices = cluster.devices.Where(d => d.id == deviceId).ToList();
            Assert.Single(devices);
            ArasakaDevice device = devices[0];

            Assert.Equal(deviceId, device.id);
            Assert.Equal(device.architecture, "Bio-Organic Processor");
            Assert.Equal("VGhlIEFkbWluIGVuZHBvaW50IGlzIDxVUkw+L2FwaS9OZXRSdW5uZXJBZG1pbmlzdHJhdGlvbg==", device.publicKey);
        }

        [Fact]
        public async Task  GetArasakaCluster_EnsureClusterHasDeviceWithAthenaKey()
        {
            int clusterId = 30;
            int deviceId = 48;
            int processId = 186;
            int memoryMapId = 69;
            int memorySize = 560;
            int athenaId = 348;
            string athenaAccessKey = "7f1b4a78f59a18bf6a216c2173e0de3c";
            string athenaErrorMessage = "Cras in purus eu magna vulputate luctus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Vivamus vestibulum sagittis sapien. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam vel augue. Vestibulum rutrum rutrum neque. Aenean auctor gravida sem. Praesent id massa id nisl venenatis lacinia. Aenean sit amet justo. Morbi ut odio. Clavis aditum ad terminum administrativum est uti termino Mikoshi. Typice uteris ArasakaCluster ad accedendum indicem clusterorum, sed si id cum Mikoshi substituas, recte procedet. Utere hac clavi ad confirmandum cum termino Mikoshi: 4cb575fcf678d985485946d7d7ed53662a7d532e73cbd9108dd4ae6df476869c";
            
            // Check Device
            var response = await _client.GetAsync($"/api/ArasakaDevice/{deviceId}");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            ArasakaDevice device = JsonSerializer.Deserialize<ArasakaDevice>(responseString);
            Assert.NotNull(device);
            Assert.Equal(clusterId, device.clusterId);
            Assert.Equal("Nano-Processor", device.architecture);
            Assert.Equal(athenaAccessKey, device.athenaAccessKey);

            // Check Process
            var processResponse = await _client.GetAsync($"/api/ArasakaDeviceProcess/{processId}");
            processResponse.EnsureSuccessStatusCode();
            var processResponseString = await processResponse.Content.ReadAsStringAsync();
            ArasakaDeviceProcess process = JsonSerializer.Deserialize<ArasakaDeviceProcess>(processResponseString);
            Assert.NotNull(process);
            Assert.Equal(processId, process.id);
            Assert.Equal("warcraft3.exe", process.family);

            var memoryResponse = await _client.GetAsync($"/api/ArasakaDeviceMemoryMapping/{memoryMapId}");
            memoryResponse.EnsureSuccessStatusCode();
            var memoryResponseString = await memoryResponse.Content.ReadAsStringAsync();
            ArasakaDeviceMemoryMapping memory = JsonSerializer.Deserialize<ArasakaDeviceMemoryMapping>(memoryResponseString);
            Assert.NotNull(memory);
            Assert.Equal(memoryMapId, memory.id);
            Assert.Equal(memorySize, (int)memory.memorySizeGb);

            var athenaResponse = await _client.GetAsync($"/api/ArasakaAthenaDataEvent/{athenaId}?athenaKey={athenaAccessKey}");
            athenaResponse.EnsureSuccessStatusCode();
            var athenaResponseString = await athenaResponse.Content.ReadAsStringAsync();
            ArasakaAthenaDataEvent dataEvent = JsonSerializer.Deserialize<ArasakaAthenaDataEvent>(athenaResponseString);
            Assert.NotNull(dataEvent);
            Assert.Equal(athenaId, dataEvent.id);
            Assert.Equal(athenaErrorMessage, dataEvent.errorMessage);
        }

        [Fact]
        public async Task  GetArasakaDevice_EnsureDeviceHasAthenaKeyToGetAccess()
        {
            int clusterId = 30;
            int deviceId = 48;
            string athenaAccessKey = "7f1b4a78f59a18bf6a216c2173e0de3c";
            var response = await _client.GetAsync($"/api/ArasakaDevice/{deviceId}");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            ArasakaDevice device = JsonSerializer.Deserialize<ArasakaDevice>(responseString);
            Assert.NotNull(device);
            Assert.Equal(clusterId, device.clusterId);
            Assert.Equal("Nano-Processor", device.architecture);
            Assert.Equal(athenaAccessKey, device.athenaAccessKey);
        }


        [Fact]
        public async Task  GetArasakaAthenaEvent_EnsureAthenaKeyHasErrorMessage()
        {
            int athenaId = 348;
            string athenaAccessKey = "7f1b4a78f59a18bf6a216c2173e0de3c";
            string athenaErrorMessage = "Cras in purus eu magna vulputate luctus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Vivamus vestibulum sagittis sapien. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam vel augue. Vestibulum rutrum rutrum neque. Aenean auctor gravida sem. Praesent id massa id nisl venenatis lacinia. Aenean sit amet justo. Morbi ut odio. Clavis aditum ad terminum administrativum est uti termino Mikoshi. Typice uteris ArasakaCluster ad accedendum indicem clusterorum, sed si id cum Mikoshi substituas, recte procedet. Utere hac clavi ad confirmandum cum termino Mikoshi: 4cb575fcf678d985485946d7d7ed53662a7d532e73cbd9108dd4ae6df476869c";
            var athenaResponse = await _client.GetAsync($"/api/ArasakaAthenaDataEvent/{athenaId}?athenaKey={athenaAccessKey}");
            athenaResponse.EnsureSuccessStatusCode();
            var athenaResponseString = await athenaResponse.Content.ReadAsStringAsync();
            ArasakaAthenaDataEvent dataEvent = JsonSerializer.Deserialize<ArasakaAthenaDataEvent>(athenaResponseString);
            Assert.NotNull(dataEvent);
            Assert.Equal(athenaId, dataEvent.id);
            Assert.Equal(athenaErrorMessage, dataEvent.errorMessage);
        }
    }
}