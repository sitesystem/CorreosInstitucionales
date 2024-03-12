export const reCAPTCHA =
{
    scriptLoaded : null
};

reCAPTCHA.waitScriptLoaded = function (resolve) {
    if (typeof (grecaptcha) !== 'undefined' && typeof (grecaptcha.render) !== 'undefined')
        resolve();
    else
        setTimeout(() => reCAPTCHA.waitScriptLoaded(resolve), 100);
}

reCAPTCHA.init = function () {
    const scripts = Array.from(document.getElementsByTagName('script'));

    if (!scripts.some(s => (s.src || '').startsWith('https://www.google.com/recaptcha/api.js'))) {
        const script = document.createElement('script');
        script.src = 'https://www.google.com/recaptcha/api.js?render=explicit';
        script.async = true;
        script.defer = true;
        document.head.appendChild(script);
    }
    if (reCAPTCHA.scriptLoaded === null)
        reCAPTCHA.scriptLoaded = new Promise(reCAPTCHA.waitScriptLoaded);

    return reCAPTCHA.scriptLoaded;
}

reCAPTCHA.render = function (dotNetObj, selector, siteKey) {
    console.log(dotNetObj);
    console.log(selector);
    console.log(siteKey);

    return grecaptcha.render(selector, {
        'sitekey': siteKey,
        'callback': (response) => { dotNetObj.invokeMethodAsync('CallbackOnSuccess', response); },
        'expired-callback': () => { dotNetObj.invokeMethodAsync('CallbackOnExpired'); }
    });
}

reCAPTCHA.getResponse = function (widgetId) {
    return grecaptcha.getResponse(widgetId);
}
