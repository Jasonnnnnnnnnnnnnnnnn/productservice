FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy everything and restore dependencies
COPY . ./
RUN dotnet restore

# Build the project
RUN dotnet publish -c Release -o out

# Use the official .NET runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the build output from the build environment
COPY --from=build-env /app/out .

# Set environment variables for MySQL connection string
ENV ASPNETCORE_URLS=http://+:80

# Expose port 80
EXPOSE 80

# Run the application
ENTRYPOINT ["dotnet", "productservice.dll"]

