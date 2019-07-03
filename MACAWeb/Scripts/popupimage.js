/*$(document).ready(function () {
    $(".popupimage a").mouseover(function () {
        //$(".popupimage a img").css("display", "none"); // hide all product images
        $(this).find("img").css({
            "display": "block",
            "position": "absolute",
            "width": "80",
            "height": "90",
            "border-radius": "15px",
            "border": "1px solid #000000",
            "padding": "5px",
            "background-color": "var(--white)"
        });//show div
    })
    $(".popupimage a").mouseout(function () {
        $(".popupimage a img").css("display", "none"); // hide all product images
    })
});*/
$(document).ready(function () {
    $(".popup").mouseover(function () {
        var AllElements = $("div");
        //var parentOffset = $(this).parent.offset();
        //var mouseX = event.pageX - parentOffset.left;
        //var mouseY = event.pageY - parentOffset.top;
        
        $(this).find(AllElements).css({
            "display": "block",
            "position": "absolute",
            "top": 25,
            "left": 50,
            "z-index": "10"
        });//show div
    })
    $(".popup").mouseout(function () {
        var AllElements = $("div");
        $(this).find(AllElements).css("display", "none");
    })
});