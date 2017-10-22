angular
    .module('adminApp', ['riversideUtilities', 'riversideForms', 'riversideGrids', 'ng-sortable'])
    .controller('UpdateLayoutController', ['$scope', '$http', 'riversideFormsFactory', function ($scope, $http, riversideFormsFactory) {
        $scope.toggleHamburgerMenu = function () {
            $scope.hamburgerMenuActive = !$scope.hamburgerMenuActive;
        }
        $scope.initialise = function () {
            $http.get('/apps/admin/layouts/updateviewmodel/5').success(function (viewModel) {
                $scope.body = viewModel.body;
                $scope.navigation = viewModel.navigation;
                riversideFormsFactory.initialiseForm($scope.body.form, $scope.form);
            }).error(function () {
            });
        }
        $scope.submitForm = function () {
            $scope.submitting = true;
            $http.post('/api/LayoutsApi/5', $scope.viewModel).success(function (data) {
                $scope.submitting = false;
            }).error(function () {
                $scope.submitting = false;
            });
        }
    }])
    .controller('CreateLayoutController', ['$scope', '$http', '$window', 'riversideFormsFactory', function ($scope, $http, $window, riversideFormsFactory) {
        $scope.toggleHamburgerMenu = function () {
            $scope.hamburgerMenuActive = !$scope.hamburgerMenuActive;
        }
        $scope.initialise = function () {
            $http.get('/apps/admin/api/layouts?create=true').success(function (adminPageViewModel) {
                $scope.layout = adminPageViewModel.model;
                $scope.navigation = adminPageViewModel.navigation;
            }).error(function () {
            });
        }
        $scope.getNewId = function () {
            if ($scope.newId == undefined)
                $scope.newId = 0;
            $scope.newId = $scope.newId - 1;
            return $scope.newId;
        }
        $scope.addLayoutZone = function () {
            var layoutZoneId = $scope.getNewId();
            var layoutZone = {
                layoutZoneId: layoutZoneId,
                name: riversideFormsFactory.createField('layoutZoneName' + layoutZoneId, $scope.layout.fields['layoutZoneName']),
                cssClass: riversideFormsFactory.createField('layoutZoneCssClass' + layoutZoneId, $scope.layout.fields['layoutZoneCssClass']),
                adminType: riversideFormsFactory.createField('layoutZoneAdminType' + layoutZoneId, $scope.layout.fields['layoutZoneAdminType']),
                contentType: riversideFormsFactory.createField('layoutZoneContentType' + layoutZoneId, $scope.layout.fields['layoutZoneContentType']),
                beginRender: riversideFormsFactory.createField('layoutZoneBeginRender' + layoutZoneId, $scope.layout.fields['layoutZoneBeginRender']),
                endRender: riversideFormsFactory.createField('layoutZoneEndRender' + layoutZoneId, $scope.layout.fields['layoutZoneEndRender']),
                layoutZoneElements: []
            };
            $scope.layout.layoutZones.push(layoutZone);
            $scope.selectLayoutZone(layoutZone);
        }
        $scope.addLayoutZoneElement = function (layoutZone) {
            var layoutZoneElementId = $scope.getNewId();
            var elementTypeItem = riversideFormsFactory.getSelectedItem($scope.layout.elementType);
            var layoutZoneElement = {
                layoutZoneElementId: layoutZoneElementId,
                elementTypeItem: elementTypeItem
            };
            layoutZone.layoutZoneElements.push(layoutZoneElement);
            $scope.selectLayoutZoneElement(layoutZone, layoutZoneElement);
        }
        $scope.selectLayoutZone = function (layoutZone) {
            $scope.layout.activeLayoutZoneId = layoutZone.layoutZoneId;
        }
        $scope.selectLayoutZoneElement = function (layoutZone, layoutZoneElement) {
            layoutZone.activeLayoutZoneElementId = layoutZoneElement.layoutZoneElementId;
        }
        $scope.removeLayoutZone = function (layoutZone) {
            var index = $.inArray(layoutZone, $scope.layout.layoutZones);
            if (index >= 0) {
                var newIndex = index + 1;
                if (newIndex > $scope.layout.layoutZones.length - 1)
                    newIndex = index - 1;
                if (newIndex >= 0)
                    $scope.selectLayoutZone($scope.layout.layoutZones[newIndex]);
                $scope.layout.layoutZones.splice(index, 1);
            }
        }
        $scope.removeLayoutZoneElement = function (layoutZone) {
            var result = $.grep(layoutZone.layoutZoneElements, function (layoutZoneElement) { return layoutZoneElement.layoutZoneElementId == layoutZone.activeLayoutZoneElementId; });
            if (result.length == 1) {
                var layoutZoneElement = result[0];
                var index = $.inArray(layoutZoneElement, layoutZone.layoutZoneElements);
                if (index >= 0) {
                    var newIndex = index + 1;
                    if (newIndex > layoutZone.layoutZoneElements.length - 1)
                        newIndex = index - 1;
                    if (newIndex >= 0)
                        $scope.selectLayoutZoneElement(layoutZone, layoutZone.layoutZoneElements[newIndex]);
                    layoutZone.layoutZoneElements.splice(index, 1);
                }
            }
        }
        $scope.submitForm = function () {
            $scope.submitting = true;
            //var formFieldValues = riversideFormsFactory.getFieldValues($scope.layout);
            $http.post('/apps/admin/api/layouts?dataAction=create', $scope.layout).success(function (layout) {
                $scope.layout = layout;
                $window.location.href = '/apps/admin/layouts';
                //riversideFormsFactory.applyFormState($scope.layout, formState);
            }).error(function () {
                $scope.submitting = false;
            });
        }
    }])
    .controller('ReadLayoutController', ['$scope', '$http', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, $http, riversideUtilitiesFactory, riversideFormsFactory) {
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
    .controller('SearchLayoutsController', ['$scope', '$http', 'riversideUtilitiesFactory', 'riversideGridsFactory', function ($scope, $http, riversideUtilitiesFactory, riversideGridsFactory) {
        $scope.toggleHamburgerMenu = function () {
            $scope.hamburgerMenuActive = !$scope.hamburgerMenuActive;
        }
        $scope.initialise = function () {
            var search = encodeURIComponent(riversideUtilitiesFactory.getParameterByName('search'));
            var page = encodeURIComponent(riversideUtilitiesFactory.getParameterByName('page'));
            $http.get('/apps/admin/api/layouts?page=' + page + '&search=' + search).success(function (adminPageViewModel) {
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
                $http.get('/apps/admin/api/layouts?grid=true&page=1&search=' + search).success(function (grid) {
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
            $http.get('/apps/admin/api/layouts?grid=true&page=' + $scope.model.pager.page + '&search=' + search).success(function (grid) {
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