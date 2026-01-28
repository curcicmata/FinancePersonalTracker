//window.cookieConsent = {
//    accept: function () {
//        localStorage.setItem("cookieConsent", "granted");

//        if (typeof gtag === "function") {
//            gtag('consent', 'update', {
//                ad_storage: 'granted',
//                analytics_storage: 'granted'
//            });
//        }
//    },

//    decline: function () {
//        localStorage.setItem("cookieConsent", "denied");
//    },

//    check: function () {
//        return localStorage.getItem("cookieConsent");
//    }
//};

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

        // Optional: explicitly update consent to denied
        if (typeof gtag === "function") {
            gtag('consent', 'update', {
                ad_storage: 'denied',
                analytics_storage: 'denied'
            });
        }
    },

    check: function () {
        return localStorage.getItem("cookieConsent");
    },

    // Initialize consent on page load
    init: function () {
        const consent = this.check();
        if (consent === "granted") {
            this.accept();
        }
    }
};

// Auto-initialize when script loads
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', function () {
        window.cookieConsent.init();
    });
} else {
    window.cookieConsent.init();
}
