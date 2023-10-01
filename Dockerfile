FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
ENV PORT 8080
ENV ASPNETCORE_URLS "http://*:${PORT}"
EXPOSE ${PORT}

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["movie-library-backend.csproj", "./"]
RUN dotnet restore "movie-library-backend.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "movie-library-backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "movie-library-backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "movie-library-backend.dll"]