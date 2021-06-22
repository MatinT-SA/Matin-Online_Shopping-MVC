
function showMessage(msg) {
    $.msg({ content: msg });
}

function maxLenght(textbox, max) {

    if ($(textbox).val().length >= max)
        return false;
}

function LoadStates(component) {
    component.empty(); //@Url.Action("GetCities")
    component.prepend($('<option></option>').html('کمی صبر کنید ...'));
    $.getJSON('/Base/GetStates',
        {})
    .done(function (data) {
        component.empty();
        $.each(data, function (i, city) {
            var option = '<option value="' + city.SelectedValue + '"> ' + city.DisplayValue + '</option>';// $('<option />').html(city);
            component.append(option);
        });
    })
    .fail(function (jqxhr, textStatus, error) {
        component.empty();
        var err = textStatus + ", " + error;
        console.log("Request Failed: " + err);
    });
}

function LoadParent(component, Id) {

    if (Id.length > 0) {
        component.empty();
        component.prepend($('<option></option>').html('کمی صبر کنید ...'));
        $.getJSON('/Base/GetParent',
            { ParentID: Id })
        .done(function (data) {
            component.empty();
            $.each(data, function (i, city) {
                if (city.SelectedValue == null) {
                    city.SelectedValue = '';
                }
                var option = '<option value="' + city.SelectedValue + '"> ' + city.DisplayValue + '</option>';
                component.append(option);
            });
        })
        .fail(function (jqxhr, textStatus, error) {
            component.empty();
            var err = textStatus + ", " + error;
            console.log("Request Failed: " + err);
        });
    }
}


//ajaxError
$(document).on({

    ajaxStart: function () {
        var $focused = $(':focus');
        if ($focused != undefined && $focused != null)
            if ($focused.hasClass("NotIndicate"))
                return true;
        $('#loading-indicator').css("visibility", "visible");
    },
    ajaxStop: function () {
        $('#loading-indicator').css("visibility", "collapse");
        $("select:not(.noSelect)").select2({ dir: 'rtl' });
    },

    ajaxError: function (event, request, settings, thrownError) {
        $('#loading-indicator').css("visibility", "collapse");
        $("select:not(.noSelect)").select2({ dir: 'rtl' });
        //if (request.status == 300) {
        //    showMessage(request.responseText);
        //    return false;
        //} else
        //   showMessage("Error In Server")
        //showMessage('پردازش درخواستی با مشکل روبرو شد');
    },

});

function check_KeyPress(e, AllowPast) {
    var keycode;
    if (e.charCode != undefined && e.charCode != 0)
        keycode = e.charCode;
    else if (e.keyCode != undefined && e.keyCode != 0)
        keycode = e.keyCode;
    else
        return true;

    if (keycode == 46)
        return false;
    if (keycode >= 48 && keycode <= 57)
        return true;
    else if (keycode == 8 || keycode == 9)
        return true;
    else if (AllowPast == true && keycode == 118)
        return true;
    else
        return false;
}

function checkComma(textbox) {
    var comma = ',';
    while (textbox.value.indexOf(comma) > 0) {
        textbox.value = textbox.value.replace(comma, '');
    }
    var num = textbox.value;
    if (num.length != 0 && parseInt(num)) {
        var regex = new RegExp('(-?[0-9]+)([0-9]{3})');
        while (regex.test(num)) {
            num = num.replace(regex, '$1' + comma + '$2');
        }
    }
    textbox.value = num;
};

function LoadCities(state, component) {
    if (state.length > 0) {

        component.empty();
        component.prepend($('<option></option>').html('کمی صبر کنید ...'));
        $.getJSON('/Base/GetCities',
            { StateID: state })
        .done(function (data) {
            component.empty();
            $.each(data, function (i, city) {
                if (city.SelectedValue == null) {
                    city.SelectedValue = '';
                }
                var option = '<option value="' + city.SelectedValue + '"> ' + city.DisplayValue + '</option>';// $('<option />').html(city);
                component.append(option);
            });
        })
        .fail(function (jqxhr, textStatus, error) {
            component.empty();
            var err = textStatus + ", " + error;
            console.log("Request Failed: " + err);
        });
    }
}

function LoadCities2(state, component) {
    if (state.length > 0) {

        component.empty();
        component.prepend($('<option></option>').html('کمی صبر کنید ...'));
        $.getJSON('/Base/GetCities2',
            { StateID: state })
        .done(function (data) {
            component.empty();
            $.each(data, function (i, city) {
                if (city.SelectedValue == null) {
                    city.SelectedValue = '';
                }
                var option = '<option value="' + city.SelectedValue + '"> ' + city.DisplayValue + '</option>';// $('<option />').html(city);
                component.append(option);
            });
        })
        .fail(function (jqxhr, textStatus, error) {
            component.empty();
            var err = textStatus + ", " + error;
            console.log("Request Failed: " + err);
        });
    }
}

