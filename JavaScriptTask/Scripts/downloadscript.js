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
        $('<a id="war2"> "Maksymalna wartość musi być większa od zera.</a>')
            .appendTo('#myTable')
            .appendTo('#col2');
    }
    else if (MaxValue > 2147483647) {
        $('<a id="war2"> "Maksymalna wartość musi być mniejsza niż 2147483647." </a>')
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
                }, 5000);
    
});


$(document).on('click', '#genButton', function () {
    var fileNumber = parseInt($("#textbox1").val());
    var MaxValue = parseInt($("#textbox2").val());
    var id = Date.now();

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
            console.log("request send" );
        },
        error: function (xhr) {
            console.log('error request',xhr.status);
        }
    });
});



$(".Download").ready(
    function () {
        window.aa = window.setInterval(
            function () {

                var getUrl = '@Url.Action("DownloadQueue","Home")';

                $.ajax({
                    url: '/Home/DownloadQueue',
                    dataType: "json",
                    data: JSON.stringify({ file: { Id: id, FileAmount: fileNumber, MaxValue: MaxValue } }),
                    type: "GET",
                    success: function (data) {
                        //console.log("chce odebrac");

                        if (data.success)
                           window.location = getUrl + "?fileName=" + data.fName;
                    },
                    error: function (xhr) {
                        console.log("chce odebrac ale nie moge",xhr.status, xhr.statusText);
                    }

                });
            },5000)
    });

