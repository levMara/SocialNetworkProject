
function initFacebook() {
    //set up facebook javascript sdk configuration
    window.fbAsyncInit = function () {
        FB.init({
            appId: '523392351508330',
            cookie: true,
            xfbml: true,
            version: 'v3.2'
        });
    };

    // Load the SDK asynchronously
    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) return;
        js = d.createElement(s); js.id = id;
        js.src = 'https://connect.facebook.net/en_US/sdk.js#xfbml=1&version=v3.2&appId=523392351508330&autoLogAppEvents=1';
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));

    
}


//public_profile, user_location, user_birthday
function getFbUserData() {
    FB.api('/me', { locale: 'en_US', fields: 'id,name,birthday,location' },
        function (response) {
            //"FacebookId=&FullName=&BirthDay=&City="
            //?returnAction=Index&returnController=Home
            console.log(response);

            //send http post request
            $.post("../account/FacebookLogin", {
                FacebookId: response.id, FullName: response.name,
                BirthDay: response.birthday, City: response.location.name
            });
            document.cookie = "username=John Doe; expires=Thu, 18 Dec 2013 12:00:00 UTC; path=/"
            //redirect
            window.location.href = "../Home/Index";



            //birthday: "12/31/1993", id: "108626436880589", name: "Saed Abu"
            //console.log(response.location);
            //console.log(response.hometown.name);
            
            //document.getElementById('status').innerHTML = 'Thanks for logging in, ' + response.first_name + '!';
            //document.getElementById('userData').innerHTML = '<p><b>FB ID:</b> ' + response.id + '</p><p><b>Name:</b> ' + response.first_name + ' ' + response.last_name + '</p><p><b>Email:</b> ' + response.email + '</p><p><b>•user_age_range:</b> ' + response.user_age_range + '</p><p><b>Locale:</b> ' + response.locale + '</p><p><b>Picture:</b> <img src="' + response.picture.data.url + '"/></p><p><b>FB Profile:</b> <a target="_blank" href="' + response.link + '">click to view profile</a></p>';
        });
}


function fbOnLogin() {
    //$.post("../account/FacebookLogin", { Username: $("#login_form input[name=Username]").val(), Password: "2pm", isFaceBookUser: false });
    getFbUserData();
}

function fbLogout() {
    FB.logout(function () {
        hideFbUserData();
    });
}