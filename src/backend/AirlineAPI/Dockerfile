#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/backend/AirlineAPI/AirlineAPI.csproj", "src/backend/AirlineAPI/"]
COPY ["src/common/Shared/Shared.csproj", "src/common/Shared/"]
RUN dotnet restore "src/backend/AirlineAPI/AirlineAPI.csproj"
COPY . .
WORKDIR "/src/src/backend/AirlineAPI"
RUN dotnet build "AirlineAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AirlineAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AirlineAPI.dll"]