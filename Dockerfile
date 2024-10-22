FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

EXPOSE 8080

# copy csproj and restore as distinct layers
COPY CyberpunkPariahNexusApi/ CyberpunkPariahNexusApi/
RUN dotnet restore CyberpunkPariahNexusApi/CyberpunkPariahNexusApi.csproj


FROM build AS publish
WORKDIR /source/CyberpunkPariahNexusApi
RUN dotnet publish --no-restore -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT [ ]
CMD dotnet CyberpunkPariahNexusApi.dll