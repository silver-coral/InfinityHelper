$(document).ready(function () {
    $(".progress-bar").css("width", "100%");
    $(".progress-bar-exp").css("width", function (i, p) {
        return $(this).data("percent");
    });

    $(".char-refresh").on("click", function () {
        $.postJson("/api/clearcache");
    });

    $(".attr-add").click(function () {
        var all = parseInt($(".attr-points").text());
        if (all > 0) {
            var cur = parseInt($(this).prev().text());
            $(this).prev().text(cur + 1);

            var cura = parseInt($(this).prev().data("add"));
            $(this).prev().data("add", cura + 1);

            all--;
            $(".attr-points").text(all);
        }
    });

    $(".attr-add10").click(function () {
        var all = parseInt($(".attr-points").text());
        if (all >= 10) {
            var cur = parseInt($(this).prev().prev().text());
            $(this).prev().prev().text(cur + 10);

            var cura = parseInt($(this).prev().prev().data("add"));
            $(this).prev().prev().data("add", cura + 10);

            all -= 10;
            $(".attr-points").text(all);
        }
    });

    $(".attr-save").click(function () {
        confirm("确认按此方案配置属性点？", function () {
            var csa = $("#char-str").data("add");
            var cda = $("#char-dex").data("add");
            var cea = $("#char-eng").data("add");

            $.postJson("/api/attrsupdate?csa=" + csa + "&cda=" + cda + "&cea=" + cea);
        });
    });

    $(".epm-reset").click(function () {
        confirm("确认重置效率？", function () { 
            $.postJson("/api/reset");
        });
    });

    $("#modalRealm").on('show.bs.modal', function (event) {  
        var modal = $(this);
        $.postJson("/api/realmmaterial", {}, function (data) {
            modal.find(".modal-body").html(data);
            $(".realm-up").one("click", function () {
                $.postJson("/api/realmup");
            });
        }, "html");
    });
});