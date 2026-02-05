FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/AnonymousStudentReviews.Api/AnonymousStudentReviews.Api.csproj", "src/AnonymousStudentReviews.Api/"]
COPY ["src/AnonymousStudentReviews.Core/AnonymousStudentReviews.Core.csproj", "src/AnonymousStudentReviews.Core/"]
COPY ["src/AnonymousStudentReviews.Infrastructure/AnonymousStudentReviews.Infrastructure.csproj", "src/AnonymousStudentReviews.Infrastructure/"]
COPY ["src/AnonymousStudentReviews.UseCases/AnonymousStudentReviews.UseCases.csproj", "src/AnonymousStudentReviews.UseCases/"]

RUN dotnet restore "src/AnonymousStudentReviews.Api/AnonymousStudentReviews.Api.csproj"

COPY . .

WORKDIR "/src/src/AnonymousStudentReviews.Api"
RUN dotnet build "AnonymousStudentReviews.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AnonymousStudentReviews.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "AnonymousStudentReviews.Api.dll"]