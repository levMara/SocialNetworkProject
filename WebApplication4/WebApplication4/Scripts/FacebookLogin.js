
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
    FB.getLoginStatus(function (response) {
        if (response.status === 'connected') {
            var accessToken = response.authResponse.accessToken;

            FB.api('/me', { locale: 'en_US', fields: 'id,name,birthday,location' },
                function (response) {
                    //"FacebookId=&FullName=&BirthDay=&City="
                    //?returnAction=Index&returnController=Home
                    console.log(response);
                    console.log(accessToken);
                    //send http post request
                    $.post("../account/FacebookLogin", {
                        AccessToken: accessToken,
                        FacebookId: response.id, FullName: response.name,
                        BirthDay: response.birthday, City: response.location.name,
                        WorkPlace: null
                    }, function (fbLoginSuccess) {
                        if (fbLoginSuccess) {
                            window.location.href = "../Home/Index";
                        }
                        else {
                            window.location.href = "../Account/FacebookLoginError";
                        }
                    });

                });
        }
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