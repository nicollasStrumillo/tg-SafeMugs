FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY backend.csproj .
RUN dotnet restore backend.csproj

COPY . .
RUN dotnet publish backend.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Copia do wwwroot usada pelo Auto Recover a cada inicializacao
RUN cp -r /app/wwwroot /app/wwwroot-seed

ENV ASPNETCORE_HTTP_PORTS="8080;9090"
EXPOSE 8080
ENTRYPOINT ["dotnet", "backend.dll"]
