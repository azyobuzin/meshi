FROM mcr.microsoft.com/dotnet/sdk:5.0 AS builder

COPY . /source/
WORKDIR /source/MeshiRoulette
RUN dotnet publish --output /app/ --configuration Release

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=builder /app .

ENTRYPOINT ["dotnet", "MeshiRoulette.dll"]
EXPOSE 80
