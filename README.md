
<div  align="center">

<img  src="https://scontent.fszg1-1.fna.fbcdn.net/v/t39.30808-6/302132769_441825131304131_1597088261001296784_n.png?_nc_cat=108&ccb=1-7&_nc_sid=5f2048&_nc_ohc=jy4LL5286wMQ7kNvgHQhUDe&_nc_ht=scontent.fszg1-1.fna&oh=00_AYCygu4IKqSgBxd9OyN3VwpfnBfIXpi9Ly8t760RcgSF7w&oe=66627005">

</div>

  

# Welcome to ChatBuddy!

[ChatBuddy](https://github.com/Dragxy/chatbuddy) is an [open-source](https://de.wikipedia.org/wiki/Open_Source), simple chat application intended for non-commercial use. It features a [Spring Boot](https://spring.io/projects/spring-boot) backend, as well as a web server and a [Microsoft WPF](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/overview/?view=netdesktop-8.0) frontend, and uses a [MongoDB](https://www.mongodb.com/) database for storage.

  
  

# Overview

```mermaid

flowchart TD

subgraph Backend["Backend"]

M["MongoDB Database"]

D["Web Server"]

A["Spring Boot"]

end

subgraph Clients["Clients"]

C["WPF Client"]

E["Browser Client"]

end

C --> A

A --> M

E --> A

D --> E

style M stroke:#D50000

style D stroke:#2962FF

style A stroke:#00C853

style C stroke:#AA00FF

style E stroke:#2962FF

```

Chat messaging occurs between the Spring Boot backend and the clients. While the WPF application is hosted by the user independently, the Web-Server is hosted by the backend itself. The MongoDB database is exclusively accessed by the Spring Boot backend. *(A more detailed description can be found under Implementation)*

  

# Getting started

  

To use ChatBuddy, you must first set up a local MongoDB database server or configure Spring Boot to use a cloud-based MongoDB instance. The local MongoDB server can be downloaded from [here](https://www.mongodb.com/try/download/community). Once the database server is set up, ensure it is running on the correct port, or modify the Spring Boot configuration accordingly. The default port is `27017`.

  

After setting up the database server, you can run the ChatBuddy application. It should immediately connect to the database server and create the `chatbuddy` database, along with the default collection of users and the default global chat. If this does not happen, check the application"s permissions. If issues persist, create the necessary collections manually. (Default data can be found in the `DataInitializer` class.)

  

Now you can let the chatting begin! To do so, start any browser and connect to the IP address of your server, or launch the WPF application. And again, ensure that everything is running on the correct ports or adjust the configurations as needed. The default port configured is `8080`.

  

# How to use

*(All frontend features are available in both the WPF and Web versions)*

## Registration-Page

<div  style="display: flex; justify-content: center;">

<img  src="./markdown_docu_images/SignUp.PNG"  style="margin-right: 10px;">

<img  src="./markdown_docu_images/Login.PNG">

</div>

On the first page the user is confronted with the Login/SignUp page, where he may enter his credentials to access his user or, create one, if he has not done so before. The page will throw an Error, if the username or the email is already being used or the user enters bad credentials. Once logged in the user can take free use of all the features, unless his authoraztion token expires. (Default value configured is: `24h`).

  

## User-Page

<div  style="display: flex; justify-content: center; flex-wrap: nowrap;">

<img  src="./markdown_docu_images/User1.PNG"  style="margin-right: 2px; max-width: 33%;">

<img  src="./markdown_docu_images/User2.PNG"  style="margin-right: 2px; max-width: 34%;">

<img  src="./markdown_docu_images/User3.PNG"  style="max-width: 33%;">

</div>

This takes the user to the User page, where he is presented with an overview of all his chats aswell as the opportunity to create more, which pops up another dialog. With a simple button click he can navigate to a specific chat, which will get him to the Chat-Page.

  

## Chat-Page

<img  src="./markdown_docu_images/Chat1.PNG">

On the Chat-Page the user is able to communicate with all other members in the chat, by writing in the textbox below and pressing send. His own messages will be shown on the right, event messages in the center and messages from other users on the left.

  
  

<div  style="display: flex; justify-content: center;">

<img  src="./markdown_docu_images/Chat2.PNG"  style="margin-right: 2px; max-width: 50%;">

<img  src="./markdown_docu_images/Chat3.PNG"  style="max-width: 50%;">

</div>

  

The user is also able to invite/add other user to the channel. By pressing the according button, the user will be presented with a new dialog, where he must enter the username of the user he wants to invite. The dialog automatically displays suggestions based on the searched text. The user may also leave the dedicated chatroom or go back to the User-Page to chat in another chatroom.

  

# Implementation

This part of the documentation will go further into the actual implementation of ChatBuddy.

## Frontend

The frontend of ChatBuddy has 2 variations which provide the same features.

- The WebApp which is hosted on the Backend-Server and allows the User to use ChatBuddy with any browser without needing to download any application.

- The WPF-App which can be downloaded from the user to provide an on desktop-experience for those who do not want to run it on the browser.

## Backend

The backend of ChatBuddy consists of 3 components.

- The Rest-API which is the target of client Api-calls to retrieve, modify or add data to the database.

- The Websocket-Broker which is used for the actual chatting. It is needed to synchronise chat messages and send them to all online users in the chat.

- The MongoDB-Database Server which is used to permanetly store user information. It also stores the message history of all chatrooms, so they are not lost on a shutdown of the server.

- The WebServer which hosts the Web-Frontend of ChatBuddy.

## SignUp-Procedure

To fully explain how ChatBuddy works, we need to take a closer look into how some actions are processed.

```mermaid

sequenceDiagram

autonumber

loop until signed up or quit

Client->>Server: SignUp Request

Server->>Server: Check if user exists (Make DB Call)

alt exists

Server->>Client: SignUp accepted (JWT-Token)

else does not exist

Server->>Client: SignUp refused

end

end

```
`/api/auth/signup` [POST]

##### Headers:
  ```json
{
	"Content-Type": "application/json"
}
  ```

##### Body:
  ```json
{
	"username": "Tyrion",
	"email": "tyrion.lannister@got.com",
	"password": "Lannister"
}
  ```
##### Return Value:
  ```json
{
    "User registered successfully!"
}
  ```
On sign-up the client sends the user credentials to the server which checks the database, if the username or email are already registered and responds accordingly.

## Login-Procedure
```mermaid

sequenceDiagram

autonumber

loop until logged in

Client->>Server: Login Request

Server->>Server: Check Credentials (Make DB Call)

alt is valid

Server->>Client: Login accepted (JWT-Token)

else is unvalid

Server->>Client: Login refused

end

end

```
`/api/auth/signin` [POST]

##### Headers:
  ```json
{
	"Content-Type": "application/json"
}
  ```

##### Body:
  ```json
{
	"username": "Tyrion",
	"password": "Lannister"
}
  ```
##### Return Value:
  ```json
{
    "token": "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJUeXJpb24gTGFubmlzdGVyIiwiaWF0IjoxNzE3MzY3NTI1LCJleHAiOjE3MTc0NTM5MjV9.DfpmmSf_OEdtCLblREy11I_fdcdJtYOxNWrYmaA7PYs",
    "id": "665cb37de0a434198cce5260",
    "username": "Tyrion Lannister",
    "email": "tyrion.lannister@got.com",
    "roles": [
        "ROLE_USER"
    ],
    "tokenType": "Bearer"
}
  ```
The login is the most important procedure as it retrieves the **JWT-Token** (SesssionToken), which is required for all further procedures for authorization. The JWT-Token is generated in the backend, based on the users information and can not be reverse-engineered (without a key in the backend), so it is perfect to ensure security. It expires after a set amount of time.

## UserPage-Procedure

```mermaid

sequenceDiagram

autonumber

Client->>Server: Request UserData

Server->>Server: Check JWT-Token

alt is valid

Server->>Server: retrieve Userdata (Make DB call)

Server->>Client: return Userdata

else is unvalid

Server->>Client: return Unauthorized Error

end
```

`/api/info/{username}` [GET]

##### Headers:
  ```json
{
	"Content-Type": "application/json",
	"Authorization": "Bearer eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJUeXJpb24gTGFubmlzdGVyIiwiaWF0IjoxNzE3MzY3NTI1LCJleHAiOjE3MTc0NTM5MjV9.DfpmmSf_OEdtCLblREy11I_fdcdJtYOxNWrYmaA7PYs"
}
  ```

##### Body:
  ```json
{}
  ```
##### Return Value:
  ```json
    {
    "id": "665cb37de0a434198cce5260",
    "username": "Tyrion Lannister",
    "chatrooms": [
        {
            "id": "665c517eb42c0b1dc8013998",
            "name": "global",
        },
        {
            "id": "665cb3f1e0a434198cce5261",
            "name": "GoT Fanclub",
        }
    ]
}
  ```
On load of the User Page the client requests the user data from the Api and visualizes it. It requires the JWT-Token from the login.

## ChatPage-Procedure

```mermaid

sequenceDiagram

autonumber

Client->>Server: Request ChatData

Server->>Server: Check JWT-Token

alt is valid

Server->>Server: retrieve Chatdata (Make DB call)

Server->>Client: return Chatdata

else is unvalid

Server->>Client: return Unauthorized Error

end

```
`/api/info/chat/{chatroomId}` [GET]

##### Headers:
  ```json
{
	"Content-Type": "application/json",
	"Authorization": "Bearer eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJUeXJpb24gTGFubmlzdGVyIiwiaWF0IjoxNzE3MzY3NTI1LCJleHAiOjE3MTc0NTM5MjV9.DfpmmSf_OEdtCLblREy11I_fdcdJtYOxNWrYmaA7PYs"
}
  ```

##### Body:
  ```json
{}
  ```
##### Return Value:
  ```json
    {
    "id": "665cb3f1e0a434198cce5261",
    "name": "GoT Fanclub",
    "messages": [
        {
            "content": "Tyrion Lannister joined!",
            "username": "Tyrion Lannister",
            "publish_time": "2024-06-02T20:03:29.583",
            "type": "JOIN"
        },
        {
            "content": "Jon Snow joined!",
            "username": "Jon Snow",
            "publish_time": "2024-06-02T20:14:21.505",
            "type": "JOIN"
        },
        {
            "content": "Hey, Tyrion!",
            "username": "Jon Snow",
            "publish_time": null,
            "type": "CHAT"
        },
        {
            "content": "What's up, Jon?",
            "username": "Tyrion Lannister",
            "publish_time": "2024-06-02T18:17:46",
            "type": "CHAT"
        },
        {
            "content": "Not much. Can you maybe add Daenerys?",
            "username": "Jon Snow",
            "publish_time": null,
            "type": "CHAT"
        },
        {
            "content": "Sure thing!",
            "username": "Tyrion Lannister",
            "publish_time": "2024-06-02T18:18:19",
            "type": "CHAT"
        },
        {
            "content": "Daenerys Targaryen joined!",
            "username": "Daenerys Targaryen",
            "publish_time": "2024-06-02T20:23:54.922",
            "type": "JOIN"
        },
        {
            "content": "Hey guys!",
            "username": "Daenerys Targaryen",
            "publish_time": null,
            "type": "CHAT"
        }
    ]
}
  ```
On selection of a chat the client requests the specific chat data from the Api and visualizes it. It requires the JWT-Token from the login. It also loads the chat history.

  

## Messaging-Procedure

```mermaid

sequenceDiagram

autonumber

Client->>Server: Connect to Websocket (Broker)

Server->>Server: Check JWT-Token

alt is valid

Server->>Client: accept Connection

loop while not disconnected

Client->>Server: send message

Server->>Server: Check JWT-Token

alt is valid

Server->>Server: Save to DB

Server->>Client: send to all Clients in Chat

else is unvalid

Server->>Client: return Unauthorized Error

end

end

else is unvalid

Server->>Client: refuse Connection

end

```
`ws://localhost:8080/ws/websocket`[WEB SOCKET CONNECTION POINT]
`/topic/{chatroomId}` [WEB SOCKET BROKER SUBSCIRBE TO]
`/app/chat.send/{chatroomId}` [WEBSOCKET ENPOINT]

#### Sending:
##### Headers:
  ```json
{
	"Authorization": "Bearer eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJUeXJpb24gTGFubmlzdGVyIiwiaWF0IjoxNzE3MzY3NTI1LCJleHAiOjE3MTc0NTM5MjV9.DfpmmSf_OEdtCLblREy11I_fdcdJtYOxNWrYmaA7PYs"
}
  ```

##### Body:
  ```json
{
		"content": "What's up, Jon?",
      "username": "Tyrion Lannister",
      "publish_time": {
        "$date": "2024-06-02T16:17:46.000Z"
      },
      "type": "CHAT"
}
  ```

#### Receiving (from Subscription):
##### Return Value:
  ```json
  {
		"content": "Not so much, you?",
      "username": "Jon Snow",
      "publish_time": {
        "$date": "2024-06-02T16:17:52.000Z"
      },
      "type": "CHAT"
}
  ```
On selection of a chat the client tries to establish a websocket connection to the server. While this connection persists each message sent from the client is checked for its authorization, saved to the database and then sent to the broker which forwards it to all online users in the chat. (At the same time the user user listens for messages from the broker sent from other users)


## InviteUser

```mermaid

sequenceDiagram

autonumber

Client->>Server: Request UserInvite

Server->>Server: Check JWT-Token

alt is valid

Server->>Server: Check if User exists (Make DB call)
alt exists
Server->>Server: Check if User is not already in chat (Make DB call)
alt is not
Server->>Server: inviteUserToChat (Make DB call)

Server->>Client: return SuccessMessage
Server->>Client: send JoinMessage to all chats(from Broker)
else is
Server->>Client: return FailureMessage (User already in Chat)
end

else doesn not exist
Server->>Client: return FailureMessage (User does not exist)
end

else is unvalid
Server->>Client: return Unauthorized Error
end

```
`/api/info/inviteUser/{chatroomId}` [PUT]

##### Headers:
  ```json
{
	"Content-Type": "application/json",
	"Authorization": "Bearer eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJUeXJpb24gTGFubmlzdGVyIiwiaWF0IjoxNzE3MzY3NTI1LCJleHAiOjE3MTc0NTM5MjV9.DfpmmSf_OEdtCLblREy11I_fdcdJtYOxNWrYmaA7PYs"
}
  ```

##### Body:
  ```json
{
    "Username": "Jon Snow"
}
  ```
##### Return Value:
  ```json
{
    "User was successfully added to chatroom!"
}
  ```
To invite/add another user to a selected chatroom the clients sends an Api request to the Infocontroller. It requires the Authorizationtoken as a header, the username of the user that is the target if the invitation in the body and the chatroomId of the chatroom that the user is invited to in the url. It returns a simple Message that informs on the success of the action. Furthermore the Server automatically sends an event message to all users in the chat, that the new user has joined.

  
# Troubleshooting
## Common Issues
### ChatBuddy does not create database structure?
ChatBuddy should on its first start automatically create the database structure consisting of the database `chatbuddy`, and the collection `roles`, `chatrooms` (including the `global` chat). If this is not the case, ChatBuddy might not have the needed permissions or can not access the MongoDB-Server. Doublecheck the Mongo-Connectionstring and if ChatBuddy has all the necessary permissions of the os.
### ChatBuddy immediatly crashes on start?
If ChatBuddy immediatly crashes on start, look in the console and the logs. There might be a missing packages or dependency or a different JDK installed, in which case these must be installed accordingly. (It can also help, to just Reload the Maven project.)
If there is still in issue, check the ports again. Another service might already run on that port, preventing ChatBuddy from starting. The default ChatBuddy port is: `8080` and the default MongoDB-Server port is `27017`.
### WebClient fails to send requests to Server?
If the WebClient repeatedly fails to make requests to the server and is not able to establish a connection, please validate, that the url or the ip of the server is correct, and the HTTP protocol is not blocked by any security measures such as a firewall.
If it still does not work, try clearing the browser chache. This should fix all issues.
# Notes from Developer
## During Development
During development, many changes and different ideas were experimented with. Initially, Kafka was tried, but that idea was scrapped as the project did not need to be scaled to that extent, and it would have required unnecessary research. Therefore, it defaulted back to a simple WebSocket broker of Spring Boot. Furthermore, if there had been more time, an Angular web server would have been implemented instead of the basic HTML/CSS one. Despite the limited timeframe and resources, the development process was a valuable learning experience. The most important features were successfully implemented, and the application has a solid foundation to be built upon. Future improvements could include enhancing the user interface, adding more advanced features, and optimizing performance to handle a larger user base. The project demonstrates the potential of combining different technologies to create a functional and efficient chat application.

## Choice of Technologies
The technologies and programming languages used in this project were chosen based on the developer"s experience, as well as their usefulness and functionality to satisfy the assignment as efficiently as possible. Spring Boot was selected for the backend because it provides a simple REST API, easy database integration, and an integrated web server for a simple HTML webpage. MongoDB was used as the database server because it is document-oriented, making it easy to expand and experiment with, and the performance of a relational database model was not required. WPF was chosen as the second frontend as part of the given specifications.

## My Motivation

This application was developed as part of a school project. The primary goal was to satisfy all the requirements of the assignment. Therefore, it should be treated as such. There is a lot to improve and expand, yet there was a limited amount of time. With the assignment due, there will most likely be no further development on this project. The goal was never to replace any of the established chat services out there, but to provide educational insight into the technologies used and how to implement them. And if it helped any other programmer out there, you"re most welcome ; ).

# Ressources

[Materialize](http://materializecss.com)

  

[WPF](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/overview/?view=netdesktop-8.0)

  

[Netina Stomp Client](https://github.com/Netina/Netina.Stomp.Client)

  

[Spring Boot](https://spring.io/projects/spring-boot)

  

[MongoDB](https://www.mongodb.com/docs/)







