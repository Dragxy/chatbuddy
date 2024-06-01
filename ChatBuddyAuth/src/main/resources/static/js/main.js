"use strict";
var registrationPage = document.querySelector("#registration-page");
var loginForm= document.querySelector("#login-page")
var signupForm= document.querySelector("#signup-page")
var userPage=document.querySelector("#user-page")
var chatListContainer = document.querySelector("#chatlist-container");

var chatPage = document.querySelector("#chat-page");
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

function signin(event) {
  username = loginForm.querySelector(".username").value.trim();
  password = loginForm.querySelector(".password").value;
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

          registrationPage.classList.add("hidden");
          userPage.classList.remove("hidden");
          loadChats();
        })
        .catch(error => {
          console.log(error.message)
        });
  event.preventDefault();
}
function signup(event) {
  username = signupForm.querySelector(".username").value.trim();
  var email = signupForm.querySelector(".email").value.trim();
  password = signupForm.querySelector(".password").value;
  fetch('/api/auth/signup', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({ username: username, email: email, password: password })
  })
      .then(response => {
        if (response.ok) {
          return response.json();
        } else {
          throw new Error('Authentication failed');
        }
      })
      .then(data => {
        window.location.reload();
      })
      .catch(error => {
        console.log(error.message)
      });
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
        chatListContainer.innerHTML = ""; // Clear previous list items

        chats.forEach(chat => {
          const listItem = document.createElement("li");
          const chatButton = document.createElement("button");
          chatButton.textContent = chat.name;

          chatButton.addEventListener("click", function() {
            chatroomId = chat.id;
            var chatnameform = chatPage.querySelector("#chatname")
            chatnameform.textContent=chat.name;
            loadChat();
          });

          listItem.appendChild(chatButton);
          chatListContainer.appendChild(listItem);
        });
      })
      .catch(error => {

      });
}
function loadChat() {
  userPage.classList.add("hidden");
  chatPage.classList.remove("hidden");

  loadHistory();

  var socket = new SockJS("/ws"); // WebSocket connection URL
  stompClient = Stomp.over(socket);
  var headers = {
    "Authorization": "Bearer " + localStorage.getItem("jwtToken")
  };
  stompClient.connect(headers, onConnected, onError);
}
function inviteUser(username){
  const requestBody = {
    username: username
  };
  fetch('/api/info/inviteUser/'+chatroomId, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('jwtToken')
    },
    body: JSON.stringify(requestBody)
  })
      .then(response => {
        if (response.ok) {
          return response.json();
        } else {
          throw new Error('Authentication failed');
        }
      })
      .catch(error => {
        console.error('Error:', error);
      });
}
function leaveChat(username){
  const requestBody = {
    username: username
  };
  fetch('/api/info/leaveChat/'+chatroomId, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('jwtToken')
    },
    body: JSON.stringify(requestBody)
  })
      .then(response => {
        if (response.ok) {
          loadChats()
          return response.json();
        } else {
          throw new Error('Authentication failed');
        }
      })
      .catch(error => {
        console.error('Error:', error);
      });
}
function createChat(chatname){
  const requestBody = {
    chatName: chatname,
    username: username
  };

  fetch('/api/info/chats/add', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('jwtToken')
    },
    body: JSON.stringify(requestBody)
  })
      .then(response => {
        if (response.ok) {
          loadChats()
          return response.json();
        } else {
          throw new Error('Authentication failed');
        }
      })
      .catch(error => {
        console.error('Error:', error);
      });
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
        //clear messageArea
        messageArea.innerHTML=""
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
    const now = new Date();
    const formattedDateTime = now.toISOString().slice(0, 19);
    var chatMessage = {
      username: username,
      content: messageInput.value,
      type: "CHAT",
      publish_time: formattedDateTime,
    };
    stompClient.send("/app/chat.send/" + chatroomId, headers, JSON.stringify(chatMessage));
    messageInput.value = "";
  }
  event.preventDefault();
}

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

    var usernameElement = document.createElement("span");
    var usernameText = document.createTextNode(message.username);
    usernameElement.appendChild(usernameText);
    messageElement.appendChild(avatarElement);
    messageElement.appendChild(usernameElement);
    // * update
    usernameElement.style["color"] = getAvatarColor(message.username);
    //* update end
  }

  var textElement = document.createElement("p");
  var messageText = document.createTextNode(message.content);
  textElement.appendChild(messageText);


  // * update
  if (message.username === username) {
    // Add a class to float the message to the right
    messageElement.classList.add("own-message");
  }

  messageElement.appendChild(textElement);
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

messageForm.addEventListener("submit", send, true);
loginForm.addEventListener("submit", signin, true);
signupForm.addEventListener("submit", signup, true);

document.addEventListener('DOMContentLoaded', () => {
  const createChatButton = document.getElementById('createChatButton');
  const chatDialog = document.getElementById('chatDialog');
  const chatCloseButton = document.getElementById('chatCloseButton');
  const chatForm = document.getElementById('chatForm');

  createChatButton.addEventListener('click', () => {
    chatDialog.style.display = 'flex';
  });

  chatCloseButton.addEventListener('click', () => {
    chatDialog.style.display = 'none';
  });

  chatForm.addEventListener('submit', (e) => {
    e.preventDefault();
    const chatName = document.getElementById('chatNameToCreate').value;
    chatDialog.style.display = 'none';
    chatForm.reset();
    createChat(chatName)
  });

  window.addEventListener('click', (e) => {
    if (e.target == chatDialog) {
      chatDialog.style.display = 'none';
    }
  });

  const inviteUserButton = document.getElementById('inviteUserButton');
  const inviteDialog = document.getElementById('inviteDialog');
  const inviteCloseButton = document.getElementById('inviteCloseButton');
  const inviteForm = document.getElementById('inviteForm');

  inviteUserButton.addEventListener('click', () => {
    inviteDialog.style.display = 'flex';
  });

  inviteCloseButton.addEventListener('click', () => {
   inviteDialog.style.display = 'none';
  });

  inviteForm.addEventListener('submit', (e) => {
    e.preventDefault();
    const username = document.getElementById('autocomplete-input').value;
    inviteUser(username)
    inviteDialog.style.display = 'none';
    inviteForm.reset();
  });

  window.addEventListener('click', (e) => {
    if (e.target == inviteDialog) {
      inviteDialog.style.display = 'none';
    }
  });

  $('#autocomplete-input').devbridgeAutocomplete({
    serviceUrl: '/suggestion',
    paramName: 'searchstr',
    params: {
      get chattarget() {
        return chatroomId;
      }
    },
    minChars: 1,
    autoSelectFirst: true,
    beforeAjaxRequest: function (input, ajaxSettings) {
      ajaxSettings.headers = {
        'Authorization': 'Bearer ' + localStorage.getItem('jwtToken')
      };
    }
  });
  const returnUserPageButton = document.getElementById("returnUserPageButton")
  returnUserPageButton.addEventListener('click', (e) => {
    if (stompClient !== null) {
      stompClient.disconnect();
    }
    chatPage.classList.add("hidden");
    userPage.classList.remove("hidden");
    loadChats();
    chatroomId=null
  });
  const leaveChatButton = document.getElementById("leaveChatButton")
  leaveChatButton.addEventListener('click', (e) => {
    if (stompClient !== null) {
      stompClient.disconnect();
    }
    chatPage.classList.add("hidden");
    userPage.classList.remove("hidden");
    leaveChat(username)
    chatroomId=null

  });
});