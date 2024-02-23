#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-jammy-chiseled-amd64 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG assemblyInformationalVersion=1.0.0

WORKDIR /work
COPY ["src", "."]
RUN dotnet restore "ShoppingApp.SiloHost/ShoppingApp.SiloHost.csproj"
WORKDIR "/work/ShoppingApp.SiloHost"
RUN dotnet build "ShoppingApp.SiloHost.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShoppingApp.SiloHost.csproj" -c Release -r linux-x64 \
	--self-contained -o /app/publish /p:UseAppHost=true /p:PublishTrimmed=false \
	/p:InformationalVersion=$assemblyInformationalVersion

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./ShoppingApp.SiloHost"]