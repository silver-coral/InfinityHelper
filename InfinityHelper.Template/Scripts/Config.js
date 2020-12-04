$(document).ready(function () {
    $(".config-magic-add").click(function () {
        var cd = $(".condition-template").clone();
        cd.removeClass("condition-template").removeClass("hidden").addClass("condition");
        $("#modalConfig .modal-body").append(cd);
    });

    $(".config-magic-remove").click(function () {
        $(".condition").last().remove();
    });

    $(".config-apply").on("click", function () {
        var data = {};
        data.color = $("#power").val();
        data.realCategory = $("#type").val();
        data.items = [];
        $(".condition").each(function (i, p) {
            var value = {};
            value.name = $(p).find(".prefix").val();            
            value.value = $(p).find(".min").val();            
            data.items.push(value);
        });

        $.postJson("/api/filtercreate", data);
    });

    $(".config-delete").click(function () {
        var id = $(this).data("id");
        confirm("确认删除规则？", function () {            
            $.postJson("/api/filterremove?id=" + id)
        });
    });

    $(".config-clear").click(function () {
        confirm("确认删除全部规则？", function () {
            $.postJson("/api/filterclear");
        });
    });

    $(".config-copy-apply").click(function () {
        var name = $("#charName").val();
        $.postJson("/api/filtercopy?name=" + name);        
    });
});