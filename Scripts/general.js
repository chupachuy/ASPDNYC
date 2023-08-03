var contador = 0;
var ladas = new Array("81", "55", "33");
var isLada = false;
var ubiError;
var url_analytics = '';

var contador_error_500 = 0;

var ConectoresPersonaFisica = ['A', 'O', 'U', 'DEL', 'DE', 'LA', 'LOS', 'LAS', 'Y', 'DÉL', 'DÉ', 'LÁ'];
var ConectoresPersonaMoral = ['SA', 'CV', 'S', 'C', 'S.A.', 'C.V.'];

$(document).ready(function () {
    $('img[src="https://www.AgenteTop.com/Content/img/comodo_secure.png"]').addClass('logosecurity');

    if (document.location.hostname.search("prueba.") != -1) {
        $("#dDominio").html("Ambiente de Prueba");
    } else if (document.location.hostname.search("localhost") != -1) {
        $("#dDominio").html("Ambiente de Desarrollo");
    }
    $("#dDominio").css({
        'font-size': '50px'
    });
});

function SetGoogleAnalytics(url) {
    url_analytics = url;
    ////ESTO SIRVE PARA DEPURAR Y VER COMO SE MUEVE LA URL
    //var _url = window.location.href;
    //if (_url.indexOf("?p=") > 0)
    //    _url = _url.substring(0, url.indexOf("?p="));
    //_url += "?p=" + url;
    //window.history.pushState({}, "", _url);
    //console.log(url);

    if ((document.location.hostname.search("AgenteTop.com") !== -1 || document.location.hostname.search("AgenteTop.com.mx") !== -1) && (document.location.hostname.search("prueba.") == -1)) {
        if ("ga" in window) {
            try {
                tracker = ga.getAll()[0];
                if (tracker)
                    tracker.send("pageview", url);
            }
            catch (e) {
                console.log("***** Error *****");
                console.log(e);
                console.log("***** Error *****");
            }
        }
    }
}

