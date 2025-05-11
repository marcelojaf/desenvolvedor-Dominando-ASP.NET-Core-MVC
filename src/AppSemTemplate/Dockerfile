# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["AppSemTemplate.csproj", "AppSemTemplate/"]
RUN dotnet restore "./AppSemTemplate/AppSemTemplate.csproj"
WORKDIR "AppSemTemplate"
COPY . .

# Criação do diretório antes de exportar o certificado
RUN mkdir -p /root/.aspnet/https

# Geração e exportação do certificado HTTPS
RUN dotnet build "AppSemTemplate.csproj" -c Release -o /app/build
RUN dotnet dev-certs https -ep /root/.aspnet/https/aspnetapp.pfx -p Teste@123
RUN dotnet dev-certs https --trust

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
COPY --from=build /root/.aspnet/https/aspnetapp.pfx /root/.aspnet/https/
RUN dotnet publish "AppSemTemplate.csproj" -c Release -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /root/.aspnet/https/aspnetapp.pfx /root/.aspnet/https/
VOLUME /var/data_protection_keys
# Certifique-se de que o ambiente está configurado antes do ENTRYPOINT
ENV ASPNETCORE_ENVIRONMENT=Docker
ENV ASPNETCORE_URLS="https://+:80;https://+:443;"
ENV ASPNETCORE_Kestrel__Certificates__Default__Password="Teste@123"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/root/.aspnet/https/aspnetapp.pfx
ENV DOTNET_ENVIRONMENT=Docker
ENTRYPOINT ["dotnet", "AppSemTemplate.dll"]