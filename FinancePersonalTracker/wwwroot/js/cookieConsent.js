window.cookieConsent = {
    accept: function () {
        localStorage.setItem("cookieConsent", "granted");

        if (typeof gtag === "function") {
            gtag('consent', 'update', {
                ad_storage: 'granted',
                analytics_storage: 'granted'
            });
        }
    },

    decline: function () {
        localStorage.setItem("cookieConsent", "denied");
    },

    check: function () {
        return localStorage.getItem("cookieConsent");
    }
};
