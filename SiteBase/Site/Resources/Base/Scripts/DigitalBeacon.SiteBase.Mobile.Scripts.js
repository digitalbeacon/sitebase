
if(typeof DigitalBeacon == 'undefined') DigitalBeacon = {};
if(!DigitalBeacon.SiteBase) DigitalBeacon.SiteBase = {};
if(!DigitalBeacon.SiteBase.Mobile) DigitalBeacon.SiteBase.Mobile = {};

DigitalBeacon.SiteBase.Mobile.BaseController = (function() {
    function BaseController() {
        this.alerts = new Array(0);
        this.model = {};
        this.data = {};
    }
    var p = BaseController.prototype;
    p._scope = null;
    p._routerState = null;
    p._location = null;
    p.get_Scope = function() {
        return (this._scope) || this;
    };
    p.set_Scope = function(value) {
        this._scope = value;
    };
    p.get_RouterState = function() {
        return this._routerState;
    };
    p.set_RouterState = function(value) {
        this._routerState = value;
    };
    p.get_RouterParams = function() {
        return this._routerState ? this._routerState.params : {};
    };
    p.get_Location = function() {
        return this._location;
    };
    p.set_Location = function(value) {
        this._location = value;
    };
    p.get_DefaultHandler = function() {
        return Blade.del(this, function(response) {
            DigitalBeacon.SiteBase.ApiResponseHelper.handleResponse(response, this.get_Scope())});
    };
    p.get_ResponseHandler = function() {
        return Blade.del(this, function(response) {
            this.handleResponse(response)});
    };
    p.init = function () {
    };
    p.hasAlert = function (key) {
        if (!this.alerts || this.alerts.length === 0) {
            return false;
        }
        for(var i = 0; i < this.alerts.length; i++) {
            if (this.alerts[i].key === key) {
                return true;
            }
        }
        return false;
    };
    p.clearAlerts = function (key) {
        key = (key !== undefined) ? key : null;
        if (this.alerts && this.alerts.length > 0) {
            if (key) {
                for(var i = this.alerts.length - 1; i >= 0; i--) {
                    if (this.alerts[i].key === key) {
                        this.closeAlert(i);
                    }
                }
            } else {
                this.alerts.length = 0;
            }
        }
    };
    p.closeAlert = function (index) {
        this.alerts.splice(index, 1);
    };
    p.fileChanged = function (fileInput) {
        if (!fileInput) {
            return;
        }
        var files = fileInput.files;
        if (fileInput.files && fileInput.files.length) {
            this.data.fileInput = fileInput;
        } else {
            this.data.fileInput = null;
        }
    };
    p.handleResponse = function (response) {
    };
    return BaseController;
})();
DigitalBeacon.SiteBase.Mobile.BaseController.extend = function (target, obj) {
    ($.extend(target, obj)).init();
};

DigitalBeacon.SiteBase.Mobile.BaseEntityService = (function() {
    function BaseEntityService() {
    }
    var p = BaseEntityService.prototype;
    p.$Resource = null;
    p.get_Resource = function() {
        return this.$Resource;
    };
    p.set_Resource = function(value) {
        this.$Resource = value;
    };
    p.get = function (parameters, responseHandler) {
        responseHandler = (responseHandler !== undefined) ? responseHandler : null;
        return this.get_Resource().get(parameters, responseHandler);
    };
    p.search = function (parameters, response) {
        this.get_Resource().search(parameters, response);
    };
    p.save = function (id, postData, responseHandler) {
        responseHandler = (responseHandler !== undefined) ? responseHandler : null;
        if (id) {
            this.get_Resource().update({
                id: id
            }, postData, responseHandler);
        } else {
            this.get_Resource().save(postData, responseHandler);
        }
    };
    p.delete = function (parameters, responseHandler) {
        responseHandler = (responseHandler !== undefined) ? responseHandler : null;
        this.get_Resource().delete(parameters, responseHandler);
    };
    p.sendFormData = function (http, entityTarget, id, model, files, responseHandler) {
        responseHandler = (responseHandler !== undefined) ? responseHandler : null;
        var headers = {};
        headers['Content-Type'] = undefined;
        http({
            method: id ? 'PUT' : 'POST',
            url: DigitalBeacon.SiteBase.ControllerHelper.getJsonUrl('~/' + entityTarget + (id ? ('/' + id) : '')),
            headers: headers,
            data: DigitalBeacon.SiteBase.Mobile.BaseEntityService.constructFormData(model, files),
            transformRequest: Blade.del(this, function(x) {
                return x;
            })
        }).success(responseHandler);
    };
    Blade.impl(BaseEntityService, 'DigitalBeacon.SiteBase.Mobile.IEntityService');
    return BaseEntityService;
})();
DigitalBeacon.SiteBase.Mobile.BaseEntityService.constructFormData = function (model, files) {
    var data = new FormData();
    var k = null;
    var $k_enum = Object.keys(model).GetEnumerator();
    while($k_enum.MoveNext()) {
        k = $k_enum.get_Current();
        if (k === 'ListItems' || k.slice(0, 1) === '$') {
            continue;
        }
        if (model[k] || $.digitalbeacon.isOfType(model[k], 'boolean')) {
            data.append(k, model[k]);
        }
    }
    if (files) {
        for(var i = 0; i < files.length; i++) {
            data.append('file' + i, files[i]);
        }
    }
    return data;
};

