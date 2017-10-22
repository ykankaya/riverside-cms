angular
    .module('riversideUtilities', [])
    .factory('riversideUtilitiesFactory', ['$window', function ($window) {
        function getParameterByNameHelper(name) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec(location.search);
            return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }
        return {
            getParameterByName: function getParameterByName(name) {
                return getParameterByNameHelper(name);
            },
            getRouteId: function getRouteId() {
                var url = location.href;
                var queryIndex = url.indexOf("?");
                if (queryIndex >= 0)
                    url = url.substring(0, queryIndex);
                var lastSlashIndex = url.lastIndexOf("/");
                url = url.substring(lastSlashIndex + 1);
                return url;
            },
            redirectToReturnUrl: function redirectToReturnUrl() {
                var returnUrl = getParameterByNameHelper('returnurl');
                if (returnUrl.startsWith('/'))
                    $window.location.href = returnUrl;
                else
                    $window.close();
            }
        }
    }]);