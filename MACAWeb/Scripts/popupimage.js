$(document).ready(function () {
    $(".popupimage a").mouseover(function () {
        $(".popupimage a img").css("display", "none"); // hide all product images
        $(this).find("img").css({
            //display: "inline-block",
            "display": "grid",
            "position": "absolute",
            "width": "80",
            "height": "90",
            "border-radius": "15px",
            "border": "2px solid #000000"
        });//show div
    })
    $(".popupimage a").mouseout(function () {
        $(".popupimage a img").css("display", "none"); // hide all product images
    })
});