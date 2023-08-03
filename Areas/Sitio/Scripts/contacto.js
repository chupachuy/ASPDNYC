var AvisoPrivaciodad = false;
//var cpCorrecto = true;

var popPreguntas = '',
    popProbado = '',
    popTemporada = '',
    popCompras = '',
    popImportados = '';

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

    $('.a_home').attr('href', 'https://' + window.location.host + '/');

    $('#check_terminos').change(function () {
        if ($('[id=check_terminos]').is(':checked')) {
            AvisoPrivaciodad = true;
        }
        else {
            AvisoPrivaciodad = false;
        }

        valForm_SolicitarContacto();
    });
});


function btn_Acceso_Contacto() {
    window.location.href = "https://" + window.location.host + "/contacto";
}

function btn_Acceso_Home() {
    window.location.href = "https://" + window.location.host + "/";
}

function btn_Acceso_Descargar_Recetario() {
    window.location.href = "https://" + window.location.host + "/descargar-recetario";
}

function btn_Acceso_Videos() {
    window.location.href = "https://" + window.location.host + "/videos-recetario";
}



/**
 * Validador de Correos
 * @param {any} email
 */
function validar_email(email) {
    var regex = /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/;
    return regex.test(email) ? true : false;
}

function Ukuvivalidar_RegexNombre(text) {
    var regex = /^[a-zA-ZÀ-ÿ\u00f1\u00d1]+(\s*[a-zA-ZÀ-ÿ\u00f1\u00d1]*)*[a-zA-ZÀ-ÿ\u00f1\u00d1]+$/;
    return regex.test(text) ? true : false;
}

