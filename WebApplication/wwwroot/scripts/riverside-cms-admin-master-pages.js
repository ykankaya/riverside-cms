angular
    .module('adminApp', ['riversideUtilities', 'riversideForms', 'riversideGrids', 'ng-sortable'])
    .controller('ReadMasterPagesController', ['$scope', '$http', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, $http, riversideUtilitiesFactory, riversideFormsFactory) {
        $scope.toggleHamburgerMenu = function () {
            $scope.hamburgerMenuActive = !$scope.hamburgerMenuActive;
        }
        $scope.initialise = function () {
            var id = encodeURIComponent(riversideUtilitiesFactory.getRouteId());
            $http.get('/apps/admin/api/layouts/' + id).success(function (adminPageViewModel) {
                $scope.layout = adminPageViewModel.model;
                $scope.navigation = adminPageViewModel.navigation;
            }).error(function () {
            });
        }
        $scope.selectLayoutZone = function (layoutZone) {
            $scope.layout.activeLayoutZoneId = layoutZone.layoutZoneId;
        }
        $scope.selectLayoutZoneElement = function (layoutZone, layoutZoneElement) {
            layoutZone.activeLayoutZoneElementId = layoutZoneElement.layoutZoneElementId;
        }
    }])
    .controller('SearchMasterPagesController', ['$scope', '$http', 'riversideUtilitiesFactory', 'riversideGridsFactory', function ($scope, $http, riversideUtilitiesFactory, riversideGridsFactory) {
        $scope.toggleHamburgerMenu = function () {
            $scope.hamburgerMenuActive = !$scope.hamburgerMenuActive;
        }
        $scope.initialise = function () {
            var search = encodeURIComponent(riversideUtilitiesFactory.getParameterByName('search'));
            var page = encodeURIComponent(riversideUtilitiesFactory.getParameterByName('page'));
            $http.get('/apps/admin/api/masterpages?page=' + page + '&search=' + search).success(function (adminPageViewModel) {
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
                $http.get('/apps/admin/api/masterpages?grid=true&page=1&search=' + search).success(function (grid) {
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
            $http.get('/apps/admin/api/masterpages?grid=true&page=' + $scope.model.pager.page + '&search=' + search).success(function (grid) {
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