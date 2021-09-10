#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["OrchardOpenId/OrchardOpenId.csproj", "OrchardOpenId/"]
RUN dotnet restore "OrchardOpenId/OrchardOpenId.csproj"
COPY . .
WORKDIR "/src/OrchardOpenId"
RUN dotnet build "OrchardOpenId.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrchardOpenId.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrchardOpenId.dll"]