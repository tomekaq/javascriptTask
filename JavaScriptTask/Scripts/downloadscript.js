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
        $('<input type="button" id="genButton" value= "Generuj"/>')
            .appendTo('.genbutton');
    }
}
//$('#genButton').click()



function reset() {
    $("#war1").remove();
    $("#war2").remove();
    $("#okButton").attr("disabled", false);
    $("#textbox1").attr("disabled", false).val(0);
    $("#textbox2").attr("disabled", false).val(0);
    $('.genbutton').children().next().remove();
}