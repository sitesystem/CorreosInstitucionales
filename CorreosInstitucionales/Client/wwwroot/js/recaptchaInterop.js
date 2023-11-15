window.initRecaptcha = function () {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Ld0YAkpAAAAAExADS4hr3ZCw0f6P3NdzJ64d8cg', { action: 'submit' }).then(function (response) {
            DotNet.invokeMethodAsync('Registro.razor', 'RecaptchaCallback', response);
            //const itoken = document.getElementById('token');
            //itoken.value = response;
            
        });
    });
};
