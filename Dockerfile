FROM mcr.microsoft.com/dotnet/aspnet:7.0.7-jammy AS base
WORKDIR /app
# EXPOSE 5000

# ENV ASPNETCORE_URLS=http://+:5000

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
RUN apt update; apt install net-tools curl -y
RUN apt-get update && DEBIAN_FRONTEND=noninteractive TZ=Etc/UTC apt-get -y install tzdata
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0.304-jammy AS build

ARG GITHUB_USERNAME
ARG GITHUB_ACCESS_TOKEN
ARG GITHUB_PACKAGE_URL

WORKDIR /src
COPY . .

# COPY add_nuget_source.sh /src/
# RUN chmod +x /src/add_nuget_source.sh
# RUN /src/add_nuget_source.sh

RUN dotnet nuget add source \
    --username $GITHUB_USERNAME \
    --password $GITHUB_ACCESS_TOKEN \
    --store-password-in-clear-text \
    --name github $GITHUB_PACKAGE_URL

RUN dotnet restore "src/Service/Service.csproj"
WORKDIR "/src/src/Service"
RUN dotnet build "Service.csproj" -c Release -o /app/build
WORKDIR "/src"
RUN dotnet test --no-restore --no-build --configuration Release --filter FullyQualifiedName\!~IntegrationTests --verbosity normal --logger:"console;noprogress=true" || true

WORKDIR "/src/src/Service"
FROM build AS publish
RUN dotnet publish "Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Service.dll"]