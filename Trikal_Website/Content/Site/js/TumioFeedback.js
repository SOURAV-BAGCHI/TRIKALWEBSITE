(function () {
    var IsPaginationActive = true;
    $(document).ready(function () {

    });

    function GetFeedback(Offset) {
        let Data = {'Offset':Offset};
        $.ajax({
            type: "GET",
            url: "",
            data: Data,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (response) {

            },
            error: function (response) {

            }
        });
    }

})();