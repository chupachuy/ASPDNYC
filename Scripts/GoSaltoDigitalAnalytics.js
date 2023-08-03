"use strict";
var _xhrPoolAnalytics = [];

$(document).ready(function () {
    //UkAnalytics_Habilitar();
});

function UkAnalytics_Habilitar() {
    $(".ukAnalytics").unbind("click.ukAnalytics");
    $(".ukAnalytics").on('click.ukAnalytics', function () {
        UkAnalytics_1Objeto(this);
    });
}

function UkAnalytics_1Objeto(pT) {
    var vT = $(pT);
    var vTipo = vT.get(0).tagName;
    var vUbic = vT.attr("ukUbic");
    var vNomb = vT.attr("ukNomb");
    if (!vUbic || !vNomb || vUbic == "" || vNomb == "") { console.log("Error: Objeto ukAnalytics sin Datos: " + vTipo + " | " + vT.attr("id") + " | " + vT.text()); }
    var vJSON = { Tipo: vTipo, Texto: vT.text(), Title: vT.attr("title"), Ubicacion: vUbic, Nombre: vNomb, Inputs: [] };
    $("input").each(function () {
        var input = $(this);
        var vJSONInput = { Val: input.val(), Type: input.attr('type'), Id: input.attr('id'), Name: input.attr('name'), Checked: input.prop('checked') };
        vJSON.Inputs.push(vJSONInput);
    });
    GetJSON_AgenteTopAnalytics('/Generic/AgenteTopAnalytics', vJSON);
}

var oldbeforeunload = window.onbeforeunload;
window.onbeforeunload = function () {
    var r = oldbeforeunload ? oldbeforeunload() : undefined;
    if (r == undefined) {
        $.each(_xhrPoolAnalytics, function (idx, jqXHR) {
            try {
                //       jqXHR.abort();
            }
            catch (e) { }
        });
    }
    return r;
}

function GetJSON_AgenteTopAnalytics(pURL, pJSONObject) {
    var xhrAgenteTopAnalytics = $.ajax({
        url: pURL,
        timeout: 10000,
        data: { pJSON: _JSON(pJSONObject) },
        dataType: "json",
        type: "POST",
        cache: true,
        success: function (data) { },
        error: function (xhr, ajaxOptions, thrownError) { }
    });
    _xhrPoolAnalytics.push(xhrAgenteTopAnalytics);
}
