## Base runtime image
#FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
#WORKDIR /app
#EXPOSE 80
#
## Build stage
#FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["FinancePersonalTracker/FinancePersonalTracker.csproj", "FinancePersonalTracker/"]
#RUN dotnet restore "./FinancePersonalTracker/FinancePersonalTracker.csproj"
#COPY . .
#WORKDIR "/src/FinancePersonalTracker"
#RUN dotnet build "./FinancePersonalTracker.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
## Publish stage
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./FinancePersonalTracker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
## Final image
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "FinancePersonalTracker.dll"]
#

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["FinancePersonalTracker/FinancePersonalTracker.csproj", "FinancePersonalTracker/"]
RUN dotnet restore "FinancePersonalTracker/FinancePersonalTracker.csproj"
COPY . .
WORKDIR "/src/FinancePersonalTracker"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

#ENV ASPNETCORE_URLS=http://+:80
ENV DOTNET_GC_SERVER=1
ENV DOTNET_GC_CONCURRENT=1
ENV DOTNET_GC_HEAP_HARD_LIMIT=1500000000
ENV DOTNET_ThreadPool_MinThreads=4

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FinancePersonalTracker.dll"]
