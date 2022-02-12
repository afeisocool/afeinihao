angular.module('app', []).controller('app', ['$scope', '$compile', function ($scope, $compile) {

	//历史上的今天
	gethistory();
	$scope.historys = [];
	var now = $.getdate(new Date(),'-MM-DD')
	function gethistory() {
		$afei.get('/Home/GetAnotherGetApi', { url: 'https://zhufred.gitee.io/zreader/ht/event/' + $.getdate(new Date(), 'MMDD') + '.json' }).then(res => {
			var arr = [];
			res.forEach(item => {
				var temp = {};
				temp.time =item.year+now;
				temp.vals = item.title.substring(6);
				arr.push(temp)
			})
			$scope.historys = arr;
			$scope.$applyAsync();
		})
	}
















    



















}])
