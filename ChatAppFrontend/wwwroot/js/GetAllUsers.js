$(document).ready(function () {
    AllUsers();
})


function userListClick(name, receiverId) {
    sessionStorage.setItem('name', name);
    sessionStorage.setItem('id', receiverId);
}


function AllUsers() {
    $.ajax({
        url: "https://localhost:44304/api/User",
        data: JSON.stringify(),
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            console.log("result", result);
            $.each(result.data, function (key, item) {
                console.log("Id======", item.id);
                $('#users #lists').append('<li onClick="userListClick(\'' + item.firstName + '\', \'' + item.id + '\')"><a href="Message.html"><h3>' + item.firstName + '</h3></a></li>')
            })
        },
        error: function (error) {
            console.log("Error", error);
        }
    })
};
function SignOut() {
    localStorage.clear();
    window.location.href = "/";
}
