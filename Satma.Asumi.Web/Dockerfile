FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Satma.Asumi.Web/Satma.Asumi.Web.csproj", "Satma.Asumi.Web/"]
RUN dotnet restore "Satma.Asumi.Web/Satma.Asumi.Web.csproj"
COPY . .
WORKDIR "/src/Satma.Asumi.Web"
RUN dotnet build "Satma.Asumi.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Satma.Asumi.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Satma.Asumi.Web.dll"]
