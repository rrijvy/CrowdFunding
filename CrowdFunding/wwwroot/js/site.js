$(document).ready(function () {
    
    var prevPosition = window.pageYOffset;
    
    

    window.onscroll = function () {
        var scrollPosition = window.pageYOffset;

        if (prevPosition > scrollPosition) {
            $(".top-header").css("top", "0");
            $("#nav").css("top", "40px");
        } else {
            $(".top-header").css("top", "-40px");
            $("#nav").css("top", "0");
        }
        prevPosition = scrollPosition;




        

    };



});

