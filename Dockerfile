# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the solution file and the csproj files
COPY xv_dotnet_demo.sln ./
COPY xv_dotnet_demo_v2/xv_dotnet_demo.csproj xv_dotnet_demo_v2/
COPY xv_dotnet_demo_v2_services/xv_dotnet_demo_v2_services.csproj xv_dotnet_demo_v2_services/
COPY xv_dotnet_demo_v2_infrastructure/xv_dotnet_demo_v2_infrastructure.csproj xv_dotnet_demo_v2_infrastructure/
COPY xv_dotnet_demo_v2_domain/xv_dotnet_demo_v2_domain.csproj xv_dotnet_demo_v2_domain/

# Restore dependencies
RUN dotnet restore xv_dotnet_demo.sln

# Copy the rest of the project files
COPY . .

# Build the project
WORKDIR /src/xv_dotnet_demo_v2
RUN dotnet build xv_dotnet_demo.csproj -c Release -o /app/build

# Stage 2: Publish the application
FROM build AS publish
WORKDIR /src/xv_dotnet_demo_v2
RUN dotnet publish xv_dotnet_demo.csproj -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Run the application
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .

# Expose ports
EXPOSE 5002

# Run the application
ENTRYPOINT ["dotnet", "xv_dotnet_demo.dll"]
