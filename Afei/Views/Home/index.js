angular.module('app', []).controller('app', ['$scope', '$compile', function ($scope, $compile) {
	$afei.logincheck();
	$scope.menustate = $afei.menustate;
	$scope.viewstate = '';
	$scope.menuitem;
	$scope.menucheck = function (item) {
		var cX = document.body.clientWidth;
		if (!item.check) {
			$scope.menu.forEach(temp => {
				temp.check = false;
			})
			item.check = true;
			$scope.menuitem = item;
			$scope.viewstate = 'menu-card';
		} else {
			$scope.menu.forEach(temp => {
				temp.check = false;
			})
			$scope.viewstate = '';
			$scope.menuitem = null;
		}
		$scope.$applyAsync();
	}
	//菜单切换的时候，替换iframe里面的界面
	//加载页句柄
	$scope.loadflag;
	$scope.$watch('menuitem', function (n) {
		if (n) {
			$scope.loadflag = false;
			$scope.$applyAsync();
			console.log(n);
		} else {

		}
	})
	//监控页面加载
	$("#iframe")[0].onload = function () {
		$scope.loadflag = true;
		$scope.$applyAsync();
		console.log("加载成功");
	}
	//历史上的今天
	gethistory();
	$scope.historys = [];
	function gethistory() {
		$afei.get('/Home/GetAnotherGetApi', { url: 'https://zhufred.gitee.io/zreader/ht/event/'+$.getdate(new Date(),'MMDD')+'.json' }).then(res => {
			var arr = [];
			res.forEach(item => {
				if (item.title.length < 30) {
					arr.push(item.title)
                }
			})
			$scope.historys = arr;
			$scope.$applyAsync();
			setInterval(function () {
				scollhistory();
			}, 5000)
		})
	}
	//获得菜单
	getmenu();
	$scope.menu = [];
	function getmenu() {
		$afei.post('/Home/Getmenu').then(res => {
			$scope.menu = res;
			$scope.$applyAsync();
		})
    }
	//诗词滚动
	function scollhistory() {
		var $first = $("#history .history:first");
		$first.addClass('totop');
		setTimeout(function () {
			$first.appendTo($("#history"));
			$first.removeClass('totop');
		}, 250)
	}
	//登录信息
	getlogin();
	function getlogin() {
		$afei.post('/home/getlogin').then(res => {
			$scope.user = res;
			$scope.$applyAsync();
			console.log(res);
		})
	}
	//登录
	$scope.tologin = function () {
		if (!$scope.user) {
			$afei.tologin(function () {
				$scope.user = true;
				getlogin();
			});
		} else {
			$scope.usershow = true;
			$scope.$applyAsync();
        }
	}
	$scope.userhead = function () {
		$("#upimg").click();
	}
	var headfd;//缓存的fd
	$("#upimg").upimg(function (fd) {
		headfd = fd;
	}, function (base64) {
		$("#userhead").attr('src', base64);
	})

	$scope.usersave = function () {
		if (headfd) {
			$afei.postfile('/home/savehd', headfd).then(res => {
				layer.msg(res, { time: 500 });
				headfd = null;
			})
		}
		$afei.post('/home/saveuser', $scope.user).then(res => {
			layer.msg(res, { time: 500 });
		})
	}
	$scope.userexit = function () {
		$scope.usershow = false;
		$scope.$applyAsync();
	}















}])





















