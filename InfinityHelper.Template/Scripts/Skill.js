$(document).ready(function () {    
    $(".skill-on").on("click", function () {
        var sid = $(this).data("id");
        var type = $(this).data("type");
        $.postJson("/api/skillon?sid=" + sid + "&type=" + type);
    });

    $(".skill-off").on("click", function () {
        var sid = $(this).data("id");
        var type = $(this).data("type");
        $.postJson("/api/skilloff?sid=" + sid + "&type=" + type);
    });

    $(".skill-upgrade").on("click", function () {
        var sid = $(this).data("id");        
        var skill = $(this).parent().find(".skill-name");
        confirm("确认升级技能" + skill.prop("outerHTML") + "？", function () {
            $.postJson("/api/skillupgrade?sid=" + sid);
        });
    });
});