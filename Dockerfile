FROM mcr.microsoft.com/dotnet/sdk:9.0
WORKDIR /app

COPY . .

RUN dotnet restore
RUN dotnet publish src/AnonymousStudentReviews.Api/AnonymousStudentReviews.Api.csproj \
    -c Release -o /app/publish

EXPOSE 5000
CMD ["dotnet", "/app/publish/AnonymousStudentReviews.Api.dll"]
