using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CyberpunkPariahNexusApi.Models.Arasaka
{
    public class ArasakaDevice
    {
        public int id { get; set; }
        public string name { get; set; }
        public string architecture { get; set; }
        public string processorType {get;set;}
        public string region { get; set; }
        public string athenaAccessKey {get;set;}
        public int clusterId {get;set;} // Used to link up the ArasakaCluster
        [JsonIgnore]
        public ArasakaCluster cluster {get;set;}
        public ICollection<ArasakaDeviceProcess>? processes {get; set;}
        public ICollection<ArasakaDeviceMemoryMapping>? memoryMappings {get; set;}
        public ICollection<ArasakaAthenaDataEvent>? dataEvents {get; set;}
    }
}