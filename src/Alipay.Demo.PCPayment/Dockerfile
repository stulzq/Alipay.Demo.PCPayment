FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
RUN apt-get install -y libgdiplus && apt-get install -y libc6-dev
RUN ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll
COPY . .
EXPOSE 80
ENTRYPOINT ["dotnet", "Alipay.Demo.PCPayment.dll"]