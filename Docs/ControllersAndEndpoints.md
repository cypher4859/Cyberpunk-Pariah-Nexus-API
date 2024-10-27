## Documentation for Arasaka Controllers in CyberpunkPariahNexusApi

This document provides an overview and explanation of the **controller classes** used in the `CyberpunkPariahNexusApi`, specifically focusing on **Arasaka** related models. The controllers serve as the entry points for handling API requests and managing interactions with the **DataContext** class, which is configured using **Entity Framework Core**.

### 1. ArasakaAthenaDataEventController

The **`ArasakaAthenaDataEventController`** is responsible for managing **Arasaka Athena Data Events**, which store detailed logs and information collected from devices.

#### Endpoints
- **`GET /api/ArasakaAthenaDataEvent`**: Retrieves all **Arasaka Athena Data Events**. Requires an **Athena or Admin key** for authorization.
- **`GET /api/ArasakaAthenaDataEvent/{id}`**: Retrieves a specific data event by its **ID**. Requires an **Athena or Admin key**.
- **`PUT /api/ArasakaAthenaDataEvent/{id}`**: Updates an existing data event. Requires an **Athena or Admin key** and performs **authorization** checks.
- **`POST /api/ArasakaAthenaDataEvent`**: Creates a new data event. Requires an **Athena or Admin key** for authorization.
- **`DELETE /api/ArasakaAthenaDataEvent/{id}`**: Deletes a specific data event. Requires an **Athena or Admin key**.

#### Notes
- All endpoints perform **authorization checks** using `AuthorizationService` to ensure only authorized users can access or modify data.
- The `PUT` and `DELETE` endpoints include checks to ensure the resource **exists** before modifying or deleting.

### 2. ArasakaClusterController

The **`ArasakaClusterController`** is responsible for managing **Arasaka Clusters**, which represent logical groups of devices.

#### Endpoints
- **`GET /api/ArasakaCluster`**: Retrieves all clusters, using **lazy-loading** for better performance when there are many clusters.
- **`GET /api/ArasakaCluster/{id}`**: Retrieves a specific cluster with **eager-loading**, including related **devices, processes, and memory mappings**.
- **`PUT /api/ArasakaCluster/{id}`**: Updates an existing cluster. Requires an **Admin key** for authorization.
- **`POST /api/ArasakaCluster`**: Creates a new cluster using a **DTO** for data transfer, which ensures data integrity. Requires an **Admin key**.
- **`DELETE /api/ArasakaCluster/{id}`**: Deletes a specific cluster by its **ID**. Requires an **Admin key** for authorization.

#### Notes
- Relationships between `ArasakaCluster`, `ArasakaDevice`, and other entities are **eager-loaded** in certain cases to provide all related data.
- `POST` endpoint also handles saving related **devices** and their default **processes** and **memory mappings**.
- **`ApiExplorerSettings(IgnoreApi = true)`** is used for some endpoints to prevent them from appearing in Swagger.

### 3. ArasakaDeviceController

The **`ArasakaDeviceController`** handles API interactions for managing **Arasaka Devices**, which are the individual physical or virtual devices in the system.

#### Endpoints
- **`GET /api/ArasakaDevice`**: Retrieves all devices.
- **`GET /api/ArasakaDevice/{id}`**: Retrieves a specific device by its **ID**, including related **processes**, **memory mappings**, and **data events**.
- **`PUT /api/ArasakaDevice/{id}`**: Updates a device. Requires an **Admin key** for authorization.
- **`POST /api/ArasakaDevice`**: Creates a new device. Requires an **Admin key**.
- **`DELETE /api/ArasakaDevice/{id}`**: Deletes a device by its **ID**. Requires an **Admin key**.

#### Notes
- The `GET` endpoint for a specific device includes **eager-loading** to retrieve related data.
- All modifications (PUT, POST, DELETE) require **Admin authorization**.

### 4. ArasakaDeviceMemoryMappingController

The **`ArasakaDeviceMemoryMappingController`** manages **Arasaka Device Memory Mappings**, which store memory-related information for devices.

#### Endpoints
- **`GET /api/ArasakaDeviceMemoryMapping`**: Retrieves all memory mappings.
- **`GET /api/ArasakaDeviceMemoryMapping/{id}`**: Retrieves a specific memory mapping by **ID**.
- **`PUT /api/ArasakaDeviceMemoryMapping/{id}`**: Updates an existing memory mapping. Requires an **Admin key**.
- **`POST /api/ArasakaDeviceMemoryMapping`**: Creates a new memory mapping. Requires an **Admin key**.
- **`DELETE /api/ArasakaDeviceMemoryMapping/{id}`**: Deletes a specific memory mapping. Requires an **Admin key**.

### 5. ArasakaDeviceProcessController

The **`ArasakaDeviceProcessController`** handles API interactions for managing **processes** running on a device.

#### Endpoints
- **`GET /api/ArasakaDeviceProcess`**: Retrieves all processes.
- **`GET /api/ArasakaDeviceProcess/{id}`**: Retrieves a specific process by **ID**.
- **`PUT /api/ArasakaDeviceProcess/{id}`**: Updates an existing process. Requires an **Admin key**.
- **`POST /api/ArasakaDeviceProcess`**: Creates a new process. Requires an **Admin key**.
- **`DELETE /api/ArasakaDeviceProcess/{id}`**: Deletes a process by its **ID**. Requires an **Admin key**.

### 6. MikoshiController

The **`MikoshiController`** provides a simple endpoint for **Mikoshi** related operations, mainly used for administrative checks.

#### Endpoint
- **`GET /api/Mikoshi`**: Returns a simple response **"You won! Great Job Net Runner!"** if the **Admin key** is valid.

#### Notes
- **`ApiExplorerSettings(IgnoreApi = true)`** is used to prevent this endpoint from appearing in Swagger documentation.
- This controller is primarily used for internal or special administrative operations.

### 7. NetRunnerAdministrationController

The **`NetRunnerAdministrationController`** handles interactions for managing **NetRunner Administration** information, such as adding and retrieving administrative data.

#### Endpoints
- **`GET /api/NetRunnerAdministration`**: Retrieves all administrative entries. Requires an **Admin key**.
- **`GET /api/NetRunnerAdministration/{id}`**: Retrieves a specific administration entry by **ID**. Requires an **Admin key**.
- **`POST /api/NetRunnerAdministration`**: Creates a new administration entry. Requires an **Admin key**.
- **`DELETE /api/NetRunnerAdministration/{id}`**: Deletes an administration entry by **ID**. Requires an **Admin key**.

#### Notes
- **`ApiExplorerSettings(IgnoreApi = true)`** prevents this controller from appearing in the Swagger documentation.
- The controller is primarily used to manage **administrative information** in the system.

### Summary
The **Arasaka Controllers** manage various entities related to **clusters**, **devices**, **processes**, **memory mappings**, and **administrative information** within the **CyberpunkPariahNexusApi**. Each controller provides standard **CRUD** operations, with **authorization checks** to ensure secure access. Relationships between entities are handled using **eager-loading** or **lazy-loading** as appropriate, providing flexibility and efficiency in data retrieval. Special controllers like **Mikoshi** and **NetRunnerAdministration** are intended for internal administrative purposes.

