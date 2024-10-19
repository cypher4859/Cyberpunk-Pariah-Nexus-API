using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CyberpunkPariahNexusApi.Models.Arasaka
{
    public class ArasakaAthenaDataEvent
    {
        public int id {get;set;}
        public int userId { get; set; }
        public string ipAddress { get; set; }
        public string macAddress {get;set;}
        public string eventTimestamp {get;set;}
        public string eventType {get;set;}
        public string source {get;set;}
        public string severity {get;set;}
        public string location {get;set;}
        public string userAgent {get;set;}
        public string deviceBrand {get;set;}
        public string deviceModel {get;set;}
        public string osVersion {get;set;}
        public string appName {get;set;}
        public string appVersion {get;set;}
        public int errorCode {get;set;}
        public string errorMessage {get;set;}
        public int responseTime {get;set;}
        public bool success {get;set;}
        public int deviceId {get;set;}
        [JsonIgnore]
        public ArasakaDevice device {get;set;}

    }
}