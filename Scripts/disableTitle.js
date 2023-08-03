$(document).ready(function () {
    $.each($('.no-tooltip'), function (i, value) {
        $(this).hover(function () {
            var title = $(this).attr("title");
            $(this).attr("tmp_title", title);
            $(this).attr("title", "");

            var alt = $(this).attr("alt");
            $(this).attr("tmp_alt", alt);
            $(this).attr("alt", "");
        }, function () {
            var title = $(this).attr("tmp_title");
            $(this).attr("title", title);

            var alt = $(this).attr("tmp_alt");
            $(this).attr("alt", alt);
        }
        );
    });
});