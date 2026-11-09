# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and project files
COPY *.sln ./
COPY src/RaceDay.Core/RaceDay.Core.csproj src/RaceDay.Core/
COPY src/RaceDay.Api/RaceDay.Api.csproj src/RaceDay.Api/
COPY src/RaceDay.Web/RaceDay.Web.csproj src/RaceDay.Web/
COPY src/RaceDay.Tests/RaceDay.Tests.csproj src/RaceDay.Tests/

RUN dotnet restore

# Copy full source code
COPY . .
WORKDIR /app/src/RaceDay.Web
RUN dotnet publish -c Release -o /out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "RaceDay.Web.dll"]
