﻿// <auto-generated />
using CyberpunkPariahNexusApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CyberpunkPariahNexusApi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20241017213655_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("CyberpunkPariahNexusApi.Models.Arasaka.ArasakaAthenaDataEvent", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("appName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("appVersion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("deviceBrand")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("deviceId")
                        .HasColumnType("int");

                    b.Property<string>("deviceModel")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("errorCode")
                        .HasColumnType("int");

                    b.Property<string>("errorMessage")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("eventTimestamp")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("eventType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ipAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("location")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("macAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("osVersion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("responseTime")
                        .HasColumnType("int");

                    b.Property<string>("severity")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("source")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("success")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("userAgent")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("deviceId");

                    b.ToTable("arasakaDataEvents");
                });

            modelBuilder.Entity("CyberpunkPariahNexusApi.Models.Arasaka.ArasakaCluster", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("clusterName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("cpuCores")
                        .HasColumnType("int");

                    b.Property<string>("creationDate")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("environment")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("kubernetesVersion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("memoryGb")
                        .HasColumnType("int");

                    b.Property<int>("nodeCount")
                        .HasColumnType("int");

                    b.Property<string>("region")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("storageTb")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("arasakaClusters");
                });

            modelBuilder.Entity("CyberpunkPariahNexusApi.Models.Arasaka.ArasakaDevice", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("architecture")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("athenaAccessKey")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("clusterId")
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("processorType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("region")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.HasIndex("clusterId");

                    b.ToTable("arasakaDevices");
                });

            modelBuilder.Entity("CyberpunkPariahNexusApi.Models.Arasaka.ArasakaDeviceMemoryMapping", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("deviceId")
                        .HasColumnType("int");

                    b.Property<int>("memoryEccSupport")
                        .HasColumnType("int");

                    b.Property<string>("memoryFormFactor")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("memoryHeatSpreader")
                        .HasColumnType("int");

                    b.Property<int>("memoryLatency")
                        .HasColumnType("int");

                    b.Property<float>("memorySizeGb")
                        .HasColumnType("float");

                    b.Property<int>("memorySpeedMhz")
                        .HasColumnType("int");

                    b.Property<string>("memoryTechnology")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("memoryType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<float>("memoryVoltage")
                        .HasColumnType("float");

                    b.Property<int>("memoryWarrantyYears")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("deviceId");

                    b.ToTable("arasakaDevicesMemoryMappings");
                });

            modelBuilder.Entity("CyberpunkPariahNexusApi.Models.Arasaka.ArasakaDeviceProcess", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("deviceId")
                        .HasColumnType("int");

                    b.Property<string>("family")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("memory")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("openFiles")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.HasIndex("deviceId");

                    b.ToTable("arasakaDeviceProcesses");
                });

            modelBuilder.Entity("CyberpunkPariahNexusApi.Models.Arasaka.ArasakaAthenaDataEvent", b =>
                {
                    b.HasOne("CyberpunkPariahNexusApi.Models.Arasaka.ArasakaDevice", "device")
                        .WithMany("dataEvents")
                        .HasForeignKey("deviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("device");
                });

            modelBuilder.Entity("CyberpunkPariahNexusApi.Models.Arasaka.ArasakaDevice", b =>
                {
                    b.HasOne("CyberpunkPariahNexusApi.Models.Arasaka.ArasakaCluster", "cluster")
                        .WithMany("devices")
                        .HasForeignKey("clusterId");

                    b.Navigation("cluster");
                });

            modelBuilder.Entity("CyberpunkPariahNexusApi.Models.Arasaka.ArasakaDeviceMemoryMapping", b =>
                {
                    b.HasOne("CyberpunkPariahNexusApi.Models.Arasaka.ArasakaDevice", "device")
                        .WithMany("memoryMappings")
                        .HasForeignKey("deviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("device");
                });

            modelBuilder.Entity("CyberpunkPariahNexusApi.Models.Arasaka.ArasakaDeviceProcess", b =>
                {
                    b.HasOne("CyberpunkPariahNexusApi.Models.Arasaka.ArasakaDevice", "device")
                        .WithMany("processes")
                        .HasForeignKey("deviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("device");
                });

            modelBuilder.Entity("CyberpunkPariahNexusApi.Models.Arasaka.ArasakaCluster", b =>
                {
                    b.Navigation("devices");
                });

            modelBuilder.Entity("CyberpunkPariahNexusApi.Models.Arasaka.ArasakaDevice", b =>
                {
                    b.Navigation("dataEvents");

                    b.Navigation("memoryMappings");

                    b.Navigation("processes");
                });
#pragma warning restore 612, 618
        }
    }
}
