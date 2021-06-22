function hexc(colorval) {
    var parts = colorval.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);
    delete (parts[0]);
    for (var i = 1; i <= 3; ++i) {
        parts[i] = parseInt(parts[i]).toString(16);
        if (parts[i].length == 1) parts[i] = '0' + parts[i];
    }
    color = '#' + parts.join('');

    return color;
}

function fastSearch() {

    var attribute = "";
    var slider = $("#range").data("ionRangeSlider");
    var minprice = slider.result.from;
    var maxprice = slider.result.to;
    var color = "";
     var brand = "";
    var IsAvailable = false;
    var SortType = "";
    var SortCondition = "";
    var StartRowIndex = "";
    var PageCount = "";

    var selectedDivs = "";
    jQuery('#selectedList').html('');

    $('.n-navbar ul li').each(function () {
        var nowAtt = "[";
        jQuery(this).children('ul').children('li').children('a').children('label').children('input').each(function () {
            if ($(this).is(':checked')) {
                selectedDivs += '<div class="item">' + jQuery(this).parent('label').parent('a').parent('li').parent('ul').parent('li').children('a').text() + " : " + jQuery(this).parent('label').text() + ' <span class="fa fa-close"></span> </div>';
                nowAtt = nowAtt + $(this).attr('data-value') + ",";
            }
        });
        if (nowAtt != "[") {
            nowAtt = nowAtt.toString().substring(0, nowAtt.length - 1);
            nowAtt += "]";
            attribute += nowAtt + "#";
        }
    });


    //$('ul.attributevalue li a label input').each(function () {
    // if ($(this).is(':checked')) {
    // //attribute = attribute + $(this).attr('data-value') + ",";
    // //selectedDivs += '<div class="item">' + jQuery(this).parent('label').parent('a').parent('li').parent('ul').parent('li').children('a').text() + " : " + jQuery(this).parent('label').text() + ' <span class="fa fa-close"></span> </div>';
    // }
    //})

    $('.ajax-loading-block-window').show();
    jQuery('#selectedList').append(selectedDivs);

    $('ul.ulcolor li a label input').each(function () {
        if ($(this).is(':checked')) {
            var x = $(this).parent('label').children('span').css('backgroundColor');
            color = color + hexc(x) + ",";
        }
    })
    $('ul.Brand li a label input').each(function () {
        if ($(this).is(':checked'))
            brand = brand + $(this).attr('data-value') + ",";
    })

    if (jQuery("#switch-OnOff").is(":checked")) {
        IsAvailable = true;
    }
    SortCondition = jQuery('#SortCondition :selected').val();
    SortType = jQuery('#SortType :selected').val();
    PageCount = jQuery('#PageCount :selected').val();
    StartRowIndex = jQuery("#paginationList>li.active").text();
    var d = { "attribute": attribute.substr(0, attribute.toString().length - 1), "brand": brand.substr(0, brand.toString().length - 1), "color": color.substr(0, color.toString().length - 1), "IsAvailable": IsAvailable, "SortCondition": SortCondition, "SortType": SortType, "min": minprice, "max": maxprice, "StartRowIndex": StartRowIndex, "PageCount": PageCount, "CategoryID": $("#catid").val(), "ParentID": $("#parentid").val() };

    $('#featured-product').html('');
    $.ajax({
        cache: false,
        type: 'post',
        url: "/Catalog/FastSearch",
        data: d,
        success: function (data) {
            if (data.length > 0) {
                var AllProduct = parseInt(data[0].toString());
                Pagination(AllProduct, PageCount);
                    var allChildCount = jQuery("#paginationList").children().length;
                    if (StartRowIndex < allChildCount) {
                        jQuery("#paginationList>li.active").removeClass("active");
                        jQuery("#paginationList li:nth-child(" + (parseInt(StartRowIndex) + 1) + ")").addClass("active");
                    }
            }


            var liItem = "";
            for (var i = 1; i < data.length; i++) {
                liItem += data[i];
            }

            var ulli = "<ul class='shop-item-list row list-inline nomargin'>"

            "</ul>";
            if (liItem.toString().length > 0) {
                ulli = "<ul class='shop-item-list row list-inline nomargin'>" +
                               liItem +
                       "</ul>";
            }
            $('#featured-product').append(ulli);
            $('.ajax-loading-block-window').hide('slow');
        },
        error: function (e) {
            var ulli = "<ul class='shop-item-list row list-inline nomargin'>" +
            " کالایی یافت نشد . . .  " +
            "</ul>";

            $('#featured-product').append(ulli);
            $('.ajax-loading-block-window').hide('slow');
        }
    });
}


function Pagination(AllProduct , PageCount) {
    jQuery("#paginationList").html('');

    var Previous = '<li data-id="prev"><a href="#" aria-label="Previous"><span aria-hidden="true">&laquo;</span></a></li>';
    var Next = '<li data-id="next"><a href="#" aria-label="Next"><span aria-hidden="true">&raquo;</span></a></li>';
    var NumberList = "";
    AllProductCount = AllProduct;
    if (AllProductCount > 0 && AllProductCount > PageCount) {
        var pageNumbers = AllProductCount / PageCount;
        var pageEnd = AllProductCount % PageCount;
        for (var i = 0; i < pageNumbers; i++) {
            if (i == 0) {
                NumberList += '<li data-id="num" class="active"><a href="#">' + (i + 1) + '</a></li>';
            } else {
                NumberList += '<li data-id="num" ><a href="#">' + (i + 1) + '</a></li>';
            }
        }
    } else {
        NumberList += '<li data-id="num" ><a href="#">' +1 + '</a></li>';
    }

    jQuery("#paginationList").append(Previous + NumberList + Next);
}


jQuery("#paginationList").off().on('click', 'li', function () {

    var ElementID = jQuery(this).attr('data-id');
    if (ElementID != "next" & ElementID != "prev") {
        jQuery("#paginationList>li.active").removeClass("active");
        jQuery(this).addClass("active");
    } else {


        var allChildCount = jQuery("#paginationList").children().length-2;
        var CurrentElementIndex = jQuery("#paginationList>li.active").index()-1;

        if (ElementID == "next") {
            if (CurrentElementIndex < allChildCount-1) {
                var nextElemet = jQuery("#paginationList>li.active").next('li');
                jQuery("#paginationList>li.active").removeClass("active");
                nextElemet.addClass("active");
            }
        } else {
            if (CurrentElementIndex > 0) {
                var prevElemet = jQuery("#paginationList>li.active").prev('li');
                jQuery("#paginationList>li.active").removeClass("active");
                prevElemet.addClass("active");
            }
        }
    }
    fastSearch();
});

jQuery('.n-selectedList').off().on('click', 'span', function () {
    var elementText = jQuery(this).parent('div').text();
    var parentText = elementText.split(":")[0];
    var ChildText = elementText.split(":")[1];
    $('ul.attributevalue li a label input').each(function () {
        if (jQuery(this).is(':checked') &
            jQuery(this).parent('label').parent('a').parent('li').parent('ul').parent('li').children('a').text().trim() == parentText.toString().trim() &
            jQuery(this).parent('label').text().trim() == ChildText.trim()) {
            jQuery(this).attr("checked", false);
            
        }
    })
    jQuery(this).parent('div').hide();
    fastSearch();
});

 