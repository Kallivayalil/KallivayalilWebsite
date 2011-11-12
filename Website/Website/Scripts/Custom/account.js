function edit() {
    var element = $("#viewAccountDetails");
    var element1 = $("#editAccountDetails");
    if (element.hasClass("hidden")) {
        element.removeClass("hidden");
        element1.addClass("hidden");
    }
    else {
        element.addClass("hidden");
        element1.removeClass("hidden");
    }
}



$(function () {
    $("#userUpdate").click(function () {
        edit();
        var person = getUserInfo();

        var constituent = JSON.stringify(person);

        $.ajax({
            url: "/AccountDetails/Save",
            type: "POST",
            datatype: "json",
            data: constituent,
            accept: "application/json",
            contentType: "application/json charset=utf-8",
        });

    });
});


function getUserInfo() {
    var password = $("#Password").val();
    var isAdmin = $("#IsAdmin").val();
    var id = $("#Id").val();
    var createdDateTime = $("#CreatedDateTime").val();
    var createdBy = $("#CreatedBy").val();

    return { Password: password, 
              IsAdmin: isAdmin,
              Id: id,
            CreatedDateTime: createdDateTime,
            CreatedBy:createdBy };
};