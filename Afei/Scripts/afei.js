var $afei = {};
$afei.menustate = [
	{cd: '0',nm: '新建文件夹了'},
	{cd: '1',nm: '勉强能用上了'},
	{cd: '2',nm: 'bug影响使用'},
]
$afei.discuss = function (url) {
	var $discuss = $("<div class='discuss'></div>");
	$discuss.appendTo($("#body"));
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
$.prototype.upimg = function (func) {
	var $that = this;
	//绑定change事件用来获取选中的值
	$that.change(function () {
		var fds = new FormData();
		var files = $(this)[0].files;
		var file = files[0];
		if (file) {
			func(file);
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