DigitalBeacon.SiteBase.Mobile.BaseListState = (function() {
    function BaseListState() {
        this.sortDirectionOptions = [new Option('Ascending', ''), new Option('Descending', '-DESC')];
    }
    var p = BaseListState.prototype;
    p.isFiltered = null;
    p.searchText = null;
    p.sortText = null;
    p.sortDirection = null;
    p.sortTextOptions = null;
    p.page = 1;
    p.pageSize = 4;
    p.pageCount = 1;
    p.footerHeight = 140;
    p.visible = true;
    return BaseListState;
})();
if(!DigitalBeacon.SiteBase.Mobile.Contacts) DigitalBeacon.SiteBase.Mobile.Contacts = {};

DigitalBeacon.SiteBase.Mobile.Contacts.ContactsModule = (function() {
    function ContactsModule() {
    }
    return ContactsModule;
})();
if(!DigitalBeacon.SiteBase.Mobile.Identity) DigitalBeacon.SiteBase.Mobile.Identity = {};

DigitalBeacon.SiteBase.Mobile.Identity.IdentityModule = (function() {
    function IdentityModule() {
    }
    return IdentityModule;
})();

DigitalBeacon.SiteBase.Mobile.Identity.IdentityService = (function() {
    function IdentityService() {
    }
    return IdentityService;
})();

DigitalBeacon.SiteBase.Mobile.ListIem = (function() {
    function ListIem() {
    }
    var p = ListIem.prototype;
    p.Item = null;
    return ListIem;
})();

DigitalBeacon.SiteBase.Mobile.SiteBaseModule = (function() {
    function SiteBaseModule() {
    }
    return SiteBaseModule;
})();

DigitalBeacon.SiteBase.Mobile.BaseDetailsController = (function() {
    Blade.derive(BaseDetailsController, DigitalBeacon.SiteBase.Mobile.BaseController);
    var $base = DigitalBeacon.SiteBase.Mobile.BaseController.prototype;
    function BaseDetailsController() {
        $base.constructor.call(this);
    }
    var p = BaseDetailsController.prototype;
    p.id = null;
    p.get_ReturnToList = function() {
        return Blade.del(this, function(response) {
            if (response.Success) {
                this.showList(response);
            } else {
                DigitalBeacon.SiteBase.ApiResponseHelper.handleResponse(response, this.get_Scope());
            }
        });
    };
    p.init = function () {
        $base.init.call(this);
        this.hideList();
    };
    p.getDisplayObject = function (x) {
        var retVal = {};
        var key = null;
        var $key_enum = Object.keys(x).GetEnumerator();
        while($key_enum.MoveNext()) {
            key = $key_enum.get_Current();
            var val = x[key];
            if (!val || (Blade.is(val, Array) && val.length === 0)) {
                continue;
            }
            retVal[key] = x[key];
        }
        return retVal;
    };
    p.showList = function (response) {
        response = (response !== undefined) ? response : null;
        this.get_Scope().$emit('showList', response);
    };
    p.hideList = function () {
        this.get_Scope().$emit('hideList');
    };
    p.submit = function (isValid) {
    };
    p.delete = function () {
    };
    p.cancel = function () {
        this.showList();
    };
    return BaseDetailsController;
})();

