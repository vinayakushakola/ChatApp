function LoginForm() {
    var isValid = true;
    if ($('#email').val().trim() == "") {
        $('#email').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#email').css('border-color', 'lightgrey');
    }
    if ($('#password').val().trim() == "") {
        $('#password').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#password').css('border-color', 'lightgrey');
    }

    if (isValid == true) {
        var userData = {
            Email: $('#email').val(),
            Password: $('#password').val()
        };
        console.log("=============>", userData);
        $.ajax({
            url: "https://localhost:44304/api/user/login",
            data: JSON.stringify(userData),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                console.log("Login", result);
                localStorage.setItem("token", result.data.token);
                localStorage.setItem("id", result.data.id);
                console.log("id============>", result.data.id);
                var location = window.location.href = "./Template//Dashboard.html";
                console.log("location", location);

            },
            error: function (errormessage) {
                console.log("error", errormessage.responseText);
            }
        });
    }
    else {
        alert("Enter Register email & password");
    }
}

function RegisterForm() {
    var isValid = true;
    if ($('#firstName').val().trim() == "") {
        $('#firstName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#firstName').css('border-color', 'lightgrey');
    }
    if ($('#lastName').val().trim() == "") {
        $('#lastName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#lastName').css('border-color', 'lightgrey');
    }
    if ($('#email').val().trim() == "") {
        $('#email').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#email').css('border-color', 'lightgrey');
    }
    if ($('#password').val().trim() == "") {
        $('#password').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#password').css('border-color', 'lightgrey');
    }

    if (isValid == true) {
        console.log("=============", isValid);
        console.log("Register");
        var userData = {
            FirstName: $('#firstName').val(),
            LastName: $('#lastName').val(),
            Email: $('#email').val(),
            Password: $('#password').val(),
        };
        console.log("Register data", userData);
        $.ajax({
            url: "https://localhost:44304/api/user/registration",
            data: JSON.stringify(userData),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                console.log("Register", result);
            },
            error: function (errorMessage) {
                console.log("Error", errorMessage);
            }
        })
    }
    else {
        alert("Fill Required Fields");
    }
}
