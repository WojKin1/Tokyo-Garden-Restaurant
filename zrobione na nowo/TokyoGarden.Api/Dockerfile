# ---------- BUILD ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# kopiujemy CAŁY kontekst (root = "zrobione na nowo")
COPY . .

# RESTORE tylko projektu API (pociągnie zależne biblioteki przez ProjectReference)
RUN dotnet restore "TokyoGarden.Api/TokyoGarden.Api.csproj"

# PUBLISH projektu API
RUN dotnet publish "TokyoGarden.Api/TokyoGarden.Api.csproj" -c Release -o /app

# ---------- RUNTIME ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .

# Render ustawia $PORT — wystartuj pod ten port
CMD ["sh", "-c", "dotnet TokyoGarden.Api.dll --urls http://0.0.0.0:$PORT"]
