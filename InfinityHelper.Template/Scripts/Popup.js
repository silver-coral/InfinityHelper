$(document).ready(function () {
    $.initPopup();
});

$.extend({
    initPopup: function () {
        var getPlacement = function (offsetTop, contentHeight) {
            var totalHeight = $(document.body).height();
            if (totalHeight < window.innerHeight) {
                totalHeight = window.innerHeight;
            }
            return offsetTop + contentHeight <= totalHeight || contentHeight > offsetTop ? "bottom" : "top";
        };

        var triggerPopover = function (target, contentDiv, click) {
            var html = contentDiv.html();
            if (html != undefined && html != null) {
                var top = $(target).offset().top;
                var height = contentDiv.height();

                if (html.trim().length > 0) {
                    $(target).popover({
                        placement: getPlacement(top, height),
                        trigger: ($.isMobile() || !click) ? "hover" : "hover click",
                        html: true,
                        content: html,
                    });
                    $(target).popover("show");
                }
            }
        };

        $(".equip-name").hover(function () {
            var contentDiv = $(this).parent().next();
            triggerPopover($(this), contentDiv, true);
        });

        $(".skill-name").hover(function () {
            var contentDiv = $(this).next();
            if (!contentDiv.hasClass("skill-content")) {
                contentDiv = $(this).parent().next();
            }
            if (contentDiv.hasClass("skill-content")) {
                triggerPopover($(this), contentDiv, false);
            }
        });

        $(".group-name").hover(function () {
            var contentDiv = $(this).parent().next();
            triggerPopover($(this), contentDiv, false);
        });

        //IOS hover trigger
        if ($.isMobile()) {
            document.body.addEventListener('touchstart', function () { });
            //$("tr").hover(function () { });
            //$(".notice-content").hover(function () { });
            //$("sr-container").hover(function () { });
        }
    }
});