
1. Finding the Admin Endpoint
    - Interrogate the clusters; You will need to find a device that has a cluster whose env is "DevForge", region is sa-east-1, and created in 2024.
    - The device itself needs to be a Bio-Organic Processor
    - This device's public key can be grabbed and decoded (base64) explain how to get to the Administrator Endpoint
    - There may or may not be be more than device that fits these criteria so be mindful.
    - ANSWER: 
        - Cluster-42 (id 42).
        - Device: id 11 (Security Sentinel)
        - Base64 value: VGhlIEFkbWluIGVuZHBvaW50IGlzIDxVUkw+L2FwaS9OZXRSdW5uZXJBZG1pbmlzdHJhdGlvbg==
        - Translated value: "The Admin endpiont is <URL>/api/NetRunnerAdministration"

2. Getting the Athena Access Key (id of 48)
    - One of the Athena Access Key's on an Arasaka Device will give access to the Athena Data Events
    - The device must have a cluster that is in the TechHub environment, has less than 15 Nodes, and less than 30 cpuCores.
    - The device itself must be a Nano-Processor
    - The device is running a process of Warcraft3.exe
    - The device also has memory capabilities of about 560Gb of ROM
    - ANSWER:
        - Cluster-30
        - Device 48
            - Process 186
            - Memory Mapping 69
        - Athena Access key = 7f1b4a78f59a18bf6a216c2173e0de3c

3. Getting the Administrator Key
    - Now that you can access Athena you will need to find the Data Event that is related to that device you discovered.
    - You'll need to analyze the error message to see if you can get an Admin Key to Authenticate
    - ANSWER:
        - device 48 has the key
        - Key is 7f1b4a78f59a18bf6a216c2173e0de3c
        - Athena data event 348
        - errorMessage needs to be:
            ```
            Cras in purus eu magna vulputate luctus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Vivamus vestibulum sagittis sapien. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam vel augue. Vestibulum rutrum rutrum neque. Aenean auctor gravida sem. Praesent id massa id nisl venenatis lacinia. Aenean sit amet justo. Morbi ut odio. Clavis aditum ad terminum administrativum est uti termino Mikoshi. Typice uteris ArasakaCluster ad accedendum indicem clusterorum, sed si id cum Mikoshi substituas, recte procedet. Utere hac clavi ad confirmandum cum termino Mikoshi: 4cb575fcf678d985485946d7d7ed53662a7d532e73cbd9108dd4ae6df476869c
            ```
        - Admin Key: 4cb575fcf678d985485946d7d7ed53662a7d532e73cbd9108dd4ae6df476869c
