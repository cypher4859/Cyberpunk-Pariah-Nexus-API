using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CyberpunkPariahNexusApi.Helpers
{
    public static class AuthorizationService
    {
        public static bool HandleAdminAuthorization(string apiKey) {
            string adminApiKey = "4cb575fcf678d985485946d7d7ed53662a7d532e73cbd9108dd4ae6df476869c";
            return apiKey == adminApiKey;
        }

        public static bool HandleAthenaAuthorization(string apiKey) {
            string athenaApiKey = "7f1b4a78f59a18bf6a216c2173e0de3c";
            
            return athenaApiKey == apiKey;
        }
    }
}