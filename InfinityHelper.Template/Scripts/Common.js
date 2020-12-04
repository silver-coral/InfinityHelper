$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
})
window.jsVersion = "2.0.2";
window.oldAlert = window.alert;
window.oldConfirm = window.confirm;

window.alert = function (msg) {
    $("#modalAlert .modal-body").html(msg);
    $("#modalAlert").modal("show");
};

window.confirm = function (msg, callback) {
    $("#modalConfirm .modal-body").html(msg);
    $("#modalConfirm .confirm-ok").off().one("click", callback);
    $("#modalConfirm").modal("show");
};

JSON.tryParse = function (str) {
    if (typeof str == 'string') {
        try {
            var obj = JSON.parse(str);
            if (str.indexOf('{') > -1) {
                return obj;
            }
        } catch (e) {

        }
    }
    return false;
}

$.extend({
    postJson: function (url, data, callback, dataType) {
        if (!data) {
            data = {};
        }
        if (!callback) {
            callback = function () {
                window.location.reload();
            };
        }
        if (!dataType) {
            dataType = "json";
        }

        $.ajax({
            type: "POST",
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(data),
            cache: false,
            success: callback,
            error: function (e) {
                alert(e.responseText);
            },
            dataType: dataType
        });
    },
    isMobile: function () {
        var sUserAgent = navigator.userAgent.toLowerCase();
        var bIsIpad = sUserAgent.match(/ipad/i) == "ipad";
        var bIsIphoneOs = sUserAgent.match(/iphone os/i) == "iphone os";
        var bIsMidp = sUserAgent.match(/midp/i) == "midp";
        var bIsUc7 = sUserAgent.match(/rv:1.2.3.4/i) == "rv:1.2.3.4";
        var bIsUc = sUserAgent.match(/ucweb/i) == "ucweb";
        var bIsAndroid = sUserAgent.match(/android/i) == "android";
        var bIsCE = sUserAgent.match(/windows ce/i) == "windows ce";
        var bIsWM = sUserAgent.match(/windows mobile/i) == "windows mobile";

        if (bIsIpad || bIsIphoneOs || bIsMidp || bIsUc7 || bIsUc || bIsAndroid || bIsCE || bIsWM) {
            return true;
        } else {
            return false;
        }
    },
    isHtml5: function () {
        var elem = document.createElement('canvas');
        return !!(elem.getContext && elem.getContext('2d'));
    },
    disableClick: function (obj) {
        $(obj).click(null);
    },
    drawAnimation: function (fps, func) {
        var requestAnimationFrame =
            window.requestAnimationFrame || //Chromium  
            window.webkitRequestAnimationFrame || //Webkit 
            window.mozRequestAnimationFrame || //Mozilla Geko 
            window.oRequestAnimationFrame || //Opera Presto 
            window.msRequestAnimationFrame;

        var now;
        var then = Date.now();
        var interval = 1000 / fps;
        var delta;
        var stop = false;

        var tick = function () {
            if (stop) {
                return;
            }

            if (requestAnimationFrame) {
                requestAnimationFrame(tick);
                now = Date.now();
                delta = now - then;
                if (delta > interval) {
                    then = now - (delta % interval);
                    if (func() == false) {
                        stop = true;
                    }
                }
            }
            else {
                setTimeout(tick, interval);
                if (func() == false) {
                    stop = true;
                }
            }
        }
        tick();
    }
});