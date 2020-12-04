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
});