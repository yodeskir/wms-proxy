#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["wms-proxy/wmsproxy.csproj", "wms-proxy/"]
COPY ["wms-data-access/wms-data-access.csproj", "wms-data-access/"]
COPY ["wms-tools/wms-tools.csproj", "wms-tools/"]
COPY ["wms-shared/wms-shared.csproj", "wms-shared/"]
RUN dotnet restore "wms-proxy/wmsproxy.csproj"
COPY . .
WORKDIR "/src/wms-proxy"
RUN dotnet build "wmsproxy.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "wmsproxy.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "wmsproxy.dll"]