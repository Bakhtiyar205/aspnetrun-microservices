#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS api-base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS api-build
WORKDIR /src
COPY ["Services/Discount/Discount.API/Discount.API.csproj", "Services/Discount/Discount.API/"]
RUN dotnet restore "Services/Discount/Discount.API/Discount.API.csproj"
COPY . .
WORKDIR "/src/Services/Discount/Discount.API"
RUN dotnet build "Discount.API.csproj" -c Release -o /app/build

FROM api-build AS api-publish
RUN dotnet publish "Discount.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM api-base AS api-final
WORKDIR /app
COPY --from=api-publish /app/publish .
ENTRYPOINT ["dotnet", "Discount.API.dll"]