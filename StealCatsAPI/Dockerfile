# Use the official .NET 8 runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build stage with SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy and restore projects
COPY ["StealCatsAPI/StealCatsAPI.csproj", "StealCatsAPI/"]
COPY ["StealCatsRepo/StealCatsRepo.csproj", "StealCatsRepo/"]
COPY ["StealCatsModels/StealCatsModels.csproj", "StealCatsModels/"]
COPY ["StealCatsServices/StealCatsServices.csproj", "StealCatsServices/"]
RUN dotnet restore "StealCatsAPI/StealCatsAPI.csproj"

# Copy all files
COPY . .

# Build the application
WORKDIR "/src/StealCatsAPI"
RUN dotnet build -c Release -o /app/build

# Install dotnet-ef tool
RUN dotnet tool install --global dotnet-ef

# Ensure the tool is available in the PATH
ENV PATH="${PATH}:/root/.dotnet/tools"

# Apply database migrations in the build stage (where .NET SDK is available)
RUN dotnet ef database update --project /src/StealCatsAPI/StealCatsAPI.csproj --startup-project /src/StealCatsAPI/StealCatsAPI.csproj

# Publish the application
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Final runtime stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Start the application
ENTRYPOINT ["dotnet", "StealCatsAPI.dll"]
