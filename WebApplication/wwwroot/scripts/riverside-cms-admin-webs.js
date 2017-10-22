angular
    .module('adminApp', ['riversideUtilities', 'riversideForms', 'riversideGrids', 'ng-sortable'])
    .controller('SearchWebsController', ['$scope', '$http', 'riversideUtilitiesFactory', 'riversideGridsFactory', function ($scope, $http, riversideUtilitiesFactory, riversideGridsFactory) {
        $scope.toggleHamburgerMenu = function () {
            $scope.hamburgerMenuActive = !$scope.hamburgerMenuActive;
        }
        $scope.initialise = function () {
            var search = encodeURIComponent(riversideUtilitiesFactory.getParameterByName('search'));
            var page = encodeURIComponent(riversideUtilitiesFactory.getParameterByName('page'));
            $http.get('/apps/admin/api/webs?page=' + page + '&search=' + search).success(function (adminPageViewModel) {
                $scope.model = adminPageViewModel.model;
                $scope.navigation = adminPageViewModel.navigation;
            }).error(function () {
            });
        }
        $scope.gridSearchChanged = function () {
            $scope.model.updating = true;
            if ($scope.throttle) {
                clearTimeout($scope.throttle);
            }
            $scope.throttle = setTimeout(function () {
                $scope.throttle = undefined;
                var search = encodeURIComponent($scope.model.search);
                $http.get('/apps/admin/api/webs?grid=true&page=1&search=' + search).success(function (grid) {
                    if (grid.search == $scope.model.search || grid.search == null && $scope.model.search == '') {
                        $scope.model = grid;
                    }
                }).error(function () {
                    $scope.model.updating = false;
                });
            }, 500);
        }
        $scope.pageChange = function () {
            var search = '';
            if ($scope.model.search != null && $scope.model.search != undefined)
                search = encodeURIComponent($scope.model.search);
            $scope.model.updating = true;
            $http.get('/apps/admin/api/webs?grid=true&page=' + $scope.model.pager.page + '&search=' + search).success(function (grid) {
                var gridSearch = grid.search;
                if (gridSearch == null || gridSearch == undefined)
                    gridSearch = '';
                var modelSearch = $scope.model.search;
                if (modelSearch == null || modelSearch == undefined)
                    modelSearch = '';
                if (gridSearch == modelSearch) {
                    $scope.model = grid;
                }
            }).error(function () {
                $scope.model.updating = false;
            });
        }
    }])
    .controller('CreateWebController', ['$scope', '$http', '$window', 'riversideFormsFactory', function ($scope, $http, $window, riversideFormsFactory) {
        $scope.toggleHamburgerMenu = function () {
            $scope.hamburgerMenuActive = !$scope.hamburgerMenuActive;
        }
        $scope.initialise = function () {
            $http.get('/apps/admin/api/webs?create=true').success(function (adminPageViewModel) {
                $scope.model = adminPageViewModel.model;
                $scope.navigation = adminPageViewModel.navigation;
            }).error(function () {
            });
        }
        $scope.submitForm = function () {
            $scope.submitting = true;
            $http.post('/apps/admin/api/webs?dataAction=create', $scope.model).success(function (model) {
                $scope.model = model;
                $window.location.href = '/apps/admin/webs';
            }).error(function () {
                $scope.submitting = false;
            });
        }
    }])