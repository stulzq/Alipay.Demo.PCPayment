FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY Alipay.Demo.PCPayment.sln ./
COPY Alipay.Demo.PCPayment/Alipay.Demo.PCPayment.csproj Alipay.Demo.PCPayment/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Alipay.Demo.PCPayment
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
RUN apt-get update
RUN apt-get install libgdiplus --assume-yes
RUN cd /usr/lib
RUN ln -s libgdiplus.so gdiplus.dll
ENTRYPOINT ["dotnet", "Alipay.Demo.PCPayment.dll"]
