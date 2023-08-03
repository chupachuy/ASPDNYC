//#region variablesglobales

var ValTam = $(window).width();
if (ValTam < 768)
    _isMovilApiGoogle = true;
else
    _isMovilApiGoogle = false;

var ValTam = $(window).width();
if (ValTam < 768)
    api_IsMovil = 'SI';
else
    api_IsMovil = 'NO';

var txtInput;

var _map;
var _pos;
var _geocoder;
var _markersArray = [];

var _autocomplete;
var _componentForm = {
    locality: 'long_name',
    administrative_area_level_1: 'long_name',
    latitude:
    longitude
};
//#endregion variablesglobales



function GetJSONUBICACION(pURL, pJSONObject, callbackfunction, accion) {
    var _xhrUbicacion = '';
    //if (_xhrUbicacion)
    //{
    //    _xhrUbicacion.remove();
    //}
    _xhrUbicacion = $.ajax({
        url: pURL,
        timeout: 10000,
        data: { pJSON: _JSON(pJSONObject) },
        dataType: "json",
        type: "POST",
        cache: true,
        success: function (data) {
            //        $.unblockUI();
            if (data.Respuesta.status == '200' || data.Respuesta.status == 'Ok') {
                callbackfunction(data.Respuesta);
            }
        }
    });
}



//#region Dibujamapa

function roundToTwo(num) {
    return +(Math.round(num + "e+2") + "e-2");
}

function initMap() {

    if ($(".divMCCiudad").is(":visible")) {
        $('.inputweb').remove();
    }
    else {
        $('.inputmovil').remove();
    }


    if (document.getElementById('map') != null) {
        _map = new google.maps.Map(document.getElementById('map'), {
            center: { lat: 19.432608, lng: -99.133209 }, //México DF, por si no se permite localización. se fija punto central
            zoom: 12,
            // How you would like to style the map. 
            // This is where you would paste any style found on Snazzy Maps.
            styles: [{ "featureType": "water", "elementType": "geometry", "stylers": [{ "color": "#e9e9e9" }, { "lightness": 17 }] }, { "featureType": "landscape", "elementType": "geometry", "stylers": [{ "color": "#f5f5f5" }, { "lightness": 20 }] }, { "featureType": "road.highway", "elementType": "geometry.fill", "stylers": [{ "color": "#ffffff" }, { "lightness": 17 }] }, { "featureType": "road.highway", "elementType": "geometry.stroke", "stylers": [{ "color": "#ffffff" }, { "lightness": 29 }, { "weight": 0.2 }] }, { "featureType": "road.arterial", "elementType": "geometry", "stylers": [{ "color": "#ffffff" }, { "lightness": 18 }] }, { "featureType": "road.local", "elementType": "geometry", "stylers": [{ "color": "#ffffff" }, { "lightness": 16 }] }, { "featureType": "poi", "elementType": "geometry", "stylers": [{ "color": "#f5f5f5" }, { "lightness": 21 }] }, { "featureType": "poi.park", "elementType": "geometry", "stylers": [{ "color": "#dedede" }, { "lightness": 21 }] }, { "elementType": "labels.text.stroke", "stylers": [{ "visibility": "on" }, { "color": "#ffffff" }, { "lightness": 16 }] }, { "elementType": "labels.text.fill", "stylers": [{ "saturation": 36 }, { "color": "#333333" }, { "lightness": 40 }] }, { "elementType": "labels.icon", "stylers": [{ "visibility": "off" }] }, { "featureType": "transit", "elementType": "geometry", "stylers": [{ "color": "#f2f2f2" }, { "lightness": 19 }] }, { "featureType": "administrative", "elementType": "geometry.fill", "stylers": [{ "color": "#fefefe" }, { "lightness": 20 }] }, { "featureType": "administrative", "elementType": "geometry.stroke", "stylers": [{ "color": "#fefefe" }, { "lightness": 17 }, { "weight": 1.2 }] }]

        });
    }


    // localización dispositivo
    if (Autobusqueda) {
        if (isVentana == 'Cotizador') {
            if (api_IsMovil == 'SI') {
                txtInput = 'txtDireccionMov';
            } else {
                txtInput = 'txtDireccion';
            }
        }
        else {
            txtInput = 'txtDireccion';
        }
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                _pos = {
                    lat: roundToTwo(position.coords.latitude),
                    lng: roundToTwo(position.coords.longitude)
                };
                fun_obtienepostalodireccion();
                marca(_pos);
            }, function () {
            });
        } else {
        }
    }

    var options = {
        types: ['(cities)'],
        language: "es",
        componentRestrictions: { country: "mx" }
    };

    if (isVentana == 'Cotizador') {
        if (api_IsMovil == 'SI') {
            _autocomplete = new google.maps.places.Autocomplete(
                (document.getElementById('txtDireccionMov')), options);
        } else {
            _autocomplete = new google.maps.places.Autocomplete(
                (document.getElementById('txtDireccion')), options);
        }
    }
    else {
        _autocomplete = new google.maps.places.Autocomplete(
            (document.getElementById('txtDireccion')), options);
    }



    google.maps.event.addListener(_autocomplete, 'place_changed', function () {
        fillInAddress();
    });

    if (document.getElementById('map') != null) {
        google.maps.event.addListener(_map, 'click', function (e) {
            _pos = {
                lat: roundToTwo(e.latLng.lat()),
                lng: roundToTwo(e.latLng.lng())
            };
            var geocoder = geocoder = new google.maps.Geocoder();


            geocoder.geocode({ 'latLng': _pos }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    if (results[0]) {
                        fun_muestraresultado(results, false);
                    }
                }
            });

        });

        google.maps.event.addListener(_map, "idle", function () {
            google.maps.event.trigger(_map, 'resize');
        });
    }
}