function LoadPlacements(cityid, component) {
    if (cityid.length > 0) {

        component.empty();
        component.prepend($('<option></option>').html('کمی صبر کنید ...'));
        $.getJSON('/Base/GetPlacements',
            { CityID: cityid })
        .done(function (data) {
            component.empty();
            $.each(data, function (i, city) {
                if (city.SelectedValue == null) {
                    city.SelectedValue = '';
                }
                var option = '<option value="' + city.SelectedValue + '"> ' + city.DisplayValue + '</option>';// $('<option />').html(city);
                component.append(option);
            });
        })
        .fail(function (jqxhr, textStatus, error) {
            component.empty();
            var err = textStatus + ", " + error;
            console.log("Request Failed: " + err);
        });
    }
}

function LoadTownShip(city, component) {
    if (city.length > 0) {

        component.empty(); //@Url.Action("GetCities")
        component.prepend($('<option></option>').html('کمی صبر کنید ...'));
        $.getJSON('/Base/GetTownships',
            { CityID: city })
        .done(function (data) {
            component.empty();
            $.each(data, function (i, city) {
                if (city.SelectedValue == null) {
                    city.SelectedValue = '';
                }
                var option = '<option value="' + city.SelectedValue + '"> ' + city.DisplayValue + '</option>';// $('<option />').html(city);
                component.append(option);
            });
        })
        .fail(function (jqxhr, textStatus, error) {
            component.empty();
            var err = textStatus + ", " + error;
            console.log("Request Failed: " + err);
        });
    }
}

function LoadSections(city, component) {
    if (city.length > 0) {

        component.empty(); //@Url.Action("GetCities")
        component.prepend($('<option></option>').html('کمی صبر کنید ...'));
        $.getJSON('/Base/GetSections',
            { CityID: city })
        .done(function (data) {
            component.empty();
            $.each(data, function (i, city) {
                if (city.SelectedValue == null) {
                    city.SelectedValue = '';
                }
                var option = '<option value="' + city.SelectedValue + '"> ' + city.DisplayValue + '</option>';// $('<option />').html(city);
                component.append(option);
            });
        })
        .fail(function (jqxhr, textStatus, error) {
            component.empty();
            var err = textStatus + ", " + error;
            console.log("Request Failed: " + err);
        });
    }
}

function LoadStreets(city, component) {
    if (city.length > 0) {

        component.empty();
        component.prepend($('<option></option>').html('کمی صبر کنید ...'));
        $.getJSON('/Base/GetStreets',
            { CityID: city })
        .done(function (data) {
            component.empty();
            $.each(data, function (i, city) {
                if (city.SelectedValue == null) {
                    city.SelectedValue = '';
                }
                var option = '<option value="' + city.SelectedValue + '"> ' + city.DisplayValue + '</option>';// $('<option />').html(city);
                component.append(option);
            });
        })
        .fail(function (jqxhr, textStatus, error) {
            component.empty();
            var err = textStatus + ", " + error;
            console.log("Request Failed: " + err);
        });
    }
}

function LoadActivityContext(actTypeID, component) {
    if (actTypeID.length > 0) {

        component.empty(); //@Url.Action("GetCities")
        component.prepend($('<option></option>').html('کمی صبر کنید ...'));
        $.getJSON('/Base/GetContexts',
            { ActTypeID: actTypeID })
        .done(function (data) {
            component.empty();
            $.each(data, function (i, context) {
                if (context.SelectedValue == null) {
                    context.SelectedValue = '';
                }
                var option = '<option value="' + context.SelectedValue + '"> ' + context.DisplayValue + '</option>';
                component.append(option);
            });
        })
        .fail(function (jqxhr, textStatus, error) {
            component.empty();
            var err = textStatus + ", " + error;
            console.log("Request Failed: " + err);
        });
    }
}

function LoadFields(id, component) {
    if (id.length > 0) {
        component.empty();
        component.prepend($('<option></option>').html('کمی صبر کنید ...'));
        $.getJSON('/Base/GetFields',
            { MajorID: id })
        .done(function (data) {
            component.empty();
            $.each(data, function (i, fields) {
                if (fields.SelectedValue == null) {
                    fields.SelectedValue = '';
                }
                var option = '<option value="' + fields.SelectedValue + '"> ' + fields.DisplayValue + '</option>';
                component.append(option);
            });
        })
        .fail(function (jqxhr, textStatus, error) {
            component.empty();
            var err = textStatus + ", " + error;
            console.log("Request Failed: " + err);
        });
    }
}

function PostData(formn, errContent, idd, event) {

    event.preventDefault();
    $(this).attr("disabled", true);
    var _form = $("#" + formn);
    var _validator = _form.validate();
    var hasError = false;
    _form.find("input").each(function () {
        if ($(this).attr("type") != "hidden") {
            if (!_validator.element(this)) {
                hasError = true;
            }
        }
    });

    if (hasError) {
        alert("لطفا خطاهای به وجود امده را تصحیح کنید");
    }
    else {
        var url = _form.attr("action");
        var formData = _form.serialize();
        $("#" + formn + " input[disabled]").each(function () {
            formData += '&' + $(this).attr("name") + '=' + $(this).val();
        });
        $.ajax(
            {
                url: url,
                data: formData,
                type: "Post",

                success: function (data) {
                    $("#" + errContent).empty();
                    if (data.IsValid == "true" || data.IsValid == true) {
                        $("#" + idd).attr("value", $(data)[0].ID).trigger('change');
                        $("#" + errContent).append($(data)[0].Message);
                    }
                    else {
                        $("#" + errContent).append($(data)[0].Message);
                    }
                }
            });
    }
}

