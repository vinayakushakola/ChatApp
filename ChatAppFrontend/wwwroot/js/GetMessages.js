$(document).ready(function () {
    Receiver();
    AllMessages();

    function connect() {
        var webSocketProtocol = location.protocol == "https:" ? "wss:" : "ws:";
        var webSocketURI = webSocketProtocol + "//localhost:44304/ws";
        socket = new WebSocket(webSocketURI);
        socket.onopen = function () {
            console.log("Web Socket Connected.");
        };

        socket.onclose = function (event) {
            if (event.wasClean) {
                console.log('Disconnected.');
            } else {
                console.log('Connection lost.'); // for example if server processes is killed
            }
            console.log('Code: ' + event.code + '. Reason: ' + event.reason);
        };
        socket.onmessage = function (event) {
            recName = sessionStorage.getItem('name');
            console.log("Message received By: " + recName + ' message: ' + event.data);
            $('#chatArea #li').prepend('<li>' + event.data + '</li>');
        };
        socket.onerror = function (error) {
            console.log("Error: " + error);
        };
        /*  $('#messageToSend').keypress(function (e) {
              if (e.which != 13) {
                  return;
              }
              e.preventDefault();
              //var name = sessionStorage.getItem('name');
              var message = recName + ":" + $('#messageToSend').val();
              socket.send(message);
          });     */
    }
    connect();

    document.getElementById('sendButton').addEventListener("click", function () {
        var sendMessage = function (element) {
            console.log("Sending message", element.value);
            socket.send(element.value);
        }
        var message = document.getElementById('messageToSend');
        sendMessage(message);
    });
})

function Receiver() {
    var name = sessionStorage.getItem('name');
    console.log("name", name);
    recName = $('#chatBox').prepend('<h1>' + name + '</h1>').text();
    console.log("receiver name", recName);
    id = sessionStorage.getItem('id');
    console.log("id", id);
}

function SendMessage() {
    var userdata = {
        Message: $('#messageToSend').val(),
        ReceiverId: id
    };
    console.log("=============>", userdata);
    $.ajax({
        url: "https://localhost:44304/api/chat/SendMessage",
        data: JSON.stringify(userdata),
        type: "post",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        headers: { "Authorization": 'Bearer ' + localStorage.getItem('token') },
        success: function (result) {
            console.log("send message", result);
        },
        error: function (errormessage) {
            console.log("error", errormessage.responsetext);
        }
    });
}
function AllMessages() {
    recId = sessionStorage.getItem('id');
    console.log("========", recId);
    $.ajax({
        url: "https://localhost:44304/api/Chat/GetMessage/" + recId ,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        headers: { "Authorization": 'Bearer ' + localStorage.getItem('token') },
        success: function (result) {
            console.log("All Messages====================>", result);
            senId = localStorage.getItem('id');
            var reverse = (result.data).reverse();
            console.log("Reverse array", reverse);
            $.each(reverse, function (key, item) {
                if (item.receiverId == recId && item.senderId == senId || item.receiverId == senId && item.senderId == recId) {
                    console.log("=================>", item.receiverId);
                    // $('#chatArea').prepend(item.message+ '<br>')
                    if (item.receiverId == recId) {
                        $('#chatArea').prepend('<div style="text-align:right; color:blue; margin-right:10px; font-size:20px">' + item.message + '</div>')
                    }
                    else {
                        $('#chatArea').prepend('<div style="text-align:left; color:purple;margin-left:10px; font-size:20px">' + item.message + '</div>')
                    }
                }
            })
        },
        error: function (errormessage) {
            console.log("error", errormessage.responsetext);
        }
    })
}
