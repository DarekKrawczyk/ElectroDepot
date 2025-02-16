# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ENV SERVER_NAME=mssqlserver
ENV DB_NAME=ElectroDepotDB
ENV DB_USER=sa
ENV DB_PASSWORD=Password123

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Server/Server.csproj", "Server/"]
COPY ["ElectroDepotClassLibrary/ElectroDepotClassLibrary.csproj", "ElectroDepotClassLibrary/"]
RUN dotnet restore "./Server/Server.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "./Server.csproj" -c $BUILD_CONFIGURATION -o /app/build


# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

RUN touch /home/server_config.json 
# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER root
RUN mkdir -p /home/app/Services
RUN touch /home/app/Services/server_config.json
RUN mkdir -p /home/app/Services/ImageStorage
RUN chmod -R 777 /home/app/Services
USER $APP_UID
ENTRYPOINT ["dotnet", "Server.dll"]