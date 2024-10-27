## Documentation for Arasaka Models in CyberpunkPariahNexusApi

This document provides an overview and explanation of the main models used in the `CyberpunkPariahNexusApi`, specifically focusing on **Arasaka** related models and how they are configured within the **DataContext** class for Entity Framework Core.

### 1. DataContext

The **`DataContext`** class is the main entry point for configuring **Entity Framework Core** to interact with the database. It defines **DbSet** properties for each model, allowing the application to work with the corresponding database tables.

```csharp
public DbSet<ArasakaCluster> arasakaClusters { get; set; }
public DbSet<ArasakaDevice> arasakaDevices { get; set; }
public DbSet<ArasakaDeviceMemoryMapping> arasakaDevicesMemoryMappings { get; set; }
public DbSet<ArasakaDeviceProcess> arasakaDeviceProcesses { get; set; }
public DbSet<ArasakaAthenaDataEvent> arasakaDataEvents { get; set; }
public DbSet<NetRunnerAdministration> netRunnerAdministrations { get; set; }
```

- The `OnModelCreating` method configures **relationships** between models:
  - **One-to-Many** relationships between `ArasakaCluster` and `ArasakaDevice`, `ArasakaDevice` and `ArasakaDeviceProcess`, `ArasakaDevice` and `ArasakaDeviceMemoryMapping`, and `ArasakaDevice` and `ArasakaAthenaDataEvent`.
  - Configurations include **foreign keys**, **principal keys**, and whether a relationship is **required** or optional.

### 2. ArasakaCluster

The **`ArasakaCluster`** model represents a cluster of devices in the system.

```csharp
public int id { get; set; }
public string clusterName { get; set; }
public int nodeCount { get; set; }
public int cpuCores { get; set; }
public int memoryGb { get; set; }
public int storageTb { get; set; }
public string creationDate { get; set; }
public string environment { get; set; }
public string kubernetesVersion { get; set; }
public string region { get; set; }
public ICollection<ArasakaDevice> devices { get; set; }
```

- The `ArasakaCluster` contains metadata about the cluster, including **hardware specifications** and **environment details**.
- It has a **collection of devices** (`ICollection<ArasakaDevice>`), representing the devices in this cluster.

### 3. ArasakaDevice

The **`ArasakaDevice`** model represents a physical or virtual device within a cluster.

```csharp
public int id { get; set; }
public string name { get; set; }
public string publicKey { get; set; }
public string architecture { get; set; }
public string processorType { get; set; }
public string region { get; set; }
public string athenaAccessKey { get; set; }
public int clusterId { get; set; }
[JsonIgnore] public ArasakaCluster cluster { get; set; }
public ICollection<ArasakaDeviceProcess>? processes { get; set; }
public ICollection<ArasakaDeviceMemoryMapping>? memoryMappings { get; set; }
public ICollection<ArasakaAthenaDataEvent>? dataEvents { get; set; }
```

- An `ArasakaDevice` belongs to a **cluster** (`ArasakaCluster`) and may have multiple **processes**, **memory mappings**, and **data events**.
- Relationships to `ArasakaCluster` and other child models are configured with `[JsonIgnore]` to avoid circular references during serialization.

### 4. ArasakaDeviceProcess

The **`ArasakaDeviceProcess`** model represents a running process on a device.

```csharp
public int id { get; set; }
public string memory { get; set; }
public string family { get; set; }
public string openFiles { get; set; }
public int deviceId { get; set; }
[JsonIgnore] public ArasakaDevice device { get; set; }
```

- Contains information such as **memory usage**, **family**, and **open files** for a running process.
- Each `ArasakaDeviceProcess` is linked to a specific `ArasakaDevice`.

### 5. ArasakaDeviceMemoryMapping

The **`ArasakaDeviceMemoryMapping`** model represents memory details for a specific device.

```csharp
public int id { get; set; }
public string memoryType { get; set; }
public float memorySizeGb { get; set; }
public int memorySpeedMhz { get; set; }
public string memoryTechnology { get; set; }
public int memoryLatency { get; set; }
public float memoryVoltage { get; set; }
public string memoryFormFactor { get; set; }
public bool memoryEccSupport { get; set; }
public bool memoryHeatSpreader { get; set; }
public int memoryWarrantyYears { get; set; }
public int deviceId { get; set; }
[JsonIgnore] public ArasakaDevice device { get; set; }
```

- Stores memory-related information such as **type**, **size**, **speed**, and **voltage**.
- Maps to a specific `ArasakaDevice`.

### 6. ArasakaAthenaDataEvent

The **`ArasakaAthenaDataEvent`** model represents events and logs collected from devices.

```csharp
public int id { get; set; }
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
public float responseTime { get; set; }
public bool success { get; set; }
public int deviceId { get; set; }
[JsonIgnore] public ArasakaDevice device { get; set; }
```

- Contains detailed information about **events** and **log entries** that occur on a device.
- Tracks **user interactions**, **device information**, and **error codes** for detailed logging.

### 7. NetRunnerAdministration

The **`NetRunnerAdministration`** model represents administrative information for managing the system.

```csharp
public int id { get; set; }
public string fullName { get; set; }
```

- Stores basic information about an administrator in the system.
- This model may be expanded as more administrative features are added.

### Relationships Overview
- **ArasakaCluster** has **many** `ArasakaDevice` instances.
- **ArasakaDevice** has **many** `ArasakaDeviceProcess`, `ArasakaDeviceMemoryMapping`, and `ArasakaAthenaDataEvent` instances.
- Relationships are configured in the **`OnModelCreating`** method of `DataContext`, defining foreign keys and navigation properties.

### Summary
The **Arasaka models** define the core entities for managing **clusters**, **devices**, and their associated data in the **CyberpunkPariahNexusApi**. Relationships are configured using **Entity Framework Core** to ensure data integrity, with many-to-one and one-to-many relationships between clusters, devices, and their respective processes, memory mappings, and events.

