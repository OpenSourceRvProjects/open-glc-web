# Stage 1: Build the Angular client
FROM node:20-alpine AS nodestage
ENV ASPNETCORE_ENVIRONMENT=qa
WORKDIR /src/ClientApp
COPY OpenGLC.MVC/ClientApp/. .
RUN npm install

# Stage 2: Build the .NET application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ENV ASPNETCORE_ENVIRONMENT=qa
WORKDIR /src
COPY ["OpenGLC.MVC/OpenGLC.MVC.csproj", "OpenGLC.MVC/"]
COPY ["OpenGLC.Backend/OpenGLC.Backend.csproj", "OpenGLC.Backend/"]
COPY ["OpenGLC.Emailer/OpenGLC.Emailer.csproj", "OpenGLC.Emailer/"]
COPY ["OpenGLC.Data/OpenGLC.Data.csproj", "OpenGLC.Data/"]
COPY ["OpenGLC.Models/OpenGLC.Models.csproj", "OpenGLC.Models/"]
COPY ["OpenGLC.Infrastructure/OpenGLC.Infrastructure.csproj", "OpenGLC.Infrastructure/"]
COPY ["OpenGLC.Security/OpenGLC.Security.csproj", "OpenGLC.Security.csproj"]

# Add npm and node in build stage to run npm install
RUN apt-get update && apt-get install -y npm

# Restore dependencies
RUN dotnet restore "OpenGLC.MVC/OpenGLC.MVC.csproj"
COPY . .

# Build the project
RUN dotnet build "OpenGLC.MVC/OpenGLC.MVC.csproj" -c Release -o /app/build 

# Copy the /src/ClientApp from nodestage above to wwwroot
COPY --from=nodestage /src/ClientApp wwwroot/

# Optionally list the contents of wwwroot
RUN ls -la wwwroot/*

# Stage 3: Publish the .NET application
FROM build AS publish
ENV ASPNETCORE_ENVIRONMENT=qa
RUN dotnet publish "OpenGLC.MVC/OpenGLC.MVC.csproj" -c Release -o /app/publish 

# Stage 4: Final image with Angular and .NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_ENVIRONMENT=qa
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy the published .NET application
COPY --from=publish /app/publish .

# Set the entry point to the .NET application
ENTRYPOINT ["dotnet", "OpenGLC.MVC.dll", "--environment=QA"]
