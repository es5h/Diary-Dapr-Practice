FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MvcFront/MvcFront.csproj", "MvcFront/"]
RUN dotnet restore "MvcFront/MvcFront.csproj"
COPY . .
WORKDIR "/src/MvcFront"
RUN dotnet build "MvcFront.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MvcFront.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MvcFront.dll"]
