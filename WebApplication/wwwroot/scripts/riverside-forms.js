angular
    .module('riversideForms', ['ngFileUpload'])
    .factory('riversideFormsFactory', ['$http', '$q', 'Upload', function ($http, $q, Upload) {

        function getFieldValueHelper(formFieldViewModel) {
            if (formFieldViewModel.value != null && formFieldViewModel.value.value != undefined)
                return formFieldViewModel.value.value;   // Lists of items
            else
                return formFieldViewModel.value;
        }

        function getItemByValueHelper(formFieldViewModel, formFieldValue) {
            var result = $.grep(formFieldViewModel.items, function (item) { return item.value == formFieldValue; });
            if (result.length == 1)
                return result[0];
            else
                return null;
        }
        
        function resultIsSuccessHelper(form, result) {
            // Remove form and field custom error messages
            form.customErrorMessages = [];
            angular.forEach(form.fields, function (field, name) {
                field.customErrorMessages = [];
            });
            // If no errors to report, result is successful
            var success = (result.errors == null || result.errors == undefined);
            // If not successful, register errors with form or fields
            if (!success) {
                for (var index = 0; index < result.errors.length; index++) {
                    var error = result.errors[index];
                    var field = form.fields[error.name];
                    if (field == undefined)
                        form.customErrorMessages.push(error.message);
                    else
                        field.customErrorMessages.push(error.message);
                }
            }
            // Trigger model to DOM updates for all fields to ensure custom validity set correctly
            angular.forEach(form.fields, function (field, name) {
                // Credit: http://stackoverflow.com/questions/122102/what-is-the-most-efficient-way-to-clone-an-object (John Resig)
                form.fields[name] = jQuery.extend(true, {}, form.fields[name]);
            });
            // Return success or failure
            return success;
        }

        return {
            get: function (id, context) {
                var url = '/apps/admin/api/forms/' + encodeURIComponent(id);
                if (context != null && context != undefined)
                    url += '?context=' + encodeURIComponent(context);
                return $http.get(url).then(function successCallback(response) {
                    var form = response.data;
                    angular.forEach(form.fields, function (field, name) {
                        if (field.fieldType == 6 /* FormFieldType.UploadField */) {
                            field.formId = form.id;
                            field.formContext = form.context;
                        }
                    });
                    return form;
                });
            },
            post: function (form) {
                var url = '/apps/admin/api/forms/' + encodeURIComponent(form.id);
                if (form.context != null && form.context != undefined)
                    url += '?context=' + encodeURIComponent(form.context);
                return $http.post(url, form).then(function successCallback(response) {
                    var result = response.data;
                    result.success = resultIsSuccessHelper(form, result);
                    return result;
                });
            },
            postBasic: function (form) {
                return $q.resolve({ status: null, success: true });
            },
            initialiseForm: function (formViewModel, formDom) {
                angular.forEach(formViewModel.fields, function (formField, key) {
                    // Set custom error messages
                    if (formField.customErrorMessages != null)
                        formDom[key].$setValidity("custom", false);
                    // Ensure correct drop down list items selected
                    if (formField.items != undefined && formField.value != null) {
                        for (var index = 0; index < formField.items.length; index++) {
                            if (formField.items[index].value == formField.value) {
                                formField.value = formField.items[index];
                                break;
                            }
                        }
                    }
                });
            },
            resultIsSuccess: function (form, result) {
                return resultIsSuccessHelper(form, result);
            },
            fieldLink: function (scope, elem, attr, ngModel) {
                // Model to DOM validation (Credit: http://stackoverflow.com/questions/12581439/how-to-add-custom-validation-to-an-angularjs-form)
                ngModel.$formatters.unshift(function (value) {
                    if (value != undefined) {
                        var valid = value.customErrorMessages == null || value.customErrorMessages.length == 0;
                        ngModel.$setValidity('custom', valid);
                    }
                    return value;
                });
            },
            fieldController: function ($scope) {
                $scope.valueChange = function () {
                    if ($scope.ngModel.value != undefined) {
                        $scope.ngModel.customErrorMessages = [];
                        $scope.ngModel = jQuery.extend(true, {}, $scope.ngModel);
                        if ($scope.ngChange != undefined)
                            $scope.ngChange();
                    }
                }
                $scope.uploadFiles = function ($files) {
                    var formId = $scope.ngModel.formId;
                    var formContext = $scope.ngModel.formContext;
                    var url = '/apps/admin/api/formuploads/' + encodeURIComponent(formId);
                    if (formContext != null && formContext != undefined)
                        url += '?context=' + encodeURIComponent(formContext);
                    for (var i = 0; i < $files.length; i++) {
                        var $file = $files[i];
                        $scope.ngModel.progress = 0;
                        $scope.ngModel.customErrorMessages = [];
                        $scope.ngModel.customErrorMessages.push('Uploading file...');
                        $scope.ngModel = jQuery.extend(true, {}, $scope.ngModel);
                        Upload.upload({
                            url: url,
                            file: $file
                        }).then(function (resp) {
                            var result = resp.data;
                            $scope.ngModel.progress = null;
                            $scope.ngModel.customErrorMessages = [];
                            var success = (result.errors == null || result.errors == undefined);
                            if (!success) {
                                for (var index = 0; index < result.errors.length; index++) {
                                    var error = result.errors[index];
                                    $scope.ngModel.customErrorMessages.push(error.message);
                                }
                            }
                            $scope.ngModel.value = result.status;
                            $scope.ngModel = jQuery.extend(true, {}, $scope.ngModel);
                            if ($scope.ngChange != undefined)
                                $scope.ngChange();
                        }, function (resp) {
                            // console.log('Error status: ' + resp.status);
                        }, function (evt) {
                            var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                            $scope.ngModel.progress = progressPercentage;
                            $scope.ngModel = jQuery.extend(true, {}, $scope.ngModel);
                            // console.log('progress: ' + progressPercentage + '%');
                        });
                    }
                }
            },
            getFieldValue: function (formFieldViewModel) {
                return getFieldValueHelper(formFieldViewModel);
            },
            getSelectedItem: function (formFieldViewModel) {
                var formFieldValue = getFieldValueHelper(formFieldViewModel);
                return getItemByValueHelper(formFieldViewModel, formFieldValue);
            },
            getItemByValue: function (formFieldViewModel, formFieldValue) {
                return getItemByValueHelper(formFieldViewModel, formFieldValue);
            },
            removeItem: function (formFieldViewModel, item) {
                var index = $.inArray(item, formFieldViewModel.items);
                var newItem = null;
                if (index >= 0) {
                    var newIndex = index + 1;
                    if (newIndex > formFieldViewModel.items.length - 1)
                        newIndex = index - 1;
                    if (newIndex >= 0)
                        formFieldViewModel.value = formFieldViewModel.items[newIndex].value;
                    formFieldViewModel.items.splice(index, 1);
                }
            },
            copyFieldSet: function (fieldSet, defaultId, newId) {
                var newFieldSet = jQuery.extend(true, {}, fieldSet);
                angular.forEach(newFieldSet.fields, function (field, key) {
                    field.name = field.name.replace(defaultId, newId);
                });
                return newFieldSet;
            },
            deleteFieldSet: function (fieldSets, fieldSet) {
                var activeFieldSet = null;
                var index = $.inArray(fieldSet, fieldSets);
                if (index >= 0) {
                    var newIndex = index + 1;
                    if (newIndex > fieldSets.length - 1)
                        newIndex = index - 1;
                    if (newIndex >= 0)
                        activeFieldSet = fieldSets[newIndex];
                    fieldSets.splice(index, 1);
                }
                return activeFieldSet;
            },
            addItem: function (formFieldViewModel, item, alphabetical) {
                formFieldViewModel.items.push(item);
                if (alphabetical == true) {
                    formFieldViewModel.items.sort(function (a, b) {
                        if (a.name < b.name) return -1;
                        if (a.name > b.name) return 1;
                        return 0;
                    })
                }
                formFieldViewModel.value = item.value;
            },
            getFieldValues: function (formViewModel) {
                var fieldValues = {};
                angular.forEach(formViewModel.fields, function (formField, key) {
                    fieldValues[key] = getFieldValueHelper(formField);
                });
                return fieldValues;
            },
            applyFormState: function (formViewModel, formState) {
                // Traverse form view model looking for fields with keys that match form state
                // When found, add custom errors - we may also need to set the form invalid
            },
            createField: function(name, formField) {
                var newFormField = JSON.parse(JSON.stringify(formField));
                newFormField.name = name;
                return newFormField;
            }
        };
    }])
    .filter('emptyindicator', function () {
        return function (input) {
            if (input == undefined || input == null || input == '')
                return '-';
            return input;
        }
    })
    .directive('validationSummary', function () {
        return {
            template: '\
                <div class="alert alert-danger validation-summary-errors" ng-show="ngModel.length > 0">\
                    <ul>\
                        <li ng-repeat="customErrorMessage in ngModel">{{customErrorMessage}}</li>\
                    </ul>\
                </div>',
            restrict: 'AE',
            replace: true,
            scope: {
                ngModel: '='
            }
        }
    })
    // Credit: http://stackoverflow.com/questions/18185357/form-validation-within-custom-directive
    .directive('textField', ['riversideFormsFactory', function (forms) {
        return {
            require: 'ngModel',
            link: forms.fieldLink,
            controller: forms.fieldController,
            template: '\
                <div ng-form="formField">\
                    <div class="form-group" ng-class="{ \'has-error\': formField.$invalid }">\
                        <label class="control-label" for="{{ngModel.name}}">{{ngModel.label}}</label>\
                        <input type="text" id="{{ngModel.name}}" name="{{ngModel.name}}" autocomplete="off" class="form-control" ng-model="ngModel.value" ng-change="valueChange()"\
                            ng-pattern="ngModel.pattern" ng-required="ngModel.required" ng-minlength="ngModel.minLength" ng-maxlength="ngModel.maxLength" />\
                        <div role="alert" class="help-block" ng-if="formField.$invalid && !formField.$pristine">\
                            <div ng-if="formField.$error.pattern">{{ngModel.patternErrorMessage}}</div>\
                            <div ng-if="formField.$error.required">{{ngModel.requiredErrorMessage}}</div>\
                            <div ng-if="formField.$error.minlength">{{ngModel.minLengthErrorMessage}}</div>\
                            <div ng-if="formField.$error.maxlength">{{ngModel.maxLengthErrorMessage}}</div>\
                            <div ng-if="formField.$error.custom" ng-repeat="errorMessage in ngModel.customErrorMessages">{{errorMessage}}</div>\
                        </div>\
                    </div>\
                </div>',
            restrict: 'AE',
            replace: true,
            scope: {
                ngModel: '=',
                ngChange: '='
            }
        }
    }])
    .directive('uploadField', ['riversideFormsFactory', function (forms) {
        return {
            require: 'ngModel',
            link: forms.fieldLink,
            controller: forms.fieldController,
            template: '\
                <div ng-form="formField">\
                    <div class="form-group" ng-class="{ \'has-error\': formField.$invalid }">\
                        <label class="control-label" for="{{ngModel.name}}">{{ngModel.label}}</label>\
                        <input type="hidden" id="{{ngModel.name}}" name="{{ngModel.name}}" ng-model="ngModel.value" ng-change="valueChange()" />\
                        <input ngf-no-file-drop type="file" ngf-select="uploadFiles($files)">\
                        <div ngf-drop="uploadFiles($files)" ngf-select="uploadFiles($files)" ng-model="files" class="drop-box" ngf-drag-over-class="\'dragover\'" ngf-multiple="false" ngf-allow-dir="false" accept="image/*" ngf-pattern="\'image/*\'">Drop image here or click to upload</div>\
                        <div class="progress" ng-if="ngModel.progress">\
                            <div class="progress-bar" role="progressbar" aria-valuenow="{{ngModel.progress}}" aria-valuemin="0" aria-valuemax="100" style="width: {{ngModel.progress}}%;">\
                                {{ngModel.progress}}%\
                            </div>\
                        </div>\
                        <div role="alert" class="help-block" ng-if="formField.$invalid && !formField.$pristine">\
                            <div ng-if="formField.$error.custom" ng-repeat="errorMessage in ngModel.customErrorMessages">{{errorMessage}}</div>\
                        </div>\
                    </div>\
                </div>',
            restrict: 'AE',
            replace: true,
            scope: {
                ngModel: '=',
                ngChange: '='
            }
        }
    }])
    .directive('passwordTextField', ['riversideFormsFactory', function (forms) {
        return {
            require: 'ngModel',
            link: forms.fieldLink,
            controller: forms.fieldController,
            template: '\
                <div ng-form="formField">\
                    <div class="form-group" ng-class="{ \'has-error\': formField.$invalid }">\
                        <label class="control-label" for="{{ngModel.name}}">{{ngModel.label}}</label>\
                        <input type="password" id="{{ngModel.name}}" name="{{ngModel.name}}" autocomplete="off" class="form-control" ng-model="ngModel.value" ng-change="valueChange()"\
                            ng-pattern="ngModel.pattern" ng-required="ngModel.required" ng-minlength="ngModel.minLength" ng-maxlength="ngModel.maxLength" />\
                        <div role="alert" class="help-block" ng-if="formField.$invalid && !formField.$pristine">\
                            <div ng-if="formField.$error.pattern">{{ngModel.patternErrorMessage}}</div>\
                            <div ng-if="formField.$error.required">{{ngModel.requiredErrorMessage}}</div>\
                            <div ng-if="formField.$error.minlength">{{ngModel.minLengthErrorMessage}}</div>\
                            <div ng-if="formField.$error.maxlength">{{ngModel.maxLengthErrorMessage}}</div>\
                            <div ng-if="formField.$error.custom" ng-repeat="errorMessage in ngModel.customErrorMessages">{{errorMessage}}</div>\
                        </div>\
                    </div>\
                </div>',
            restrict: 'AE',
            replace: true,
            scope: {
                ngModel: '=',
                ngChange: '='
            }
        }
    }])
    .directive('multiLineTextField', ['riversideFormsFactory', function (forms) {
        return {
            require: 'ngModel',
            link: forms.fieldLink,
            controller: forms.fieldController,
            template: '\
                <div ng-form="formField">\
                    <div class="form-group" ng-class="{ \'has-error\': formField.$invalid }">\
                        <label class="control-label" for="{{ngModel.name}}">{{ngModel.label}}</label>\
                        <textarea rows="{{ngModel.rows}}" type="text" id="{{ngModel.name}}" name="{{ngModel.name}}" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" class="form-control" ng-model="ngModel.value" \
                            ng-change="valueChange()" ng-pattern="ngModel.pattern" ng-required="ngModel.required" ng-minlength="ngModel.minLength" ng-maxlength="ngModel.maxLength">\
                        </textarea>\
                        <div role="alert" class="help-block" ng-if="formField.$invalid && !formField.$pristine">\
                            <div ng-if="formField.$error.pattern">{{ngModel.patternErrorMessage}}</div>\
                            <div ng-if="formField.$error.required">{{ngModel.requiredErrorMessage}}</div>\
                            <div ng-if="formField.$error.minlength">{{ngModel.minLengthErrorMessage}}</div>\
                            <div ng-if="formField.$error.maxlength">{{ngModel.maxLengthErrorMessage}}</div>\
                            <div ng-if="formField.$error.custom" ng-repeat="errorMessage in ngModel.customErrorMessages">{{errorMessage}}</div>\
                        </div>\
                    </div>\
                </div>',
            restrict: 'AE',
            replace: true,
            scope: {
                ngModel: '=',
                ngChange: '='
            }
        }
    }])
    .directive('selectField', ['riversideFormsFactory', function (forms) {
        return {
            require: 'ngModel',
            link: forms.fieldLink,
            controller: forms.fieldController,
            template: '\
                <div ng-form="formField">\
                    <div class="form-group" ng-class="{ \'has-error\': formField.$invalid }">\
                        <label class="control-label" for="{{ngModel.name}}">{{ngModel.label}}</label>\
                        <select id="{{ngModel.name}}" name="{{ngModel.name}}" class="form-control" ng-model="ngModel.value" ng-required="ngModel.required" ng-change="valueChange()">\
                            <option ng-repeat="item in ngModel.items" value="{{item.value}}">{{item.name}}</option>\
                        </select>\
                        <div role="alert" class="help-block" ng-if="formField.$invalid && !formField.$pristine">\
                            <div ng-if="formField.$error.required">{{ngModel.requiredErrorMessage}}</div>\
                            <div ng-if="formField.$error.custom" ng-repeat="errorMessage in ngModel.customErrorMessages">{{errorMessage}}</div>\
                        </div>\
                    </div>\
                </div>',
            restrict: 'AE',
            replace: true,
            scope: {
                ngModel: '=',
                ngChange: '='
            }
        }
    }])
    .directive('checkboxField', ['riversideFormsFactory', function (forms) {
        return {
            require: 'ngModel',
            link: forms.fieldLink,
            controller: forms.fieldController,
            template: '\
                <div ng-form="formField">\
                    <div class="checkbox" ng-class="{ \'has-error\': formField.$invalid }">\
                        <label>\
                            <input type="checkbox" id="{{ngModel.name}}" name="{{ngModel.name}}" ng-model="ngModel.value" ng-change="valueChange()" />\
                            {{ngModel.label}}\
                        </label>\
                        <div role="alert" class="help-block" ng-if="formField.$invalid && !formField.$pristine">\
                            <div ng-if="formField.$error.custom" ng-repeat="errorMessage in ngModel.customErrorMessages">{{errorMessage}}</div>\
                        </div>\
                    </div>\
                </div>',
            restrict: 'AE',
            replace: true,
            scope: {
                ngModel: '=',
                ngChange: '='
            }
        }
    }])
    .directive('integerField', ['riversideFormsFactory', function (forms) {
        return {
            require: 'ngModel',
            link: forms.fieldLink,
            controller: forms.fieldController,
            template: '\
                <div ng-form="formField">\
                    <div class="form-group" ng-class="{ \'has-error\': formField.$invalid }">\
                        <label class="control-label" for="{{ngModel.name}}">{{ngModel.label}}</label>\
                        <input type="number" id="{{ngModel.name}}" name="{{ngModel.name}} autocomplete="off" class="form-control" min="{{ngModel.min}}" max="{{ngModel.max}}" ng-model="ngModel.value"\
                            ng-change="valueChange()" ng-pattern="ngModel.pattern" ng-required="ngModel.required" />\
                        <div role="alert" class="help-block" ng-if="formField.$invalid && !formField.$pristine">\
                            <div ng-if="formField.$error.required">{{ngModel.requiredErrorMessage}}</div>\
                            <div ng-if="formField.$error.min">{{ngModel.minErrorMessage}}</div>\
                            <div ng-if="formField.$error.max">{{ngModel.maxErrorMessage}}</div>\
                            <div ng-if="formField.$error.custom" ng-repeat="errorMessage in ngModel.customErrorMessages">{{errorMessage}}</div>\
                        </div>\
                    </div>\
                </div>',
            restrict: 'AE',
            replace: true,
            scope: {
                ngModel: '=',
                ngChange: '='
            }
        }
    }]);