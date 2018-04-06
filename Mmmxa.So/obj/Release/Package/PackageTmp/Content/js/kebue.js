//用户注销
function btnOutLogin() {
    parent.layer.confirm('确定要退出吗？', {
        btn: ['是', '我点错了'], //按钮
        shade: false //不显示遮罩
    }, function () {
        $.post("../User/OutLogin", function (result) {
            parent.layer.msg(result, { icon: 1 });
            window.location.href = "../User/AdminLogin";
        });
    });
}

