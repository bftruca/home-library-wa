FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["HomeLibrary.Worker/HomeLibrary.Worker.csproj", "HomeLibrary.Worker/"]
COPY ["HomeLibrary.Shared/HomeLibrary.Shared.csproj", "HomeLibrary.Shared/"]

RUN dotnet restore "HomeLibrary.Worker/HomeLibrary.Worker.csproj"

COPY . .

RUN dotnet publish "HomeLibrary.Worker/HomeLibrary.Worker.csproj" \
    -c Release \
    -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "HomeLibrary.Worker.dll"]