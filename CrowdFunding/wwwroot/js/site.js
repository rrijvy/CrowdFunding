//---- facebook ---//
(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = 'https://connect.facebook.net/en_US/sdk.js#xfbml=1&version=v3.2';
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));

jQuery(document).ready(function () {
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


    $('.favourite').click(function () {
        var projectId = $(this).val();
        console.log(projectId);
        var url = '/Home/RemindProject/' + projectId;
        $.ajax({
            url: url,
            cache: false,
            dataType: "JSON",
            contentType: "application/x-www-form-urlencoded",
            type: 'GET'
        });
    });

    $('.sidebarBtn').on('click', function () {
        $('.sidebar').slideToggle(500);

    });

    $('.userDetails').on('click', function (e) {
        var url = this.getAttribute('href');
        console.log(url);
        e.preventDefault();
        $('.user').fadeIn(500);
        var userDetails = $('.user-details');        
        $.ajax({
            url: url,
            type: 'GET',
            cache: false,
            dataType: 'JSON',
            contentType: "application/x-www-form-urlencoded",
            success: function (response) {
                var str = "";
                str += '<div><img src="/images/' + response.email + '/' + response.avater + '" /></div>';
                str += '<h1>' + response.name + '</h1>';
                str += '<p>' + response.about + '</p>';
                str += '<p>' + response.email + '</p>';
                str += '<p>' + response.website + '</p>';
                str += '<p>From ' + response.country + '</p>';
                str += '<p>Backed ' + response.backed + ' projects.</p>';
                str += '<p> Have ' + response.companies + ' companies.</p>';
                userDetails.html(str);                
            }
        });
    });
});