function valForm_SolicitarContacto(ClicBtn = false) {
    $('#msjVal').removeClass('msjVal_visible');

    var txtCorreo = $('#txtCorreo').val().trim(),
        txtNombre = $('#txtnombre').val().trim(),
        //txtApellido = $('#txtapellido').val().trim();
        txtEstado = $('#txtEstado').val().trim();

    $('#txtCorreo').removeClass('inputObligatorio');
    $('#txtnombre').removeClass('inputObligatorio');
    //$('#txtapellido').removeClass('inputObligatorio');
    $('#txtEstado').removeClass('inputObligatorio');
    $('#txtEdad').removeClass('inputObligatorio');
    //$('#txtCP').removeClass('inputObligatorio');

    var valNombre = false,
        //valApellido = true,
        valEstado = true,
        valCorreo = false,
        //valCP = true,
        valEdad = true;

    valNombre = (txtNombre.length > 2 && Ukuvivalidar_RegexNombre(txtNombre)) ? true : false;
    valCorreo = ($('#ValCorreo').valid() && validar_email(txtCorreo) && txtCorreo != "") ? true : false;

    /*if (txtApellido != '')
        valApellido = (txtApellido.length > 2 && Ukuvivalidar_RegexNombre(txtApellido)) ? true : false;*/

    if (txtEstado != '')
        valEstado = (txtEstado.length > 2 && Ukuvivalidar_RegexNombre(txtEstado)) ? true : false;

    if (ventana == 'recetas') {
        if ($('#txtEdad').val().trim() != '')
            valEdad = (parseInt($('#txtEdad').val().trim()) > 10 && parseInt($('#txtEdad').val().trim()) < 80) ? true : false;

        /*if ($('#txtCP').val().trim() != '')
            valCP = cpCorrecto;*/
    }

    //var validador = valNombre && valApellido && valCorreo && valEdad && valCP && AvisoPrivaciodad;
    var validador = valNombre && valCorreo && valEdad && valEstado && AvisoPrivaciodad;

    if (validador) {
        if (ventana == 'contactanos') {
            $('#btn-contacto').show();
            $('#btn-contacto-dummy').hide();
        }
        else if (ventana == 'recetas') {
            $('#btn-recetario').show();
            $('#btn-recetario-dummy').hide();
        }
        else {
            $('#btn-videos-recetas').show();
            $('#btn-videos-recetas-dummy').hide();
        }
    }
    else {
        if (ventana == 'contactanos') {
            $('#btn-contacto-dummy').show();
            $('#btn-contacto').hide();
        }
        else if (ventana == 'recetas') {
            $('#btn-recetario-dummy').show();
            $('#btn-recetario').hide();
        }
        else {
            $('#btn-videos-recetas-dummy').show();
            $('#btn-videos-recetas').hide();
        }

        if (ClicBtn) {
            if (!AvisoPrivaciodad) {
                $('#msjVal').html("<span class='material-icons'>warning</span>Acepta los términos de usos  y  el aviso de privacidad");
            }
            if (ventana == 'recetas') {
                if (!valEdad) {
                    $("#txtEdad").addClass("inputObligatorio");
                    $('#msjVal').html("<span class='material-icons'>warning</span>Ingresar un Edad valida 10 a 80 Años");
                }
                /*if (!valCP) {
                    $("#txtCP").addClass("inputObligatorio");
                    $('#msjVal').html("<span class='material-icons'>warning</span>Ingresar un Código Postal valido");
                }*/
            }
            if (!valNombre) {
                $("#txtnombre").addClass("inputObligatorio");
                $('#msjVal').html("<span class='material-icons'>warning</span>Ingresar un Nombre valido");
            }
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
        if (ventana == 'contactanos') {
            SendRegistroHubspot();
        }
        else {
            if ($('#postOrigen').val() == 'cuestionario')
                $('#modalPreguntas').modal('show');
            else
                SendRegistroHubspot();
        }
    }
}

function SendRegistroHubspot() {
    $('#myModalActualizando').modal('show');
    var txtCorreo = $('#txtCorreo').val().trim(),
        txtNombre = $('#txtnombre').val().trim(),
        //txtApellido = $('#txtapellido').val().trim(),
        txtEstado = $('#txtEstado').val().trim(),
        txtEdad = '',
        //txtEstado = '',
        txtPreguntas = '';

    if (ventana == 'contactanos') {
        txtPagina = 'Pagina de Contactanos';
    }
    else if (ventana == 'recetas') {
        txtPagina = 'Pagina de Recetario';
        txtEdad = $('#txtEdad').val().trim();
        //txtCP = $('#txtCP').val().trim();
    }
    else {
        txtPagina = 'Pagina de Videos con recetas';
    }

    if (ventana != 'contactanos') {
        if (popPreguntas == 'No los conozco') {
            txtPreguntas += '¿Conoces los Duraznos, Nectarinas y Ciruelas de California, EUA? ' + popPreguntas +
                '  -  ¿Prefieres duraznos, nectarinas o ciruelas importadas de otro país? ' + popImportados;
        }
        else {
            txtPreguntas += '¿Conoces los Duraznos, Nectarinas y Ciruelas de California, EUA? ' + popPreguntas +
                '  -  ¿Los has probado como parte de un platillo preparado? ' + popProbado +
                '  -  ¿Sabes cuál es su temporada? ' + popTemporada +
                '  -  ¿Los has comprado? ' + popCompras;
        }
    }

    var oParam = {
        "txtNombre": txtNombre,
        //"txtApellido": txtApellido,
        "txtEstado": txtEstado,
        "txtCorreo": txtCorreo,
        "txtEdad": txtEdad,
        //"txtCP": txtCP,
        "txtPreguntas": txtPreguntas,
        "txtPagina": txtPagina
    }

    var oParam2 = "{ 'pJSON': '" + JSON.stringify(oParam) + "'}";

    $.ajax({
        type: "POST",
        url: _Root + 'FormComentario',
        data: oParam2,
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        cache: false,
        success: function (r) {
            if (r.success) {
                console.log('Send Hubspot');

                if (ventana == 'contactanos') {
                    $('#modalConfirmacion').modal('show');
                    setTimeout(function () {
                        $('#modalConfirmacion').modal('hide');
                        btn_Acceso_Home();
                    }, 3000);
                }
                else if (ventana == 'recetas') {
                    btn_Acceso_Descargar_Recetario();
                }
                else {
                    btn_Acceso_Videos();
                }

                if (ventana == 'contactanos') {
                    $('#btn-contacto-dummy').show();
                    $('#btn-contacto').hide();
                }
                else if (ventana == 'recetas') {
                    $('#btn-recetario-dummy').show();
                    $('#btn-recetario').hide();
                }
                else {
                    $('#btn-videos-recetas-dummy').show();
                    $('#btn-videos-recetas').hide();
                }

                $('#myModalActualizando').modal('hide');

                $('#txtCorreo').val('');
                $('#txtnombre').val('');
                //$('#txtapellido').val('');
                $('#txtEstado').val('');
                $('#txtEdad').val('');
                //$('#txtCP').val('');

                AvisoPrivaciodad = false;
                $('#check_terminos').prop("checked", false);

                $('#modalPreguntas').modal('hide');
                $('#popPregunta').show();
                $('#popProbado').hide();
                $('#popTemporada').hide();
                $('#popCompras').hide();
                $('#popImportados').hide();
                $('#popGracias').hide();
                popPreguntas = '';
                popProbado = '';
                popTemporada = '';
                popCompras = '';
                popImportados = '';
            }
        },
        error: function (xhr) {
            $('#myModalActualizando').modal('hide');
            console.log('No Send Hubspot');
        }
    });
}


/*function txtCPKeyup(elem) {
    var CpCode = '#' + elem.id;
    $(CpCode).removeClass('input_valido');
    $(CpCode).removeClass('input_invalido');

    if ($(CpCode).val() !== '') {
        if ($(CpCode).val().length === 5 && $.isNumeric($(CpCode).val())) {
            cpCorrecto = true;
            valForm_SolicitarContacto();
        }
        else if ($(CpCode).val().length !== '5') {
            cpCorrecto = false;
            valForm_SolicitarContacto();
        }
    }
}*/


//Flujo de ventanas de PopUp
function moverFlujoPopUpPreguntas(ventana, respuesta) {
    $('#popPregunta').hide();
    $('#popProbado').hide();
    $('#popTemporada').hide();
    $('#popCompras').hide();
    $('#popImportados').hide();
    $('#popGracias').hide();

    switch (ventana) {
        case 0:
            if (respuesta == 0) {
                popPreguntas = 'Sí, los conozco';
                $('#popProbado').show();
            }
            else {
                popPreguntas = 'No los conozco';
                $('#popImportados').show();
            }
            break;
        case 1:
            if (respuesta == 0) {
                popProbado = 'Sí';
            }
            else {
                popProbado = 'No';
            }

            $('#popTemporada').show();
            break;
        case 2:
            if (respuesta == 0) {
                popTemporada = 'Sí';
            }
            else {
                popTemporada = 'No';
            }

            $('#popCompras').show();
            break;
        case 3:
            if (respuesta == 0) {
                popCompras = 'Nunca';
            }
            else if (respuesta == 1) {
                popCompras = 'Menos de una vez al mes';
            }
            else if (respuesta == 2) {
                popCompras = 'Al menos una vez al mes';
            }
            else {
                popCompras = 'Dos o más veces al mes';
            }

            SendRegistroHubspot();
            $('#popGracias').show();
            break;
        case 4:
            if (respuesta == 0) {
                popImportados = 'Sí';
            }
            else {
                popImportados = 'No';
            }

            SendRegistroHubspot();
            $('#popGracias').show();
            break;
    }
}
