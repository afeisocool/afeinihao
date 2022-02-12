angular.module('app', []).controller('app', ['$scope', '$compile', function ($scope, $compile) {

	$scope.src = '/Content/imgs/null.png';
	$scope.file = null;

	//拖拽上传图片的
	$("#img").on('dragenter', function (e) {
		e.preventDefault();
		e.stopPropagation();
	}).on('dragover', function (e) {
		e.preventDefault();
		e.stopPropagation();
	}).on('dragleave', function (e) {
		e.preventDefault();
		e.stopPropagation();
	}).on('drop', function (e) {
		var files = e.originalEvent.dataTransfer.files;
		var file = files[0];
		if (file.type.split('/')[0] == 'image') {
			$scope.file = file;
			$scope.$applyAsync();
		} else {
			console.log("格式不是image")
        }
		e.preventDefault();
		e.stopPropagation();
	})
	$("#img").click(function (e) {
		$("#upimg").click();
		e.stopPropagation();
	})
	$("#upimg").upimg(function (file) {
		$scope.file = file;
		$scope.$applyAsync();
	})

	//假如file里面发生变化
	$scope.base64 = '未选择图片';
	$scope.blob = null;
	$scope.img = null;
	$scope.$watch('file', function (n) {
		if (n) {
			//转换成base64格式
			var reader = new FileReader();
			reader.readAsDataURL(n);
			reader.onload = e => {
				$scope.base64 = e.target.result;
				loadimg();
				$scope.src = $scope.base64;
				//转换成blob格式
				$scope.blob = base64ToBlob(e.target.result);
				$scope.$applyAsync();
			}
        }
	})
	function loadimg() {
		$scope.img = new Image();
		$scope.imgflag = false;
		$scope.img.src = $scope.base64;
		$scope.img.onload = function () {
			$scope.imgflag = true;
			$scope.width = this.width;
			$scope.height = this.height;
			$scope.scale = $scope.width / $scope.height;
			$scope.$applyAsync();
		}
    }
	//base64转blob
	base64ToBlob = base64 => {
		let arr = base64.split(','),
			mime = arr[0].match(/:(.*?);/)[1],
			bstr = atob(arr[1]),
			n = bstr.length,
			u8arr = new Uint8Array(n);
		while (n--) {
			u8arr[n] = bstr.charCodeAt(n);
		}
		return new Blob([u8arr], {
			type: mime
		});
	};
	$scope.quality = 0.92;
	$scope.zoom = 1;
	$scope.factor = '16*16';
	$scope.scale = '';
	$scope.width = '';
	$scope.height = '';
	//
	$scope.copy = () => {
		$.copy($scope.base64).then(res => {
			if (res) {
				layer.msg('复制成功', { time: 500 });
			} else {
				layer.msg('复制失败', { time: 500 });
            }
		});
	}
	//
	$scope.getzoom = () => {
		if ($scope.file) {
			var cs = $("<canvas></canvas>")[0];
			var ct = cs.getContext('2d');
			cs.width = $scope.width * $scope.zoom;
			cs.height = $scope.height * $scope.zoom;
			ct.drawImage($scope.img, 0, 0, cs.width, cs.height);
			//处理图片图片
			var newbase64 = cs.toDataURL($scope.file.type, $scope.quality);
			//图片转文件流
			var newblob = $.dataurlToblob(newbase64);
			var a = $("<a></a>")[0];
			a.href = URL.createObjectURL(newblob);
			a.download = $scope.file.name;
			a.click();
			URL.revokeObjectURL(a.href);
        }
		//压缩图片获取新的base64
	}
	$scope.getico = () => {
		if ($scope.file) {
			//image/x-icon
			var arr = $scope.factor.split("*");
			var cs = $("<canvas></canvas>")[0];
			var ct = cs.getContext('2d');
			cs.width = parseInt(arr[0]);
			cs.height = parseInt(arr[1]);
			ct.drawImage($scope.img, 0, 0, cs.width, cs.height);
			//处理图片图片
			var newbase64 = cs.toDataURL('image/x-icon', 1);
			//图片转文件流
			var newblob = $.dataurlToblob(newbase64);
			var a = $("<a></a>")[0];
			a.href = URL.createObjectURL(newblob);
			a.download = 'favicon.ico';
			a.click();
			URL.revokeObjectURL(a.href);
        }
	}
























}])