function handleLocationError(browserHasGeolocation, infoWindow, pos) {
    //console.log('handleLocationError');
    infoWindow.setPosition(pos);
    infoWindow.setContent(browserHasGeolocation ?
        'Error: The Geolocation service failed.' :
        'Error: Your browser doesn\'t support geolocation.');
}
//#endregion Dibujamapa

//#region Muestra

function fillInAddress() {
    try {
        //console.log('fillInAddress');
        var place = _autocomplete.getPlace();
        var place_id = place.place_id;
        var lat = roundToTwo(place.geometry.location.lat());
        var lng = roundToTwo(place.geometry.location.lng());
        _pos = {
            lat: lat,
            lng: lng
        };
        fun_obtienepostalodireccion(place_id);
    }
    catch (e) {
        //console.log('catch fillInAddress');
        for (var component in _componentForm) {
            document.getElementById(component).value = '';
            document.getElementById(component).disabled = false;
        }
    }

}

function marca(myLatLng) {

    var icon = {
        url: '/Content/img/placeholder-gradient.svg'
        , size: new google.maps.Size(36, 45)
    };

    fun_limpiamarcas();
    var marker = new google.maps.Marker({
        position: myLatLng
        , map: _map
        , icon: icon
    });
    _markersArray.push(marker);
}

function fun_limpiamarcas() {
    while (_markersArray.length) { _markersArray.pop().setMap(null); }
}

function fun_muestraresultado(result, centrar) {
    //console.log('fun_muestraresultado');
    for (var component in _componentForm) {
        document.getElementById(component).value = '';
        document.getElementById(component).disabled = false;
    }

    var searchAddressComponents = result[0].address_components;
    var postal;
    var city;

    if (result[0]) {
        $.each(searchAddressComponents, function () {
            if (this.types[0] == "postal_code") {
                postal = this.short_name;
            }
            else
                if (this.types[0] == "administrative_area_level_1") {
                    city = this.long_name;
                }
        });
    }



    if (postal == "" || postal == undefined) {

        //vamos a la base con latitud longitud
        GetJSONUBICACION(_Root + '/ObtieneDireccion', { NUM_LATI: _pos.lat, NUM_LONG: _pos.lng }, funResultadoDireccion, 'direccion', 'ObtieneDireccion');
    } else {
        $('#postal').val(postal);
        $('#' + txtInput).val(city);
        //console.log('txtdireccion ' + $('#txtDireccion').val());
        //console.log('postal ' + $('#postal').val());

        funCargaDireccion(postal, city, true);

        for (var i = 0; i < result[0].address_components.length; i++) {
            var addressType = result[0].address_components[i].types[0];
            if (_componentForm[addressType]) {
                var val = result[0].address_components[i][_componentForm[addressType]];
                document.getElementById(addressType).value = val;
            }
        }
    }



}

function funResultadoDireccion(data) {
    //console.log('funResultadoDireccion');
    if (data.Direccion != undefined) {

        if (data.Direccion.latitud > 0) {

            _pos = {
                lat: parseFloat(data.Direccion.latitud),
                lng: parseFloat(data.Direccion.longitud)
            };

            funCargaDireccion(data.Direccion.codigo_postal, data.Direccion.direccion, true, true);
        }
        else {
            $('#direccionCorrecto').text(false);
            $('#msjValDireccion').attr("style", "visibility: visible");
            $('#' + txtInput).focus();
            $("#btnDireccion").addClass('btnBloqueado');
            $("#btnMovDireccion").addClass('btnBloqueoMovil');
            $('#Mov_msjerrorDireccion').show();
        }
    }
    else {
        $('#direccionCorrecto').text(false);
        $('#msjValDireccion').attr("style", "visibility: visible");
        $('#' + txtInput).focus();
        $("#btnDireccion").addClass('btnBloqueado');
        $("#btnMovDireccion").addClass('btnBloqueoMovil');
        $('#Mov_msjerrorDireccion').show();
    }
}


