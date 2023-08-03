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
});

/**
 * Validador de Correos,RFP, CURP
 * @param {any} text valor a validar
 * @param {any} opcion que es correo rfc o curp
 */
function AgenteTopvalidar_Regex(text, opcion) {
    var regex;
    switch (opcion) {
        case 'email':
            regex = /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/;
            return regex.test(text) ? true : false;
        case 'curp':
            regex = /^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$/;
            return regex.test(text) ? true : false;
        case 'rfc':
            regex = /^([A-ZÑ&]{3,4}) ?(?:- ?)?(\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])) ?(?:- ?)?([A-Z\d]{2})([A\d])$/;
            return regex.test(text) ? true : false;
        case 'fecha':
            regex = /^([0][1-9]|[12][0-9]|3[01])(\/|-)([0][1-9]|[1][0-2])\2(\d{4})$/;
            return regex.test(text) ? true : false;
        case 'montoEntero':
            regex = /^[0-9]{1,6}?$/;
            return regex.test(text) ? true : false;
        case 'montoDecimal':
            regex = /^[0-9]{1,6}(\\.\\d{1,2})?$/;
            return regex.test(text) ? true : false;
        case 'nombre':
            regex = /^[a-zA-ZÀ-ÿ\u00f1\u00d1]+(\s*[a-zA-ZÀ-ÿ\u00f1\u00d1]*)*[a-zA-ZÀ-ÿ\u00f1\u00d1]+$/;
            return regex.test(text) ? true : false;
    }
}

/**
 * Mascara telefonica a 10 digitos
 * @param {any} elem el componente
 */
function AgenteTopMask_LadaTelefonica(elem) {
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

/**
 * Mascara Fechas
 * @param {any} elem el componente
 */
function AgenteTopMask_Fecha(elem) {
    $(elem).maskFecha("99/99/9999");
}

/**
 * Validador de correo
 * @param {any} _correo valor del input
 * @param {any} validCorreoJs el form para validar js
 */
function AgenteTop_Correo(_correo, validCorreoJs) {
    var resultado = false;
    var _txtCorreo = _correo;
    var valjsCorreo = '#' + validCorreoJs;

    if (_txtCorreo !== "") {
        if ($(valjsCorreo).valid() && AgenteTopvalidar_Regex(_txtCorreo, 'email'))
            resultado = true;
    }

    return resultado;
}

/**
 * Maxima longitud de caractes en type='number'
 * @param {any} object el objeto input
 */
function AgenteTopmaxLengthCheck(object) {
    if (object.value.length > object.maxLength)
        object.value = object.value.slice(0, object.maxLength);
}

/**
 * Mayusculas un input
 * @param {any} e
 */
function AgenteTop_Mayusculas(e) {
    var idInput = '#' + e.id;
    var valInput = e.value;

    $(idInput).val(valInput.toUpperCase());
}

/**
 * Aldair
Validadores de campos utilizados en AgenteTop
 * @param {any} input '#txtInput' el id del input
 * @param {any} valInput 'Valor' el valor del input
 * @param {any} tipoInput 'Etiqueta' para validar
 */
function AgenteTop_ValInputs(input, valInput, tipoInput, paintElement = true) {
    var resultado = false;
    switch (tipoInput) {
        case 'Nombre':
            valInput = valInput.trim();
            if (valInput.length > 2 && AgenteTopvalidar_Regex(valInput, 'nombre'))
                resultado = true;
            break;
        case 'CURP':
            valInput = valInput.trim();
            if (valInput.length == 18 && AgenteTopvalidar_Regex(valInput, 'curp'))
                resultado = true;
            break;
        case 'Fecha':
            if (valInput.length > 9 && AgenteTopvalidar_Regex(valInput, 'fecha'))
                resultado = true;
            break;
        case 'RFC':
            valInput = valInput.trim();
            if (valInput.length == 13 && AgenteTopvalidar_Regex(valInput, 'rfc'))
                resultado = true;
            break;
        case 'listas':
            if (valInput !== 'null')
                resultado = true;
            break;
        case 'Email':
            valInput = valInput.trim();
            if (AgenteTop_Correo(valInput, 'ValCorreo'))
                resultado = true;
            break;
        case 'CP':
            if (valInput)
                resultado = true;
            break;
        case 'Texto':
            valInput = valInput.trim();
            if (valInput !== '')
                resultado = true;
            break;
        case 'listas2':
            if (valInput !== '' && $(input).prop('selectedIndex') > 0)
                resultado = true;
            break;
        case 'Telefono':
            if (valInput.length == 14)
                resultado = true;
            break;
        case 'MontoDecimal':
            if (valInput !== '') //&& AgenteTopvalidar_Regex(valInput, 'montoDecimal'))
                resultado = true;
            break;
        case 'hasClass':
            if ($(input).hasClass(valInput)) //&& AgenteTopvalidar_Regex(valInput, 'montoDecimal'))
                resultado = true;
            break;
    }

    if (paintElement === true) {
        if (resultado)
            $(input).removeClass('AgenteTop_inputObligatorio');
        else
            $(input).addClass('AgenteTop_inputObligatorio');
    }
    
    return resultado;
}

/**
 * Obtener los servicios actuales de AgenteTop
 * @param {any} controlador Ruta del controlador
 * @param {any} funcion Funcion a regresar datos del sp a donde se envian recibedatos(data)
 * @param {any} etiqueta 
 */
function GetServicioAgenteTop(controlador, funcion, etiqueta, marcaError) {
    var json;
    switch (etiqueta) {
        case 'aseguradoras':
            json = { dummy: "1" };
            break;
        case 'parentescos':
            json = { COD_INTE: "COD_PARENTESCO" };
            break;
        case 'ocupaciones':
            json = { CID_TAB: "COD_OCUPACION" };
            break;
        case 'EstadosSiglas':
            json = { dummy: "1" };
            break;
    }
    GetJSON(controlador, json, funcion, marcaError);
}