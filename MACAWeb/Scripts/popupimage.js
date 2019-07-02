$(document).ready(function () {
    $(".popupimage a").mouseover(function () {
        $(".popupimage a img").css("display", "none"); // hide all product images
        $(this).find("img").css("display", "inline-block"); // show current hover image
    })
    $(".popupimage a").mouseout(function () {
        $(".popupimage a img").css("display", "none"); // hide all product images
    })
});