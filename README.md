Prerequisites

    Docker installed and running

Start Redis with Docker

Run the following command to start a Redis container:

docker run -d --name redis -p 6379:6379 redis

This will:

    Pull the latest official Redis image (if not already available)

    Start Redis in a background container

    Expose Redis on localhost:6379

Verify Redis is Running

To enter the Redis CLI inside the running container:

docker exec -it redis redis-cli


Stop & Remove Redis (Optional)

If you want to stop and clean up the container:

docker stop redis
docker rm redis