$(document).ready(function () {
    
});

function SetGoogleAnalytics(url) {
    if ((document.location.hostname.search("AgenteTop.com") !== -1 || document.location.hostname.search("AgenteTop.com.mx") !== -1) && (document.location.hostname.search("prueba.") == -1)) {
        if ("ga" in window) {
            try {
                tracker = ga.getAll()[0];
                if (tracker)
                    tracker.send("pageview", url);
            }
            catch (e) {
                console.log("***** Error *****");
                console.log(e);
                console.log("***** Error *****");
            }
        }
    }
}