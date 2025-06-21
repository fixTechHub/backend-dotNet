# Multi-stage build for .NET 8.0 Web API
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WebApiDotNet.csproj", "./"]
RUN dotnet restore "WebApiDotNet.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "WebApiDotNet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApiDotNet.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApiDotNet.dll"] 