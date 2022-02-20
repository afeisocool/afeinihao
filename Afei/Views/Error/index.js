angular.module('app', []).controller('app', ['$scope', '$compile', function ($scope, $compile) {
	


	//加载评论功能
	$afei.discuss('error', $scope, $compile);


}])





















