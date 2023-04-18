FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TimeTracker/TimeTracker.csproj", "TimeTracker/"]
RUN dotnet restore "TimeTracker/TimeTracker.csproj"
COPY . .
WORKDIR "/src/TimeTracker"
RUN dotnet build "TimeTracker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TimeTracker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TimeTracker.dll"]