#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ApiGateways/Web.Bff.Shopping/Web.Shopping.HttpAggregator/Web.Shopping.HttpAggregator.csproj", "ApiGateways/Web.Bff.Shopping/Web.Shopping.HttpAggregator/"]
RUN dotnet restore "ApiGateways/Web.Bff.Shopping/Web.Shopping.HttpAggregator/Web.Shopping.HttpAggregator.csproj"
COPY . .
WORKDIR "/src/ApiGateways/Web.Bff.Shopping/Web.Shopping.HttpAggregator"
RUN dotnet build "Web.Shopping.HttpAggregator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Web.Shopping.HttpAggregator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.Shopping.HttpAggregator.dll"]