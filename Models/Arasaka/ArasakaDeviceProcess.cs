using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CyberpunkPariahNexusApi.Models.Arasaka
{
    public class ArasakaDeviceProcess
    {
        public int id { get; set; }
        public string memory { get; set; }
        public string family { get; set; }
        public string openFiles { get; set; }
        public int deviceId {get;set;}
        [JsonIgnore]
        public ArasakaDevice device {get;set;}
    }
}