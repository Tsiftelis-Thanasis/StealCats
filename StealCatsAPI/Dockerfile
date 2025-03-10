# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["StealCatsAPI/StealCatsAPI.csproj", "StealCatsAPI/"]
COPY ["StealCatsRepo/StealCatsRepo.csproj", "StealCatsRepo/"]
COPY ["StealCatsModels/StealCatsModels.csproj", "StealCatsModels/"]
COPY ["StealCatsServices/StealCatsServices.csproj", "StealCatsServices/"]

# Restore dependencies
RUN dotnet restore "StealCatsAPI/StealCatsAPI.csproj"

# Copy all source files
COPY . .

# Build
WORKDIR /src/StealCatsAPI
RUN dotnet build -c Release -o /app/build

# Install EF tools and add to PATH
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# 🚀 ✅ ONLY UPDATE THE DATABASE (Skip creating the migration)
RUN dotnet ef database update --project /src/StealCatsRepo --startup-project /src/StealCatsAPI

# Publish
RUN dotnet publish -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "StealCatsAPI.dll"]
