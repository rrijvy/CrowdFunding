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

    


    //var myNode = document.querySelector('.user');
    //var element = document.querySelectorAll('#user');
    //for (var i = 0; i < element.length; i++) {
    //    element[i].addEventListener('click', function (e) {
    //        e.preventDefault();
    //        var url = element[i].getAttribute('href');
    //        console.log(url);
    //        myNode.style.display = 'block';
    //        $.ajax({
    //            url: url,
    //            type: 'GET',
    //            cache: false,
    //            dataType: JSON,
    //            contentType: "application/x-www-form-urlencoded",
    //            success: function (response) {
    //                var str = "";
    //                str += '<img src="~/images/' + response.email + '/' + response.avater + '" />';
    //                str += '<h1>' + response.name + '</h1>';
    //                str += '<p>' + response.about + '</p>';
    //                str += '<p>' + response.email + '</p>';
    //                str += '<p>' + response.website + '</p>';
    //                str += '<p>' + response.backed + '</p>';
    //                str += '<p>' + response.companies + '</p>';
    //                console.log(str);

    //                myNode.children().html(str);

    //            }
    //        });
    //    });
    //}

});

