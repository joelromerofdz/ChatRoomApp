"use strict";

const form = document.getElementById('chatFrm');
const sendButton = document.getElementById("sendBtn");

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you
    // should be aware of possible script injection concerns.
    const messagesMainContainer = document.getElementsByClassName('msger-chat')[0];
    const div = document.createElement('div');
    let userName = document.getElementById("userName").value;
    let className = userName == user ? "msg right-msg" : "msg left-msg";

    div.className = className;//"msg right-msg"
    div.innerHTML = `
    <div class="msg-bubble">
        <div class="msg-info">
            <div class="msg-info-name">${user}</div>
            <div class="msg-info-time">${dateTimeNow()}</div>
        </div>
        <div class="msg-text">
            ${message}
        </div>
    </div>`;

    const messagesContainer = document.getElementById('box-chat');
    messagesContainer.appendChild(div);
    messagesMainContainer.scrollTop += 500;
    document.getElementById("messageInput").value = "";
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userName").value;
    var message = document.getElementById("messageInput").value;

    if (message === null || message.trim() === "") {
        alert("You have to add a message in the textbox.");
        return false;
    }

    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });

    postMessageAjax();
    event.preventDefault();

});

var postMessageAjax = () => {
    const formData = new FormData(form);
    const botMessage = document.getElementById("bot-message");

    fetch('/Home/AddMessage', {
        method: 'POST',
        body: formData
    })
    .then(response => response.json())
    .then(data => {
        botMessage.innerHTML = data?.BotMessage;
    })
    .catch(error => {
        console.log(error);
    });
}

var dateTimeNow = () => {
    let date = new Date();
    let month = date.getMonth() + 1;
    let day = date.getDate();
    let year = `${date.getFullYear()} ${date.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric' })}`;
    let dateTimeNowResult = `${month}/${day}/${year}`;

    return dateTimeNowResult
}