DigitalBeacon.SiteBase.Mobile.BaseListController = (function() {
    Blade.derive(BaseListController, DigitalBeacon.SiteBase.Mobile.BaseController);
    var $base = DigitalBeacon.SiteBase.Mobile.BaseController.prototype;
    function BaseListController() {
        this.list = new DigitalBeacon.SiteBase.Mobile.BaseListState();
        $base.constructor.call(this);
    }
    var p = BaseListController.prototype;
    p.init = function () {
        $base.init.call(this);
        this.get_Scope().$on('showList', Blade.del(this, function(evt, args) {
            this.showList(args);
        }));
        this.get_Scope().$on('hideList', Blade.del(this, function() {
            this.hideList();
        }));
    };
    p.search = function (requestMore) {
        requestMore = (requestMore !== undefined) ? requestMore : false;
    };
    p.loadMore = function () {
        if (this.list.page < this.list.pageCount) {
            this.list.page++;
            this.search(true);
        }
    };
    p.addNew = function () {
        this.clearAlerts();
        this.get_RouterState().go('list.new');
        this.model = {};
    };
    p.showDetails = function (id) {
        this.clearAlerts();
        this.get_RouterState().go('list.edit', {
            id: id
        });
    };
    p.hideList = function () {
        this.list.visible = false;
        this.disableLoadMoreOnScroll();
    };
    p.showList = function (response) {
        response = (response !== undefined) ? response : null;
        this.list.visible = true;
        this.get_RouterState().go('list');
        this.enableLoadMoreOnScroll();
        if (response) {
            if (response.Success) {
                this.search();
            }
            DigitalBeacon.SiteBase.ApiResponseHelper.handleResponse(response, this.get_Scope());
        }
    };
    p.clearSearchText = function () {
        this.list.searchText = '';
        if (this.list.isFiltered) {
            this.search();
        }
        this.list.isFiltered = false;
        this.list.page = 1;
    };
    p.getSortValue = function () {
        return this.list.sortText + this.list.sortDirection;
    };
    p.enableLoadMoreOnScroll = function () {
        if (this.list.pageCount <= 1) {
            return;
        }
        $(self).on('scroll.sbClientListPanel', null, null, (Blade.del(this, function(e) {
            var w = $(self);
            var d = $(document);
            if ((w.scrollTop() >= d.height() - w.height() - this.list.footerHeight) || (w.scrollTop() >= d.height() / 2)) {
                this.loadMore();
            }
        })));
    };
    p.disableLoadMoreOnScroll = function () {
        $(self).off('scroll.sbClientListPanel');
    };
    return BaseListController;
})();

DigitalBeacon.SiteBase.Mobile.Contacts.ContactDetailsController = (function() {
    Blade.derive(ContactDetailsController, DigitalBeacon.SiteBase.Mobile.BaseDetailsController);
    var $base = DigitalBeacon.SiteBase.Mobile.BaseDetailsController.prototype;
    function ContactDetailsController(scope, state, location, contactService) {
        $base.constructor.call(this);
        this.set_Scope(scope);
        this.set_RouterState(state);
        this.set_Location(location);
        this._contactService = contactService;
    }
    var p = ContactDetailsController.prototype;
    p._contactService = null;
    p.contact = null;
    p.photoUrl = null;
    p.init = function () {
        $base.init.call(this);
        if (this.get_RouterParams().id) {
            this.load(this.get_RouterParams().id);
        }
    };
    p.load = function (id) {
        this.model = this._contactService.get({
            id: id
        }, Blade.del(this, function(response) {
            this.photoUrl = $.digitalbeacon.resolveUrl('~/contacts/{0}/photo?x={1}'.formatWith(response.Id, response.PhotoId))}));
    };
    p.submit = function (isValid) {
        this.model.submitted = true;
        if (!isValid) {
            scrollTo(0, 0);
            return;
        }
        if (this.data.fileInput) {
            this._contactService.save(this.model.Id, this.model, this.data.fileInput.files, Blade.del(this, function(response) {
                DigitalBeacon.SiteBase.ApiResponseHelper.handleResponse(response, this.get_Scope());
                this.load(this.model.Id);
            }));
        } else {
            this._contactService.save(this.model.Id, this.model, this.get_ReturnToList());
        }
    };
    p.delete = function () {
        if (this.model.Id && confirm($.sb.localization.confirmText)) {
            this._contactService.delete({
                id: this.model.Id
            }, this.get_ReturnToList());
        }
    };
    p.deletePhoto = function () {
        if (this.model.Id && confirm($.sb.localization.confirmText)) {
            this._contactService.deletePhoto(this.model.Id, this.get_ResponseHandler());
        }
    };
    p.rotatePhotoCounterclockwise = function () {
        if (this.model.Id && confirm($.sb.localization.confirmText)) {
            this._contactService.rotatePhotoCounterclockwise(this.model.Id, this.get_ResponseHandler());
        }
    };
    p.rotatePhotoClockwise = function () {
        if (this.model.Id && confirm($.sb.localization.confirmText)) {
            this._contactService.rotatePhotoClockwise(this.model.Id, this.get_ResponseHandler());
        }
    };
    p.handleResponse = function (response) {
        DigitalBeacon.SiteBase.ApiResponseHelper.handleResponse(response, this.get_Scope());
        if (response.Success) {
            this.load(this.model.Id);
        }
    };
    return ContactDetailsController;
})();

