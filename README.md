# Kasa Plug API

A simple REST API wrapper around TP-Link Kasa smart plugs (or similar devices) to enable programmatic control (e.g. turn on/off, status, etc.). This repository provides the backend logic, HTTP endpoints, and Docker support to run it in a container.

## Table of Contents

- [What It Does](#what-it-does)  
- [Prerequisites](#prerequisites)  
- [Project Structure](#project-structure)  
- [Configuration](#configuration)  
- [Running Locally (without Docker)](#running-locally-without-docker)  
- [Building with Docker](#building-with-docker)  
- [Running the Docker Container](#running-the-docker-container)  
- [API Endpoints / Usage](#api-endpoints--usage)  
- [Troubleshooting & Tips](#troubleshooting--tips)  
- [License](#license)

## What It Does

This service allows clients to send HTTP requests to turn Kasa (smart) plugs on or off, query their status, or perform related control operations. It acts as a bridge between web clients (or other automation systems) and the Kasa plug protocol / SDK. The API abstracts away direct communication with the device and centralizes control.

## Prerequisites

- .NET SDK / Runtime (likely .NET 6 or .NET 7, depending on target)  
- Docker (for container operation)  
- Network access to the smart plugs (they must be reachable from the container)  
- (Optional) Environment variables or configuration (e.g. credentials, IP addresses, etc.)

## Project Structure

Here is a high-level of directories and files:

```
/ (root)
├── src/                 # C# source code for the API
│   ├── Controllers
│   ├── Models
│   ├── Services
│   ├── Program.cs
│   └── ...  
├── Dockerfile           # Definition for building a container image
├── README.md             (this file)
└── LICENSE               (GPL‑3.0)
```

The `src` folder contains the API, including controllers handling HTTP routes and service logic for interacting with Kasa plugs.

## Configuration

You’ll want to configure some aspects before running:

- **Device IP / addresses** — the IP(s) or hostnames of the Kasa plug(s) you wish to control.  
- **Credentials / authentication** — if your plugs require a username/password or token.  
- **Port(s)** — which port the API should listen on (e.g. `80` or `5000`).  
- **Logging / environment** — development vs production settings.

These can often be supplied using environment variables or an `appsettings.json` file within the .NET project (check the code for how configuration is injected).

## Running Locally (without Docker)

To build and run locally (for development / debugging):

1. From the root, go to the `src` (or solution) directory.  
2. Use the .NET CLI:

   ```bash
   dotnet build
   dotnet run
   ```

3. The API should start listening on the configured port (often `http://localhost:5000` or similar).  
4. Use a tool like `curl` or Postman to invoke endpoints (e.g. `/api/plugs/on`, `/api/plugs/off`, `/api/plugs/status`).

## Building with Docker

The repository includes a **Dockerfile** to containerize the application. Here is a typical flow:

1. Build the Docker image:

   ```bash
   git clone https://github.com/sudipmandal/kasa-plug-api
   cd kasa-plug-api/
   docker build -t kasa-plug-api .
   ```

   This command looks for the `Dockerfile` in the current directory and tags the built image as `kasa-plug-api`.

2. (Optional) You can specify build arguments or a different tag, e.g.:

   ```bash
   docker build --pull --no-cache -t myrepo/kasa-plug-api:latest .
   ```

Inside the Dockerfile, the steps likely:

- Use a base image (e.g. `mcr.microsoft.com/dotnet/aspnet:6.0` or SDK image)  
- Copy the project files  
- Restore dependencies  
- Build the project in release mode  
- Configure the runtime image  
- Expose the HTTP port  
- Define the entrypoint (e.g. `dotnet KasaPlugApi.dll`)

## Running the Docker Container

Once built, you can deploy the container with Docker:

```bash
docker run -d   --name kasa-plug-api   -e ASPNETCORE_ENVIRONMENT=Production   -e [OTHER_ENV_VARS]   -p 8080:80   kasa-plug-api
```

- `-d` puts the container in detached/background mode.  
- `-e` is used to pass environment variables (e.g. config / credentials).  
- `-p 8080:80` maps host port 8080 to container port 80 (adjust depending on the app’s listening port).  
- You might also mount a config file or secret via `-v` if needed.  
- Use `docker logs kasa-plug-api` to see the startup logs or any errors.

You can also orchestrate using Docker Compose or Kubernetes if needed.

## API Endpoints & Usage

*(Note: The following are illustrative; consult the source code for accurate routes and payloads.)*

| HTTP Method | Endpoint               | Description                        | Payload / Query Params                |
|-------------|-------------------------|------------------------------------|----------------------------------------|
| `POST`      | `/api/plugs/on`         | Turn one plug on                   | JSON: `{ "ip": "192.168.1.100" }`     |
| `POST`      | `/api/plugs/off`        | Turn one plug off                  | JSON: `{ "ip": "192.168.1.100" }`     |
| `GET`       | `/api/plugs/status`     | Get status (on/off etc.)           | Query: `?ip=192.168.1.100`             |

### Example using `curl`

```bash
curl -X POST http://localhost:8080/api/plugs/on     -H "Content-Type: application/json"     -d '{"ip":"192.168.1.100"}'
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
