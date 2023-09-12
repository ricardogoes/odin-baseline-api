FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY *.sln .
COPY src/Odin.Baseline.Api/*.csproj ./src/Odin.Baseline.Api/
COPY src/Odin.Baseline.Application/*.csproj ./src/Odin.Baseline.Application/
COPY src/Odin.Baseline.Domain/*.csproj ./src/Odin.Baseline.Domain/
COPY src/Odin.Baseline.Infra.Data.EF/*.csproj ./src/Odin.Baseline.Infra.Data.EF/
COPY src/Odin.Baseline.Infra.Messaging/*.csproj ./src/Odin.Baseline.Infra.Messaging/
COPY tests/Odin.Baseline.UnitTests/*.csproj ./tests/Odin.Baseline.UnitTests/
COPY tests/Odin.Baseline.IntegrationTests/*.csproj ./tests/Odin.Baseline.IntegrationTests/
COPY tests/Odin.Baseline.EndToEndTests/*.csproj ./tests/Odin.Baseline.EndToEndTests/

RUN dotnet restore

# copy everything else and build app
COPY src/Odin.Baseline.Api/. ./src/Odin.Baseline.Api/
COPY src/Odin.Baseline.Application/. ./src/Odin.Baseline.Application/
COPY src/Odin.Baseline.Domain/. ./src/Odin.Baseline.Domain/
COPY src/Odin.Baseline.Infra.Data.EF/. ./src/Odin.Baseline.Infra.Data.EF/ 
COPY src/Odin.Baseline.Infra.Messaging/. ./src/Odin.Baseline.Infra.Messaging/ 

#WORKDIR "/src/Odin.Baseline.Api"
RUN dotnet build "./src/Odin.Baseline.Api/Odin.Baseline.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./src/Odin.Baseline.Api/Odin.Baseline.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Odin.Baseline.Api.dll"]