DigitalBeacon.SiteBase.Mobile.Contacts.ContactListController = (function() {
    Blade.derive(ContactListController, DigitalBeacon.SiteBase.Mobile.BaseListController);
    var $base = DigitalBeacon.SiteBase.Mobile.BaseListController.prototype;
    function ContactListController(scope, state, location, contactService) {
        $base.constructor.call(this);
        this.set_Scope(scope);
        this.set_RouterState(state);
        this.set_Location(location);
        this._contactService = contactService;
    }
    var p = ContactListController.prototype;
    p._contactService = null;
    p.CommentTypeId = '';
    p.BirthMonth = '';
    p.Inactive = '';
    p.contacts = null;
    p.init = function () {
        $base.init.call(this);
        this.list.sortTextOptions = [new Option('Last Name', 'LastName')];
        this.list.sortText = this.list.sortTextOptions[0].value;
        this.list.sortDirection = this.list.sortDirectionOptions[1].value;
        this.search();
    };
    p.search = function (requestMore) {
        requestMore = (requestMore !== undefined) ? requestMore : false;
        if (!requestMore) {
            this.list.page = 1;
        }
        this.list.isFiltered = this.list.searchText && this.list.searchText.hasText();
        this._contactService.search({
            PageSize: this.list.pageSize,
            Page: this.list.page,
            SearchText: this.list.searchText,
            SortValue: this.getSortValue(),
            CommentTypeId: this.CommentTypeId,
            BirthMonth: this.BirthMonth,
            Inactive: this.Inactive
        }, (Blade.del(this, function(x) {
            this.handleResponse(x, requestMore)})));
    };
    p.handleResponse = function (response, isRequestForMore) {
        if (isRequestForMore) {
            var c = null;
            var $c_enum = response.Data.GetEnumerator();
            while($c_enum.MoveNext()) {
                c = $c_enum.get_Current();
                this.contacts.push(c);
            }
        } else {
            this.contacts = response.Data;
            this.list.pageCount = Math.ceil(response.Total / this.list.pageSize);
            this.enableLoadMoreOnScroll();
        }
    };
    return ContactListController;
})();

DigitalBeacon.SiteBase.Mobile.Contacts.ContactService = (function() {
    Blade.derive(ContactService, DigitalBeacon.SiteBase.Mobile.BaseEntityService);
    var $base = DigitalBeacon.SiteBase.Mobile.BaseEntityService.prototype;
    function ContactService(http, resource) {
        $base.constructor.call(this);
        this._http = http;
        this.set_Resource(resource(DigitalBeacon.SiteBase.ControllerHelper.getJsonUrl('~/contacts/:id/:action'), {
            id: '@id'
        }, {
            update: {
                method: 'PUT'
            },
            search: {
                method: 'POST',
                params: {
                    action: 'search'
                }
            },
            deletePhoto: {
                method: 'POST',
                params: {
                    action: 'deletePhoto'
                }
            },
            rotatePhotoCounterclockwise: {
                method: 'POST',
                params: {
                    action: 'rotatePhotoCounterclockwise'
                }
            },
            rotatePhotoClockwise: {
                method: 'POST',
                params: {
                    action: 'rotatePhotoClockwise'
                }
            }
        }));
    }
    var p = ContactService.prototype;
    p._http = null;
    p.save = function (id, model, files, responseHandler) {
        responseHandler = (responseHandler !== undefined) ? responseHandler : null;
        this.sendFormData(this._http, 'contacts', id, model, files, responseHandler);
    };
    p.deletePhoto = function (id, responseHandler) {
        responseHandler = (responseHandler !== undefined) ? responseHandler : null;
        this.get_Resource().deletePhoto({
            id: id
        }, responseHandler);
    };
    p.rotatePhotoCounterclockwise = function (id, responseHandler) {
        responseHandler = (responseHandler !== undefined) ? responseHandler : null;
        this.get_Resource().rotatePhotoCounterclockwise({
            id: id
        }, responseHandler);
    };
    p.rotatePhotoClockwise = function (id, responseHandler) {
        responseHandler = (responseHandler !== undefined) ? responseHandler : null;
        this.get_Resource().rotatePhotoClockwise({
            id: id
        }, responseHandler);
    };
    Blade.impl(ContactService, 'DigitalBeacon.SiteBase.Mobile.IEntityService');
    return ContactService;
})();

