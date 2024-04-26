"use strict";

var usernamePage = document.querySelector("#username-page");
var chatListPage = document.querySelector("#chatlist-page");
var chatListPageContainer = document.querySelector("#chatlist-page-container");
var chatPage = document.querySelector("#chat-page");
var usernameForm = document.querySelector("#usernameForm");
var messageForm = document.querySelector("#messageForm");
var messageInput = document.querySelector("#message");
var messageArea = document.querySelector("#messageArea");
var connectingElement = document.querySelector(".connecting");

var stompClient = null;
var username = null;
var password = null;
var chatroomId = null;


var colors = [
  "#2196F3",
  "#32c787",
  "#00BCD4",
  "#ff5652",
  "#ffc107",
  "#ff85af",
  "#FF9800",
  "#39bbb0",
  "#fcba03",
  "#fc0303",
  "#de5454",
  "#b9de54",
  "#54ded7",
  "#54ded7",
  "#1358d6",
  "#d611c6",
];

function connect(event) {
  username = document.querySelector("#name").value.trim();
  password = document.querySelector("#password").value;
  if (username) {
    // Send HTTP POST request to authenticate user
    fetch('/api/auth/signin', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ username: username, password: password })
    })
        .then(response => {
          if (response.ok) {
            return response.json();
          } else {
            throw new Error('Authentication failed');
          }
        })
        .then(data => {
          // Authentication successful
          const jwtToken = data.token;
          localStorage.setItem('jwtToken', jwtToken);

          let mes = document.getElementById("mes");
          mes.innerText = "Authentication successful: ";

          usernamePage.classList.add("hidden");
          chatListPage.classList.remove("hidden");
          loadChats();
        })
        .catch(error => {
          // Authentication failed
          let mes = document.getElementById("mes");
          mes.innerText = "Error: " + error.message;
        });
  }
  event.preventDefault();
}
function loadChats(){
  fetch('/api/info/user/'+username, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer '+localStorage.getItem('jwtToken')
    },
  })
      .then(response => {
        if ( response.status === 401) {
          throw new Error('Unauthorized');
        } else if (response.ok) {
          return response.json();
        } else {
          throw new Error('Something went wrong');
        }
      })
      .then(data => {
        // Authentication successful
        var chats = data.chatrooms;
        chatListPageContainer.innerHTML = ""; // Clear previous list items

        chats.forEach(chat => {
          // Create a list item for the chat
          var listItem = document.createElement("li");

          // Create a span for the chat name
          var chatNameSpan = document.createElement("span");
          chatNameSpan.textContent = chat.name;

          // Create a "Load" button for the chat
          var loadButton = document.createElement("button");

          // Add click event listener to the "Load" button
          loadButton.addEventListener("click", function() {
            loadChat(chat.id); // Assuming you have a function to load a chat by its ID
          });

          // Append the chat name and "Load" button to the list item
          listItem.appendChild(chatNameSpan);
          listItem.appendChild(loadButton);

          // Append the list item to the chat list
          chatListPageContainer.appendChild(listItem);
        });
      })
      .catch(error => {

      });
}
function loadChat(id) {
  usernamePage.classList.add("hidden");
  chatListPage.classList.add("hidden");
  chatPage.classList.remove("hidden");
  chatroomId = id;

  loadHistory();

  var socket = new SockJS("/ws"); // WebSocket connection URL
  stompClient = Stomp.over(socket);
  var headers = {
    "Authorization": "Bearer " + localStorage.getItem("jwtToken")
  };
  stompClient.connect(headers, onConnected, onError);
}

