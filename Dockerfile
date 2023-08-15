FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY *.sln .
COPY src/Odin.Baseline.Api/*.csproj ./src/Odin.Baseline.Api/
COPY src/Odin.Baseline.Service/*.csproj ./src/Odin.Baseline.Service/
COPY src/Odin.Baseline.Domain/*.csproj ./src/Odin.Baseline.Domain/
COPY src/Odin.Baseline.Data/*.csproj ./src/Odin.Data.Domain/
COPY src/Odin.Baseline.CrossCutting.AutoMapper/*.csproj ./src/Odin.Baseline.CrossCutting.AutoMapper/

RUN dotnet restore

# copy everything else and build app
COPY src/Odin.Baseline.Api/. ./src/Odin.Baseline.Api/
COPY src/Odin.Baseline.Service/. ./src/Odin.Baseline.Service/
COPY src/Odin.Baseline.Domain/. ./src/Odin.Baseline.Domain/ 
COPY src/Odin.Baseline.Data/. ./src/Odin.Baseline.Data/ 
COPY src/Odin.Baseline.CrossCutting.AutoMapper/. ./src/Odin.Baseline.CrossCutting.AutoMapper/ 

#WORKDIR "/src/Odin.Baseline.Api"
RUN dotnet build "./src/Odin.Baseline.Api/Odin.Baseline.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./src/Odin.Baseline.Api/Odin.Baseline.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Odin.Baseline.Api.dll"]