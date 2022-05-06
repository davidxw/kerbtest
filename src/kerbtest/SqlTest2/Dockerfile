#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SqlTest2.csproj", "SqlTest2/"]
RUN dotnet restore "SqlTest2/SqlTest2.csproj"
COPY . SqlTest2
WORKDIR "/src/SqlTest2"
RUN dotnet build "SqlTest2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SqlTest2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


# Install krb5 packages
RUN apt-get update
RUN apt-get remove krb5-config krb5-user
RUN apt install -y krb5-config 
RUN apt-get install -y krb5-user

# Copy kerberso configuration and keytab
COPY --from=build /src/SqlTest2/krb5.conf /etc/krb5.conf

COPY --from=build /src/SqlTest2/dwatson.keytab /app/dwatson.keytab


# copy the launch script
COPY --from=build /src/SqlTest2/launch.sh /launch.sh

CMD /app/launch.sh