function loadHistory(){
  fetch('/api/info/chat/'+chatroomId, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      "Authorization": "Bearer " + localStorage.getItem("jwtToken")
    }
  })
      .then(response => {
        if (response.ok) {
          return response.json();
        } else {
          throw new Error('Authentication failed');
        }
      })
      .then(data => {
        console.debug(data)
        data.messages.forEach(message =>{
          var messageElement = document.createElement("li");

          if (message.type === "JOIN") {
            messageElement.classList.add("event-message");
            message.content = message.username + " joined!";
          } else if (message.type === "LEAVE") {
            messageElement.classList.add("event-message");
            message.content = message.username + " left!";
          } else {
            messageElement.classList.add("chat-message");

            var avatarElement = document.createElement("i");
            var avatarText = document.createTextNode(message.username[0]);
            avatarElement.appendChild(avatarText);
            avatarElement.style["background-color"] = getAvatarColor(message.username);

            messageElement.appendChild(avatarElement);

            var usernameElement = document.createElement("span");
            var usernameText = document.createTextNode(message.username);
            usernameElement.appendChild(usernameText);
            messageElement.appendChild(usernameElement);
            // * update
            usernameElement.style["color"] = getAvatarColor(message.username);
            //* update end
          }

          var textElement = document.createElement("p");
          var messageText = document.createTextNode(message.content);
          textElement.appendChild(messageText);

          messageElement.appendChild(textElement);
          // * update
          if (message.username === username) {
            // Add a class to float the message to the right
            messageElement.classList.add("own-message");
          } // * update end
          messageArea.appendChild(messageElement);
          messageArea.scrollTop = messageArea.scrollHeight;
        })
      })
      .catch(error => {
        // Authentication failed
        let mes = document.getElementById("mes");
        mes.innerText = "Error: " + error.message;
      });
}
function onConnected() {
  var headers = {
    "Authorization": "Bearer " + localStorage.getItem("jwtToken")
  };
  stompClient.subscribe("/topic/" + chatroomId, onMessageReceived, headers);

  /*stompClient.send(
      "/app/chat.register/"+chatroomId,
      headers,
      JSON.stringify({ username: username, type: "JOIN" })
  );*/

  connectingElement.classList.add("hidden");
}

function onError(error) {
  connectingElement.textContent =
    "Could not connect to WebSocket! Please refresh the page and try again or contact your administrator.";
  connectingElement.style.color = "red";
}

function send(event) {
  var headers = {
    "Authorization": "Bearer " + localStorage.getItem("jwtToken")
  };
  var messageContent = messageInput.value.trim();
  if (messageContent && stompClient) {
    var chatMessage = {
      username: username,
      content: messageInput.value,
      type: "CHAT",
    };
    stompClient.send("/app/chat.send/" + chatroomId, headers, JSON.stringify(chatMessage));
    messageInput.value = "";
  }
  event.preventDefault();
}
/**
 * Handles the received message and updates the chat interface accordingly.
 * param {Object} payload - The payload containing the message data.
 */
function onMessageReceived(payload) {
  var message = JSON.parse(payload.body);

  var messageElement = document.createElement("li");

  if (message.type === "JOIN") {
    messageElement.classList.add("event-message");
    message.content = message.username + " joined!";
  } else if (message.type === "LEAVE") {
    messageElement.classList.add("event-message");
    message.content = message.username + " left!";
  } else {
    messageElement.classList.add("chat-message");

    var avatarElement = document.createElement("i");
    var avatarText = document.createTextNode(message.username[0]);
    avatarElement.appendChild(avatarText);
    avatarElement.style["background-color"] = getAvatarColor(message.username);

    messageElement.appendChild(avatarElement);

    var usernameElement = document.createElement("span");
    var usernameText = document.createTextNode(message.username);
    usernameElement.appendChild(usernameText);
    messageElement.appendChild(usernameElement);
    // * update
    usernameElement.style["color"] = getAvatarColor(message.username);
    //* update end
  }

  var textElement = document.createElement("p");
  var messageText = document.createTextNode(message.content);
  textElement.appendChild(messageText);

  messageElement.appendChild(textElement);
  // * update
  if (message.username === username) {
    // Add a class to float the message to the right
    messageElement.classList.add("own-message");
  } // * update end
  messageArea.appendChild(messageElement);
  messageArea.scrollTop = messageArea.scrollHeight;
}

function getAvatarColor(messageSender) {
  var hash = 0;
  for (var i = 0; i < messageSender.length; i++) {
    hash = 31 * hash + messageSender.charCodeAt(i);
  }

  var index = Math.abs(hash % colors.length);
  return colors[index];
}

usernameForm.addEventListener("submit", connect, true);
messageForm.addEventListener("submit", send, true);
