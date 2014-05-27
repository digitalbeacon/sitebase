
BaseController = (function() {
    function BaseController() {
        this.formData = {};
    }
    var p = BaseController.prototype;
    p._scope = null;
    p._routeParams = null;
    p._location = null;
    p.alerts = null;
    p.get_Scope = function() {
        return this._scope || this;
    };
    p.set_Scope = function(value) {
        this._scope = value;
    };
    p.get_RouteParams = function() {
        return this._routeParams;
    };
    p.set_RouteParams = function(value) {
        this._routeParams = value;
    };
    p.get_Location = function() {
        return this._location;
    };
    p.set_Location = function(value) {
        this._location = value;
    };
    p.closeAlert = function (index) {
        this.alerts.splice(index, 1);
    };
    p.init = function () {
    };
    return BaseController;
})();

BaseService = (function() {
    function BaseService() {
    }
    return BaseService;
})();

ContactsModule = (function() {
    function ContactsModule() {
    }
    return ContactsModule;
})();
ContactsModule.getContactsController = function () {
    return ['$scope', '$routeParams', '$location', 'contactService', (Blade.del(this, function(scope, routeParams, location, contactService) {
        ContactsModule.extend(scope, new ContactsController(scope, routeParams, location, contactService))}))];
};
ContactsModule.extend = function (target, object1) {
    ($.extend(target, object1)).init();
};

ContentModule = (function() {
    function ContentModule() {
    }
    return ContentModule;
})();
if(typeof DigitalBeacon == 'undefined') DigitalBeacon = {};
if(!DigitalBeacon.SiteBase) DigitalBeacon.SiteBase = {};
if(!DigitalBeacon.SiteBase.Mobile) DigitalBeacon.SiteBase.Mobile = {};

DigitalBeacon.SiteBase.Mobile.ListIem = (function() {
    function ListIem() {
    }
    var p = ListIem.prototype;
    p.Item = null;
    return ListIem;
})();

IdentityModule = (function() {
    function IdentityModule() {
    }
    return IdentityModule;
})();

IdentityService = (function() {
    function IdentityService() {
    }
    return IdentityService;
})();

DigitalBeacon.SiteBase.Mobile.BaseListController = (function() {
    Blade.derive(BaseListController, BaseController);
    var $base = BaseController.prototype;
    function BaseListController() {
        this.SortDirectionOptions = [new Option('Ascending', ''), new Option('Descending', '-DESC')];
        $base.constructor.call(this);
    }
    var p = BaseListController.prototype;
    p.IsFiltered = null;
    p.SearchText = null;
    p.SortText = null;
    p.SortDirection = null;
    p.SortTextOptions = null;
    p.Page = 1;
    p.PageSize = 4;
    p.PageCount = 1;
    p.FooterHeight = 140;
    p.search = function (requestMore) {
        requestMore = (requestMore !== undefined) ? requestMore : false;
    };
    p.loadMore = function () {
        if (this.Page < this.PageCount) {
            this.Page++;
            this.search(true);
        }
    };
    p.addNew = function () {
        this.disableLoadMoreOnScroll();
        this.get_Location().path('/new');
    };
    p.showDetails = function (id) {
        this.disableLoadMoreOnScroll();
        this.get_Location().path('/' + id);
    };
    p.clearSearchText = function () {
        this.SearchText = '';
        if (this.IsFiltered) {
            this.search();
        }
        this.IsFiltered = false;
        this.Page = 1;
    };
    p.getSortValue = function () {
        return this.SortText + this.SortDirection;
    };
    p.enableLoadMoreOnScroll = function () {
        $(self).on('scroll.sbClientListPanel', null, null, (Blade.del(this, function(e) {
            var w = $(self);
            var d = $(document);
            if ((w.scrollTop() >= d.height() - w.height() - this.FooterHeight) || (w.scrollTop() >= d.height() / 2)) {
                this.loadMore();
            }
        })));
    };
    p.disableLoadMoreOnScroll = function () {
        $(self).off('scroll.sbClientListPanel');
    };
    return BaseListController;
})();

