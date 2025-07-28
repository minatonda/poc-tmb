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
echo "✅ PostgreSQL está pronto. Construindo projeto e aplicando migrations..."

cd /src/OrderApi
dotnet build OrderApi.csproj -c Release

# Debug: listar arquivos gerados para verificar se deps.json existe
echo "📁 Conteúdo do diretório de build:"
ls -l ./bin/Release/net8.0/

dotnet-ef database update --project OrderApi.csproj --startup-project OrderApi.csproj --configuration Release

echo "🚀 Iniciando aplicação..."
cd /app
dotnet OrderApi.dll
