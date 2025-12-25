FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

EXPOSE 5081
COPY *.sln .

# 2. Kopira sve projekte 
COPY MyMovie.Api/*.csproj MyMovie.Api/
COPY MyMovie.Application/*.csproj MyMovie.Application/
COPY MyMovie.Domain/*.csproj MyMovie.Domain/
COPY MyMovie.Infrastructure/*.csproj MyMovie.Infrastructure/

# 3. Restore paketa
RUN dotnet restore

# 4. Kopira ostatak koda
COPY . .

# 5. Build i Publish 
WORKDIR /app/MyMovie.Api
RUN dotnet publish -c Release -o /out

# 6. Runtime faza
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

## Run migrations
#RUN dotnet tool install --global dotnet-ef 
#ENV PATH="$PATH:/root/.dotnet/tools"
#RUN dotnet-ef database update --project src/MyFood.Infrastructure/ --startup-project src/MyFood.Api/


ENTRYPOINT ["dotnet", "MyMovie.Api.dll"]