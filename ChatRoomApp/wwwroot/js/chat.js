"use strict";

const form = document.getElementById('chatFrm');
const sendButton = document.getElementById("sendBtn");

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    alert("ss");
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;

    const div = document.createElement('div');
    div.className = "msg right-msg"
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
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;

    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });

    postMessageAjax();
    event.preventDefault();

});

var postMessageAjax = () => {
    const formData = new FormData(form);
    console.log(form);
    fetch('/Home/AddMessage', {
        method: 'POST',
        body: formData
    })
    .then(response => response.json())
    .then(data => {
        // Handle the response if needed
    })
    .catch(error => {
        // Handle the error if needed
        conole.log(error);
    });
}


//var sendMessageAjax = () => {
    //sendButton.addEventListener('click', function (event) {
    //    event.preventDefault();
    //    alert("si");
    //    const formData = new FormData(form);
    //    console.log(form);
    //    fetch('/Home/AddMessage', {
    //        method: 'POST',
    //        body: formData
    //    })
    //    .then(response => response.json())
    //    .then(data => {
    //        // Handle the response if needed
    //    })
    //    .catch(error => {
    //        // Handle the error if needed
    //    });
    //});
//}

var dateTimeNow = () => {
    let date = new Date();
    let month = date.getMonth() + 1;
    let day = date.getDate();
    let year = `${date.getFullYear()} ${date.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric' })}`;
    let dateTimeNowResult = `${month}/${day}/${year}`;

    return dateTimeNowResult
}