function funCargaDireccion(postal, city, centrar, cambiadireccion) {
    //console.log('funCargaDireccion');
    $('#postal').val(postal);
    if (cambiadireccion) {
        $('#' + txtInput).val(city);
        // $('#txtDireccionMov').val(city);
    }

    $('#direccionCorrecto').text(true);
    $('#' + txtInput).removeClass('inputObligatorio');

    $("#btnDireccion").removeClass('btnBloqueado');
    $("#btnMovDireccion").removeClass('btnBloqueoMovil');
    $("#btnMovDireccion").removeClass('btnBloqueado');

    $("#latitude").val(_pos.lat);
    $("#longitude").val(_pos.lng);

    if (centrar)
        if (document.getElementById('map') != null) {
            _map.setCenter(_pos);
            marca(_pos);
        }

}

function fun_obtienepostalodireccion(placeid) {
    //console.log('fun_obtienepostalodireccion');
    _geocoder = new google.maps.Geocoder;

    if (placeid) {
        _geocoder.geocode({
            'placeId': placeid
        }, function (result, status) {
            if (status === 'OK') {
                fun_muestraresultado(result, true);
            }
        });
    }
    else {
        _geocoder.geocode({
            'location': _pos, 'region': 'mx'
        }, function (result, status) {
            if (status === 'OK') {
                fun_muestraresultado(result, true);
            }
        });
    }

}

function fun_obtiene_porciudad(ciudad) {
    //console.log('fun_obtiene_porciudad');
    _geocoder = new google.maps.Geocoder;

    _geocoder.geocode({
        'componentRestrictions':
        { 'locality': ciudad, 'country': "mx" }
    }, function (result, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            var lat = roundToTwo(result[0].geometry.location.lat());
            var lng = roundToTwo(result[0].geometry.location.lng());
            _pos = {
                lat: lat,
                lng: lng
            };
            $('#direccionCorrecto').text(true);
            $("#btnDireccion").removeClass('btnBloqueado');
            $("#btnMovDireccion").removeClass('btnBloqueoMovil');
            fun_muestraresultado(result, true);
        }
        else {
            $('#direccionCorrecto').text(false);
            $('#' + txtInput).focus();
            $("#btnMovDireccion").addClass('btnBloqueoMovil');
            $("#btnDireccion").addClass('btnBloqueado');
        }
    });
}

function fun_obtiene_porzip(zip) {
    //console.log('fun_obtiene_porzip');
    _geocoder = new google.maps.Geocoder;

    _geocoder.geocode({
        'componentRestrictions':
        { 'postalCode': zip, 'country': "mx" }
    }, function (result, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            var lat = roundToTwo(result[0].geometry.location.lat());
            var lng = roundToTwo(result[0].geometry.location.lng());
            _pos = {
                lat: lat,
                lng: lng
            };

            $('#direccionCorrecto').text(true);
            $("#btnDireccion").removeClass('btnBloqueado');
            $("#btnMovDireccion").removeClass('btnBloqueoMovil');
            $("#btnMovDireccion").removeClass('btnBloqueado');
            fun_muestraresultado(result, true);
        }
        else {

            //vamos a la base con latitud longitud
            GetJSONUBICACION(_Root + '/ObtieneDireccionxPostal', { POSTAL: zip }, funResultadoDireccion, 'direccion','ObtieneDireccionxPostal');
        }
    });

}

//#endregion Postal

//#region campostexto

function analisis_texto(pthis) {
    //console.log('analisis_texto');
    $('#Mov_msjerrorDireccion').hide();
    txtInput = pthis.id;
    if (pthis.value.length == 5 && $.isNumeric(pthis.value))
        fun_obtiene_porzip(pthis.value);
    else {
        $("#btnDireccion").addClass('btnBloqueado');
        $("#btnMovDireccion").addClass('btnBloqueoMovil');
        $('#direccionCorrecto').text(false);
    }
}

function CpIncorrecto() {
    //console.log('CpIncorrecto');
    $('#direccionCorrecto').text(false);
    $('#msjValDireccion').attr("style", "visibility: visible");
    $("#btnDireccion").addClass('btnBloqueado');
    $('#btnMovDireccion').addClass('btnBloqueado');
    $('#Mov_msjerrorDireccion').show();
}

//#endregion