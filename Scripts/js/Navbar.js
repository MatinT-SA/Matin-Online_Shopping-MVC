/// <reference path="jquery-2.2.0.js" />

var maxwidth = 960;

var width = jQuery(window).width();
if (width > maxwidth) {
    jQuery('.es-nav-mobile').hide();
    jQuery('.es-navbar').show();
} else {
    jQuery('.es-navbar').hide();
    jQuery('.es-nav-mobile').show();
}

jQuery(window).resize(function () {

    width = jQuery(window).width();
    if (width > maxwidth) {

        jQuery('.es-nav-mobile').hide();
        jQuery('.es-navbar').show();
        jQuery('.es-nav-mobile span').removeClass('fa-times');
        jQuery('.es-nav-mobile span').addClass('fa-bars');


    } else {
        if (jQuery('.es-nav-mobile span').hasClass('fa-bars')) {

            jQuery('.es-nav-mobile').show();
            jQuery('.es-navbar').hide();
            jQuery('.es-nav-mobile span').removeClass('fa-times');
            jQuery('.es-nav-mobile span').addClass('fa-bars');

        }
    }

})

jQuery('.es-nav-mobile').on('click', function () {
    if (jQuery('.es-navbar').is(':visible')) {
        jQuery('.es-navbar').fadeOut();
        jQuery('.es-nav-mobile span').removeClass('fa-times');
        jQuery('.es-nav-mobile span').addClass('fa-bars');
    } else {
        jQuery('.es-navbar').fadeIn();
        jQuery('.es-nav-mobile span').removeClass('fa-bars');
        jQuery('.es-nav-mobile span').addClass('fa-times');
    }
});

jQuery('.second-level .second-level-2').on('mouseleave', function () {

    jQuery('.third-level').hide();
    jQuery('.third-level').children('div').hide();

});

jQuery('.second-level .nav-btn').on('mouseenter', function () {

    var element = jQuery(this);
    var element_id = element.attr('id');

    jQuery(this).css({
        "border-bottom-color": "#e82139",
        "border-bottom-weight": "3px",
        "border-bottom-style": "solid"
    });

    if (width > maxwidth) {

        jQuery('.third-level').hide();
        jQuery('.third-level').children('div').hide();


        jQuery('.third-level').show();
        jQuery('.third-level  .' + element_id).show();



        var img = jQuery(this).attr('data-img');
       
        jQuery('.third-level  .' + element_id)
            .delay(500)
            .queue(function (next) {
                $(this)
                    .css({ "background-image": "url( " + img + " )" });
            });


    }

}).on('mouseleave', function () {

    jQuery(this).css({ "border-bottom": "none" });

});

jQuery('.third-level  .third-level-2').on('mouseenter', function () {

    var element = jQuery(this);
    var element_id = element.attr('data-id');

    jQuery('.second-level .second-level-2 #' + element_id).css({
        "border-bottom-color": "#e82139",
        "border-bottom-weight": "3px",
        "border-bottom-style": "solid"
    });

    jQuery('.third-level').hide();
    jQuery('.third-level').children('div').hide();


    jQuery('.third-level').show();
    jQuery('.third-level  .' + element_id).show();



}).on('mouseleave', function () {

    jQuery('.third-level').hide();
    jQuery('.third-level').children('div').hide();

    jQuery('.second-level .second-level-2 .nav-btn').css({ "border-bottom": "none" });

});

