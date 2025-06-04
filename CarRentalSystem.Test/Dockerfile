# Use ASP.NET Core 9.0 runtime (NOT just 'runtime')
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CarRentalSystem.Test/CarRentalSystem.Test.csproj", "CarRentalSystem.Test/"]
COPY ["CarRentalSystem.Api/CarRentalSystem.Api.csproj", "CarRentalSystem.Api/"]
COPY ["CarRentalSystem.Db/CarRentalSystem.Db.csproj", "CarRentalSystem.Db/"]
RUN dotnet restore "CarRentalSystem.Test/CarRentalSystem.Test.csproj"
COPY . .
WORKDIR "/src/CarRentalSystem.Test"
RUN dotnet build "CarRentalSystem.Test.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CarRentalSystem.Test.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage (using aspnet, not runtime)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarRentalSystem.Test.dll"]