# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["OrderApi/OrderApi.csproj", "OrderApi/"]
RUN dotnet restore "OrderApi/OrderApi.csproj"
COPY . .
WORKDIR /src/OrderApi
RUN dotnet publish "OrderApi.csproj" -c Release -o /app/publish

# Etapa de runtime usando SDK (precisa do SDK para dotnet-ef)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS runtime
WORKDIR /app

# Instala dependências auxiliares
RUN apt-get update && apt-get install -y curl netcat-openbsd

# Copia a aplicação publicada para /app
COPY --from=build /app/publish/ /app/

# Copia o código fonte para /src/OrderApi para rodar migrations no entrypoint
COPY OrderApi /src/OrderApi

# Copia o entrypoint
COPY OrderApi/entrypoint.sh .

# Permite execução do script
RUN chmod +x ./entrypoint.sh

# Ajusta PATH para dotnet tools (dotnet-ef será instalado no entrypoint)
ENV PATH="$PATH:/root/.dotnet/tools"

ENTRYPOINT ["./entrypoint.sh"]
