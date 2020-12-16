$(document).ready(function () {
    $(".map-apply").on("click", function () {
        var mapId = $("#map").val();
        $.postJson("/api/swithmap?id=" + mapId);
    });

    $(".battle-guaji").on("click", function () {
        $.postJson("/api/guaji");
    });

    $(".battle-cancel").on("click", function () {
        $.postJson("/api/cancelguaji");
    });

    $(".offline-start").on("click", function () {
        confirm("五分钟内无法退出离线，确认操作？", function () {
            $.postJson("/api/offline");
        });
    });

    $(".offline-cancel").on("click", function () {
        $.postJson("/api/canceloffline");    
    });

    $(".map-d-apply").on("click", function () {
        var mapId = $("#dungeon").val();
        $.postJson("/api/swithdmap?id=" + mapId);
    });

    $(".battle-d-guaji").on("click", function () {
        $.postJson("/api/dguaji");
    });

    $(".battle-d-cancel").on("click", function () {
        $.postJson("/api/canceldguaji");
    });
});