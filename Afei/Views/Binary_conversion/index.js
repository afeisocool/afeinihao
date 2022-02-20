angular.module('app', []).controller('app', ['$scope', '$compile', function ($scope, $compile) {
	$scope.origintype = '10';
	$scope.originvals = '1';
	$scope.typearr = [];
	for (var i = 2; i <= 36; i++) {
		$scope.typearr.push(i);
		var $btnbox = $("<div class='btnbox'></div>");
		var $btndesc = $("<div class='btndesc'>"+i+"</div>");
		var $btninput = $("<div class='btninput' ng-dblclick='copy(change_"+i+")'>{{change_"+i+"}}</div>");
		$btnbox.appendTo($("#body"));
		$btndesc.appendTo($btnbox);
		$btninput.appendTo($btnbox);
		$compile($btnbox)($scope);
	}
	$scope.tochange = function () {
		//先转回10进制
		var theval = parseInt($scope.originvals, $scope.origintype);
		for (var i = 2; i <= 36; i++) {
			var changevals = theval.toString(i) ? theval.toString(i) : 'error';
			$scope['change_' + i] = changevals;
		}
		$scope.$applyAsync();
	}
	$scope.copy = function (vals) {
		$.copy(vals).then(res => {
			if (res) {
				layer.msg('复制成功', { time: 500 });
			} else {
				layer.msg('复制失败', { time: 500 });
			}
		});
	}
	//加载评论功能
	$afei.discuss('binary_conversion',$scope,$compile);
	































}])
