FROM --platform=linux/arm64 mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:9.0 AS publish
ARG TARGETARCH

COPY . /dotnet-app
RUN cd /dotnet-app && echo $(ls -1)
RUN \
    cd /dotnet-app && \
    dotnet restore "./Rocket.Web.Host/Rocket.Web.Host.csproj" -a $TARGETARCH && \
    dotnet publish "./Rocket.Web.Host/Rocket.Web.Host.csproj" --ignore-failed-sources --no-restore -c Release -o /app/publish -a $TARGETARCH /p:UseAppHost=false
    
FROM base AS final
WORKDIR /app
ARG APP_VERSION=0.0.0
ENV APP_VERSION=$APP_VERSION
COPY --from=publish /app/publish .
CMD ["dotnet", "Rocket.Web.Host.dll"]