#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS grpc-base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS grpc-build
WORKDIR /src
COPY ["Services/Discount/Discount.Grpc/Discount.Grpc.csproj", "Services/Discount/Discount.Grpc/"]
RUN dotnet restore "Services/Discount/Discount.Grpc/Discount.Grpc.csproj"
COPY . .
WORKDIR "/src/Services/Discount/Discount.Grpc"
RUN dotnet build "Discount.Grpc.csproj" -c Release -o /app/build

FROM grpc-build AS grpc-publish
RUN dotnet publish "Discount.Grpc.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM grpc-base AS grpc-final
WORKDIR /app
COPY --from=grpc-publish /app/publish .
ENTRYPOINT ["dotnet", "Discount.Grpc.dll"]