FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["HomeLibrary.Api/HomeLibrary.Api.csproj", "HomeLibrary.Api/"]
COPY ["HomeLibrary.Shared/HomeLibrary.Shared.csproj", "HomeLibrary.Shared/"]

RUN dotnet restore "HomeLibrary.Api/HomeLibrary.Api.csproj"

COPY . .

RUN dotnet publish "HomeLibrary.Api/HomeLibrary.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "HomeLibrary.Api.dll"]