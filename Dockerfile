FROM microsoft/aspnetcore-build:2 AS builder

COPY . /source/
WORKDIR /source/MeshiRoulette
RUN dotnet publish --output /app/ --configuration Release

FROM microsoft/aspnetcore:2
WORKDIR /app
COPY --from=builder /app .

ENTRYPOINT ["dotnet", "MeshiRoulette.dll"]
EXPOSE 80
