#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["ValidationsAPI.Host/ValidationsAPI.Host.csproj", "ValidationsAPI.Host/"]
COPY ["ValidationsAPI.Models/ValidationsAPI.Models.csproj", "ValidationsAPI.Models/"]
COPY ["ValidationsAPI.Services/ValidationsAPI.Services.csproj", "ValidationsAPI.Services/"]
COPY ["ValidationsAPI.UnitTests/ValidationsAPI.UnitTests.csproj", "ValidationsAPI.UnitTests/"]
RUN dotnet restore "ValidationsAPI.Host/ValidationsAPI.Host.csproj"
COPY . .
WORKDIR "/src/ValidationsAPI.Host"
RUN dotnet build "ValidationsAPI.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ValidationsAPI.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ValidationsAPI.Host.dll"]