ContactsController = (function() {
    Blade.derive(ContactsController, DigitalBeacon.SiteBase.Mobile.BaseListController);
    var $base = DigitalBeacon.SiteBase.Mobile.BaseListController.prototype;
    function ContactsController(scope, routeParams, location, contactService) {
        $base.constructor.call(this);
        this.set_Scope(scope);
        this.set_RouteParams(routeParams);
        this.set_Location(location);
        this._contactService = contactService;
        (scope).$on('$destroy', Blade.del(this, function() {
            console.log('here')}));
    }
    var p = ContactsController.prototype;
    p._contactService = null;
    p.CommentTypeId = '';
    p.BirthMonth = '';
    p.Inactive = '';
    p.contacts = null;
    p.contact = null;
    p.init = function () {
        if (this.get_Location().path() === '/') {
            this.SortTextOptions = [new Option('Last Name', 'LastName')];
            this.SortText = this.SortTextOptions[0].value;
            this.SortDirection = this.SortDirectionOptions[1].value;
            this.search();
        } else if (this.get_RouteParams().id) {
            this.contact = this._contactService.get({
                id: this.get_RouteParams().id
            });
        }
    };
    p.search = function (requestMore) {
        requestMore = (requestMore !== undefined) ? requestMore : false;
        if (!requestMore) {
            this.Page = 1;
        }
        this.IsFiltered = this.SearchText && this.SearchText.hasText();
        this._contactService.search({
            PageSize: this.PageSize,
            Page: this.Page,
            SearchText: this.SearchText,
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
            this.PageCount = Math.ceil(response.Total / this.PageSize);
            if (this.PageCount > 1) {
                this.enableLoadMoreOnScroll();
            }
        }
    };
    p.submit = function () {
        this._contactService.save(this.formData, (Blade.del(this, function(response) {
            if (response.Success) {
                this.get_Location().path('/');
            }
            DigitalBeacon.SiteBase.ApiResponseHelper.handleResponse(response, this.get_Scope());
        })));
    };
    p.cancel = function () {
        this.get_Location().path('/');
    };
    return ContactsController;
})();

ContactService = (function() {
    Blade.derive(ContactService, BaseService);
    var $base = BaseService.prototype;
    function ContactService() {
        $base.constructor.call(this);
    }
    return ContactService;
})();

ContentController = (function() {
    Blade.derive(ContentController, BaseController);
    var $base = BaseController.prototype;
    function ContentController(scope) {
        $base.constructor.call(this);
        this.set_Scope(scope);
    }
    var p = ContentController.prototype;
    p.init = function () {
    };
    return ContentController;
})();

DigitalBeacon.SiteBase.Mobile.BaseDetailsController = (function() {
    Blade.derive(BaseDetailsController, BaseController);
    var $base = BaseController.prototype;
    function BaseDetailsController() {
        $base.constructor.call(this);
    }
    var p = BaseDetailsController.prototype;
    p.id = null;
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
    p.submit = function () {
    };
    return BaseDetailsController;
})();

DetailsController = (function() {
    Blade.derive(DetailsController, DigitalBeacon.SiteBase.Mobile.BaseDetailsController);
    var $base = DigitalBeacon.SiteBase.Mobile.BaseDetailsController.prototype;
    function DetailsController(scope, routeParams, location, contactService) {
        $base.constructor.call(this);
        this.set_Scope(scope);
        this._routeParams = routeParams;
        this._location = location;
        this._contactService = contactService;
    }
    var p = DetailsController.prototype;
    p._contactService = null;
    p._routeParams = null;
    p._location = null;
    p.contact = null;
    p.init = function () {
        this.contact = this._contactService.get({
            id: this._routeParams.id
        });
    };
    p.submit = function () {
        this._contactService.save(this.formData, (Blade.del(this, function(response) {
            if (response.Success) {
                this._location.path('/');
            }
            DigitalBeacon.SiteBase.ApiResponseHelper.handleResponse(response, this.get_Scope());
        })));
    };
    p.cancel = function () {
        this._location.path('/');
    };
    return DetailsController;
})();

IdentityController = (function() {
    Blade.derive(IdentityController, BaseController);
    var $base = BaseController.prototype;
    function IdentityController(scope, identityService) {
        $base.constructor.call(this);
        this.set_Scope(scope);
        this._identityService = identityService;
    }
    var p = IdentityController.prototype;
    p._identityService = null;
    p.init = function () {
    };
    p.signIn = function () {
        this._identityService.signIn(this.formData, (Blade.del(this, function(response) {
            DigitalBeacon.SiteBase.ApiResponseHelper.handleResponse(response, this.get_Scope())})));
    };
    return IdentityController;
})();

angular.module('contacts', ['ngRoute', 'ui.bootstrap', 'contactService']).config(['$routeProvider', '$locationProvider', (function(routeProvider, locationProvider) {
    routeProvider.when('/', {
        templateUrl: DigitalBeacon.SiteBase.ControllerHelper.getTemplateUrl('~/contacts/'),
        controller: ContactsModule.getContactsController()
    }).when('/new', {
        templateUrl: DigitalBeacon.SiteBase.ControllerHelper.getTemplateUrl('~/contacts/new'),
        controller: ContactsModule.getContactsController()
    }).when('/:id', {
        templateUrl: DigitalBeacon.SiteBase.ControllerHelper.getTemplateUrl('~/contacts/edit'),
        controller: ContactsModule.getContactsController()
    }).otherwise({
        redirectTo: '/'
    });
    locationProvider.html5Mode(true);
})]);
angular.module('content', ['ui.bootstrap']).controller('contentController', ['$scope', (function(scope) {
    $.extend(scope, new ContentController(scope)).init()})]);
angular.module('identity', ['ui.bootstrap', 'identityService']).controller('identityController', ['$scope', 'identityService', (function(scope, identityService) {
    $.extend(scope, new IdentityController(scope, identityService)).init()})]);
angular.module('identityService', ['ngResource']).factory('identityService', ['$resource', (function(resource) {
    return resource(DigitalBeacon.SiteBase.ControllerHelper.getApiUrl('~/identity/:operation'), {
    }, {
        signIn: {
            method: 'POST',
            params: {
                operation: 'signIn'
            }
        }
    });
})]);
angular.module('contactService', ['ngResource']).factory('contactService', ['$resource', (function(resource) {
    return resource(DigitalBeacon.SiteBase.ControllerHelper.getApiUrl('~/contacts/:id'), {
    }, {
        update: {
            method: 'PUT'
        },
        search: {
            method: 'POST',
            params: {
                id: 'search'
            }
        }
    });
})]);
