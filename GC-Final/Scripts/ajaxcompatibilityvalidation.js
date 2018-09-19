function checkCompatibility(partType) {

    //setup before functions
    var typingTimer;                //timer identifier
    var doneTypingInterval = 5000;  //time in ms, 5 second for example
    var $input = $('#myInput');

    //on keyup, start the countdown
    $input.on('keyup', function () {
        clearTimeout(typingTimer);
        typingTimer = setTimeout(doneTyping, doneTypingInterval);
    });

    //on keydown, clear the countdown 
    $input.on('keydown', function () {
        clearTimeout(typingTimer);
    });

    //user is "finished typing," do something
    function doneTyping() {

        var pid = document.getElementById(partType + "_search")

        $.ajax({
            url: 'api/Verify/GetID/',
            type: 'POST',
            data: JSON.stringify(pid),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (valid) {
                if (valid != null) {
                    console.log("Request 1 Succeeded");
                } else {
                    console.log("Request 1 Failed");
                }

                var data;

                data = {
                    "motherboard_id: " + document.getElementsByName("")
                }

                $.ajax({
                    url: 'api/Verify/CheckCompatibility/',
                    type: 'POST',
                    data: JSON.stringify(data),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (valid2) {
                        if (valid2 != null) {
                            console.log("Request 2 Succeeded");
                        } else {
                            console.log("Request 2 Failed");
                        }
                    }
                });
            }
        });
    }
}