# ==============================
# STAGE 1: Build da aplicação
# ==============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia tudo e compila em modo Release
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out

# ==============================
# STAGE 2: Runtime (mais leve)
# ==============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/out .

# Porta padrão do Render (10000)
ENV ASPNETCORE_URLS=http://0.0.0.0:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "DSIN.dll"]
