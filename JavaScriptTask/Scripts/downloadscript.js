function validate() {
    var fileNumber = parseInt($("#textbox1").val());
    var MaxValue = parseInt($("#textbox2").val());

    $("#war1").remove();
    $("#war2").remove();

    if ((fileNumber == 0 | isNaN(fileNumber)) & (MaxValue == 0 | isNaN(MaxValue))) {
        $('<a id="war1"> Wprowadz ile liczb wygenerować.</a>')
            .appendTo('#myTable')
            .appendTo('#col1');
        $('<a id="war2"> Wprowadz maksymalną wartość.</a>')
            .appendTo('#myTable')
            .appendTo('#col2');
    }
    else if (fileNumber == 0 | isNaN(fileNumber)) {
        $('<a id="war1">Wprowadz ile liczb wygenerować.</a>')
            .appendTo('#myTable')
            .appendTo('#col1');
    }

    else if (MaxValue == 0 | isNaN(MaxValue)) {
        $('<a id="war2"> Wprowadz maksymalną wartość.</a>')
            .appendTo('#myTable')
            .appendTo('#col2');
    }
    else if (MaxValue < 0) {
        $('<a id="war2"> Maksymalna wartość musi być większa od zera.</a>')
            .appendTo('#myTable')
            .appendTo('#col2');
    }
    else if (MaxValue > 2147483647) {
        $('<a id="war2"> Maksymalna wartość musi być mniejsza niż 2147483647. </a>')
            .appendTo('#myTable')
            .appendTo('#col2');
    }
    else {
        $("#okButton").attr("disabled", true);
        $("#textbox1").attr("disabled", true);
        $("#textbox2").attr("disabled", true);
        $('<button type="button" id = "genButton" class="btn btn-default btn-sm"><span class="glyphicon glyphicon-paperclip"></span> Generuj</button>')
            .appendTo('.genbutton');
    }
}

function reset() {
    $("#war1").remove();
    $("#war2").remove();
    $("#okButton").attr("disabled", false);
    $("#textbox1").attr("disabled", false).val(0);
    $("#textbox2").attr("disabled", false).val(0);
    $('.genbutton').children().next().remove();
}

$(".Download").ready(function () {
    window.hwnd = window.setInterval(
        function () {
            console.info('ok');
        }, 10000);

});
var requestClientList = [];
var downloadedList = [];
var id = -1;
//$('.genButton').click( function () {
$(document).on('click', '#genButton', function () {
    var fileNumber = parseInt($("#textbox1").val());
    var MaxValue = parseInt($("#textbox2").val());
    id++;

    $.ajax({
        url: '/Home/RequestQueue',
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ file: { Id: id, FileAmount: fileNumber, MaxValue: MaxValue } }),
        async: true,
        processData: false,                                             
        cache: false,
        success: function (data) {
            if (data.success) {
                console.log("request send ");
                requestClientList.push(id);
            }
        },
        error: function (xhr) {
            console.log('error request', xhr.status);
        }
    });
});

$(".Download").ready(function () {
    aa = window.setInterval(function () {
        if (downloadedList.length == 0) {
            {
                clearInterval(aa);
                return;
            }
        }
        var s = downloadedList.pop();

        $.ajax({
            url: '/Home/SendDownload',
            type: 'POST',
            dataType: "JSON",
            data: { file: s },
            success: function (data) {
                if (data.success) {
                    console.log("Remove from server " + s);
                }
                else {
                    downloadedList.push(s);
                    console.log("Problem with remove " + s);
                }
            },
            error: function (xhr) {
                console.log("Error with remove " + s);
                downloadedList.push(s);
            }
        });
    }, 5000);
});