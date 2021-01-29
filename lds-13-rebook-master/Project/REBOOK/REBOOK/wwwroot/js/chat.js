"use strict";

let data = sessionStorage.getItem('access_token');
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub", { accessTokenFactory: () => data }).build();

function AddMessage(message) {
    var li = document.createElement("li");
    li.textContent = message;
    document.getElementById("messagesList").appendChild(li);
}

function GetString(id) {
    document.getElementById(id).value;
}

connection.on("ReceiveMessage", function (message, SenderName) {
    AddMessage(SenderName + ": " + message);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var destinatario =  parseInt(document.getElementById("userInput").value);
    var remetente = parseInt(document.getElementById("groupInput").value);
    var message = document.getElementById("messageInput").value;
    var d = new Date();

    connection.invoke("SendMessage", destinatario, remetente, message, d).catch(function (err) {
        return console.error(err.toString());
    });

    event.preventDefault();
    document.getElementById("messageInput").value = "";
});