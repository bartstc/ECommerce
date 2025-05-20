FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/ProductCatalog/ProductCatalog.API/ProductCatalog.API.csproj", "src/Services/ProductCatalog/ProductCatalog.API/"]
COPY ["src/Services/ProductCatalog/ProductCatalog.Infrastructure/ProductCatalog.Infrastructure.csproj", "src/Services/ProductCatalog/ProductCatalog.Infrastructure/"]
COPY ["src/Core/ECommerce.Core/ECommerce.Core.csproj", "src/Core/ECommerce.Core/"]
COPY ["src/Core/ECommerce.Core.Infrastructure/ECommerce.Core.Infrastructure.csproj", "src/Core/ECommerce.Core.Infrastructure/"]
COPY ["src/Services/ProductCatalog/ProductCatalog.Domain/ProductCatalog.Domain.csproj", "src/Services/ProductCatalog/ProductCatalog.Domain/"]
COPY ["src/Services/ProductCatalog/ProductCatalog.Application/ProductCatalog.Application.csproj", "src/Services/ProductCatalog/ProductCatalog.Application/"]
RUN dotnet restore "src/Services/ProductCatalog/ProductCatalog.API/ProductCatalog.API.csproj"
COPY . .
WORKDIR "/src/src/Services/ProductCatalog/ProductCatalog.API"
RUN dotnet build "ProductCatalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ProductCatalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductCatalog.API.dll"]
