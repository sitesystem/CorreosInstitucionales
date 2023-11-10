var onloadCallback = function () {
    RCJS.scriptLoaded = true;
}
RCJS = {
    scriptLoaded: null,
    doNetObjReference: null,
    divId: null,
    publicKey: null,
    Init: function (doNetObjReference, divId, publicKey) {
        RCJS.doNetObjReference = doNetObjReference;
        RCJS.divId = divId;
        RCJS.publicKey = publicKey;
        if (RCJS.scriptLoaded === true) {
            RCJS.Render();
        } else {
            RCJS.WaitForRender();
        }
    },
    WaitForRender: function () {
        if (RCJS.scriptLoaded === true) {
            RCJS.Render();
        } else {
            setTimeout(() => RCJS.WaitForRender(), 100);
        }
    },
    Render: function () {
        grecaptcha.render(RCJS.divId,
            {
                'sitekey': RCJS.publicKey,
                'callback': (response) => { RCJS.doNetObjReference.invokeMethodAsync('RecaptchaSuccess', response); },
                'expired-callback': () => { RCJS.doNetObjReference.invokeMethodAsync('RecaptchaExpired'); }
            });        
    }
}