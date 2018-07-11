#!/bin/bash

echo Linux Docker build

cd ../

rm -rf src/publish

cd src/Alipay.Demo.PCPayment

dotnet publish -c Release -o ../publish

cd ../publish

echo publish success

docker build -t alipaydemo .
docker rmi $(docker images -f "dangling=true" -q)

docker stop alipaydemo
docker rm alipaydemo
docker run --name=alipaydemo -p 32775:80 -d  alipaydemo
