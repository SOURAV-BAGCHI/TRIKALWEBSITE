$(document).ready(function () {

    var IsAdminPresetn = parseInt($("#AdminPresent").val(), 10);

  //  $('#BrochureFooter').show();

    $('body').on('click', '.close', function () {

        $('#id01').modal('hide');
    });

    $('body').on('click', '#AdminLoginDialogOpen', function () {

        $('#id01').modal('show');

    });

    $('body').on('click', '#arrowupdown', function () {

        var arrowpos = $(this).attr("arrowpos");

        if (arrowpos == '1') {
            $("#arrowupdown").attr("arrowpos", "0");
            $('#arrowupdown').removeClass('fa-angle-up');
            $('#arrowupdown').addClass('fa-angle-down');
            $('#SendBrochureForm').show('fast');
           // $(window).load(function () {
                $("html, body").animate({ scrollTop: $(document).height() }, 1000);
          //  });
        }
        else {
            $("#arrowupdown").attr("arrowpos", "1");
            $('#arrowupdown').removeClass('fa-angle-down');
            $('#arrowupdown').addClass('fa-angle-up');
            $('#SendBrochureForm').hide('fast');
        }
     
    });

    $('body').on('click', '#SendBrochure', function () {

        var Name = $('#name').val();
        var Email = $('#email').val();
        var SendBrochureButton = $("#SendBrochure");
        var Progressbar = $("#BrochureRequestLoader");

        var Valid = 1;
        var Message;
        if (Name.length == 0) {
            Valid = 0;
            Message = 'Please enter name';
            $('#namevalidationmessage').text(Message);
            $('#namevalidationmessage').show();
          

            setTimeout(function () {
                $('#namevalidationmessage').hide('fast')

            }, 3000);
        }
        if (!isEmail(Email) && Valid==1) {
            Valid = 0;
            Message = 'Please enter a valid email id';
            $('#emailvalidationmessage').text(Message);
            $('#emailvalidationmessage').show();


            setTimeout(function () {
                $('#emailvalidationmessage').hide('fast')

            }, 3000);

           
        }

        if (Valid == 1) {
            var data = { 'Name': Name, 'Email': Email, 'MarkAsSpam': false,'SendBit':false };
            if (Valid > 0) {
                SendBrochureButton.hide();
                Progressbar.show();

                $.ajax({
                    type: "POST",
                    url: "/BrochureRequest/BrochureRequestInsert",
                    data: JSON.stringify(data),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        debugger;
                        Progressbar.hide();
                        var RequestBrochureMessage = $("#requestsendmessage");
                        if (response["Success"] != "0") {
                            // alert(response["Message"]);
                            RequestBrochureMessage.text(response["Message"]);
                            RequestBrochureMessage.css("color", "green");
                            RequestBrochureMessage.show();

                            var arrowpos = $(this).attr("arrowpos");

                            setTimeout(function () {
                                RequestBrochureMessage.text('');
                                RequestBrochureMessage.hide();

                                if (arrowpos == '1') {
                                    $("#arrowupdown").attr("arrowpos", "0");
                                    $('#arrowupdown').removeClass('fa-angle-up');
                                    $('#arrowupdown').addClass('fa-angle-down');
                                    $('#SendBrochureForm').show('fast');
                                }
                                else {
                                    $("#arrowupdown").attr("arrowpos", "1");
                                    $('#arrowupdown').removeClass('fa-angle-down');
                                    $('#arrowupdown').addClass('fa-angle-up');
                                    $('#SendBrochureForm').hide('fast');
                                }

                                $('#name').val('');
                                $('#email').val('');
                                SendBrochureButton.show();
                               

                            }, 3000);

                            
                        }
                        else {
                            //alert(response["Message"]);

                            RequestBrochureMessage.text(response["Message"]);
                            RequestBrochureMessage.css("color", "red");
                            RequestBrochureMessage.show();

                            setTimeout(function () {
                                RequestBrochureMessage.text('');
                                RequestBrochureMessage.hide();
                                SendBrochureButton.show();
                            }, 5000);
                        }

                    },

                    error: function (response) {
                        alert(response.responseText);
                       
                    }
                });

               


            }
        }
        else {
         //   alert(Message);
        }
        return false;
    });

    $('body').on('click', '#login', function () {
       
        document.getElementById("mySidenav").style.width = "310px";
    });

    $('body').on('click', '.closebtn', function () {

        document.getElementById("mySidenav").style.width = "0";
        $('#password').val('');
        $('#emailphone').val('');
    })

    $('body').on('click', '#signuplogin', function (){

        var Password = $('#password').val();
        var EmailOrPhone = $('#emailphone').val();

        var Valid = 1;

        var Button = $('#signuplogin');
        var Progressbar = $('.lds-ring');
        var MessageResp = $("#messageresponse");

        debugger;

        var Message;
        if (EmailOrPhone.length == 0) {
            Valid = 0;
            Message = 'Please enter email or phone';
            $('#emailphonevalidationmessage').text(Message);
            $('#emailphonevalidationmessage').show();


            setTimeout(function () {
                $('#emailphonevalidationmessage').hide('fast')

            }, 3000);
        }
        if (Password.length==0 && Valid == 1) {
            Valid = 0;
            Message = 'Please enter password';
            $('#passwordvalidationmessage').text(Message);
            $('#passwordvalidationmessage').show();

            setTimeout(function () {
                $('#passwordvalidationmessage').hide('fast')

            }, 3000);


        }

        if (Valid == 1) {
            Button.hide();
            Progressbar.show();

            var data = { 'EmailOrPhone': EmailOrPhone, 'Password': Password};
            if (Valid > 0) {

                $.ajax({
                    type: "POST",
                    url: "/Home/AdminLogin",
                    data: JSON.stringify(data),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        debugger;
                        Progressbar.hide();
                        if (response["UserId"] != "") {
                            //  alert(response["Message"]);
                            //MessageResp.text("Login successful");
                            //MessageResp.show();
                            Progressbar.show();
                            $.ajax({
                                type: "GET",
                                url: "Home/GetAdminDropdownOption",
                                contentType: "application/json; charset=utf-8",
                                dataType:"html",
                                success: function (response) {

                                    debugger;
                                    $("#UserInfoDiv").html(response);
                                    Progressbar.hide();
                                    MessageResp.hide();
                                    Button.show();
                                    $("#mySidenav").hide();
                                    $("#BrochureFooter").html('');
                                    GetRequestCount();
                                },
                                error: function (response) {


                                }
                            });



                            //setTimeout(function () {
                            //    MessageResp.hide();
                            //    Button.show();
                            //    $("#mySidenav").hide();
                            //}, 5000);
                        }
                        else {
                            //alert(response["Message"]);
                            Button.show();
                            MessageResp.text("Login unsuccessful");
                            MessageResp.show();

                            setTimeout(function () {
                                MessageResp.hide();
                            }, 5000);
                        }

                    },

                    error: function (response) {
                        alert(response.responseText);

                    }
                });




            }

        } else {
            //   alert(Message);
        }
        return false;

    });

    


    if (IsAdminPresetn == 1) {
        GetRequestCount();
    }


    function GetRequestCount() {
        $.ajax({
            type: "GET",
            url: "/BrochureRequest/GetRequestCount",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

                if (parseInt(response,10) > 0) {
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

    function isEmail(email) {
        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        return regex.test(email);
    }

});