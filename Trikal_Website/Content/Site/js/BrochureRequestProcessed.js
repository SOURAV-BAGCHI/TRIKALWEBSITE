$(document).ready(function () {
    var ListType = 4;
    //$("#BrochureRequestDetailsProcessed").DataTable();

    $("#BrochureRequestDetailsProcessed").DataTable();

    GetRequestCount();

    function GetRequestCount() {
        $.ajax({
            type: "GET",
            url: "/BrochureRequest/GetRequestCount",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

                if (parseInt(response, 10) > 0) {
                    $(".notification-badge").text(response);
                    $(".notification-badge").show();
                }
                else {
                    $(".notification-badge").text('');
                    $(".notification-badge").hide();
                }
            },
            error: function (response) {

            }
        });
    }
});