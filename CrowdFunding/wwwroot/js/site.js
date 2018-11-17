$(document).ready(function () {
    var prevPosition = window.pageYOffset;
    window.onscroll = function () {
        var newPosition = window.pageYOffset;
        if (prevPosition > newPosition) {
            $(".top-header").css("top", "0");
            $("#nav").css("top", "40px");
        } else {
            $(".top-header").css("top", "-40px");
            $("#nav").css("top", "0");
        }
        prevPosition = newPosition;
    };
});

