$(document).ready(function () {
    var key = "infinity.login";

    function getLoginData() {
        var loginData = window.localStorage.getItem(key);
        if (loginData == null) {
            loginData = {
                name: [],
                pwd:[],
                remember : false,
            };            
        }
        else {
            loginData = JSON.parse(loginData);
        }
        return loginData;
    }

    function initLoginData() {
        var loginData = getLoginData();
        $("#remember").prop("checked", loginData.remember);
        if (loginData.remember) {
            if (loginData.name.length > 0) {
                $("#username").val(loginData.name[0]);
                $("#password").val(loginData.pwd[0]);
            }
            if (loginData.name.length > 1) {
                var lt = $(".input-group-template").clone();
                lt.removeClass("input-group-template").removeClass("hidden").addClass("input-group-btn");
                lt.on("click", "li > a", function () {
                    var selectName = $(this).text();
                    var selectIndex = loginData.name.indexOf(selectName);
                    var selectPwd = loginData.pwd[selectIndex];

                    $("#username").val(selectName);
                    $("#password").val(selectPwd);
                });

                $.each(loginData.name, function (i, p) {
                    lt.find("ul").append("<li><a class='physical'>" + p + "</a></li>");
                });

                $(".user-names").addClass("input-group").append(lt);
            }
        }
    }

    function saveLogin() {        
        var loginData = getLoginData();
        var remember = $("#remember").prop("checked");
        var name = $("#username").val();
        var pwd = $("#password").val();
        loginData.remember = remember;
        
        if (remember && name.length > 0 && pwd.length > 0) {
            var nameIndex = loginData.name.indexOf(name);
            if (nameIndex < 0) {
                loginData.name.push(name);
                loginData.pwd.push(pwd);
            }
            else {
                loginData.pwd.splice(nameIndex, 1, pwd);
            }
        }
        if (!remember) {
            loginData.name = [];
            loginData.pwd = [];            
        }

        window.localStorage.setItem(key, JSON.stringify(loginData));
    }

    $(".btn-login").click(function () {
        saveLogin();                   
    });

    initLoginData();
});