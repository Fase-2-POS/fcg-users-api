FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Fcg.Users.Api/Fcg.Users.Api.csproj Fcg.Users.Api/
COPY Fcg.Users.Domain/Fcg.Users.Domain.csproj Fcg.Users.Domain/
COPY Fcg.Users.Infrastructure/Fcg.Users.Infrastructure.csproj Fcg.Users.Infrastructure/
RUN dotnet restore Fcg.Users.Api/Fcg.Users.Api.csproj

COPY . .
RUN dotnet publish Fcg.Users.Api/Fcg.Users.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

RUN mkdir -p /app/data && chown app /app/data

COPY --from=build /app/publish .

USER app

EXPOSE 8080
ENTRYPOINT ["dotnet", "Fcg.Users.Api.dll"]
