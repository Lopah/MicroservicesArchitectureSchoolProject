# Docker Setup

All credentials are already configured in appsettings.json. The following commands are needed to setup message broker **RabbitMQ** and database instance **PostreSQL**.

## RabbitMQ

- User: user
- Password: password
- Management: http://localhost:15672/

```
docker run -d -t -it --hostname my-rabbitmq --name rabbitmq3-server -p 15672:15672 -p 5672:5672 -e RABBITMQ_DEFAULT_USER=user -e RABBITMQ_DEFAULT_PASS=password -e RABBITMQ_DEFAULT_VHOST=my_vhost rabbitmq:3-management
```

## Postgres database
- User: user
- Password: password
- Port: 5432

```
docker run -d --name some-postgres -p 5432:5432 -e POSTGRES_DB=database -e POSTGRES_USER=user -e POSTGRES_PASSWORD=password postgres
```