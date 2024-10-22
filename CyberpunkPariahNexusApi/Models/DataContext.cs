using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CyberpunkPariahNexusApi.Models.Arasaka;
using Microsoft.EntityFrameworkCore;

namespace CyberpunkPariahNexusApi.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {

        }

        public DbSet<ArasakaCluster> arasakaClusters {get;set;} = null!;
        public DbSet<ArasakaDevice> arasakaDevices {get;set;} = null!;
        public DbSet<ArasakaDeviceMemoryMapping> arasakaDevicesMemoryMappings {get;set;} = null!;
        public DbSet<ArasakaDeviceProcess> arasakaDeviceProcesses {get;set;} = null!;
        public DbSet<ArasakaAthenaDataEvent> arasakaDataEvents {get;set;} = null!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<ArasakaCluster>()
                .HasMany(cluster => cluster.devices)
                .WithOne(device => device.cluster)
                .HasForeignKey(device => device.clusterId)
                .HasPrincipalKey(cluster => cluster.id)
                .IsRequired(false);

            modelBuilder.Entity<ArasakaDevice>()
                .HasMany(device => device.processes)
                .WithOne(process => process.device)
                .HasForeignKey(process => process.deviceId)
                .HasPrincipalKey(device => device.id)
                .IsRequired(true);

            modelBuilder.Entity<ArasakaDevice>()
                .HasMany(device => device.memoryMappings)
                .WithOne(memoryMap => memoryMap.device)
                .HasForeignKey(memoryMap => memoryMap.deviceId)
                .HasPrincipalKey(device => device.id)
                .IsRequired(true);

            modelBuilder.Entity<ArasakaAthenaDataEvent>()
                .HasOne(athenaEvent => athenaEvent.device)
                .WithMany(device => device.dataEvents)
                .HasForeignKey(dataEvent => dataEvent.deviceId)
                .HasPrincipalKey(device => device.id)
                .IsRequired(true);
                
        }
    }

}