var MenuAbierto = false;

var SeleccionarTodo = false;

$(document).ready(function () {
    //Validador Js
    jQuery.validator.setDefaults({
        debug: true,
        success: "valid"
    });
    $("#ValCorreo").validate({
        rules: {
            field: {
                required: true,
                email: true
            }
        }
    });

    //Url para landings
    $('.a_facebook').attr('href', 'https://www.facebook.com/duraznosnectarinasyciruelasdecalifornia');
    $('.a_instagram').attr('href', 'https://www.instagram.com/carozos_de_california/');
    $('.a_twitter').attr('href', 'https://twitter.com/SondeCalifornia');
    $('.a_recetas').attr('href', 'https://' + window.location.host + '/recetario');
    $('.a_contactanos').attr('href', 'https://' + window.location.host + '/contactanos');
    $('.a_home').attr('href', 'https://' + window.location.host + '/');


    //Funcion para detectar cuando se realiza un scroll a la pagina
    $(window).scroll(function () {
        var scroll = $(window).scrollTop();

        if (scroll > 50) {
            $('.navbar').removeClass('backColorTransparent');
            $('.OcultarMenu').show();
        }
        else {
            $('.navbar').addClass('backColorTransparent');
            $('.OcultarMenu').hide();

            if (esResolucionMovil()) {
                if (MenuAbierto) {
                    $('.navbar').removeClass('backColorTransparent');
                }
                else {
                    if (scroll > 50) {
                        $('.navbar').removeClass('backColorTransparent');
                    }
                    else {
                        $('.navbar').addClass('backColorTransparent');
                    }
                }
            }
        }
    });
});

/**
 * Validador de Correos
 * @param {any} email
 */
function validar_email(email) {
    var regex = /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/;
    return regex.test(email) ? true : false;
}

function SeleccionarTodosPDF() {
    //download_unica("52Rcts_CarozosCalif_2021_sRGB.pdf", "https://" + window.location.host + "/Documentos/Recetas/Pdf/52Rcts_CarozosCalif_2021_sRGB.pdf");
    download_unica("2023recetas0727v2.pdf", "https://" + window.location.host + "/Documentos/Recetas/Pdf/2023recetas0727v2.pdf");
}

function descargar_recetaHelena() {
    download_unica("40_Recetario_Helena Hernandez_compressed.pdf", "https://" + window.location.host + "/Documentos/Recetas/Pdf/40_Recetario_Helena Hernandez_compressed.pdf");
}

function descargar_recetaKaruy() {
    download_unica("12_Recetario_Karuy Zazueta_compressed.pdf", "https://" + window.location.host + "/Documentos/Recetas/Pdf/12_Recetario_Karuy Zazueta_compressed.pdf");
}

function descargar_recetaBondeleite() {
    download_unica("AW_15_Rcts_Bondeleite_2022_6x4_sRGB.pdf", "https://" + window.location.host + "/Documentos/Recetas/Pdf/AW_15_Rcts_Bondeleite_2022_6x4_sRGB.pdf");
}

function btn_Acceso_Home() {
    window.location.href = "https://" + window.location.host + "/";
}

function btn_AbrirMenuMovil() {
    var scroll = $(window).scrollTop();

    if (MenuAbierto) {
        //Cerrar Menu
        MenuAbierto = false;

        if (scroll > 50) {
            $('.navbar').removeClass('backColorTransparent');
        }
        else {
            $('.navbar').addClass('backColorTransparent');
        }
    }
    else {
        //Abrir Menu
        MenuAbierto = true;
        $('.navbar').removeClass('backColorTransparent');
    }
}


function DescargarSeleccionPDF(valor) {
    $.each(ArrayPDFRecetas, function (i, f) {
        if (f.Id == valor) {
            download_unica(f.NombrePDF, "https://" + window.location.host + "/Documentos/Recetas/" + f.UrlPDF);
        }
    });
}

function valForm_SolicitarContacto(ClicBtn = false) {
    $('#msjVal').removeClass('msjVal_visible');

    var txtCorreo = $('#txtCorreo').val().trim();
    $('#txtCorreo').removeClass('inputObligatorio');

    var valCorreo = false;

    valCorreo = ($('#ValCorreo').valid() && validar_email(txtCorreo) && txtCorreo != "") ? true : false;

    var validador = valCorreo;

    if (validador) {
        $('#modalEnviarEmail .btn').removeClass('btn_inactivo');
    }
    else {
        $('#modalEnviarEmail .btn').addClass('btn_inactivo');

        if (ClicBtn) {
            if (!valCorreo) {
                $("#txtCorreo").addClass("inputObligatorio");
                $('#msjVal').html("<span class='material-icons'>warning</span>Ingresar un Correo valido");
            }

            $('#msjVal').addClass('msjVal_visible');
        }
    }

    return validador;
}

function btnSolicitarContacto() {
    if (valForm_SolicitarContacto(true)) {
        SendRegistroEmail();
    }
}

function SendRegistroEmail() {
    $('#myModalActualizando').modal('show');
    var txtCorreo = $('#txtCorreo').val().trim();

    var oParam = {
        "txtCorreo": txtCorreo
    }

    var oParam2 = "{ 'pJSON': '" + JSON.stringify(oParam) + "'}";

    $.ajax({
        type: "POST",
        url: _Root + 'FormEnviarCorreo',
        data: oParam2,
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        cache: false,
        success: function (r) {
            $('#myModalActualizando').modal('hide');

            if (r.success) {
                console.log('Send Email');

                $('#modalEnviarEmail').modal('hide');
                $('#modalConfirmacion').modal('show');
                setTimeout(function () {
                    $('#modalConfirmacion').modal('hide');
                }, 3000);

                $('#txtCorreo').val('');
                $('#txtCorreo').removeClass('inputObligatorio');
                $('#msjVal').removeClass('msjVal_visible');
                $('#modalEnviarEmail .btn').addClass('btn_inactivo');
            }
            else {
                $('#modalEnviarEmail').modal('hide');

                $('#errordb .title2ModalErrorDB').text('Parece que tuvimos un inconveniente: ' + r.message);
                $('#errordb').modal('show');
            }
        },
        error: function (xhr) {
            $('#myModalActualizando').modal('hide');
            console.log('No Send Email');
        }
    });
}