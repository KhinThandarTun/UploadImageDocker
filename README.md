## Docker Image

```
docker build -t upload-api:latest --file ./UploadImageDocker/Dockerfile .
```

## Docker Container

```
docker run -d --name upload-api -p 5001:80 -e "ASPNETCORE_ENVIRONMENT=Development" -e TZ=Asia/Yangon --mount type=bind,source=C:/my-folder,target=/app/wwwroot/excel --rm upload-api:latest
```

## Create the necessary directory and set the permissions before switching users in Docker File

```
RUN mkdir -p /app/wwwroot/excel && chmod -R 777 /app/wwwroot/excel
```
