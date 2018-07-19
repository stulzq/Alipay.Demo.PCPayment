FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY . .
RUN mv /etc/apt/sources.list /etc/apt/sources.list.bak && mv sources.list /etc/apt/ && apt-get update -y && apt-get install -y libgdiplus && apt-get clean && ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll
EXPOSE 80
ENTRYPOINT ["dotnet", "Alipay.Demo.PCPayment.dll"]