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
        $.postJson("/api/canceloffline", {}, function (data) {
            var html = $("<div></div>")
                .append($("<p></p>").append(data.OutboardTime))
                .append($("<p>战斗次数：</p>").append(data.CombatNum))
                .append($("<p>胜利次数：</p>").append(data.CombatSuccessNum))
                .append($("<p>获得经验：</p>").append(data.CountExp))
                .append($("<p>获得装备：</p>").append(data.CountItemNum))
                .append($("<p>自动出售：</p>").append(data.CountSellNum))
                .append($("<p>获得铜币：</p>").append(data.CountMoney)).html();
                
            confirm(html, function () {
                window.location.reload();
            });
        });    
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