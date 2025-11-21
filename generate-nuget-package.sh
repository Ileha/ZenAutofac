#!/bin/bash

# Create nupkgs directory if it doesn't exist
mkdir -p ./nupkgs

# Build and pack ZenAutofac
echo "Building and packing ZenAutofac..."
dotnet pack ./ZenAutofac/ZenAutofac.csproj -c Release -o ./nupkgs

# Build and pack ZenAutofac.API
echo "Building and packing ZenAutofac.API..."
dotnet pack ./ZenAutofac.API/ZenAutofac.API.csproj -c Release -o ./nupkgs

# Build and pack ZenAutofac.API.Factory
echo "Building and packing ZenAutofac.API.Factory..."
dotnet pack ./ZenAutofac.API.Factory/ZenAutofac.API.Factory.csproj -c Release -o ./nupkgs

echo "NuGet packages have been created in the ./nupkgs directory"