var MenuAbierto = false;

$(document).ready(function () {
    //Url para landings
    $('.a_facebook').attr('href', 'https://www.facebook.com/duraznosnectarinasyciruelasdecalifornia');
    $('.a_instagram').attr('href', 'https://www.instagram.com/carozos_de_california/');
    $('.a_twitter').attr('href', 'https://twitter.com/SondeCalifornia');
    $('.a_recetas').attr('href', 'https://' + window.location.host + '/recetario/cuestionario');
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

    $('#img_videoKaruy').click(function () {
        $('#img_videoKaruy').hide();
        $('#video_videoKaruy').show();
        $('#video_videoKaruy').trigger('play');
        //$('video').trigger('pause');
    });
});


function btn_Acceso_Contacto() {
    window.location.href = "https://" + window.location.host + "/contactanos";
}

function btn_Acceso_Home() {
    window.location.href = "https://" + window.location.host + "/";
}

function descargar_receta() {
    window.location.href = "https://" + window.location.host + "/recetario/cuestionario";
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