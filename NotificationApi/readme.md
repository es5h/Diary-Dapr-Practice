docker run maildev

```shell
docker run -d -p 4000:1080 -p 4025:1025 --name diary-maildev maildev/maildev:latest
```