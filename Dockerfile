FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
ENV buildConfiguration=Release
ENV solutionName=wms-proxy.sln
ENV projectToBuild=wms-proxy/wmsproxy.csproj

WORKDIR /app

COPY ./src ./
RUN dotnet restore ${solutionName} --no-cache --configfile Nuget.config
RUN dotnet build ${solutionName} --no-restore -c $buildConfiguration

# Copy everything else and build
COPY . ./
RUN dotnet publish ${projectToBuild} -c ${buildConfiguration} -o out

# # Running Unit Tests
# #RUN find . -name "*.csproj" -print | grep -i "Test.csproj" | xargs -I % sh -c 'dotnet test % --no-build -c ${buildConfiguration}'
# #RUN find . -name "*.csproj" -print | grep -i "Specs.csproj" | xargs -I % sh -c 'dotnet test % --no-build -c ${buildConfiguration}'


# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "wmsproxy.dll"]

