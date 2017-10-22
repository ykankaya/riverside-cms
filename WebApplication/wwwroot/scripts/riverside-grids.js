angular
    .module('riversideGrids', [])
    .factory('riversideGridsFactory', [function () {
        return {
        };
    }])
    .directive('listItemButton', function () {
        return {
            template: '\
                <li ng-class="{ \'active\': lbModel.state == 2, \'disabled\': lbModel.state == 1 }">\
                    <a href ng-click="onClick(lbModel)">\
                        <i class="fa fa-fw {{lbModel.icon}}\" ng-if="lbModel.icon && !lbModel.iconRight"></i>\
                        {{lbModel.text}}\
                        <i class="fa fa-fw {{lbModel.icon}}\" ng-if="lbModel.icon && lbModel.iconRight"></i>\
                    </a>\
                </li>',
            restrict: 'AE',
            replace: true,
            scope: {
                lbModel: '=',
                onClick: '&'
            }
        }
    })
    .directive('pager', function () {
        return {
            template: '\
                <ul class="pagination">\
                    <li ng-class="{ \'disabled\': pgModel.page == 1 }"><a href ng-click="onClick(1)"><i class="fa fa-fw fa-fast-backward"></i> <span class="hidden-xs">{{pgModel.firstButtonLabel}}</span></a></li>\
                    <li ng-class="{ \'disabled\': pgModel.page == 1 }"><a href ng-click="onClick(pgModel.page - 1)"><i class="fa fa-fw fa-backward"></i> <span class="hidden-xs">{{pgModel.previousButtonLabel}}</span></a></li>\
                    <li class="hidden-xs" ng-if="pgModel.page - 2 >= 1"><a href ng-click="onClick(pgModel.page - 2)">{{pgModel.page - 2}}</a></li>\
                    <li class="hidden-xs" ng-if="pgModel.page - 1 >= 1"><a href ng-click="onClick(pgModel.page - 1)">{{pgModel.page - 1}}</a></li>\
                    <li class="active"><a href ng-click="onClick(pgModel.page)">{{pgModel.page}}</a></li>\
                    <li class="hidden-xs" ng-if="pgModel.page + 1 <= pgModel.pageCount"><a href ng-click="onClick(pgModel.page + 1)">{{pgModel.page + 1}}</a></li>\
                    <li class="hidden-xs" ng-if="pgModel.page + 2 <= pgModel.pageCount"><a href ng-click="onClick(pgModel.page + 2)">{{pgModel.page + 2}}</a></li>\
                    <li ng-class="{ \'disabled\': pgModel.page == pgModel.pageCount }"><a href ng-click="onClick(pgModel.page + 1)"><span class="hidden-xs">{{pgModel.nextButtonLabel}}</span> <i class="fa fa-fw fa-forward"></i></a></li>\
                    <li ng-class="{ \'disabled\': pgModel.page == pgModel.pageCount }"><a href ng-click="onClick(pgModel.pageCount)"><span class="hidden-xs">{{pgModel.lastButtonLabel}}</span> <i class="fa fa-fw fa-fast-forward"></i></a></li>\
                </ul>',
            restrict: 'AE',
            replace: true,
            scope: {
                pgModel: '=',
                onPageChange: '&'
            },
            link: function (scope, element, attrs) {
                scope.onClick = function (page) {
                    if (page >= 1 && page <= scope.pgModel.pageCount && scope.pgModel.page != page) {
                        scope.pgModel.page = page;
                        scope.onPageChange();
                    }
                }
            }
        }
    })
    .directive('gridHeader', function () {
        return {
            template: '\
                <th>\
                    {{ghModel.label}}\
                </th>',
            restrict: 'AE',
            replace: true,
            scope: {
                ghModel: '='
            }
        }
    })
    .directive('gridCell', function () {
        return {
            template: '\
                <td>\
                    <a ng-if="gcModel.url != undefined" href="{{gcModel.url}}">{{gcModel.value}}</a>\
                    <span ng-if="gcModel.url == undefined">{{gcModel.value}}</span>\
                </td>',
            restrict: 'AE',
            replace: true,
            scope: {
                gcModel: '='
            }
        }
    });
