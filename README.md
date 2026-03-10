# Time SignalR Project

This project demonstrates a real-time time broadcasting system using SignalR with an ASP.NET Core API and two client applications: Blazor WebAssembly and Angular.

## Project Structure

- **TimeApi**: ASP.NET Core API that broadcasts the current time every second using SignalR and a background worker.
- **TimeBlazor**: Blazor WebAssembly application that connects to the `TimeApi`
- **TimeAngular**: Angular application (v21+) that connects to the `TimeApi`

## How to Run

### 1. Start the API
Navigate to the `TimeApi` folder and run:
```bash
dotnet run
```
The API will be available at `https://localhost:7033`.

### 2. Start the Blazor Client
Navigate to the `TimeBlazor` folder and run:
```bash
dotnet run
```
The Blazor app will be available at `http://localhost:5146`.

### 3. Start the Angular Client
Navigate to the `TimeAngular` folder and run:
```bash
npm install
npm start
```
The Angular app will be available at `http://localhost:4200`.
- Real-time time updates every 1 second.
- Automatic reconnection support on both clients.
- Visual connection status indicators.
- Clean and modern UI for both Blazor and Angular.
