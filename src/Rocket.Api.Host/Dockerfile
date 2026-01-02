FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS publish

COPY . /dotnet-app
RUN cd /dotnet-app && echo $(ls -1)
RUN \
    cd /dotnet-app && \
    dotnet restore "./Rocket.Api.Host/Rocket.Api.Host.csproj" && \
    dotnet publish "./Rocket.Api.Host/Rocket.Api.Host.csproj" --ignore-failed-sources --no-restore -c Release -o /app/publish /p:UseAppHost=false
    
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "Rocket.Api.Host.dll"]