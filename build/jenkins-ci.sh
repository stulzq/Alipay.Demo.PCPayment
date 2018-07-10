#!/bin/bash

echo Linux Docker build

cd ../

cd src/Alipay.Demo.PCPayment

dotnet publish -c Release -o ../publish

cd ../publish

echo publish success

docker build -t alipaydemo .

docker run --name=alipaydemo -p 32775:80 -d  alipaydemo