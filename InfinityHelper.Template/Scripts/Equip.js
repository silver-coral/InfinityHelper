$(document).ready(function () {
    $(".equip-on").on("click", function () {
        var eid = $(this).data("id");
        $.postJson("/api/equipon?eid=" + eid);
    });

    $(".equip-off").on("click", function () {
        var eid = $(this).data("id");
        $.postJson("/api/equipoff?eid=" + eid);
    });

    $(".equip-bind").on("click", function () {
        var eid = $(this).data("id");
        $.postJson("/api/equipbind?eid=" + eid);
    });

    $(".equip-unbind").on("click", function () {
        var eid = $(this).data("id");
        $.postJson("/api/equipunbind?eid=" + eid);
    });

    $(".equip-sell").on("click", function () {
        var eid = $(this).data("id");
        var equip = $(this).parent().find(".equip-name");
        confirm("确认出售" + equip.prop("outerHTML") + "？", function () {
            $.postJson("/api/equipsell?eids=" + eid);
        });
    });

    $(".equip-use").on("click", function () {
        var eid = $(this).data("id");
        var equip = $(this).parent().find(".equip-name");
        confirm("确认使用道具" + equip.prop("outerHTML") + "？", function () {
            $.postJson("/api/itemuse?eid=" + eid);
        });
    });

    $(".equip-sellall").on("click", function () {
        confirm("确认出售当前过滤下所有未绑定物品？", function () {
            var array = [];
            $(".bag-container .equip-name").each(function (i, p) {
                var bind = $(p).data("bind");
                if (bind == "0") {
                    array.push($(p).data("id"));
                }
            });

            if (array.length > 0) {
                var eids = array.join(",");
                $.postJson("/api/equipsell?eids=" + eids);
            }
        });
    });

    $("#modalUpgrade").on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var equip = $(button).parent().find(".equip-name");
        var eid = button.data('id');

        var modal = $(this);
        modal.find(".modal-title").html("强化" + equip.prop("outerHTML"));

        $.postJson("/api/equipupgradematerial?eid=" + eid, {}, function (data) {
            modal.find(".modal-body").html(data);
            $(".equip-upgrade").one("click", function () {
                $.postJson("/api/equipupgrade?eid=" + eid);
            });
        }, "html");
    });

    $(".panel-filter").each(function (i, input) {
        var value = window.localStorage.getItem($(this).attr("id"));
        if (value != null && value.length > 0) {
            $(this).val(value);
            $(this).trigger("propertychange");
        }
    });

    $(".panel-filter").on("input propertychange", function () {
        $(this).parent().prev().find(".selected").removeClass("selected")
        var value = $(this).val();
        window.localStorage.setItem($(this).attr("id"), value);
        if (value.length > 0) {
            var values = value.split(",");
            var equips = $(this).parent().prev().find(".equip-content");
            equips.each(function (i, e) {
                var match = 0;
                $.each(values, function (j, p) {
                    if ($(e).text().indexOf(p) >= 0) {
                        match++;
                    }
                });
                if (match == values.length) {
                    $(e).prev().addClass("selected");
                }
            });
        }
    });
});