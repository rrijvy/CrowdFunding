// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    var prevPosition = window.pageYOffset;
    console.log(prevPosition);
    window.onscroll = function () {
        var newPosition = window.pageYOffset;
        console.log(newPosition);
        if (prevPosition > newPosition) {
            $(".top-header").css("top", "0");
            $("#nav").css("top", "33px");
        } else {
            $(".top-header").css("top", "-33px");
            $("#nav").css("top", "0");
        }
        prevPosition = newPosition;
    };
});


