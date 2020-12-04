$(document).ready(function () {
    $("#modalBuy").on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);       
        var eid = button.data('id');
        var equip = button.parent().find(".equip-name");
        var modal = $(this);       
        modal.find(".modal-title").html("购买" + equip.prop("outerHTML"));

        $(".market-buy").on("click", function () {
            var count = modal.find(".count").val();
            $.postJson("/api/marketbuy?eid=" + eid + "&count=" + count);            
        });
    });

    $("#modalUse").on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var eid = button.data('id');
        var equip = button.parent().find(".equip-name");
        var modal = $(this);
        modal.find(".modal-title").html("使用" + equip.prop("outerHTML"));

        $(".item-use").on("click", function () {
            var count = modal.find(".count").val();
            $.postJson("/api/itemuse?eid=" + eid + "&count=" + count);
        });
    });

    $("#modalMake").on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var eid = button.data('id');
        var equip = button.parent().find(".equip-name");
        var modal = $(this);
        modal.find(".modal-title").html("合成" + equip.prop("outerHTML"));

        $(".synthetic-make").on("click", function () {
            var count = modal.find(".count").val();
            $.postJson("/api/syntheticmake?sid=" + eid + "&count=" + count);
        });
    });
});