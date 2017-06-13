# Multithreaded-Client-Server-Chat-Application

A chat application which is written in C# where users can have group chats or private conversations. Simultaneously by using the concept Multithreading.
This application involves two core components, they are Server and Client. The server is multithreaded, as it needs to handle multiple clients (up to 30).

Initially the Server should be up and running when the user launches the chat application (Client). Then the client is prompt to enter a username and then checked whether it exists, if not he/she is connected to the server.

The chat window previews the contact list that has joined the chat room (Group Conversation) 

The Design,

![er](https://user-images.githubusercontent.com/22576836/27065856-89c47d74-501d-11e7-9013-d8996a4bc12c.png)

A simple client-server architecture is as follows,

As shown there is only one server to handle all the clients, the messages that are sent from the clients are not send to the destination client directly, it is sent to the server and then to the client.
•	In order to use the functionalities of Networking in C# we use C# Sockets.
•	The protocol that is being used here is TCP, it can be used to receive messages and send messages. 


A preview of the Server & Client GUI's - 

![c](https://user-images.githubusercontent.com/22576836/27064887-558ca7c6-5017-11e7-8b94-675c1a29e3e8.png)
![s](https://user-images.githubusercontent.com/22576836/27064888-558f4b16-5017-11e7-9866-3bafcd40c537.png)
