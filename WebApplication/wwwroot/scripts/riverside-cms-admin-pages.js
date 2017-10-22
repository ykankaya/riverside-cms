angular
    .module('adminApp', ['riversideUtilities', 'riversideForms', 'riversideGrids', 'ng-sortable', 'ui.tinymce'])
    .controller('SearchPagesController', ['$scope', '$http', 'riversideUtilitiesFactory', 'riversideGridsFactory', function ($scope, $http, riversideUtilitiesFactory, riversideGridsFactory) {
        $scope.initialise = function () {
            var search = encodeURIComponent(riversideUtilitiesFactory.getParameterByName('search'));
            var page = encodeURIComponent(riversideUtilitiesFactory.getParameterByName('page'));
            $http.get('/apps/admin/api/pages?page=' + page + '&search=' + search).success(function (adminPageViewModel) {
                $scope.model = adminPageViewModel.model;
                $scope.navigation = adminPageViewModel.navigation;
            }).error(function () {
            });
        };
        $scope.gridSearchChanged = function () {
            $scope.model.updating = true;
            if ($scope.throttle) {
                clearTimeout($scope.throttle);
            }
            $scope.throttle = setTimeout(function () {
                $scope.throttle = undefined;
                var search = encodeURIComponent($scope.model.search);
                $http.get('/apps/admin/api/pages?grid=true&page=1&search=' + search).success(function (grid) {
                    if (grid.search == $scope.model.search || grid.search == null && $scope.model.search == '') {
                        $scope.model = grid;
                    }
                }).error(function () {
                    $scope.model.updating = false;
                });
            }, 500);
        };
        $scope.pageChange = function () {
            var search = '';
            if ($scope.model.search != null && $scope.model.search != undefined)
                search = encodeURIComponent($scope.model.search);
            $scope.model.updating = true;
            $http.get('/apps/admin/api/pages?grid=true&page=' + $scope.model.pager.page + '&search=' + search).success(function (grid) {
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
        };
    }])
    .controller('ReadPagesController', ['$scope', '$http', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, $http, riversideUtilitiesFactory, riversideFormsFactory) {
        $scope.initialise = function () {
            var id = encodeURIComponent(riversideUtilitiesFactory.getRouteId());
            $http.get('/apps/admin/api/pages/' + id).success(function (adminPageViewModel) {
                $scope.layout = adminPageViewModel.model;
                $scope.navigation = adminPageViewModel.navigation;
            }).error(function () {
            });
        };
        $scope.selectLayoutZone = function (layoutZone) {
            $scope.layout.activeLayoutZoneId = layoutZone.layoutZoneId;
        };
        $scope.selectLayoutZoneElement = function (layoutZone, layoutZoneElement) {
            layoutZone.activeLayoutZoneElementId = layoutZoneElement.layoutZoneElementId;
        };
    }])
    .controller('PagesController', ['$scope', '$window', 'riversideFormsFactory', function ($scope, $window, forms) {
        $scope.getForm = function (context) {
            forms.get('ca31a1f4-ce6b-45fc-aa4d-058188acfa35', context).then(function (form) {
                $scope.form = form;
            });
        };
        $scope.postForm = function () {
            $scope.submitting = true;
            forms.post($scope.form).then(function (result) {
                if (result.success)
                    $window.location.href = result.status;
                $scope.submitting = false;
            }).catch(function () {
                $scope.submitting = false;
            });
        };
    }])
    .controller('ThemesController', ['$scope', '$window', 'riversideFormsFactory', function ($scope, $window, forms) {
        $scope.getForm = function () {
            forms.get('9602083f-d8b9-4be3-ada7-eeed6b3fd450', null).then(function (form) {
                $scope.form = form;
            });
        };
        $scope.postForm = function () {
            $scope.submitting = true;
            forms.post($scope.form).then(function (result) {
                if (result.success)
                    $window.location.href = result.status;
                $scope.submitting = false;
            }).catch(function () {
                $scope.submitting = false;
            });
        };
    }])
    .controller('MasterPagesController', ['$scope', '$window', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, $window, utilities, forms) {
        $scope.populateForm = function (form, masterPage) {
            form.fields.name.value = masterPage.Name;
            form.fields.pageName.value = masterPage.PageName;
            form.fields.pageDescription.value = masterPage.PageDescription;
            form.fields.ancestorPageId.value = masterPage.AncestorPageId == null ? '' : masterPage.AncestorPageId.toString();
            form.fields.ancestorPageLevel.value = masterPage.AncestorPageLevel == null ? '' : masterPage.AncestorPageLevel.toString();
            form.fields.pageType.value = masterPage.PageType.toString();
            form.fields.hasOccurred.value = masterPage.HasOccurred;
            form.fields.hasImage.value = masterPage.HasImage;
            form.fields.imageMinWidth.value = masterPage.ImageMinWidth;
            form.fields.imageMinHeight.value = masterPage.ImageMinHeight;
            form.fields.thumbnailImageWidth.value = masterPage.ThumbnailImageWidth;
            form.fields.thumbnailImageHeight.value = masterPage.ThumbnailImageHeight;
            form.fields.thumbnailImageResizeMode.value = masterPage.ThumbnailImageResizeMode == null ? '' : masterPage.ThumbnailImageResizeMode.toString();
            form.fields.previewImageWidth.value = masterPage.PreviewImageWidth;
            form.fields.previewImageHeight.value = masterPage.PreviewImageHeight;
            form.fields.previewImageResizeMode.value = masterPage.PreviewImageResizeMode == null ? '' : masterPage.PreviewImageResizeMode.toString();
            form.fields.creatable.value = masterPage.Creatable;
            form.fields.deletable.value = masterPage.Deletable;
            form.fields.taggable.value = masterPage.Taggable;
            form.fields.administration.value = masterPage.Administration;
            form.fields.beginRender.value = masterPage.BeginRender;
            form.fields.endRender.value = masterPage.EndRender;
        };
        $scope.populateMasterPage = function (masterPage, form) {
            masterPage.Name = form.fields.name.value;
            masterPage.PageName = form.fields.pageName.value;
            masterPage.PageDescription = form.fields.pageDescription.value;
            masterPage.AncestorPageId = form.fields.ancestorPageId.value == '' ? null : parseInt(form.fields.ancestorPageId.value); // TODO: Look at this (make IDs strings, bigints JS problems)
            masterPage.AncestorPageLevel = form.fields.ancestorPageLevel.value == '' ? null : parseInt(form.fields.ancestorPageLevel.value);
            masterPage.PageType = parseInt(form.fields.pageType.value);
            masterPage.HasOccurred = form.fields.hasOccurred.value;
            masterPage.HasImage = form.fields.hasImage.value;
            masterPage.ImageMinWidth = form.fields.imageMinWidth.value;
            masterPage.ImageMinHeight = form.fields.imageMinHeight.value;
            masterPage.ThumbnailImageWidth = form.fields.thumbnailImageWidth.value;
            masterPage.ThumbnailImageHeight = form.fields.thumbnailImageHeight.value;
            masterPage.ThumbnailImageResizeMode = form.fields.thumbnailImageResizeMode.value == '' ? null : parseInt(form.fields.thumbnailImageResizeMode.value);
            masterPage.PreviewImageWidth = form.fields.previewImageWidth.value;
            masterPage.PreviewImageHeight = form.fields.previewImageHeight.value;
            masterPage.PreviewImageResizeMode = form.fields.previewImageResizeMode.value == '' ? null : parseInt(form.fields.previewImageResizeMode.value);
            masterPage.Creatable = form.fields.creatable.value;
            masterPage.Deletable = form.fields.deletable.value;
            masterPage.Taggable = form.fields.taggable.value;
            masterPage.Administration = form.fields.administration.value;
            masterPage.BeginRender = form.fields.beginRender.value;
            masterPage.EndRender = form.fields.endRender.value;
        };
        $scope.initialise = function (context) {
            $scope.showForm = true;
            var returnUrl = utilities.getParameterByName('returnurl');
            if (returnUrl.startsWith('/'))
                $scope.returnUrl = returnUrl;
            forms.get('3b945a38-bb9c-41ff-9e1a-42607b75458e', context).then(function (form) {
                $scope.form = form;
                $scope.masterPage = JSON.parse(form.data);
                $scope.populateForm($scope.form, $scope.masterPage);
            });
        };
        $scope.postForm = function () {
            $scope.populateMasterPage($scope.masterPage, $scope.form);
            $scope.form.data = JSON.stringify($scope.masterPage);
            $scope.formSubmitting = true;
            forms.post($scope.form).then(function (result) {
                if (result.success) {
                    if ($scope.returnUrl != undefined)
                        $window.location.href = $scope.returnUrl;
                    else
                        $window.location.href = '/';
                }
                $scope.formSubmitting = false;
            }).catch(function () {
                $scope.formSubmitting = false;
            });
        };
    }])
    .controller('MasterPageZonesController', ['$scope', '$window', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, $window, utilities, forms) {
        $scope.populateZoneForm = function (form, masterPageZone, submitLabel) {
            form.submitLabel = submitLabel;
            form.fields.name.value = masterPageZone.Name;
        };
        $scope.populateMasterPageZone = function (masterPageZone, form) {
            masterPageZone.Name = form.fields.name.value;
        };
        $scope.initialise = function (context) {
            $scope.showForm = true;
            $scope.showZoneForm = false;
            var returnUrl = utilities.getParameterByName('returnurl');
            if (returnUrl.startsWith('/'))
                $scope.returnUrl = returnUrl;
            forms.get('6e628c1b-7876-436c-b2d0-ac6a4859d507', context).then(function (form) {
                $scope.form = form;
                $scope.masterPageZones = JSON.parse(form.data);
                $scope.zoneForm = form.subForms.zone;
                var zoneFormData = JSON.parse($scope.zoneForm.data);
                $scope.masterPageZone = zoneFormData.masterPageZone;
                $scope.zoneLabels = zoneFormData.labels;
            });
        };
        $scope.createZone = function () {
            $scope.populateZoneForm($scope.zoneForm, $scope.masterPageZone, $scope.zoneLabels.create);
            $scope.zoneIndex = -1;
            $scope.showForm = false;
            $scope.showZoneForm = true;
        };
        $scope.deleteZone = function (zone, index) {
            $scope.masterPageZones.splice(index, 1);
        };
        $scope.updateZone = function (zone, index) {
            $scope.populateZoneForm($scope.zoneForm, zone, $scope.zoneLabels.update);
            $scope.zoneIndex = index;
            $scope.showForm = false;
            $scope.showZoneForm = true;
        };
        $scope.cancelZone = function () {
            $scope.showZoneForm = false;
            $scope.showForm = true;
        };
        $scope.postZoneForm = function () {
            $scope.zoneFormSubmitting = true;
            forms.postBasic($scope.zoneForm).then(function (result) {
                if (result.success) {
                    var masterPageZone = null;
                    if ($scope.zoneIndex < 0) {
                        masterPageZone = {};
                        $scope.masterPageZones.push(masterPageZone);
                    } else {
                        masterPageZone = $scope.masterPageZones[$scope.zoneIndex];
                    }
                    $scope.populateMasterPageZone(masterPageZone, $scope.zoneForm);
                    $scope.showForm = true;
                    $scope.showZoneForm = false;
                }
                $scope.zoneFormSubmitting = false;
            }).catch(function () {
                $scope.zoneFormSubmitting = false;
            });
        };
        $scope.postForm = function () {
            $scope.form.data = JSON.stringify($scope.masterPageZones);
            $scope.formSubmitting = true;
            forms.post($scope.form).then(function (result) {
                if (result.success) {
                    if ($scope.returnUrl != undefined)
                        $window.location.href = $scope.returnUrl;
                    else
                        $window.location.href = '/';
                }
                $scope.formSubmitting = false;
            }).catch(function () {
                $scope.formSubmitting = false;
            });
        };
    }])
    .controller('MasterPageZoneController', ['$scope', '$window', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, $window, utilities, forms) {
        $scope.populateZoneForm = function (form, masterPageZone) {
            form.model = jQuery.extend(true, {}, masterPageZone);
            form.fields.name.value = masterPageZone.Name;
            form.fields.adminType.value = masterPageZone.AdminType.toString();
            form.fields.contentType.value = masterPageZone.ContentType.toString();
            form.fields.beginRender.value = masterPageZone.BeginRender;
            form.fields.endRender.value = masterPageZone.EndRender;
        };
        $scope.populateZoneElementForm = function (form, masterPageZoneElement, submitLabel) {
            form.submitLabel = submitLabel;
            form.model = jQuery.extend(true, {}, masterPageZoneElement);
            form.fields.elementId.value = masterPageZoneElement.ListFieldItemValue;
            form.fields.beginRender.value = masterPageZoneElement.BeginRender;
            form.fields.endRender.value = masterPageZoneElement.EndRender;
        };
        $scope.populateZoneElementNewForm = function (form, masterPageZoneElement, submitLabel) {
            form.submitLabel = submitLabel;
            form.model = jQuery.extend(true, {}, masterPageZoneElement);
            if (masterPageZoneElement.ListFieldItemValue == undefined && masterPageZoneElement.Element != undefined && masterPageZoneElement.Element != null)
                form.fields.elementType.value = masterPageZoneElement.Element.ElementTypeId;
            else
                form.fields.elementType.value = '';
            if (masterPageZoneElement.ListFieldItemValue == undefined && masterPageZoneElement.Element != undefined && masterPageZoneElement.Element != null)
                form.fields.name.value = masterPageZoneElement.Element.Name;
            else
                form.fields.name.value = '';
            form.fields.beginRender.value = masterPageZoneElement.BeginRender;
            form.fields.endRender.value = masterPageZoneElement.EndRender;
        };
        $scope.populateZoneElementTypeForm = function (form, masterPageZoneElementType, submitLabel) {
            form.submitLabel = submitLabel;
            form.model = jQuery.extend(true, {}, masterPageZoneElementType);
            form.fields.elementTypeId.value = masterPageZoneElementType.ElementTypeId == '00000000-0000-0000-0000-000000000000' ? '' : String(masterPageZoneElementType.ElementTypeId);
        };
        $scope.populateMasterPageZone = function (masterPageZone, form) {
            masterPageZone.Name = form.fields.name.value;
            masterPageZone.AdminType = parseInt(form.fields.adminType.value);
            masterPageZone.ContentType = parseInt(form.fields.contentType.value);
            masterPageZone.BeginRender = form.fields.beginRender.value;
            masterPageZone.EndRender = form.fields.endRender.value;
            masterPageZone.MasterPageZoneElements = jQuery.extend(true, [], form.model.MasterPageZoneElements);
            masterPageZone.MasterPageZoneElementTypes = jQuery.extend(true, [], form.model.MasterPageZoneElementTypes);
        };
        $scope.populateMasterPageZoneElement = function (masterPageZoneElement, form) {
            var selectedItem = forms.getSelectedItem(form.fields.elementId);
            var values = selectedItem.value.split('|');
            masterPageZoneElement.ListFieldItemValue = selectedItem.value;
            masterPageZoneElement.ElementId = parseInt(values[3]);
            masterPageZoneElement.BeginRender = form.fields.beginRender.value;
            masterPageZoneElement.EndRender = form.fields.endRender.value;
            var nameIndex = selectedItem.name.lastIndexOf('/') + 1;
            var elementName = selectedItem.name.substring(nameIndex);
            masterPageZoneElement.Element = { ElementId: masterPageZoneElement.ElementId, Name: elementName };
        };
        $scope.populateMasterPageZoneElementNew = function (masterPageZoneElement, form) {
            masterPageZoneElement.ListFieldItemValue = undefined;
            masterPageZoneElement.ElementId = 0;
            masterPageZoneElement.Element = { ElementId: 0, ElementTypeId: forms.getSelectedItem(form.fields.elementType).value, Name: form.fields.name.value };
            masterPageZoneElement.BeginRender = form.fields.beginRender.value;
            masterPageZoneElement.EndRender = form.fields.endRender.value;
        };
        $scope.populateMasterPageZoneElementType = function (masterPageZoneElementType, form) {
            var selectedItem = forms.getSelectedItem(form.fields.elementTypeId);
            masterPageZoneElementType.ElementTypeId = selectedItem.value;
            masterPageZoneElementType.ElementType = { ElementTypeId: masterPageZoneElementType.ElementTypeId, Name: selectedItem.name };
        };
        $scope.initialise = function (masterPageId, masterPageZoneId) {
            $scope.showZoneForm = true;
            $scope.showZoneElementForm = false;
            $scope.showZoneElementTypeForm = false;
            forms.get('24281fa2-edad-4af9-9f60-e4fc869061c5', masterPageId + '|' + masterPageZoneId).then(function (form) {
                $scope.zoneForm = form;
                $scope.masterPageZone = JSON.parse(form.data);
                for (var index = 0; index < $scope.masterPageZone.MasterPageZoneElements.length; index++) {
                    var masterPageZoneElement = $scope.masterPageZone.MasterPageZoneElements[index];
                    masterPageZoneElement.ListFieldItemValue = masterPageZoneElement.MasterPageId + '|' + masterPageZoneElement.MasterPageZoneId + '|' + masterPageZoneElement.MasterPageZoneElementId + '|' + masterPageZoneElement.ElementId;
                }
                $scope.populateZoneForm($scope.zoneForm, $scope.masterPageZone);
                $scope.zoneElementForm = form.subForms.zoneElement;
                $scope.zoneElementNewForm = form.subForms.zoneElementNew;
                $scope.zoneElementTypeForm = form.subForms.zoneElementType;
                var zoneElementFormData = JSON.parse($scope.zoneElementForm.data);
                $scope.masterPageZoneElement = zoneElementFormData.masterPageZoneElement;
                $scope.zoneElementLabels = zoneElementFormData.labels;
                var zoneElementNewFormData = JSON.parse($scope.zoneElementNewForm.data);
                $scope.zoneElementNewLabels = zoneElementNewFormData.labels;
                var zoneElementTypeFormData = JSON.parse($scope.zoneElementTypeForm.data);
                $scope.masterPageZoneElementType = zoneElementTypeFormData.masterPageZoneElementType;
                $scope.zoneElementTypeLabels = zoneElementTypeFormData.labels;
            });
        };
        $scope.createZoneElement = function () {
            $scope.populateZoneElementForm($scope.zoneElementForm, $scope.masterPageZoneElement, $scope.zoneElementLabels.create);
            $scope.zoneElementIndex = -1;
            $scope.showZoneForm = false;
            $scope.showZoneElementForm = true;
        };
        $scope.deleteZoneElement = function (zoneElement, index) {
            $scope.zoneForm.model.MasterPageZoneElements.splice(index, 1);
        };
        $scope.updateZoneElement = function (zoneElement, index) {
            var newElementForm = zoneElement.ListFieldItemValue == undefined;
            if (newElementForm)
                $scope.populateZoneElementNewForm($scope.zoneElementNewForm, zoneElement, $scope.zoneElementNewLabels.update);
            else
                $scope.populateZoneElementForm($scope.zoneElementForm, zoneElement, $scope.zoneElementLabels.update);
            $scope.zoneElementIndex = index;
            $scope.showZoneForm = false;
            $scope.showZoneElementNewForm = newElementForm;
            $scope.showZoneElementForm = !newElementForm;
        };
        $scope.cancelZoneElement = function () {
            $scope.showZoneElementForm = false;
            $scope.showZoneForm = true;
        };
        $scope.createZoneElementNew = function () {
            $scope.populateZoneElementNewForm($scope.zoneElementNewForm, $scope.masterPageZoneElement, $scope.zoneElementNewLabels.create);
            $scope.zoneElementIndex = -1;
            $scope.showZoneForm = false;
            $scope.showZoneElementNewForm = true;
        };
        $scope.cancelZoneElementNew = function () {
            $scope.showZoneElementNewForm = false;
            $scope.showZoneForm = true;
        };
        $scope.createZoneElementType = function () {
            $scope.populateZoneElementTypeForm($scope.zoneElementTypeForm, $scope.masterPageZoneElementType, $scope.zoneElementTypeLabels.create);
            $scope.zoneElementTypeIndex = -1;
            $scope.showZoneForm = false;
            $scope.showZoneElementTypeForm = true;
        };
        $scope.deleteZoneElementType = function (zoneElementType, index) {
            $scope.zoneForm.model.MasterPageZoneElementTypes.splice(index, 1);
        };
        $scope.updateZoneElementType = function (zoneElementType, index) {
            $scope.populateZoneElementTypeForm($scope.zoneElementTypeForm, zoneElementType, $scope.zoneElementTypeLabels.update);
            $scope.zoneElementTypeIndex = index;
            $scope.showZoneForm = false;
            $scope.showZoneElementTypeForm = true;
        };
        $scope.cancelZoneElementType = function () {
            $scope.showZoneElementTypeForm = false;
            $scope.showZoneForm = true;
        };
        $scope.postZoneElementTypeForm = function () {
            $scope.zoneElementTypeFormSubmitting = true;
            forms.postBasic($scope.zoneElementTypeForm).then(function (result) {
                if (result.success) {
                    var masterPageZoneElementType = null;
                    if ($scope.zoneElementTypeIndex < 0) {
                        masterPageZoneElementType = {};
                        $scope.zoneForm.model.MasterPageZoneElementTypes.push(masterPageZoneElementType);
                    } else {
                        masterPageZoneElementType = $scope.zoneForm.model.MasterPageZoneElementTypes[$scope.zoneElementTypeIndex];
                    }
                    $scope.populateMasterPageZoneElementType(masterPageZoneElementType, $scope.zoneElementTypeForm);
                    $scope.showZoneForm = true;
                    $scope.showZoneElementTypeForm = false;
                }
                $scope.zoneElementTypeFormSubmitting = false;
            }).catch(function () {
                $scope.zoneElementTypeFormSubmitting = false;
            });
        };
        $scope.postZoneElementForm = function () {
            $scope.zoneElementFormSubmitting = true;
            forms.postBasic($scope.zoneElementForm).then(function (result) {
                if (result.success) {
                    var masterPageZoneElement = null;
                    if ($scope.zoneElementIndex < 0) {
                        masterPageZoneElement = {};
                        $scope.zoneForm.model.MasterPageZoneElements.push(masterPageZoneElement);
                    } else {
                        masterPageZoneElement = $scope.zoneForm.model.MasterPageZoneElements[$scope.zoneElementIndex];
                    }
                    $scope.populateMasterPageZoneElement(masterPageZoneElement, $scope.zoneElementForm);
                    $scope.showZoneForm = true;
                    $scope.showZoneElementForm = false;
                }
                $scope.zoneElementFormSubmitting = false;
            }).catch(function () {
                $scope.zoneElementFormSubmitting = false;
            });
        };
        $scope.postZoneElementNewForm = function () {
            $scope.zoneElementNewFormSubmitting = true;
            forms.postBasic($scope.zoneElementNewForm).then(function (result) {
                if (result.success) {
                    var masterPageZoneElement = null;
                    if ($scope.zoneElementIndex < 0) {
                        masterPageZoneElement = {};
                        $scope.zoneForm.model.MasterPageZoneElements.push(masterPageZoneElement);
                    } else {
                        masterPageZoneElement = $scope.zoneForm.model.MasterPageZoneElements[$scope.zoneElementIndex];
                    }
                    $scope.populateMasterPageZoneElementNew(masterPageZoneElement, $scope.zoneElementNewForm);
                    $scope.showZoneForm = true;
                    $scope.showZoneElementNewForm = false;
                }
                $scope.zoneElementNewFormSubmitting = false;
            }).catch(function () {
                $scope.zoneElementNewFormSubmitting = false;
            });
        };
        $scope.postZoneForm = function () {
            $scope.populateMasterPageZone($scope.masterPageZone, $scope.zoneForm);
            $scope.zoneForm.data = JSON.stringify($scope.masterPageZone);
            $scope.formSubmitting = true;
            forms.post($scope.zoneForm).then(function (result) {
                if (result.success)
                    utilities.redirectToReturnUrl();
                $scope.formSubmitting = false;
            }).catch(function () {
                $scope.formSubmitting = false;
            });
        };
    }])
    .controller('TestimonialAdminController', ['$scope', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, utilities, forms) {
        $scope.populateTestimonialForm = function (form, testimonial) {
            form.model = jQuery.extend(true, {}, testimonial);
            form.fields.displayName.value = testimonial.DisplayName;
            form.fields.preamble.value = testimonial.Preamble;
        };
        $scope.populateTestimonialCommentForm = function (form, testimonialComment, submitLabel) {
            form.submitLabel = submitLabel;
            form.model = jQuery.extend(true, {}, testimonialComment);
            form.fields.comment.value = testimonialComment.Comment;
            form.fields.author.value = testimonialComment.Author;
            form.fields.authorTitle.value = testimonialComment.AuthorTitle;
            form.fields.commentDate.value = testimonialComment.CommentDate;
        };
        $scope.populateTestimonial = function (testimonial, form) {
            testimonial.DisplayName = form.fields.displayName.value;
            testimonial.Preamble = form.fields.preamble.value;
            testimonial.Comments = jQuery.extend(true, [], form.model.Comments);
        };
        $scope.populateTestimonialComment = function (testimonialComment, form) {
            testimonialComment.Comment = form.fields.comment.value;
            testimonialComment.Author = form.fields.author.value;
            testimonialComment.AuthorTitle = form.fields.authorTitle.value;
            testimonialComment.CommentDate = form.fields.commentDate.value;
        };
        $scope.initialise = function (pageId, elementId) {
            $scope.pageId = pageId;
            $scope.elementId = elementId;
            $scope.showTestimonialForm = true;
            $scope.showTestimonialCommentForm = false;
            var context = $scope.pageId + '|' + $scope.elementId;
            forms.get('eb479ac9-8c79-4fae-817a-e77fd3dbf05b', context).then(function (form) {
                $scope.testimonialForm = form;
                $scope.testimonial = JSON.parse(form.data);
                $scope.populateTestimonialForm($scope.testimonialForm, $scope.testimonial);
                $scope.testimonialCommentForm = form.subForms.testimonialComment;
                var testimonialCommentFormData = JSON.parse($scope.testimonialCommentForm.data);
                $scope.testimonialComment = testimonialCommentFormData.testimonialComment;
                $scope.testimonialCommentLabels = testimonialCommentFormData.labels;
            });
        };
        $scope.createTestimonialComment = function () {
            $scope.populateTestimonialCommentForm($scope.testimonialCommentForm, $scope.testimonialComment, $scope.testimonialCommentLabels.create);
            $scope.testimonialCommentIndex = -1;
            $scope.showTestimonialForm = false;
            $scope.showTestimonialCommentForm = true;
        };
        $scope.deleteTestimonialComment = function (testimonialComment, index) {
            $scope.testimonialForm.model.Comments.splice(index, 1);
        };
        $scope.updateTestimonialComment = function (testimonialComment, index) {
            $scope.populateTestimonialCommentForm($scope.testimonialCommentForm, testimonialComment, $scope.testimonialCommentLabels.update);
            $scope.testimonialCommentIndex = index;
            $scope.showTestimonialForm = false;
            $scope.showTestimonialCommentForm = true;
        };
        $scope.cancelTestimonialComment = function () {
            $scope.showTestimonialCommentForm = false;
            $scope.showTestimonialForm = true;
        };
        $scope.postTestimonialCommentForm = function () {
            $scope.testimonialCommentFormSubmitting = true;
            forms.postBasic($scope.testimonialCommentForm).then(function (result) {
                if (result.success) {
                    var testimonialComment = null;
                    if ($scope.testimonialCommentIndex < 0) {
                        testimonialComment = {};
                        $scope.testimonialForm.model.Comments.push(testimonialComment);
                    } else {
                        testimonialComment = $scope.testimonialForm.model.Comments[$scope.testimonialCommentIndex];
                    }
                    $scope.populateTestimonialComment(testimonialComment, $scope.testimonialCommentForm);
                    $scope.showTestimonialForm = true;
                    $scope.showTestimonialCommentForm = false;
                }
                $scope.testimonialCommentFormSubmitting = false;
            }).catch(function () {
                $scope.testimonialCommentFormSubmitting = false;
            });
        };
        $scope.postTestimonialForm = function () {
            $scope.populateTestimonial($scope.testimonial, $scope.testimonialForm);
            $scope.testimonialForm.data = JSON.stringify($scope.testimonial);
            $scope.formSubmitting = true;
            forms.post($scope.testimonialForm).then(function (result) {
                if (result.success)
                    utilities.redirectToReturnUrl();
                $scope.formSubmitting = false;
            }).catch(function () {
                $scope.formSubmitting = false;
            });
        };
    }])
    .controller('TagCloudAdminController', ['$scope', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, utilities, forms) {
        $scope.getForm = function (pageId, elementId) {
            $scope.pageId = pageId;
            $scope.elementId = elementId;
            var context = $scope.pageId + '|' + $scope.elementId;
            forms.get('b910c231-7dbd-4cad-92ef-775981e895b4', context).then(function (form) {
                $scope.form = form;
            });
        };
        $scope.postForm = function () {
            $scope.submitting = true;
            forms.post($scope.form).then(function (result) {
                if (result.success)
                    utilities.redirectToReturnUrl();
                $scope.submitting = false;
            }).catch(function () {
                $scope.submitting = false;
            });
        };
    }])
    .controller('MapAdminController', ['$scope', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, utilities, forms) {
        $scope.getForm = function (pageId, elementId) {
            $scope.pageId = pageId;
            $scope.elementId = elementId;
            var context = $scope.pageId + '|' + $scope.elementId;
            forms.get('9a4b77e3-2edd-42db-8e14-153ae1f47005', context).then(function (form) {
                $scope.form = form;
            });
        };
        $scope.postForm = function () {
            $scope.submitting = true;
            forms.post($scope.form).then(function (result) {
                if (result.success)
                    utilities.redirectToReturnUrl();
                $scope.submitting = false;
            }).catch(function () {
                $scope.submitting = false;
            });
        };
    }])
    .controller('CarouselAdminController', ['$scope', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, utilities, forms) {
        $scope.initialise = function (pageId, elementId) {
            $scope.pageId = pageId;
            $scope.elementId = elementId;
            $scope.showSlidesForm = true;
            $scope.showSlideForm = false;
            forms.get('aacb11a0-5532-47cb-aab9-939cee3d5175', 'slide|' + $scope.pageId + '|' + $scope.elementId).then(function (form) {
                $scope.initialSlideForm = form;
            });
            forms.get('aacb11a0-5532-47cb-aab9-939cee3d5175', 'slides|' + $scope.pageId + '|' + $scope.elementId).then(function (form) {
                $scope.slides = JSON.parse(form.data);
                $scope.slidesForm = form;
            });
        };
        $scope.createSlide = function () {
            $scope.slideForm = jQuery.extend(true, {}, $scope.initialSlideForm);
            var labels = JSON.parse($scope.slideForm.data);
            $scope.slideForm.submitLabel = labels.create;
            $scope.slideForm.data = '0';
            $scope.slideIndex = -1;
            $scope.showSlidesForm = false;
            $scope.showSlideForm = true;
        };
        $scope.deleteSlide = function (slide, index) {
            $scope.slides.splice(index, 1);
        };
        $scope.updateSlide = function (slide, index) {
            $scope.slideForm = jQuery.extend(true, {}, $scope.initialSlideForm);
            var labels = JSON.parse($scope.slideForm.data);
            $scope.slideForm.submitLabel = labels.update;
            $scope.slideForm.data = slide.carouselSlideId;
            $scope.slideForm.fields.name.value = slide.name;
            $scope.slideForm.fields.description.value = slide.description;
            $scope.slideForm.fields.page.value = slide.pageId;
            $scope.slideForm.fields.pageText.value = slide.pageText;
            $scope.slideForm.fields.upload.value = slide.thumbnailImageUploadId + '|' + slide.previewImageUploadId + '|' + slide.imageUploadId;
            $scope.slideIndex = index;
            $scope.showSlidesForm = false;
            $scope.showSlideForm = true;
        };
        $scope.cancelSlide = function () {
            $scope.showSlideForm = false;
            $scope.showSlidesForm = true;
        };
        $scope.postSlideForm = function () {
            $scope.submitting = true;
            forms.post($scope.slideForm).then(function (result) {
                if (result.success) {
                    var slide = JSON.parse(result.status);
                    if ($scope.slideIndex < 0)
                        $scope.slides.push(slide);
                    else
                        $scope.slides[$scope.slideIndex] = slide;
                    $scope.showSlidesForm = true;
                    $scope.showSlideForm = false;
                }
                $scope.submitting = false;
            }).catch(function () {
                $scope.submitting = false;
            });
        };
        $scope.postSlidesForm = function () {
            $scope.slidesForm.data = JSON.stringify($scope.slides);
            $scope.slidesSubmitting = true;
            forms.post($scope.slidesForm).then(function (result) {
                if (result.success)
                    utilities.redirectToReturnUrl();
                $scope.slidesSubmitting = false;
            }).catch(function () {
                $scope.slidesSubmitting = false;
            });
        };
    }])
    .controller('AlbumController', ['$scope', 'riversideFormsFactory', function ($scope, forms) {
        $scope.initialise = function (pageId, elementId) {
            forms.get('b539d2a4-52ae-40d5-b366-e42447b93d15', 'album|' + pageId + '|' + elementId).then(function (form) {
                $scope.photos = JSON.parse(form.data);
            });
        };
        $scope.$on('visualLoaded', function (event) {
        });
        $scope.showPhoto = function (index) {
            if ($scope.photos != undefined && $scope.selectedIndex != index) {
                $scope.loadingVisual = true;
                $scope.selectedPhoto = $scope.photos[index];
                $scope.selectedIndex = index;
                if (!$scope.visible)
                    $scope.showViewer();
            }
        };
        $scope.showViewer = function () {
            $scope.visible = true;
            $('body').addClass('rcms-no-scroll');
        };
        $scope.hideViewer = function () {
            $scope.visible = false;
            $('body').removeClass('rcms-no-scroll');
            $scope.selectedPhoto = undefined;
            $scope.selectedIndex = undefined;
        };
        $scope.previous = function () {
            if ($scope.selectedIndex != undefined) {
                var index = $scope.selectedIndex - 1;
                if (index < 0)
                    index = $scope.photos.length - 1;
                $scope.showPhoto(index);
            }
        };
        $scope.next = function () {
            if ($scope.selectedIndex != undefined) {
                var index = $scope.selectedIndex + 1;
                if (index > $scope.photos.length - 1)
                    index = 0;
                $scope.showPhoto(index);
            }
        };
        $scope.keyDown = function (event) {
            switch (event.which) {
                case 27:
                    $scope.hideViewer();
                    break;
                case 37:
                    $scope.previous();
                    break;
                case 39:
                    $scope.next();
                    break;
                default:
                    return;
            }
            event.preventDefault();
        };
    }])
    .controller('AlbumAdminController', ['$scope', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, utilities, forms) {
        $scope.initialise = function (pageId, elementId) {
            $scope.pageId = pageId;
            $scope.elementId = elementId;
            $scope.showPhotosForm = true;
            $scope.showPhotoForm = false;
            forms.get('b539d2a4-52ae-40d5-b366-e42447b93d15', 'photo|' + $scope.pageId + '|' + $scope.elementId).then(function (form) {
                $scope.initialPhotoForm = form;
            });
            forms.get('b539d2a4-52ae-40d5-b366-e42447b93d15', 'photos|' + $scope.pageId + '|' + $scope.elementId).then(function (form) {
                $scope.photos = JSON.parse(form.data);
                $scope.photosForm = form;
            });
        };
        $scope.createPhoto = function () {
            $scope.photoForm = jQuery.extend(true, {}, $scope.initialPhotoForm);
            var labels = JSON.parse($scope.photoForm.data);
            $scope.photoForm.submitLabel = labels.create;
            $scope.photoForm.data = '0';
            $scope.photoIndex = -1;
            $scope.showPhotosForm = false;
            $scope.showPhotoForm = true;
        };
        $scope.deletePhoto = function (photo, index) {
            $scope.photos.splice(index, 1);
        };
        $scope.updatePhoto = function (photo, index) {
            $scope.photoForm = jQuery.extend(true, {}, $scope.initialPhotoForm);
            var labels = JSON.parse($scope.photoForm.data);
            $scope.photoForm.submitLabel = labels.update;
            $scope.photoForm.data = photo.albumPhotoId;
            $scope.photoForm.fields.name.value = photo.name;
            $scope.photoForm.fields.description.value = photo.description;
            $scope.photoForm.fields.upload.value = photo.thumbnailImageUploadId + '|' + photo.previewImageUploadId + '|' + photo.imageUploadId;
            $scope.photoIndex = index;
            $scope.showPhotosForm = false;
            $scope.showPhotoForm = true;
        };
        $scope.cancelPhoto = function () {
            $scope.showPhotoForm = false;
            $scope.showPhotosForm = true;
        };
        $scope.postPhotoForm = function () {
            $scope.submitting = true;
            forms.post($scope.photoForm).then(function (result) {
                if (result.success) {
                    var photo = JSON.parse(result.status);
                    if ($scope.photoIndex < 0)
                        $scope.photos.push(photo);
                    else
                        $scope.photos[$scope.photoIndex] = photo;
                    $scope.showPhotosForm = true;
                    $scope.showPhotoForm = false;
                }
                $scope.submitting = false;
            }).catch(function () {
                $scope.submitting = false;
            });
        };
        $scope.postPhotosForm = function () {
            $scope.photosForm.data = JSON.stringify($scope.photos);
            $scope.photosSubmitting = true;
            forms.post($scope.photosForm).then(function (result) {
                if (result.success)
                    utilities.redirectToReturnUrl();
                $scope.photosSubmitting = false;
            }).catch(function () {
                $scope.photosSubmitting = false;
            });
        };
    }])
    .controller('ShareAdminController', ['$scope', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, utilities, forms) {
        $scope.getForm = function (pageId, elementId) {
            $scope.pageId = pageId;
            $scope.elementId = elementId;
            var context = $scope.pageId + '|' + $scope.elementId;
            forms.get('cf0d7834-54fb-4a6e-86db-0f238f8b1ac1', context).then(function (form) {
                $scope.form = form;
            });
        };
        $scope.postForm = function () {
            $scope.submitting = true;
            forms.post($scope.form).then(function (result) {
                if (result.success)
                    utilities.redirectToReturnUrl();
                $scope.submitting = false;
            }).catch(function () {
                $scope.submitting = false;
            });
        };
    }])
    .controller('ContactAdminController', ['$scope', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, utilities, forms) {
        $scope.getForm = function (pageId, elementId) {
            $scope.pageId = pageId;
            $scope.elementId = elementId;
            var context = $scope.pageId + '|' + $scope.elementId;
            forms.get('4e6b936d-e8a1-4ff2-9576-9f9b78f82895', context).then(function (form) {
                $scope.form = form;
            });
        };
        $scope.postForm = function () {
            $scope.submitting = true;
            forms.post($scope.form).then(function (result) {
                if (result.success)
                    utilities.redirectToReturnUrl();
                $scope.submitting = false;
            }).catch(function () {
                $scope.submitting = false;
            });
        };
    }])
    .controller('TableAdminController', ['$scope', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, utilities, forms) {
        $scope.getForm = function (pageId, elementId) {
            $scope.pageId = pageId;
            $scope.elementId = elementId;
            var context = $scope.pageId + '|' + $scope.elementId;
            forms.get('252ca19c-d085-4e0d-b70b-da3e1098f51b', context).then(function (form) {
                $scope.form = form;
            });
        };
        $scope.postForm = function () {
            $scope.submitting = true;
            forms.post($scope.form).then(function (result) {
                if (result.success)
                    utilities.redirectToReturnUrl();
                $scope.submitting = false;
            }).catch(function () {
                $scope.submitting = false;
            });
        };
    }])
    .controller('FormController', ['$scope', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, utilities, forms) {
        $scope.getForm = function (formAction, pageId, elementId) {
            $scope.nextId = 0;
            $scope.pageId = pageId;
            $scope.elementId = elementId;
            $scope.formAction = formAction;
            var context = $scope.formAction + '|' + $scope.pageId + '|' + $scope.elementId;
            forms.get('eafbd5ab-8c98-4edc-b8e1-42f5e8bfe2dc', context).then(function (form) {
                $scope.form = form;
                $scope.form.activeFieldSet = $scope.form.fieldSets.length > 0 ? $scope.form.fieldSets[0] : null;
            });
        };
        $scope.getNextId = function () {
            $scope.nextId = $scope.nextId - 1;
            return $scope.nextId;
        };
        $scope.addField = function () {
            $scope.form.activeFieldSet = forms.copyFieldSet($scope.form.namedFieldSets["newField"], '_0_', '_' + $scope.getNextId() + '_');
            $scope.form.fieldSets.push($scope.form.activeFieldSet);
        };
        $scope.selectField = function (fieldSet) {
            $scope.form.activeFieldSet = fieldSet;
        };
        $scope.deleteField = function () {
            $scope.form.activeFieldSet = forms.deleteFieldSet($scope.form.fieldSets, $scope.form.activeFieldSet);
        };
        $scope.postForm = function () {
            $scope.submitting = true;
            forms.post($scope.form).then(function (result) {
                if (result.success) {
                    if ($scope.formAction == 'admin')
                        utilities.redirectToReturnUrl();
                    else
                        $scope.formSubmitted = true;
                }
                $scope.submitting = false;
            }).catch(function () {
                $scope.submitting = false;
            });
        };
    }])
    .controller('FooterAdminController', ['$scope', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, utilities, forms) {
        $scope.getForm = function (pageId, elementId) {
            $scope.pageId = pageId;
            $scope.elementId = elementId;
            var context = $scope.pageId + '|' + $scope.elementId;
            forms.get('f1c2b384-4909-47c8-ada7-cd3cc7f32620', context).then(function (form) {
                $scope.form = form;
            });
        };
        $scope.postForm = function () {
            $scope.submitting = true;
            forms.post($scope.form).then(function (result) {
                if (result.success)
                    utilities.redirectToReturnUrl();
                $scope.submitting = false;
            }).catch(function () {
                $scope.submitting = false;
            });
        };
    }])
    .controller('CodeSnippetAdminController', ['$scope', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, utilities, forms) {
        $scope.getForm = function (pageId, elementId) {
            $scope.pageId = pageId;
            $scope.elementId = elementId;
            var context = $scope.pageId + '|' + $scope.elementId;
            forms.get('5401977d-865f-4a7a-b416-0a26305615de', context).then(function (form) {
                $scope.form = form;
            });
        };
        $scope.postForm = function () {
            $scope.submitting = true;
            forms.post($scope.form).then(function (result) {
                if (result.success)
                    utilities.redirectToReturnUrl();
                $scope.submitting = false;
            }).catch(function () {
                $scope.submitting = false;
            });
        };
    }])
    .controller('PageInfoAdminController', ['$scope', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, utilities, forms) {
        $scope.getForm = function (pageId, elementId) {
            $scope.pageId = pageId;
            $scope.elementId = elementId;
            var context = $scope.pageId + '|' + $scope.elementId;
            forms.get('1cbac30c-5deb-404e-8ea8-aabc20c82aa8', context).then(function (form) {
                $scope.form = form;
            });
        };
        $scope.postForm = function () {
            $scope.submitting = true;
            forms.post($scope.form).then(function (result) {
                if (result.success)
                    utilities.redirectToReturnUrl();
                $scope.submitting = false;
            }).catch(function () {
                $scope.submitting = false;
            });
        };
    }])
    .controller('NavBarAdminController', ['$scope', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, utilities, forms) {
        $scope.getForm = function (pageId, elementId) {
            $scope.nextId = 0;
            $scope.pageId = pageId;
            $scope.elementId = elementId;
            var context = $scope.pageId + '|' + $scope.elementId;
            forms.get('a94c34c0-1a4c-4c91-a669-2f830cf1ea5f', context).then(function (form) {
                $scope.form = form;
                $scope.form.activeFieldSet = $scope.form.fieldSets.length > 0 ? $scope.form.fieldSets[0] : null;
            });
        };
        $scope.getNextId = function () {
            $scope.nextId = $scope.nextId - 1;
            return $scope.nextId;
        };
        $scope.addTab = function () {
            $scope.form.activeFieldSet = forms.copyFieldSet($scope.form.namedFieldSets["newTab"], '_0_', '_' + $scope.getNextId() + '_');
            $scope.form.fieldSets.push($scope.form.activeFieldSet);
        };
        $scope.selectTab = function (fieldSet) {
            $scope.form.activeFieldSet = fieldSet;
        };
        $scope.deleteTab = function () {
            $scope.form.activeFieldSet = forms.deleteFieldSet($scope.form.fieldSets, $scope.form.activeFieldSet);
        };
        $scope.postForm = function () {
            $scope.submitting = true;
            forms.post($scope.form).then(function (result) {
                if (result.success)
                    utilities.redirectToReturnUrl();
                $scope.submitting = false;
            }).catch(function () {
                $scope.submitting = false;
            });
        };
    }])
    .controller('LatestThreadAdminController', ['$scope', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, utilities, forms) {
        $scope.getForm = function (pageId, elementId) {
            $scope.pageId = pageId;
            $scope.elementId = elementId;
            var context = $scope.pageId + '|' + $scope.elementId;
            forms.get('f9557287-ba01-48e3-9ab4-e2f4831933d0', context).then(function (form) {
                $scope.form = form;
            });
        };
        $scope.postForm = function () {
            $scope.submitting = true;
            forms.post($scope.form).then(function (result) {
                if (result.success)
                    utilities.redirectToReturnUrl();
                $scope.submitting = false;
            }).catch(function () {
                $scope.submitting = false;
            });
        };
    }])
    .controller('HtmlAdminController', ['$scope', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, utilities, forms) {
        $scope.getForm = function (pageId, elementId, editorCssPath) {
            $scope.tinymceOptions = { content_css: editorCssPath, height: 380, plugins: 'code link' };
            $scope.pageId = pageId;
            $scope.elementId = elementId;
            var context = $scope.pageId + '|' + $scope.elementId;
            forms.get('c92ee4c4-b133-44cc-8322-640e99c334dc', context).then(function (form) {
                $scope.form = form;
            });
        };
        $scope.fileUploaded = function () {
            $scope.form.fields.html.value = $scope.form.fields.html.value + '\r\n<p><img src="' + $scope.form.fields.upload.value + '" /></p>';
        };
        $scope.postForm = function () {
            $scope.submitting = true;
            forms.post($scope.form).then(function (result) {
                if (result.success)
                    utilities.redirectToReturnUrl();
                $scope.submitting = false;
            }).catch(function () {
                $scope.submitting = false;
            });
        };
    }])
    .controller('CreateUserController', ['$scope', '$http', 'riversideFormsFactory', function ($scope, $http, forms) {
        $scope.initialise = function () {
            $http.get('/apps/admin/api/forms/06e416f7-c219-4e05-9aba-ac0e1008e79a').success(function (model) {
                $scope.model = model;
            }).error(function () {
            });
        };
        $scope.submitForm = function () {
            $scope.submitting = true;
            $http.post('/apps/admin/api/forms/06e416f7-c219-4e05-9aba-ac0e1008e79a', $scope.model).success(function (result) {
                if (forms.resultIsSuccess($scope.model, result))
                    $scope.userCreated = true;
                else
                    $scope.submitting = false;
            }).error(function () {
                $scope.submitting = false;
            });
        };
    }])
    .controller('ConfirmUserSetPasswordController', ['$scope', '$http', '$window', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, $http, $window, riversideUtilitiesFactory, forms) {
        $scope.initialise = function () {
            var key = encodeURIComponent(riversideUtilitiesFactory.getParameterByName('activationkey'));
            $http.get('/apps/admin/api/forms/b0f40f31-1f06-4b89-ba48-9e31215555bd?context=' + key).success(function (model) {
                $scope.model = model;
            }).error(function () {
            });
        };
        $scope.submitForm = function () {
            $scope.submitting = true;
            var key = encodeURIComponent(riversideUtilitiesFactory.getParameterByName('activationkey'));
            $http.post('/apps/admin/api/forms/b0f40f31-1f06-4b89-ba48-9e31215555bd?context=' + key, $scope.model).success(function (result) {
                if (forms.resultIsSuccess($scope.model, result))
                    $window.location.href = '/users/login?reason=confirmusersetpassword';
                else
                    $scope.submitting = false;
            }).error(function () {
                $scope.submitting = false;
            });
        };
    }])
    .controller('LogonUserController', ['$scope', '$window', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, $window, utilities, forms) {
        $scope.getForm = function () {
            var returnUrl = utilities.getParameterByName('returnurl');
            if (returnUrl.startsWith('/'))
                $scope.returnUrl = returnUrl;
            forms.get('8bc495ba-9b4f-4d8c-b1ae-dc50dc3b22b1', null).then(function (form) {
                $scope.form = form;
            });
        };
        $scope.postForm = function () {
            $scope.submitting = true;
            forms.post($scope.form).then(function (result) {
                if (result.success) {
                    if ($scope.returnUrl != undefined)
                        $window.location.href = $scope.returnUrl;
                    else
                        $window.location.href = '/?loggedon=true';
                } else {
                    $scope.submitting = false;
                }
            }).catch(function () {
                $scope.submitting = false;
            });
        };
    }])
    .controller('ChangePasswordController', ['$scope', '$http', '$window', 'riversideFormsFactory', function ($scope, $http, $window, forms) {
        $scope.initialise = function () {
            $http.get('/apps/admin/api/forms/663928ae-1efe-4026-acc4-478202eb7916').success(function (model) {
                $scope.model = model;
            }).error(function () {
            });
        };
        $scope.submitForm = function () {
            $scope.submitting = true;
            $http.post('/apps/admin/api/forms/663928ae-1efe-4026-acc4-478202eb7916', $scope.model).success(function (result) {
                if (forms.resultIsSuccess($scope.model, result)) {
                    if (result.status == 'lockedout')
                        $window.location.href = '/users/login?reason=changepassword';
                    else
                        $scope.passwordChanged = true;
                } else {
                    $scope.submitting = false;
                }
            }).error(function () {
                $scope.submitting = false;
            });
        };
    }])
    .controller('AuthenticationController', ['$scope', '$window', 'riversideFormsFactory', function ($scope, $window, forms) {
        $scope.getForm = function (context) {
            forms.get('627a0edb-da2f-461b-9722-175c393c314f', context).then(function (form) {
                $scope.form = form;
            });
        };
        $scope.postForm = function () {
            $scope.submitting = true;
            forms.post($scope.form).then(function (result) {
                if (result.success) {
                    if (result.status == null)
                        $scope.userUpdated = true;
                    else
                        $window.location.href = result.status;
                }
                $scope.submitting = false;
            }).catch(function () {
                $scope.submitting = false;
            });
        };
    }])
    .controller('ConfirmUserController', ['$scope', '$http', '$window', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, $http, $window, riversideUtilitiesFactory, forms) {
        $scope.initialise = function () {
            var key = encodeURIComponent(riversideUtilitiesFactory.getParameterByName('activationkey'));
            $http.get('/apps/admin/api/forms/9f05385f-c30b-4a22-a764-07bc38aeb6f3?context=' + key).success(function (model) {
                $scope.model = model;
                if (model.customErrorMessages == null || model.customErrorMessages.length == 0)
                    $window.location.href = '/users/login?reason=confirmuser';
                else
                    $scope.confirmUserFailure = true;
            }).error(function () {
            });
        };
    }])
    .controller('ForgottenPasswordController', ['$scope', '$http', 'riversideFormsFactory', function ($scope, $http, forms) {
        $scope.initialise = function () {
            $http.get('/apps/admin/api/forms/080f974d-6034-4103-a255-0ec0e1ed8b52').success(function (model) {
                $scope.model = model;
            }).error(function () {
            });
        };
        $scope.submitForm = function () {
            $scope.submitting = true;
            $http.post('/apps/admin/api/forms/080f974d-6034-4103-a255-0ec0e1ed8b52', $scope.model).success(function (result) {
                if (forms.resultIsSuccess($scope.model, result)) {
                    $scope.completed = true;
                } else {
                    $scope.submitting = false;
                }
            }).error(function () {
                $scope.submitting = false;
            });
        };
    }])
    .controller('ResetPasswordController', ['$scope', '$http', '$window', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, $http, $window, riversideUtilitiesFactory, forms) {
        $scope.initialise = function () {
            var key = encodeURIComponent(riversideUtilitiesFactory.getParameterByName('resetpasswordkey'));
            $http.get('/apps/admin/api/forms/5d9cb54b-0a34-469f-9362-2306f73be7a5?context=' + key).success(function (model) {
                $scope.model = model;
            }).error(function () {
            });
        };
        $scope.submitForm = function () {
            $scope.submitting = true;
            var key = encodeURIComponent(riversideUtilitiesFactory.getParameterByName('resetpasswordkey'));
            $http.post('/apps/admin/api/forms/5d9cb54b-0a34-469f-9362-2306f73be7a5?context=' + key, $scope.model).success(function (result) {
                if (forms.resultIsSuccess($scope.model, result))
                    $window.location.href = '/users/login?reason=resetpassword';
                else
                    $scope.submitting = false;
            }).error(function () {
                $scope.submitting = false;
            });
        };
    }])
    .controller('PageListAdminController', ['$scope', '$http', 'riversideUtilitiesFactory', 'riversideFormsFactory', function ($scope, $http, utilities, forms) {
        $scope.initialise = function (pageId, elementId) {
            $scope.pageId = pageId;
            $scope.elementId = elementId;
            var context = $scope.pageId + '|' + $scope.elementId;
            $http.get('/apps/admin/api/forms/61f55535-9f3e-4ef5-96a2-bc84d648842a?context=' + context).success(function (model) {
                $scope.model = model;
            }).error(function () {
            });
        };
        $scope.submitForm = function () {
            $scope.submitting = true;
            var context = $scope.pageId + '|' + $scope.elementId;
            $http.post('/apps/admin/api/forms/61f55535-9f3e-4ef5-96a2-bc84d648842a?context=' + context, $scope.model).success(function (result) {
                if (forms.resultIsSuccess($scope.model, result))
                    utilities.redirectToReturnUrl();
                else
                    $scope.submitting = false;
            }).error(function () {
                $scope.submitting = false;
            });
        };
    }])
    .controller('ForumController', ['$scope', '$window', 'riversideFormsFactory', function ($scope, $window, forms) {
        $scope.getForm = function (context) {
            forms.get('484192d1-5a4f-496f-981b-7e0120453949', context).then(function (form) {
                $scope.form = form;
            });
        };
        $scope.postForm = function () {
            $scope.submitting = true;
            forms.post($scope.form).then(function (result) {
                if (result.success)
                    $window.location.href = result.status;
                $scope.submitting = false;
            }).catch(function () {
                $scope.submitting = false;
            });
        };
    }])
    .controller('PageZoneAdminController', ['$scope', '$window', 'riversideFormsFactory', function ($scope, $window, forms) {
        $scope.getForm = function (pageId, pageZoneId) {
            $scope.nextId = 0;
            $scope.pageId = pageId;
            $scope.pageZoneId = pageZoneId;
            var context = $scope.pageId + '|' + $scope.pageZoneId;
            forms.get('2835ba3d-0e3d-4820-8c94-fbfba293c6f0', context).then(function (form) {
                $scope.form = form;
                $scope.form.activeFieldSet = $scope.form.fieldSets.length > 0 ? $scope.form.fieldSets[0] : null;
            });
        };
        $scope.getNextId = function () {
            $scope.nextId = $scope.nextId - 1;
            return $scope.nextId;
        };
        $scope.addElement = function () {
            $scope.form.activeFieldSet = forms.copyFieldSet($scope.form.namedFieldSets["newElement"], '_0_', '_' + $scope.getNextId() + '_');
            $scope.form.fieldSets.push($scope.form.activeFieldSet);
        };
        $scope.selectElement = function (fieldSet) {
            $scope.form.activeFieldSet = fieldSet;
        };
        $scope.deleteElement = function () {
            $scope.form.activeFieldSet = forms.deleteFieldSet($scope.form.fieldSets, $scope.form.activeFieldSet);
        };
        $scope.postForm = function () {
            $scope.submitting = true;
            forms.post($scope.form).then(function (result) {
                if (result.success)
                    $window.location.href = '/pages/' + $scope.pageId;
                $scope.submitting = false;
            }).catch(function () {
                $scope.submitting = false;
            });
        };
    }])
    .directive('visual', function () {
        return {
            link: function (scope, element, attrs) {
                element.bind("load", function (e) {
                    scope.loadingVisual = false;
                    scope.visualWidth = $(this)[0].naturalWidth;
                    scope.visualHeight = $(this)[0].naturalHeight;
                    scope.$apply();
                    scope.$emit('visualLoaded');
                });
            }
        };
    })
    .directive('visualiser', function () {
        return function (scope, element, attrs) {
            var w = $(element);
            scope.getVisualiserDimensions = function () {
                return { 'h': w.height(), 'w': w.width() };
            };
            scope.$watch(scope.getVisualiserDimensions, function (newValue, oldValue) {
                scope.visualiserHeight = newValue.h;
                scope.visualiserWidth = newValue.w;
                scope.visualStyle = function () {
                    if (scope.visualWidth == undefined || scope.visualHeight == undefined) {
                        return {
                            'display': 'none'
                        };
                    }
                    var visualWidth = scope.visualWidth;
                    var visualHeight = scope.visualHeight;
                    var visualiserHeight = newValue.h;
                    var visualiserWidth = newValue.w;
                    var heightRatio = visualiserHeight / visualHeight;
                    var widthRatio = visualiserWidth / visualWidth;
                    var ratio = Math.min(heightRatio, widthRatio);
                    var width = visualWidth * ratio;
                    var height = visualHeight * ratio;
                    var top = Math.max((visualiserHeight - height) / 2, 0);
                    var left = Math.max((visualiserWidth - width) / 2, 0);
                    scope.scale = ratio;
                    return {
                        'height': height + 'px',
                        'width': width + 'px',
                        'top': top + 'px',
                        'left': left + 'px'
                    };
                };
            }, true);
            $(window).resize(function () {
                scope.$apply();
            });
        };
    });