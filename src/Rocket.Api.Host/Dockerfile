FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS publish
ARG TARGETARCH

COPY . /dotnet-app
RUN cd /dotnet-app && echo $(ls -1)
RUN \
    cd /dotnet-app && \
    dotnet restore "./Rocket.Api.Host/Rocket.Api.Host.csproj" -a $TARGETARCH && \
    dotnet publish "./Rocket.Api.Host/Rocket.Api.Host.csproj" --ignore-failed-sources --no-restore -c Release -o /app/publish -a $TARGETARCH /p:UseAppHost=false /p:PublishReadyToRun=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
ARG APP_VERSION=0.0.0
ENV APP_VERSION=$APP_VERSION
COPY --from=publish /app/publish .
CMD ["dotnet", "Rocket.Api.Host.dll"]