#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["order-service/Ordering.API/Ordering.API/Ordering.API.csproj", "order-service/Ordering.API/Ordering.API/"]
COPY ["order-service/Ordering.API/Ordering.Application/Ordering.Application.csproj", "order-service/Ordering.API/Ordering.Application/"]
COPY ["order-service/Ordering.API/Ordering.Domain/Ordering.Domain.csproj", "order-service/Ordering.API/Ordering.Domain/"]
COPY ["BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "BuildingBlocks/EventBus.Messages/"]
COPY ["order-service/Ordering.API/Ordering.Infrastructure/Ordering.Infrastructure.csproj", "order-service/Ordering.API/Ordering.Infrastructure/"]
COPY ["BuildingBlocks/Common.Logging/Common.Logging.csproj", "BuildingBlocks/Common.Logging/"]
RUN dotnet restore "order-service/Ordering.API/Ordering.API/Ordering.API.csproj"
COPY . .
WORKDIR "/src/order-service/Ordering.API/Ordering.API"
RUN dotnet build "Ordering.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ordering.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.API.dll"]