function GetJSON(pURL, pJSONObject, callbackfunction, accion, errorJSON) {
    //$.blockUI();
    var _xhr = $.ajax({
        url: pURL,
        timeout: 10000000,
        //data: { pJSON: _JSON(pJSONObject) },
        data: { pJSON: JSON.stringify(pJSONObject)},
        dataType: "json",
        type: "POST",
        cache: true,
        success: function (data) {
            //        $.unblockUI();
            if (data.Respuesta.status == '200' || data.Respuesta.status == 'Ok') {
                callbackfunction(data.Respuesta);
            }
            else {
                General_ManejaError_Objeto(data.Respuesta, accion, errorJSON);
                console.log('{ ' + data.Respuesta.message + ' },' + accion + ', ' + errorJSON);

                $('#btn_exp_interes').removeAttr('disabled');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //        $.unblockUI();
            console.log('{ status: 500, message: ajaxOptions, thrownError: thrownError },' + accion + ', ' + errorJSON);
            General_ManejaError_Objeto({ status: '500', message: ajaxOptions, thrownError: thrownError }, accion, errorJSON);
            $('#btn_exp_interes').removeAttr('disabled');
        },
        statusCode: {
            500: function () {
               
            }
        },
    });
}

function General_ManejaError_Objeto(pData, accion, errorJSON) {
    contador++;

    ubiError = accion;
    $('#Modaldatoscifrados').modal("hide");
    $('#myModalCotizando').modal('hide');
    $('#myModalActualizando').modal('hide');
    if (pData.status == "500") {
        contador_error_500++;   
        //$('#errordb_recarga').modal('show'); 
    }
    else {
        $('#errordb').modal('show');
        if (contador > 3) {
            $('#errordb').modal('hide');
            ReturnControlError();
        }
    }
}

var _StatusError = "";
function General_ManejaError_Inesperado(e) {
    $("#popupGeneral_Mensaje .modal-title").html("Error");
    $("#popupGeneral_Mensaje .Descripcion").html("");
    $("#popupGeneral_Mensaje .Mensaje").html(e.message);
    $("#bttGeneral_Mensaje").click();
    _StatusError = "";
}
function General_ManejaError_Dialog(msg) {
    $("#popupGeneral_Mensaje .modal-title").html("Error");
    $("#popupGeneral_Mensaje .Descripcion").html("");
    $("#popupGeneral_Mensaje .Mensaje").html(msg);
    $("#bttGeneral_Mensaje").click();
    _StatusError = "";
}
function General_ManejaError_Dialog_Session(pCodigo) {
    _StatusError = pCodigo;
    $("#popupGeneral_Mensaje .modal-title").html("Error");
    $("#popupGeneral_Mensaje .Descripcion").html("Hemos detectado problemas con su sesión.");
    $("#popupGeneral_Mensaje .Mensaje").html("Deberá ingresar nuevamente al sistema.");
    $("#bttGeneral_Mensaje").click();
}
function General_ManejaError_Cerrar() {
    if (_StatusError == '600') {
        $("#logOffForm").submit();
    }
}


function ReturnControlError() {
    var URLactual = window.location.host;
    switch (ubiError) {
        case 'ejemplocosto':
        case 'contratado':
            $("#frmResultados").submit();
            break;
        case 'cotizador':
        case 'perfilador':
        case 'direccion':
        case 'encuesta':
        case 'Landing':
        case 'direccionLanding':
            var urlRuta = "https://" + URLactual + "/";
            window.location.href = urlRuta;
            break;
        case 'HomeAgente':
            var urlRuta = "https://" + URLactual + "/Perfiles/Perfil/Index";
            window.location.href = urlRuta;
            break;
    }
}
function btnErrorDBRecarga() {
    var _url = window.location.href;
    if (_url.indexOf("?p=") > 0) {
        location.href = "https://www.AgenteTop.com";
    }
    else {
            location.reload();
    }
    $('#errordb_recarga').modal('hide');
}
function btnErrorDB() {
    $('#errordb').modal('hide');

    switch (ubiError) {
        case 'perfilador':
            break;
        case 'direccion':
        case 'encuesta':
            break;
        case 'cotizador':
            break;
        case 'ejemplocosto':
            break;
        case 'Landing':
            $('.blockOverlay').hide()
            break;
        case 'contratado':
            $('.btnContratar').removeAttr('disabled');
            $('#myModalActualizando').modal('hide');
            break;
    }
}

function AbrirChatHubspot() {
    $('#error_config_correo').modal('hide');
    window.HubSpotConversations.widget.open();  
}

/**
 * Maxima longitud de caractes en type='number'
 * @param {any} object el objeto input
 */
function maxLengthCheck(object) {
    if (object.value.length > object.maxLength)
        object.value = object.value.slice(0, object.maxLength);
}

function maskVencimiento(elem) {
    $(elem).maskFecha('99/99/9999');
}
function maskFechaNac(elem) {
    $(elem).maskFecha('99/99/9999');
}
function maskFechaVencimiento(elem) {
    $(elem).maskFechaVencimiento('99/9999');
}
function maskLadaTelefonica(elem) {
    var tel = $(elem).val();

    if (tel.length == 2) {
        var lada = tel.substring(0, 2);
        switch (lada) {
            case "81":
            case "55":
            case "33":
                $(elem).mask("(99) 9999-9999");
                break;
            default:
                $(elem).mask("(999) 999-9999");
                break;
        }
    }
}
function maskLadaTelefonicaConDatos(input, valor) {
    var tel = valor;

    var cadena = valor,
        separador = ")",
        arregloDeSubCadenas = cadena.split(separador);

    if (arregloDeSubCadenas[0].length == 4) {
        $(input).mask("(999) 999-9999");
    }
    else {
        $(input).mask("(99) 9999-9999");
    }
}


function General_Mayusculas(e) {
    var idInput = '#' + e.id;
    var valInput = e.value;

    if (!$.isNumeric(valInput))
        $(idInput).val(valInput.toUpperCase());
}
function General_Minusculas(e) {
    var idInput = '#' + e.id;
    var valInput = e.value;

    if (!$.isNumeric(valInput))
        $(idInput).val(valInput.toLowerCase());
}
function General_PrimeraMayuscula(e) {
    var idInput = '#' + e.id;
    var valInput = e.value;

    $(idInput).val(formatoTitulo(valInput));
}
function General_PrimeraMayuscula_Value(valor) {
    return formatoTitulo(valor);
}
function formatoTitulo(valor) {
    var arregloDeSubCadenas = valor.toUpperCase().split(' ');
    var nombreInput = '';

    for (var i = 0; i < arregloDeSubCadenas.length; i++) {
        var Formato = 0;

        for (var j = 0; j < ConectoresPersonaFisica.length; j++) {
            if (arregloDeSubCadenas[i] == ConectoresPersonaFisica[j]) {
                Formato = 1;
                break;
            }
        }

        if (Formato == 0) {
            for (var j = 0; j < ConectoresPersonaMoral.length; j++) {
                if (arregloDeSubCadenas[i] == ConectoresPersonaMoral[j]) {
                    Formato = 2;
                    break;
                }
            }
        }

        if (Formato == 0) {
            if (arregloDeSubCadenas[i].length > 1)
                nombreInput += arregloDeSubCadenas[i].charAt(0).toUpperCase() + arregloDeSubCadenas[i].slice(1).toLowerCase() + ' ';
            else
                nombreInput += arregloDeSubCadenas[i].toLowerCase() + ' ';
        }
        else if (Formato == 1) {
            nombreInput += arregloDeSubCadenas[i].toLowerCase() + ' ';
        }
        else {
            nombreInput += arregloDeSubCadenas[i].toUpperCase() + ' ';
        }
    }

    return nombreInput = nombreInput.slice(0, -1);
}

function validarInput(input) {
    $(input).removeClass('input_invalido');
    if ($(input).val() !== '') {
        $(input).addClass('input_valido');
    }
    else {
        $(input).removeClass('input_valido');
    }
}
function validarInputSelect(input) {
    $(input).removeClass('input_invalido');
    $(input).removeClass('input_valido');
    if ($(input).val() != 'null') {
        $(input).addClass('input_valido');
    }
    else {
        $(input).removeClass('input_valido');
    }
}

function RecortarCadenaTexto(valor, limite, complemento = '...') {
    if (valor != null && valor != '') {
        valor = valor.replace(" undefined", "");
        if (valor != undefined) {
            if (valor.length > limite) {
                valor = valor.substr(0, limite) + complemento;
            }
            return valor;
        }
        else
            return '';
    }
    else {
        return '';
    }
}

//Validar Resolucion Movil JS
function esResolucionMovil() {
    return ($(window).width() < 768) ? true : false;
}

//Validar Resolucion Desktop JS
function esResolucionDesktop() {
    return ($(window).width() > 991) ? true : false;
}


//Descargar_documentos
function download_unica(nombre, url) {
    var evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, false, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);

    var link = document.createElement('a');
    link.setAttribute('href', url);
    link.setAttribute('download', nombre);
    link.click();
    //link.dispatchEvent(evt);
}