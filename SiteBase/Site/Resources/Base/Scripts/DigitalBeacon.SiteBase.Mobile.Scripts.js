
BaseController = (function() {
    function BaseController() {
    }
    var p = BaseController.prototype;
    p._scope = null;
    p.alerts = null;
    p.get_Scope = function() {
        return this._scope || this;
    };
    p.set_Scope = function(value) {
        this._scope = value;
    };
    p.closeAlert = function (index) {
        this.alerts.splice(index, 1);
    };
    p.init = function () {
    };
    return BaseController;
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
    p.signIn = function (username, password) {
        this._identityService.signIn({
            Username: username,
            Password: password
        }, (Blade.del(this, function(response) {
            DigitalBeacon.SiteBase.ApiResponseHelper.handleResponse(response, this.get_Scope())})));
    };
    return IdentityController;
})();

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
