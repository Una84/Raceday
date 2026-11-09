FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.sln ./
COPY src/RaceDay.Core/*.csproj src/RaceDay.Core/
COPY src/RaceDay.Api/*.csproj src/RaceDay.Api/
COPY src/RaceDay.Web/*.csproj src/RaceDay.Web/
COPY src/RaceDay.Tests/*.csproj src/RaceDay.Tests/
RUN dotnet restore

COPY . .
WORKDIR /app/src/RaceDay.Web
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RaceDay.Web.dll"]