DigitalBeacon.SiteBase.Mobile.Identity.SignInController = (function() {
    Blade.derive(SignInController, DigitalBeacon.SiteBase.Mobile.BaseController);
    var $base = DigitalBeacon.SiteBase.Mobile.BaseController.prototype;
    function SignInController(scope, identityService) {
        $base.constructor.call(this);
        this.set_Scope(scope);
        this._identityService = identityService;
    }
    var p = SignInController.prototype;
    p._identityService = null;
    p.init = function () {
        $base.init.call(this);
    };
    p.submit = function (isValid) {
        this.model.submitted = true;
        if (!isValid) {
            return;
        }
        this._identityService.signIn(this.model, (Blade.del(this, function(response) {
            DigitalBeacon.SiteBase.ApiResponseHelper.handleResponse(response, this.get_Scope())})));
    };
    return SignInController;
})();

angular.module('contacts', ['sitebase', 'ui.router', 'contactService']).config(['$stateProvider', '$urlRouterProvider', '$locationProvider', (function(stateProvider, urlRouterProvider, locationProvider) {
    locationProvider.html5Mode(true);
    urlRouterProvider.otherwise($.digitalbeacon.resolveUrl('~/contacts'));
    stateProvider.state('list', {
        url: $.digitalbeacon.resolveUrl('~/contacts'),
        templateUrl: DigitalBeacon.SiteBase.ControllerHelper.getTemplateUrl('~/contacts'),
        controller: 'contactListController'
    }).state('list.new', {
        url: '/new',
        templateUrl: DigitalBeacon.SiteBase.ControllerHelper.getTemplateUrl('~/contacts/new'),
        controller: 'contactDetailsController'
    }).state('list.edit', {
        url: '/{id:[0-9]{1,4}}',
        templateUrl: DigitalBeacon.SiteBase.ControllerHelper.getTemplateUrl('~/contacts/0/edit'),
        controller: 'contactDetailsController'
    });
    $.digitalbeacon.loadCssFile('~/resources/base/contacts/styles.css');
})]).controller('contactListController', ['$scope', '$state', '$location', 'contactService', (function(scope, state, location, contactService) {
    DigitalBeacon.SiteBase.Mobile.BaseController.extend(scope, new DigitalBeacon.SiteBase.Mobile.Contacts.ContactListController(scope, state, location, contactService))})]).controller('contactDetailsController', ['$scope', '$state', '$location', 'contactService', (function(scope, state, location, contactService) {
    DigitalBeacon.SiteBase.Mobile.BaseController.extend(scope, new DigitalBeacon.SiteBase.Mobile.Contacts.ContactDetailsController(scope, state, location, contactService))})]);
angular.module('identity', ['sitebase', 'identityService']).controller('signInController', ['$scope', 'identityService', (function(scope, identityService) {
    DigitalBeacon.SiteBase.Mobile.BaseController.extend(scope, new DigitalBeacon.SiteBase.Mobile.Identity.SignInController(scope, identityService))})]);
angular.module('identityService', ['ngResource']).factory('identityService', ['$resource', (function(resource) {
    return resource(DigitalBeacon.SiteBase.ControllerHelper.getJsonUrl('~/identity/:operation'), {
    }, {
        signIn: {
            method: 'POST',
            params: {
                operation: 'signIn'
            }
        }
    });
})]);
angular.module('sitebase', ['ngSanitize', 'ui.bootstrap', 'ui.mask']).config(['$httpProvider', (function(httpProvider) {
    httpProvider.defaults.transformRequest.push(function(data) {
        $.sb.onAjaxStart();
        return data;
    });
    httpProvider.defaults.transformResponse.push(function(data) {
        $.sb.onAjaxEnd();
        return DigitalBeacon.Utils.convertDateStringsToDates(data);
    });
})]);
angular.module('contactService', ['ngResource']).factory('contactService', ['$http', '$resource', (function(http, resource) {
    return new DigitalBeacon.SiteBase.Mobile.Contacts.ContactService(http, resource);
})]);
