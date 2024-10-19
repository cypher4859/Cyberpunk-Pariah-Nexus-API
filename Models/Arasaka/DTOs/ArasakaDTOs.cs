using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CyberpunkPariahNexusApi.Models.Arasaka.DTOs
{
    // ArasakaClusterDto
    public class ArasakaClusterDto
    {
        public string clusterName { get; set; }
        public int nodeCount { get; set; }
        public int cpuCores { get; set; }
        public int memoryGb { get; set; }
        public int storageTb { get; set; }
        public string creationDate { get; set; }
        public string environment { get; set; }
        public string kubernetesVersion { get; set; }
        public string region { get; set; }
        public ICollection<ArasakaDeviceDto>? devices { get; set; }
    }

    // ArasakaDeviceDto
    public class ArasakaDeviceDto
    {
        public string name { get; set; }
        public string architecture { get; set; }
        public string processorType { get; set; }
        public string region { get; set; }
        public string athenaAccessKey { get; set; }
        public int clusterId { get; set; }  // Foreign key, no cluster navigation property in DTO
        public ICollection<ArasakaDeviceProcessDto>? processes { get; set; }
        public ICollection<ArasakaDeviceMemoryMappingDto>? memoryMappings { get; set; }
        public ICollection<ArasakaAthenaDataEventDto>? dataEvents { get; set; }
    }

    // ArasakaDeviceProcessDto
    public class ArasakaDeviceProcessDto
    {
        public string memory { get; set; }
        public string family { get; set; }
        public string openFiles { get; set; }
        public int deviceId { get; set; }  // Foreign key, no device navigation property in DTO
    }

    // ArasakaDeviceMemoryMappingDto
    public class ArasakaDeviceMemoryMappingDto
    {
        public string memoryType { get; set; }
        public float memorySizeGb { get; set; }
        public int memorySpeedMhz { get; set; }
        public string memoryTechnology { get; set; }
        public int memoryLatency { get; set; }
        public float memoryVoltage { get; set; }
        public string memoryFormFactor { get; set; }
        public int memoryEccSupport { get; set; }
        public int memoryHeatSpreader { get; set; }
        public int memoryWarrantyYears { get; set; }
        public int deviceId { get; set; }  // Foreign key, no device navigation property in DTO
    }

    // ArasakaAthenaDataEventDto
    public class ArasakaAthenaDataEventDto
    {
        public int userId { get; set; }
        public string ipAddress { get; set; }
        public string macAddress { get; set; }
        public string eventTimestamp { get; set; }
        public string eventType { get; set; }
        public string source { get; set; }
        public string severity { get; set; }
        public string location { get; set; }
        public string userAgent { get; set; }
        public string deviceBrand { get; set; }
        public string deviceModel { get; set; }
        public string osVersion { get; set; }
        public string appName { get; set; }
        public string appVersion { get; set; }
        public int errorCode { get; set; }
        public string errorMessage { get; set; }
        public int responseTime { get; set; }
        public bool success { get; set; }
        public int deviceId { get; set; }  // Foreign key, no device navigation property in DTO
    }
}