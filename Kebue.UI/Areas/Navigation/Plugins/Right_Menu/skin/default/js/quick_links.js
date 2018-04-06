/**
 * 右侧快速操作
 * kongge@office.weiphone.com
 * 2012.06.07
*/
jQuery(function ($) {
    //创建DOM
    var
	//quickHTML = '<div class="quick_links_panel"><div id="quick_links" class="quick_links"><a href="#top" class="return_top"><i class="top"></i><span>返回顶部</span></a><a href="#" class="my_qlinks"><i class="setting"></i><span>快速定位</span></a><a href="#" class="message_list" ><i class="message"></i><span>站内消息</span><em class="num" style="display:none"></em></a><a href="#" class="history_list"><i class="view"></i><span>最近浏览</span></a><a href="#" class="leave_message"><i class="qa"></i><span>联系客服</span></a></div><div class="quick_toggle"><a href="javascript:;" class="toggle" title="展开/收起">×</a></div></div><div id="quick_links_pop" class="quick_links_pop hide"></div>',
quickHTML = '<div class="quick_links_panel"><div id="quick_links" class="quick_links"><a href="#top" class="return_top"><i class="top"></i><span>返回顶部</span></a><a href="#" class="my_qlinks"><i class="setting"></i><span>快速定位</span></a><a href="#kebue_footer" class="leave_message"><i class="qa"></i><span>留言</span></a><a href="#" onclick="kebue_home_addurl()">添加网址</em></a><a href="#kebue_footer" class="cur">底部</a></div><div class="quick_toggle"><a href="javascript:;" class="toggle" title="展开/收起">×</a></div></div><div id="quick_links_pop" class="quick_links_pop hide"></div>',

	quickShell = $(document.createElement('div')).html(quickHTML).addClass('quick_links_wrap'),
	quickLinks = quickShell.find('.quick_links');
    quickPanel = quickLinks.parent();
    quickShell.appendTo('body');

    //具体数据操作 
    var
	quickPopXHR,
	loadingTmpl = '<iframe width="100%" height="100%" name="engine" src="../User/AddUrl" frameborder="0" marginwidth="0" marginheight="0" allowtransparency="true" style="background-color:#fff" scrolling="auto">',
	popTmpl = '<div class="title"><h3><i></i><%=title%></h3></div><div class="pop_panel"><%=content%></div><div class="arrow"><i></i></div><div class="fix_bg"></div>',
   // popTmpl = '<div class="pop_panel"><%=content%></div><div class="arrow"><i></i></div><div class="fix_bg"></div>',
    historyListTmpl = '<ul><%for(var i=0,len=items.length; i<5&&i<len; i++){%><li><a href="<%=items[i].productUrl%>" target="_blank" class="pic"><img alt="<%=items[i].productName%>" src="<%=items[i].productImage%>" width="60" height="60"/></a><a href="<%=items[i].productUrl%>" title="<%=items[i].productName%>" target="_blank" class="tit"><%=items[i].productName%></a><div class="price" title="单价"><em>&yen;<%=items[i].productPrice%></em></div></li><%}%></ul>',
	newMsgTmpl = '',
	quickPop = quickShell.find('#quick_links_pop'),
	quickDataFns = {
	    //快速定位
	    my_qlinks: {
	        title: '快速定位',
	        content: '<div class="links">' + $("#kebue_Left_Nav_Menu").html() + '</div>',//'<div class="links"><ul><li><a href="#kebue_Top" class="cur">Top</a></li><li><a href="#dw_84d1716d6e41d33" class="cur" title="73">工具</a></li><li><a href="#dw_84d1716d6e11d3" class="cur" title="47">JS插件</a></li><li><a href="#dw_84d1716d6e24d16" class="cur" title="40">前端框架</a></li><li><a href="#dw_84d1716d6e83d82" class="cur" title="92">前端开发</a></li><li><a href="#dw_84d1716d6e63d55" class="cur" title="8">移动应用</a></li><li><a href="#dw_84d1716d6e64d56" class="cur" title="18">移动开发</a></li><li><a href="#dw_84d1716d6e65d57" class="cur" title="13">移动框架</a></li><li><a href="#dw_84d1716d6e83d76" class="cur" title="3">iOS</a></li><li><a href="#dw_84d1716d6e83d79" class="cur" title="4">安卓</a></li><li><a href="#dw_84d1716d6e83d78" class="cur" title="13">PHP开发</a></li><li><a href="#dw_84d1716d6e13d5" class="cur" title="4">PHP框架</a></li><li><a href="#dw_84d1716d6e83d84" class="cur" title="33">设计</a></li><li><a href="#dw_84d1716d6e83d77" class="cur" title="7">Java</a></li><li><a href="#dw_84d1716d6e67d59" class="cur" title="15">程序社区</a></li><li><a href="#dw_84d1716d6e69d61" class="cur" title="27">素材资源</a></li><li><a href="#dw_84d1716d6e23d15" class="cur" title="9">创意范</a></li><li><a href="#dw_84d1716d6e14d6" class="cur" title="19">UED</a></li><li><a href="#dw_84d1716d6e35d27" class="cur" title="36">学习</a></li><li><a href="#dw_84d1716d6e44d36" class="cur" title="29">开源建站</a></li><li><a href="#dw_84d1716d6e68d60" class="cur" title="10">站长必备</a></li><li><a href="#dw_84d1716d6e73d65" class="cur" title="3">网络运维</a></li><li><a href="#dw_84d1716d6e61d53" class="cur" title="82">科学上网</a></li><li><a href="#dw_84d1716d6e55d47" class="cur" title="24">数据搜索</a></li><li><a href="#dw_84d1716d6e36d28" class="cur" title="43">学术搜索</a></li><li><a href="#dw_84d1716d6e49d41" class="cur" title="16">快搜索</a></li><li><a href="#dw_84d1716d6e30d22" class="cur" title="22">图片搜索</a></li><li><a href="#dw_84d1716d6e83d75" class="cur" title="28">酷站</a></li><li><a href="#dw_84d1716d6e81d73" class="cur" title="11">资讯阅读</a></li><li><a href="#dw_84d1716d6e71d63" class="cur" title="46">综合站</a></li><li><a href="#dw_84d1716d6e43d35" class="cur" title="32">开放平台</a></li><li><a href="#dw_84d1716d6e74d66" class="cur" title="31">美女写</a></li><li><a href="#dw_84d1716d6e77d69" class="cur" title="24">视频综合</a></li><li><a href="#dw_84d1716d6e20d12" class="cur" title="23">军事站</a></li><li><a href="#dw_84d1716d6e53d45" class="cur" title="23">搞笑内涵</a></li><li><a href="#dw_84d1716d6e60d52" class="cur" title="23">福利博</a></li><li><a href="#dw_84d1716d6e76d68" class="cur" title="20">视听站</a></li><li><a href="#dw_84d1716d6e19d11" class="cur" title="19">体育站</a></li><li><a href="#dw_84d1716d6e26d18" class="cur" title="19">动漫站</a></li><li><a href="#dw_84d1716d6e46d38" class="cur" title="19">影视下载</a></li><li><a href="#dw_84d1716d6e39d31" class="cur" title="18">小说站</a></li><li><a href="#dw_84d1716d6e47d39" class="cur" title="18">影视论坛</a></li><li><a href="#dw_84d1716d6e79d71" class="cur" title="17">账号分享</a></li><li><a href="#dw_84d1716d6e17d9" class="cur" title="15">云服务</a></li><li><a href="#dw_84d1716d6e72d64" class="cur" title="15">网盘搜索</a></li><li><a href="#dw_84d1716d6e40d32" class="cur" title="14">屌丝良</a></li><li><a href="#dw_84d1716d6e9d1" class="cur" title="13">BT种</a></li><li><a href="#dw_84d1716d6e50d42" class="cur" title="13">感觉不错</a></li><li><a href="#dw_84d1716d6e31d23" class="cur" title="12">在线工</a></li><li><a href="#dw_84d1716d6e10d2" class="cur" title="11">Cos</a></li><li><a href="#dw_84d1716d6e33d25" class="cur" title="11">在线视</a></li><li><a href="#dw_84d1716d6e34d26" class="cur" title="11">媒体资讯</a></li><li><a href="#dw_84d1716d6e52d44" class="cur" title="10">找工作啦</a></li><li><a href="#dw_84d1716d6e38d30" class="cur" title="9">导航站</a></li><li><a href="#dw_84d1716d6e48d40" class="cur" title="9">影视资讯</a></li><li><a href="#dw_84d1716d6e27d19" class="cur" title="8">博客</a></li><li><a href="#dw_84d1716d6e70d62" class="cur" title="8">绅士站</a></li><li><a href="#dw_84d1716d6e15d7" class="cur" title="6">VR资</a></li><li><a href="#dw_84d1716d6e18d10" class="cur" title="6">企业邮箱</a></li><li><a href="#dw_84d1716d6e57d49" class="cur" title="6">游戏站</a></li><li><a href="#dw_84d1716d6e75d67" class="cur" title="6">萌图</a></li><li><a href="#dw_84d1716d6e25d17" class="cur" title="5">办公协作</a></li><li><a href="#dw_84d1716d6e45d37" class="cur" title="5">弹幕站</a></li><li><a href="#dw_84d1716d6e56d48" class="cur" title="5">永不消</a></li><li><a href="#dw_84d1716d6e62d54" class="cur" title="5">科技类</a></li><li><a href="#dw_84d1716d6e21d13" class="cur" title="4">创业必备</a></li><li><a href="#dw_84d1716d6e42d34" class="cur" title="4">广告联盟</a></li><li><a href="#dw_84d1716d6e83d80" class="cur" title="4">建站源码</a></li><li><a href="#dw_84d1716d6e22d14" class="cur" title="3">创意|科技</a></li><li><a href="#dw_84d1716d6e29d21" class="cur" title="3">商城</a></li><li><a href="#dw_84d1716d6e54d46" class="cur" title="3">摄影</a></li><li><a href="#dw_84d1716d6e37d29" class="cur" title="3">认证</a></li><li><a href="#dw_84d1716d6e32d24" class="cur" title="2">在线影院</a></li><li><a href="#dw_84d1716d6e78d70" class="cur" title="2">设计|定制</a></li><li><a href="#kebue_footer" class="cur">底部</a></li></ul></div>',
	        init: $.noop
	    },

	    //添加网址
	    //message_list: {
	    //    title: '站内消息',
	    //    content: loadingTmpl,
	    //    init: function (ops) {

	    //        //获取实时最近浏览
	    //        quickPopXHR = $.ajax({
	    //            url: unreadNewMsgUrl,
	    //            dataType: 'json',
	    //            cache: false,
	    //            success: function (data) {
	    //                //var html = '<div class="no_data"><i></i><span>暂无消息</span></div>';
	    //                var html = ds.tmpl(newMsgTmpl, {
	    //                    items: data
	    //                });

	    //                if (1 == data.success) {
	    //                    //重写总数
	    //                    var shell = $('#quick_links a.message_list'), num = data.msgtotal;
	    //                    if (0 == num) {
	    //                        shell.find('em').remove();
	    //                    } else {
	    //                        shell.append('<em class="num"><b>' + Math.min(99, num) + '</b></em>').show();
	    //                    }

	    //                }
	    //                quickPop.html(ds.tmpl(popTmpl, {
	    //                    title: ops.title,
	    //                    content: '<div class="links">' + html + '</div>'
	    //                }));
	    //            }
	    //        });
	    //    }
	    //},
	    //最近浏览
	    history_list: {
	        title: '最近浏览',
	        content: loadingTmpl,
	        init: function (ops) {
	            //获取实时最近浏览
	            quickPopXHR = $.ajax({
	                url: recentlyViewedUrl,
	                dataType: 'json',
	                cache: false,
	                success: function (data) {
	                    var html = '<div class="no_data"><i></i><span>没有浏览记录</span></div>';
	                    if (data && data.length > 0) {
	                        html = ds.tmpl(historyListTmpl, {
	                            items: data
	                        });
	                    }
	                    quickPop.html(ds.tmpl(popTmpl, {
	                        title: ops.title,
	                        content: '<div class="slider related_slider history_slider"><div class="inner">' + html + '</div></div>'
	                    }));
	                }
	            });
	        }
	    },
	    //留言
	    leave_message: {
	        title: '给站长留言',
	        content: '<form action="./" method="post"><div class="types"><input type="radio" name="type" id="type_1" value="联系站长" checked /><label for="type_1">联系站长</label><input type="radio" name="type" id="type_3" value="我要侵权举报" /><label for="type_3">我要侵权举报</label><input type="radio" name="type" id="type_4" value="不良信息举报"/><label for="type_4">不良信息举报</label></div><div class="txt"><textarea style="width:500px;height:260px" name="msg" id="msg" cols="45" rows="50" placeholder="客官：您好，请详细阐述问题，并留下详细联系方式，以便处理回复..."></textarea></div><div class="token"></div><button type="submit" style="width:50px;height:30px;border:1px solid blue;margin-left:150px"><i class="glyphicon glyphicon-floppy-saved">提交</i></button></form>',
	        init: function (ops) {
	            setTimeout(function () {
	                quickPop.find('textarea').focus();
	            }, 100);
	            debugger;
	            var saveMessageUrl = '/AJAXRequestAPI/LeaveAMessage';
	            //效验 & 提交数据
	            var form = quickPop.find('form');
	            form.attr("action", saveMessageUrl);
	            form.bind('submit', function (e) {
	                e.preventDefault();
	                var data = form.serialize();
	                if (!checkMessageForm()) {
	                    return false;
	                }
	                var type = quickPop.find(':radio:checked').val();
	                jQuery.ajax({
	                    type: 'post',
	                    url: saveMessageUrl,
	                    data: { type: type, msg: $("#msg").val(), userAgent: navigator.userAgent },
	                    dataType: "json",
	                    error: function (value) {
	                        ds.dialog.alert(value);
	                    },
	                    success: function (value) {
	                        var success = value.status;
	                        var info = value.info;
	                        if (success == 1) {
	                            hideQuickPop();
	                            showInfoTip(info, 'success');
	                        } else {
	                            ds.dialog.alert(info);
	                        }
	                    }
	                });
	            });
	        }
	    }
	};

    //showQuickPop
    var
	prevPopType,
	prevTrigger,
	doc = $(document),
	popDisplayed = false,
	hideQuickPop = function () {
	    if (prevTrigger) {
	        prevTrigger.removeClass('current');
	    }
	    popDisplayed = false;
	    prevPopType = '';
	    quickPop.hide();
	},
	showQuickPop = function (type) {
	    if (quickPopXHR && quickPopXHR.abort) {
	        quickPopXHR.abort();
	    }
	    if (type !== prevPopType) {
	        var fn = quickDataFns[type];
	        quickPop.html(ds.tmpl(popTmpl, fn));
	        fn.init.call(this, fn);
	    }
	    doc.unbind('click.quick_links').one('click.quick_links', hideQuickPop);

	    quickPop[0].className = 'quick_links_pop quick_' + type;
	    popDisplayed = true;
	    prevPopType = type;
	    quickPop.show();
	};
    quickShell.bind('click.quick_links', function (e) {
        e.stopPropagation();
    });

    //通用事件处理
    var
	view = $(window),
	quickLinkCollapsed = !!ds.getCookie('ql_collapse'),
	getHandlerType = function (className) {
	    return className.replace(/current/g, '').replace(/\s+/, '');
	},
	showPopFn = function () {
	    var type = getHandlerType(this.className);
	    if (popDisplayed && type === prevPopType) {
	        return hideQuickPop();
	    }
	    showQuickPop(this.className);
	    if (prevTrigger) {
	        prevTrigger.removeClass('current');
	    }
	    prevTrigger = $(this).addClass('current');
	},
	quickHandlers = {
	    //购物车，最近浏览，商品咨询
	    my_qlinks: showPopFn,
	    message_list: showPopFn,
	    history_list: showPopFn,
	    leave_message: showPopFn,
	    //返回顶部
	    return_top: function () {
	        ds.scrollTo(0, 0);
	        hideReturnTop();
	    },
	    toggle: function () {
	        quickLinkCollapsed = !quickLinkCollapsed;

	        quickShell[quickLinkCollapsed ? 'addClass' : 'removeClass']('quick_links_min');
	        ds.setCookie('ql_collapse', quickLinkCollapsed ? '1' : '', 30);
	    }
	};
    quickShell.delegate('a', 'click', function (e) {
        var type = getHandlerType(this.className);
        if (type && quickHandlers[type]) {
            quickHandlers[type].call(this);
            e.preventDefault();
        }
    });

    //Return top
    var scrollTimer, resizeTimer, minWidth = 1350;

    function resizeHandler() {
        clearTimeout(scrollTimer);
        scrollTimer = setTimeout(checkScroll, 160);
    }
    function checkResize() {
        quickShell[view.width() > 1340 ? 'removeClass' : 'addClass']('quick_links_dockright');
    }
    function scrollHandler() {
        clearTimeout(resizeTimer);
        resizeTimer = setTimeout(checkResize, 160);
    }
    function checkScroll() {
        view.scrollTop() > 100 ? showReturnTop() : hideReturnTop();
    }
    function showReturnTop() {
        quickPanel.addClass('quick_links_allow_gotop');
    }
    function hideReturnTop() {
        quickPanel.removeClass('quick_links_allow_gotop');
    }

    view.bind('scroll.go_top', resizeHandler).bind('resize.quick_links', scrollHandler);
    quickLinkCollapsed && quickShell.addClass('quick_links_min');
    resizeHandler();
    scrollHandler();


    //校验商品咨询表单
    function checkMessageForm() {
        var content = $("#msg");
        if (content.val() == "") {
            ds.dialog({
                title: '消息',
                content: "请填写内容！",
                onyes: function () {
                    this.close();
                },
                width: 200,
                lock: true
            });
            return false;
        }

        var checkcode = $("#token_txt").val();
        if (checkcode == "" || checkcode == "点击获取") {
            ds.dialog({
                title: '消息',
                content: "验证码不能为空，请输入验证码！",
                onyes: function () {
                    this.close();
                },
                width: 200,
                lock: true
            });
            return false;
        }
        return true;
    }

});

//首次加载站内消息总数
jQuery(function ($) {
    var shell = $('#quick_links a.message_list');
    if (shell.find("em").length) {

        //$.ajax({
        //    url: unreadNewMsgUrl,
        //    dataType: 'json',
        //    cache: false,
        //    success: function (data) {
        //        if (1 == data.success) {
        //            if (0 == data.msgtotal) {
        //                shell.find('em').remove();
        //            } else {
        //                var num = data.msgtotal;
        //                //消息总数最大只显示 99
        //                shell.append('<em class="num"><b>' + Math.min(99, num) + '</b></em>').show();
        //            }
        //        }
        //    }
        //});
    }
});

function kebue_home_addurl() {
    layer.open({
        type: 2,
        title: '添加新网址',
        shadeClose: true,
        shade: 0.8,
        area: ['380px', '90%'],
        content: '../Home/AddUrl' //iframe的url
    });
}