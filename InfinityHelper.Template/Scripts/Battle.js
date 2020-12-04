(function ($) {
    $.fn.extend({
        "battle": function (options) {
            _this = this;

            if (!isValid(options)) {
                return _this;
            }
            opts = $.extend({}, defaluts, options);
            
            if (opts.waitTime > 0) {
                timer = setInterval(wait, 1000);
            }
            else {
                turns = _this.length;
                showNextTurn();
                timer = setInterval(showNextTurn, opts.interval);
            }

            return this;
        }
    });

    var _this;
    var turns;
    var timer;
    var opts;

    //默认参数
    var defaluts = {
        interval: 1000,
        guaji: 1,
        waitTime: 0
    };

    //检测参数是否合法
    function isValid(options) {
        return !options || (options && typeof options === "object") ? true : false;
    }
    
    function wait() {
        opts.waitTime--;
        $("#time").text(opts.waitTime);
        if (opts.waitTime <= 0) {
            clearInterval(timer);
            location.replace(location.href);            
        }
    }

    function showNextTurn() {
        if (turns < 0) {
            clearInterval(timer);
            if (opts.guaji == 1) {
                location.replace(location.href);
            }
            else {
                var height = $(".battle-data").children().height();
                $(".battle-data").removeClass("visually-hidden").height(height);
            }
        }
        else {
            var t = _this.eq(turns);
            var td = t.find(".turn-data");            

            var hpData = td.data("hp");
            for (var i in hpData) {
                var hp = hpData[i];

                var char = $("#char_" + hp.id);
                if (char != null) {
                    if (hp.chp <= 0) {
                        char.addClass("dead");
                    }

                    var chpp = (hp.chp * 100 / hp.hp).toFixed(2);
                    char.find(".progress-bar-life").css("width", chpp + "%");
                    char.find(".progress-bar-life > span").text(hp.chp + "/" + hp.hp);
                    //var cmpp = (hp.cmp * 100 / hp.mp).toFixed(2);
                    //char.find(".progress-bar-mana").css("width", cmpp + "%");
                    //char.find(".progress-bar-mana > span").text(hp.cmp + "/" + hp.mp);
                }
            }

            t.show();
            turns--;
        }
    }
})(window.jQuery);