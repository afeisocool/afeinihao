var $afei = {};
$afei.menustate = [
	{cd: '0',nm: '新建文件夹了'},
	{cd: '1',nm: '勉强能用上了'},
	{cd: '2',nm: 'bug影响使用'},
]
$afei.redate = function (date) {
	var dt = new Date(date).getTime();
	var now = new Date().getTime();
	var span = (now - dt) / 1000;
	var fh = '秒前';
	if (span < 60) {
		return parseInt(span) + fh;
	}
	span = span / 60
	fh = '分前';
	if (span < 60) {
		return parseInt(span) + fh;
	}
	span = span / 60
	fh = '小时前';
	if (span < 24) {
		return parseInt(span) + fh;
	}
	span = span / 24
	fh = '天前';
	if (span < 12) {
		return parseInt(span) + fh;
	}
	span = span / 12
	fh = '年前';
	return parseInt(span) + fh;
}
$afei.tologin = function (func) {
	$(".loginbox").remove();
	var $loginbox = $("<div class='loginbox'></div>");
	var $loginclose = $("<div class='loginclose'></div>");
	var $loginclosespan = $("<div class='loginclosespan cps'>×</div>");
	var $loginnm = $("<input type='text' class='loginnm' placeholder='账号' />");
	var $loginpwd = $("<input type='password' class='loginpwd' placeholder='密码' />");
	var $loginbtn = $("<div class='loginbtn cps'>登录</div>");
	var $loginregister = $("<div class='loginregister cps'>注册</div>");
	$loginbox.appendTo($("body"));
	$loginclose.appendTo($loginbox);
	$loginclosespan.appendTo($loginclose);
	$loginnm.appendTo($loginbox);
	$loginpwd.appendTo($loginbox);
	$loginbtn.appendTo($loginbox);
	$loginregister.appendTo($loginbox);
	//close
	$loginclosespan.click(function (e) {
		$loginbox.remove();
		e.stopPropagation();
	})
	//login
	$loginbtn.click(function (e) {
		var usernm = $loginnm.val();
		var userpwd = $loginpwd.val();
		$afei.post('/home/login', { usernm: usernm, userpwd: userpwd }).then(res => {
			layer.msg(res);
            if (res=="登录成功") {
				$loginclosespan.click();
				if (func) {
					func();
                }
            }
		})

		e.stopPropagation();
	})
	//register
	$loginregister.click(function (e) {
		var usernm = $loginnm.val();
		var userpwd = $loginpwd.val();
		$.post('/home/register', { usernm: usernm, userpwd: userpwd }).then(res => {
			layer.msg(res);
		})
		e.stopPropagation();
	})
}
$afei.discuss = function (field, $scope, $compile) {
	$scope.srtxt = '';
	$scope.cnts = 0;
	var $discuss = $("<div class='discuss'></div>");
	var $discussbox = $("<div class='discussbox'></div>");
	var $discussbar = $("<div class='discussbar'></div>");
	var $discussmsg = $("<div class='discussmsg'></div>");
	var $discussinput = $("<div class='discussinput'></div>");
	var $discussinputbox = $("<input class='discussinputbox' type='text' placeholder='说些什么……' ng-model='srtxt' />");
	var $discussbtn = $("<div class='discussbtn'>发送</div>");
	var $msgs = $(
		"<div class='dismsgbox' ng-repeat='item in msgs'>" +
			"<div class='dismsghdnm'>"+
				"<div class='dismsghd' style='background-image:url(/Content/up/{{item.userhd}})'></div>"+
				"<div class='dismsgnm'>{{item.usernm}}</div>"+
			"</div>"+
			"<div class='dismsgtxts'>"+
			"<span class='dismsgtxt'>{{item.msg}}</span>"+
			"<span class='dismsgcrt'>-{{item.crtdt}}</span>"+
			"</div>"+
		"</div>"
	)
	var $dismsgbtn = $("<div class='dismsgbtn cps'>加载更多</div>");
	$msgs.appendTo($discussmsg);
	$dismsgbtn.appendTo($discussmsg);
	$discussinput.appendTo($discussbox);
	$discussbar.appendTo($discussbox);
	$discussmsg.appendTo($discussbox);
	$discussinputbox.appendTo($discussinput);
	$discussbtn.appendTo($discussinput);
	$discuss.appendTo($("#body"));
	$discussbox.appendTo($("#body"));
	$compile($discussbox)($scope);
	$discuss.click(function (e) {
		$discussbox.addClass('show');
		$("body").addClass('divstop');
		e.stopPropagation();
	})
	$discussbar.click(function (e) {
		$discussbox.removeClass('show');
		$("body").removeClass('divstop');
		e.stopPropagation();
	})
	$discussbtn.click(function (e) {
		var login = $.cookie('afeitool_login');
		if (login!='logining') {
			layer.msg('未登录', {time:900});
			setTimeout(function () {
				$afei.tologin();
			}, 1000)
			return;
        }
		sendmsg($scope.srtxt);
		$scope.srtxt = '';
		$scope.$applyAsync();
		e.stopPropagation();
	})
	$scope.loadflag = true;
	$discussmsg.scroll(function (e) {
		var $that = $(this);
		var sh = $that[0].scrollHeight;
		var st = $that[0].scrollTop;
		var ht = $that[0].offsetHeight;
		if ($scope.loadflag && st + ht == sh) {
			loadmore();
        }
	})
	$dismsgbtn.click(function (e) {
		loadmore();
		e.stopPropagation();
	})
	function loadmore() {
		$scope.loadflag = false;
		setTimeout(function () {
			$scope.loadflag = true;
		}, 1000)
		if (page * 10 < $scope.cnts) {
			layer.msg('加载更多', { time: 500 });
			page = page + 1;
			getdismsgs();
		} else {
			$dismsgbtn.text("到底了哦");
			layer.msg('到底了噢', {time:500});
        }
    }
	function sendmsg(msg) {
		if (!msg || msg == '') {
			layer.msg('请勿输入空值');
			return;
		}
		$afei.post('/home/sendmsg', { field: field, msg: msg }).then(res => {
			layer.msg(res, { time: '500' });
			$scope.cnts = $scope.cnts + 1;
			$discuss.text($scope.cnts)
			page = 1;
			$scope.msgs = [];
			getdismsgs();
		});
    }
	var page = 1;//页码
	var flag = true;
	$scope.msgs = [];

	getcnts();
	function getcnts() {
		//获取评论总数
		$afei.post('/home/getdiscnt', { field: field }).then(res => {
			$scope.cnts = res;
			$discuss.text($scope.cnts);
			$scope.$applyAsync();
		})
    }
	getdismsgs();
	function getdismsgs() {
		$afei.post('/home/getdismsg', { field: field, page: page }).then(res => {
			res.forEach(item => {
				item.crtdt = $afei.redate(item.crtdt);
				$scope.msgs.push(item);
			})
			$scope.$applyAsync();
		})
    }
}
$.extend({
	cps: function (item) {
		return JSON.parse(JSON.stringify(item));
	},
	queststring: function (name) {
		var reg = new RegExp('(^|&)' + name + '=([^&]*)(&|$)', 'i');
		var r = window.location.search.substr(1).match(reg);
		if (r != null) {
			return unescape(r[2]);
		}
		return undefined;
	},
	getdate: function (date, format) {
		date = date == undefined ? new Date() : date;
		date = new Date(date);
		format = format == undefined ? "YY-MM-DD hh:mm:ss" : format
		var warr = ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"]
		var YY = date.getFullYear();
		var MM = (date.getMonth() + 1 < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1);
		var DD = (date.getDate() < 10 ? '0' + (date.getDate()) : date.getDate());
		var hh = (date.getHours() < 10 ? '0' + date.getHours() : date.getHours());
		var mm = (date.getMinutes() < 10 ? '0' + date.getMinutes() : date.getMinutes());
		var ss = (date.getSeconds() < 10 ? '0' + date.getSeconds() : date.getSeconds());
		var ww = warr[date.getDay()];
		format = format.replaceAll('YY', YY);
		format = format.replaceAll('MM', MM);
		format = format.replaceAll('DD', DD);
		format = format.replaceAll('hh', hh);
		format = format.replaceAll('mm', mm);
		format = format.replaceAll('ss', ss);
		format = format.replaceAll('ww', ww);
		return format;
	},
	//复制到剪切板
	copy: function (text) {
		if (navigator.clipboard && window.isSecureContext) {
			return new Promise((resolve, reject) => {
				navigator.clipboard.writeText(text).then(
					e => {
						resolve('success');
					},
					e => {
						resolve(undefined)
					}
				)
			})
		} else {
			return new Promise((resolve, reject) => {
				var textarea = $("<textarea class='hide2'></textarea>")[0];
				$(textarea).appendTo($("body"));
				textarea.value = text;
				textarea.focus();
				textarea.select();
				//执行复制
				document.execCommand('copy') ? resolve('success') : reject();
				textarea.remove();
			})
        }
	},
	dataurlToblob: (dataurl) => {
		var arr = dataurl.split(',');
		var mime = arr[0].match(/:(.*?);/)[1]
		var bstr = atob(arr[1]);
		var n = bstr.length;
		var u8arr = new Uint8Array(n);
		while (n--) {
			u8arr[n] = bstr.charCodeAt(n);
		}
		return new Blob([u8arr], { type: mime });
	},
	fileTodataurl:(file) => {
		return new Promise((resolve, reject) => {
			var reader = new FileReader();
			if (file == undefined) {
				reject('文件不存在')
			}
			reader.readAsDataURL(file);
			reader.onload = function () {
				var base64 = this.result;
				resolve(base64);
            }
		})
	}
})
$.prototype.ars = function () {
	var arr = [];
	var that = this;
	for (var i = 0; i < that.length; i++) {
		arr.push($(that[i]))
	}
	return arr;
}
$.prototype.upimg = function (func1,func2) {
	var $that = this;
	//绑定change事件用来获取选中的值
	$that.change(function () {
		var fds = new FormData();
		var files = $(this)[0].files;
		var item = files[0];
		var ars = item.name.split('.');
		var fnm = new Date().getTime() + '.' + ars[ars.length - 1];
		fds.append(fnm, item);
		if (func1) {
			func1(fds);
		}
		if (func2) {
			var reader = new FileReader();
			reader.readAsDataURL(item);
			reader.onload = function (e) {
				func2(e.target.result);
			}
        }
		//清空内容
		$(this).val('');
	})
}
//假如没有replaceAll方法就重写
if (!String.prototype.replaceAll) {
	String.prototype.replaceAll = function (o, n) {
		return this.split(o).join(n);
	}
}
$afei.cps = function (res) {
	return JSON.parse(JSON.stringify(res));
}
$afei.post = function (str, obj) {
	return new Promise((resolve, reject) => {
		$.ajax({
			type: 'post',
			url: str,
			data: obj,
			dataType: 'Json',
			async: true,
			success: res => {
				resolve(res);
			},
			error: res => {
				reject(res);
			}
		})
	})
}
$afei.postfile = function (str, obj) {
	return new Promise((resolve, reject) => {
		$.ajax({
			type: 'post',
			url: str,
			data: obj,
			dataType: 'json',
			processData: false,
			contentType: false,
			async: true,
			success: res => {
				resolve(res);
			},
			error: res => {
				reject(res);
			}
		})
	})
}
$afei.get = function (str, obj) {
	return new Promise((resolve, reject) => {
		$.ajax({
			type: 'get',
			url: str,
			data: obj,
			dataType: 'Json',
			async: true,
			success: res => {
				resolve(res);
			},
			error: res => {
				reject(res);
			}
		})
	})
}

$afei.logincheck = function (func) {
	$afei.post('/home/logincheck').then(res => {
		if (res == "logining") {
			$.cookie('afeitool_login',res);
		}
	})
}
function jiemi(str,key) {
	$afei.post('/home/jiemi', { str: str, key: key }).then(res => {
		console.log(res);
	})
}