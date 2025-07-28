#!/bin/bash
set -e

echo "📦 Instalando dotnet-ef CLI..."
export PATH="$PATH:/root/.dotnet/tools"
if ! command -v dotnet-ef >/dev/null 2>&1; then
  dotnet tool install --global dotnet-ef --version 9.0.7
fi

echo "⏳ Aguardando PostgreSQL ficar pronto..."
until nc -z -v -w5 postgres 5432
do
  echo "🔁 PostgreSQL não disponível - aguardando..."
  sleep 3
done
echo "✅ PostgreSQL está pronto. Aplicando migrations..."

cd /src/OrderApi
dotnet-ef database update --project OrderApi.csproj --startup-project OrderApi.csproj --no-build

echo "🚀 Iniciando aplicação..."
cd /app
dotnet OrderApi.dll
