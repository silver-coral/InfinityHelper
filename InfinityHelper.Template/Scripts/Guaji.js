$(document).ready(function () {
    var connection = $.hubConnection();
    var connectionHubProxy = connection.createHubProxy('userManagerHub');
    connectionHubProxy.on('onError', function (msg) {
        var content = $("<div class='require'></div>");
        content.append(msg);
        $(".panel-body").prepend(content);
    });
    connectionHubProxy.on("battleLog", function (data) {
        var content = $("<div></div>");
        var bt = JSON.tryParse(data);
        if (bt == false) {
            content.append($("<span class='require'></span>").append(data));
        }
        else {
            content.append($("<span class='name'></span>").append(bt.CharName));
            if (bt.Success) {
                content.append("战斗<span class='physical'>胜利</span>");
            }
            else {
                content.append("战斗<span class='error'>失败</span>");
            }
            if (bt.DropExp > 0) {
                content.append("，获取经验");
                content.append($("<span class='state'></span>").append(bt.DropExp));
            }
            if (bt.GameItemsList != null) {
                $.each(bt.GameItemsList, function (i, p) {
                    content.append("，");
                    content.append($("<span class='c" + p.Color + "'></span>").append("【" + p.RealName + "】"));
                });
            }
            content.append("，耗时" + bt.WaitSecond + "秒……");
            $(".panel-body").prepend(content);            
        }
    });
    connection.start();
});