function PjaxPostData(obj, formn) {
    try {
        event.preventDefault();
        //Event.preventDefault();
        $(obj).attr("disabled", true);
        var form = $("#" + formn);
        var _validator = form.validate();
        var hasError = false;
        form.find("input").each(function () {
            if ($(this).attr("type") != "hidden") {
                if (!_validator.element(this)) {
                    hasError = true;
                }
            }
        });

        if (hasError) {
            $(obj).removeAttr("disabled");
            alert("لطفا خطاهای به وجود امده را تصحیح کنید");
            event.preventDefault();
        }
        else {

            $.bootstrapModalConfirm({

                onConfirm: function () {
                    $.pjax({
                        container: "#pjaxContainer",
                        type: "Post",
                        url: form.attr("action"),
                        data: form.serialize()
                    }).done(function () {
                        $(obj).removeAttr("disabled");
                    })
                    .error(function () {
                        $(obj).removeAttr("disabled");
                    })
                },
                notConfirm: function () { $(obj).removeAttr("disabled"); }
            });
        }
    }
    catch (err) {
        $(obj).removeAttr("disabled");
    }
    return false;
}



function PjaxGetData(data, url) {
    event.preventDefault();
    $.pjax({
        container: "#pjaxContainer",
        type: "GET",
        url: url,
        data: data
    }).done(function () {

    });
    return false;
}

$(function () {
    var cooki = getCookie("CurrentLanguageCookie");
    var obj = $("div#footer > div#langManager");
    if (cooki == "En-US" && !obj.hasClass("ltr")) {
        $("div#footer > div#langManager").addClass("ltr");
        $("#footer").css("text-align", "left");
        //$("div#footer > div#langManager").addClass("ltr");
    } else if (cooki == "fa-IR" && !obj.hasClass("rtl")) {
        $("div#footer > div#langManager").addClass("rtl");
        $("#footer").css("text-align", "right");
        //$("div#footer > div#langManager").addClass("rtl");
    }

    $("a.lang").on("click", function (e) {
        e.preventDefault();
        var lang = $(this).data("lang");
        setCookie("CurrentLanguageCookie", lang, 365);

        window.location.reload();

    });

    function setCookie(name, value, activeFor) {
        var d = new Date();
        d.setTime(d.getTime() + (activeFor * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toUTCString();
        document.cookie = name + "=" + value + "; " + expires;
    };

    function getCookie(cname) {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1);
            if (c.indexOf(name) != -1) return c.substring(name.length, c.length);
        }
        return "";
    }

});

(function ($) {

    $.bootstrapModalConfirm = function (options) {
        var isCancle = true;
        var defaults = {
            caption: 'تائید عملیات',
            body: 'آیا عملیات درخواستی اجرا شود؟',
            onConfirm: null,
            notConfirm: null,
            confirmText: 'تائید',
            closeText: 'انصراف'
        };
        var options = $.extend(defaults, options);

        var confirmContainer = "#confirmContainer";
        var html = '<div class="modal fade in rtl" id="confirmContainer" role="dialog" aria-hidden="true" aria-labelledby="myModalLabel">' +
            '<div class="modal-dialog">' +
            '<div class="modal-content">' +
            '<div class="modal-header">' +
                        '<button type="button" class="close" data-dismiss="modal"><span aria-hidden="true"> &times; </span><span class="sr-only">بستن</span></button>'
                        + '<h5 class="modal-title">' + options.caption + '</h5></div>' +
                   '<div class="modal-body center"><h3>'
                        + options.body + '</h3></div> ' +
                   '<div class="modal-footer">'
                        + '<a href="#" class="btn btn-info" id="confirmBtn">' + options.confirmText + '</a> &nbsp;&nbsp;'
                    + '<a href="#" class="btn btn-default" id="notConfirmbtn" data-dismiss="modal">' + options.closeText + '</a></div></div> </div></div>';
        //<a class="close" data-dismiss="modal">&times;</a>
        $(confirmContainer).remove();
        $(html).appendTo('body');
        $(confirmContainer).modal('show');

        $('#confirmBtn', confirmContainer).click(function (e) {
            e.preventDefault();
            isCancle = false;
            if (options.onConfirm)
                options.onConfirm();
            $(confirmContainer).modal('hide');

        });

        $('#confirmContainer').on('hidden.bs.modal', function () {
            if (isCancle == true && options.notConfirm)
                options.notConfirm();
        })

        //$("#notConfirmbtn", confirmContainer).click(function () {

        //    $(confirmContainer).modal('hide');
        //});
    };

})(jQuery);