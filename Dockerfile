FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
USER $APP_UID

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /build

COPY src/AnonymousStudentReviews.Api/*.csproj src/AnonymousStudentReviews.Api/
COPY src/AnonymousStudentReviews.Infrastructure/*.csproj src/AnonymousStudentReviews.Infrastructure/
COPY src/AnonymousStudentReviews.UseCases/*.csproj src/AnonymousStudentReviews.UseCases/
COPY src/AnonymousStudentReviews.Core/*.csproj src/AnonymousStudentReviews.Core/

RUN dotnet restore src/AnonymousStudentReviews.Api/AnonymousStudentReviews.Api.csproj

COPY . .
RUN dotnet build src/AnonymousStudentReviews.Api/AnonymousStudentReviews.Api.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish src/AnonymousStudentReviews.Api/AnonymousStudentReviews.Api.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "AnonymousStudentReviews.Api.dll"]
