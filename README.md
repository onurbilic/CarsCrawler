# CarsCrawler Sample of Scraping Data Cars.com 
### not commercial, just an example

#### Sample .Net Core Data Scraping application with based on a simplified microservices architecture and Docker containers.

## Getting Started

Make sure you have [installed](https://docs.docker.com/docker-for-windows/install/) and configure docker in your environment. After that, you can run the below commands from the **/src/** directory and get started with the `CarsCrawler` immediately.

###### PowerShell
```powershell
docker-compose build
docker-compose up
```
You should be able to browse different components of the application by using the below URLs :

```
- Crawler API : http://host.docker.internal:5010/
- RabbitMq Manager :  http://host.docker.internal:15672/ _(username : rabbituser, password: passw0rd1)_
```

You should start from API for trigger to scraping data. You can use search method. Here is the example request below.

```
{
  "stockType": "used",
  "makes": "tesla",
  "models": "tesla-model_s",
  "price": "100000",
  "distance": "all",
  "zip": "94596"
}
```

### Architecture overview

This sample application is cross-platform at the server, API services capable of running on Linux or Windows containers depending on your Docker host. But Consumer works only docker windows because of CefSharp.
The architecture proposes a microservice oriented architecture  implementing different approaches with DDD, EED patterns supports asynchronous communication for data collection  based on Integration Events and an Event Bus (Masstransit,RabbitMQ)



