FROM mcr.microsoft.com/dotnet/aspnet:6.0.4-bullseye-slim AS base
EXPOSE 80
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["./WebAPI/*.csproj", "./"]
RUN dotnet restore --ignore-failed-sources
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build as publish
RUN dotnet publish -c Release -o /app/publish

# Build runtime image
FROM base as final

WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "WebAPI.dll"]