$(document).ready(function () {

    var ListType = 1;
    $("#BrochureRequestDetails").DataTable();

    $('body').on('click', '.admin-action', function (){

        debugger;
        var BrochureRequestId = $(this).attr('BrochureRequestId');
    //    var LastUpdateByAdminId = this.attr('LastUpdateByAdminId');
        var MarkAsSpam = $(this).attr('MarkAsSpam');
        var SendBit = $(this).attr('SendBit');

        var Data = { "BrochureRequestId": BrochureRequestId, "MarkAsSpam": MarkAsSpam, "SendBit": SendBit }

        $.ajax({

            type: "POST",
            url: "/BrochureRequest/BrochureRequestInsert",
            data: JSON.stringify(Data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response)
            {
                debugger;
                if (response["Success"] != "0") {
//                    alert(response["Message"]);

                    var x = document.getElementById("snackbar");
                    x.innerHTML = response["Message"];
                    x.className = "show";
                    setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);


                    var Data2 = { "ListType": ListType };
                        
                    $.ajax({
                        type: "GET",
                        url: "/BrochureRequest/GetBrochureRequest/ListType",
                        data: Data2,
                        contentType: "application/json; charset=utf-8",
                        dataType: "html",
                        success: function (response) {
                            debugger;
                            $('#BrochureRequestResult').html(response);
                            $("#BrochureRequestDetails").DataTable();
                            GetRequestCount();
                        },
                        error: function (response) {

                        }

                    });

                }
                else {
//                    alert(response["Message"]);

                    var x = document.getElementById("snackbar");
                    x.innerHTML = response["Message"];
                    x.className = "show";
                    setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);

                }

            },
            error: function () {

            }

        });

    });

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