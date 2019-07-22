var enabeledDisabled = function (id, status) {

    // var URl = "/Trans/DeleteMessage?id=" + id + "&FileName=" + filename;
    var URl = "../User/EnabledAccount";
    $("#error-div").hide();
    $("#view-err").empty();
    $.ajax({
        type: 'POST',
        url: URl, // comma here instead of semicolon   
        data: { Id: id, Status: status },
        success: function () {
            location.reload();
            $("#error-div").fadeIn("slow");
            $("#error-div").removeClass("alert-success");
            $("#error-div").addClass("alert-danger");
            if (status === "0") {
                $("#view-err").append("تم تعطيل الحساب");

            } else {
                $("#error-div").removeClass("alert-danger");
                $("#error-div").addClass("alert-success");
                $("#view-err").append("تم تفعيل الحساب");

            }
        },
        error: function () {
            $("#error-div").fadeIn("slow");
            $("#error-div").addClass("alert-danger");
            $("#view-err").append("حدث خطأ ");
        }
    }
    )
};