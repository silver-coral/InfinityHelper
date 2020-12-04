$(document).ready(function () {
    $(".group-remove").on("click", function () {
        var no = $(this).data("no");
        confirm("确认移除此人？", function () {            
            $.postJson("/api/groupremove?no=" + no);
        });
    });

    $(".group-join").on("click", function () {
        var aid = $(this).data("id");
        confirm("确认加入队伍？将自动取消挂机", function () {            
            $.postJson("/api/groupjoin?aid=" + aid);
        });
    });

    $(".group-leave").on("click", function () { 
        confirm("确认退出当前队伍？", function () {
            $.postJson("/api/groupleave");
        });
    });    

    $("#modalCreateGroup").on('show.bs.modal', function (event) {
        $(".group-create").on("click", function () {
            var name = $("#aname").val();
            $.postJson("/api/groupcreate?name=" + name);
        }); 
    });
});