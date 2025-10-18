# Kasa Plug API

A simple REST API wrapper around TP-Link Kasa smart plug to enable turning it on or off. This repository provides the backend logic, HTTP endpoints, and Docker support to run it in a container.

## Table of Contents

- [What It Does](#what-it-does)  
- [Prerequisites](#prerequisites)  
- [Project Structure](#project-structure)  
- [Building with Docker](#building-with-docker)  
- [Running the Docker Container](#running-the-docker-container)  
- [API Endpoints / Usage](#api-endpoints--usage)  
- [Troubleshooting & Tips](#troubleshooting--tips)  
- [License](#license)

## What It Does

This service allows clients to send HTTP requests to a Kasa (smart) plug to turn it on or off. It acts as a bridge between web clients (or other automation systems) and the Kasa plug protocol / SDK. The API abstracts away direct communication with the device and centralizes control.

## Prerequisites

- .NET SDK / Runtime (likely .NET 6 or .NET 7, depending on target)  
- Docker (for container operation)  
- Network access to the smart plugs (they must be reachable from the container)  
- IP address of the kasa smart plug (assign it a static address in router to ensure it does not change)

## Project Structure

Here is a high-level of directories and files:

```
/ (root)
├── src/                 # C# source code for the API
│   ├── Controllers
│   ├── Models
│   ├── Services
│   ├── Program.cs
│   └── Dockerfile  
├── README.md             (this file)
└── LICENSE               (GPL‑3.0)
```

The `src` folder contains the API, including controllers handling HTTP routes and service logic for interacting with Kasa plugs.


## Building with Docker

The repository includes a **Dockerfile** to containerize the application. Here is a typical flow:

Build the Docker image:

   ```bash
   git clone https://github.com/sudipmandal/kasa-plug-api
   cd kasa-plug-api/src/
   docker build -t kasa-plug-api .
   ```

This command looks for the `Dockerfile` in the current directory and tags the built image as `kasa-plug-api`.


## Running the Docker Container

Once built, you can deploy the container with Docker:

```bash
docker run -d   --name kasa-plug-api   -e PLUG_API=192.168.1.100  -p 8080:8080   kasa-plug-api
```

- `-d` puts the container in detached/background mode.  
- `-e` is used to pass environment variables  
- `-p 8080:8080` maps host port 8080 to container port 8080 (adjust depending on the host’s listening port).  


You can also orchestrate using Docker Compose or Kubernetes if needed.

```yaml
version: '3.8'

services:
  kasa-plug-api:
    image: kasa-plug-api:latest
    environment:
      PLUG_IP: "192.168.1.100"  # Replae with IP of kasa plug you want to control
    ports:
      - "8080:8080"
    restart: unless-stopped

```

## API Endpoints & Usage

*(Note: The following are illustrative; consult the source code for accurate routes and payloads.)*

| HTTP Method | Endpoint               | Description                        |
|-------------|-------------------------|------------------------------------|
| `GET`      | `/api/KasaPlug/turnon`         | Turn the plug on                   | 
| `GET`      | `/api/KasaPlug/turnoff`        | Turn the plug off                  | 

### Example using `curl`

```bash
curl -X GET http://localhost:8080/api/KasaPlug/turnon  -H "Content-Type: application/json"
```

## Troubleshooting & Tips

- **Network Issues**  
  Ensure the container has network reachability to your smart plugs (same subnet, firewall rules, etc.).

- **Debugging Logs**  
  Use `.NET` logging or container logs to see errors in connecting or command failures.

- **Configuration Mistakes**  
  Wrong IP, port, credentials — double-check your settings.

- **Timeouts / Latency**  
  Smart plug commands can sometimes be slow; allow for retries or delays.

- **Version Mismatch**  
  If TP-Link or Kasa changes their protocol / API, the library your code uses might break — check compatibility.

- **Use Health Checks**  
  In a production container, add a Docker healthcheck to ensure the service is responsive.

## License

This project is licensed under the **GPL‑3.0** License (see `LICENSE` file).
