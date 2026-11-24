# Build-fase: compileert de API met de .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY RecipeCalculator.Api.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app --no-restore

# Runtime-fase: alleen wat nodig is om de API te draaien
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
# Map voor de SQLite-database, eigendom van de niet-root-gebruiker 'app'
RUN mkdir /data && chown app:app /data
USER app
ENV ASPNETCORE_HTTP_PORTS=8080
ENTRYPOINT ["dotnet", "RecipeCalculator.Api.dll"]
