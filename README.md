# TODO Application
A simple micro-services todo application.

An unauthenticated user can create todo lists, add items to existing todo lists and
retrieve all/one todo list. These endpoints are documented using swagger.
Additionally a user can subscribe to a todo list and be notified
via push notifications of all items that are added to said list (which can 
be done via the signalRClient console app).

## Architecture
Vertical slice architecture is used in the todo and notifications
services (implemented with the mediatR library). Messages are sent
between the services using RabbitMQ following the eventual consistency
approach of Event-Driven Architecture. SignalR is used for the push
notifications and given its specific approach to websockets an additional
console application is included to subscribe to the specified lists (and 
therefore swagger has not been included in the notifications app, as it
contains no mappable HTTP endpoints).

## Infrastructure
Postgres and RabbitMQ are required for the application to run. These are included
in the docker-compose file in the root directory.

## Running the application
Postgres and RabbitMQ should be running before starting the application
and that can be done by running `docker-compose up` in the terminal
from the root directory.

### Terminal
To run the other projects are the following commands and in the recommended
order:
- Todos application `dotnet run --project Todos`
- Notifications application `dotnet run --project Notifications`
- SingalRClient application `dotnet run --project SignalRClient {todoListId}`

The todos application will launch swagger on startup.

## Testing
There are two end-to-end tests. They test against the development database
and test the `get` and `post` todos endpoint.