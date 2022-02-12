angular.module('app', []).controller('app', ['$scope', '$compile', function ($scope, $compile) {


    $scope.logs = [];
    getlogs();
    function getlogs() {
        $afei.post('/version_record/getlogs').then(res => {
            res.forEach(item => {
                item.time = $.getdate(item.crtdt, 'YY-MM-DD');
            })
            console.log(res);
            $scope.logs = res;
            $scope.$applyAsync();
        })
    }




















}])
