Environment Variables: 
```
NODE_VERSION: 16.13.0
```

```sh
curl -sSL https://dot.net/v1/dotnet-install.sh > dotnet-install.sh;
chmod +x dotnet-install.sh;
./dotnet-install.sh -c 6.0 -InstallDir ./dotnet;
cd ./src/Jellyfin.Blazor.Shared/ && npm ci && cd ../../;
./dotnet/dotnet publish -c Release -o ./dist ./src/Jellyfin.Blazor.Wasm/Jellyfin.Blazor.Wasm.csproj
```