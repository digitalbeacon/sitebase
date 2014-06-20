
if(typeof DigitalBeacon == 'undefined') DigitalBeacon = {};
if(!DigitalBeacon.SiteBase) DigitalBeacon.SiteBase = {};
if(!DigitalBeacon.SiteBase.Mobile) DigitalBeacon.SiteBase.Mobile = {};
if(!DigitalBeacon.SiteBase.Mobile.Account) DigitalBeacon.SiteBase.Mobile.Account = {};

DigitalBeacon.SiteBase.Mobile.Account.AccountModule = (function() {
    function AccountModule() {
    }
    return AccountModule;
})();

DigitalBeacon.SiteBase.Mobile.Account.AccountService = (function() {
    function AccountService() {
    }
    return AccountService;
})();

DigitalBeacon.SiteBase.Mobile.BaseController = (function() {
    function BaseController() {
    }
    var p = BaseController.prototype;
    p._scope = null;
    p._routerState = null;
    p._location = null;
    p.data = null;
    p.get_ScopeData = function() {
        return this.data;
    };
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
        return this._routerState ? this._routerState.params : {
        };
    };
    p.get_Location = function() {
        return this._location;
    };
    p.set_Location = function(value) {
        this._location = value;
    };
    p.get_DefaultHandler = function() {
        return Blade.del(this, function(response) {
            DigitalBeacon.SiteBase.ControllerHelper.handleResponse(response, this.get_Scope())});
    };
    p.get_ResponseHandler = function() {
        return Blade.del(this, function(response) {
            this.handleResponse(response)});
    };
    p.get_HasFileInput = function() {
        return this.get_ScopeData().fileInput;
    };
    p.get_Files = function() {
        return this.get_ScopeData().fileInput.files;
    };
    p.init = function () {
        this.data = {
            model: {
            }
        };
    };
    p.submitForm = function (modelName, isValid) {
        this.get_ScopeData()[modelName].submitted = true;
        if (!isValid) {
            scrollTo(0, 0);
            return;
        }
        this.submit(modelName);
    };
    p.resetForm = function (formName, modelName) {
        modelName = (modelName !== undefined) ? modelName : 'model';
        (this.get_Scope()[formName]).$setPristine();
        if (modelName) {
            this.get_ScopeData()[modelName].submitted = false;
        }
    };
    p.submit = function (modelName) {
    };
    p.hasAlert = function (key) {
        if (!this.get_ScopeData().alerts || this.get_ScopeData().alerts.length === 0) {
            return false;
        }
        for(var i = 0; i < this.get_ScopeData().alerts.length; i++) {
            if (this.get_ScopeData().alerts[i].key === key) {
                return true;
            }
        }
        return false;
    };
    p.clearAlerts = function (key) {
        key = (key !== undefined) ? key : null;
        if (this.get_ScopeData().alerts && this.get_ScopeData().alerts.length > 0) {
            if (key) {
                for(var i = this.get_ScopeData().alerts.length - 1; i >= 0; i--) {
                    if (this.get_ScopeData().alerts[i].key === key) {
                        this.closeAlert(i);
                    }
                }
            } else {
                this.get_ScopeData().alerts.length = 0;
            }
        }
        if (!this.get_ScopeData().alerts) {
            this.get_ScopeData().alerts = new Array(0);
        }
    };
    p.closeAlert = function (index) {
        this.get_ScopeData().alerts.splice(index, 1);
    };
    p.setAlert = function (msg, isError) {
        isError = (isError !== undefined) ? isError : true;
        this.clearAlerts();
        this.get_ScopeData().alerts.push({
            msg: msg,
            type: isError ? 'danger' : 'success'
        });
    };
    p.toggle = function (evt, dataKey) {
        evt.preventDefault();
        evt.stopPropagation();
        this.get_ScopeData()[dataKey] = this.get_ScopeData()[dataKey] ? false : true;
    };
    p.fileChanged = function (fileInput) {
        if (!fileInput) {
            return;
        }
        var files = fileInput.files;
        if (fileInput.files && fileInput.files.length) {
            this.get_ScopeData().fileInput = fileInput;
        } else {
            this.get_ScopeData().fileInput = null;
        }
        (this.get_Scope()[fileInput.form.name]).$setDirty();
        this.get_Scope().$apply();
    };
    p.handleResponse = function (response) {
        DigitalBeacon.SiteBase.ControllerHelper.handleResponse(response, this.get_Scope());
    };
    p.getStateData = function (stateName) {
        var state = this.get_RouterState().get(stateName);
        if (state) {
            if (!DigitalBeacon.Utils.isDefined(state.data)) {
                state.data = {
                };
            }
            return state.data;
        }
        return null;
    };
    p.go = function (stateName, args) {
        args = (args !== undefined) ? args : null;
        this.get_RouterState().go(stateName, args);
    };
    p.createHandler = function (handler) {
        return Blade.del(this, function(response) {
            if (response.Success) {
                handler(response);
            } else {
                DigitalBeacon.SiteBase.ControllerHelper.handleResponse(response, this.get_Scope());
            }
        });
    };
    return BaseController;
})();
DigitalBeacon.SiteBase.Mobile.BaseController.initScope = function (target, obj) {
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
    p.get = function (id, responseHandler) {
        responseHandler = (responseHandler !== undefined) ? responseHandler : null;
        return this.get_Resource().get({
            id: id
        }, responseHandler);
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
    p.$delete = function (id, responseHandler) {
        responseHandler = (responseHandler !== undefined) ? responseHandler : null;
        this.get_Resource().$delete({
            id: id
        }, responseHandler);
    };
    p.sendFormData = function (http, entityTarget, id, model, files, responseHandler) {
        responseHandler = (responseHandler !== undefined) ? responseHandler : null;
        var headers = {};
        headers['Content-Type'] = undefined;
        http({
            method: id ? 'PUT' : 'POST',
            url: $.digitalbeacon.resolveUrl('~/' + entityTarget + (id ? ('/' + id) : '') + '/json'),
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
        if (k.slice(0, 1) === '$') {
            continue;
        }
        if (model[k] || $.digitalbeacon.isOfType(model[k], 'boolean')) {
            if (angular.isDate(model[k])) {
                data.append(k, JSON.stringify(model[k]).replace('\"', '').replace('\"', ''));
            } else {
                data.append(k, model[k]);
            }
        }
    }
    if (files) {
        for(var i = 0; i < files.length; i++) {
            data.append('file' + i, files[i]);
        }
    }
    return data;
};
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

DigitalBeacon.SiteBase.Mobile.Account.ChangePasswordController = (function() {
    Blade.derive(ChangePasswordController, DigitalBeacon.SiteBase.Mobile.BaseController);
    var $base = DigitalBeacon.SiteBase.Mobile.BaseController.prototype;
    function ChangePasswordController(scope, accountService) {
        $base.constructor.call(this);
        this.set_Scope(scope);
        this._accountService = accountService;
    }
    var p = ChangePasswordController.prototype;
    p._accountService = null;
    p.submit = function (modelName) {
        if (this.get_ScopeData().model.NewPasswordConfirm !== this.get_ScopeData().model.NewPassword) {
            this.setAlert($.sb.localization.passwordConfirmNotMatched);
            return;
        }
        if (this.get_ScopeData().model.NewPassword === this.get_ScopeData().model.Username) {
            this.setAlert($.sb.localization.passwordInvalid);
            return;
        }
        this._accountService.changePassword(this.get_ScopeData().model, this.get_DefaultHandler());
    };
    return ChangePasswordController;
})();

DigitalBeacon.SiteBase.Mobile.Account.ChangeSecurityQuestionController = (function() {
    Blade.derive(ChangeSecurityQuestionController, DigitalBeacon.SiteBase.Mobile.BaseController);
    var $base = DigitalBeacon.SiteBase.Mobile.BaseController.prototype;
    function ChangeSecurityQuestionController(scope, accountService) {
        $base.constructor.call(this);
        this.set_Scope(scope);
        this._accountService = accountService;
    }
    var p = ChangeSecurityQuestionController.prototype;
    p._accountService = null;
    p.submit = function (modelName) {
        this._accountService.changeSecurityQuestion(this.get_ScopeData().model, this.get_DefaultHandler());
    };
    return ChangeSecurityQuestionController;
})();

DigitalBeacon.SiteBase.Mobile.Account.UpdateProfileController = (function() {
    Blade.derive(UpdateProfileController, DigitalBeacon.SiteBase.Mobile.BaseController);
    var $base = DigitalBeacon.SiteBase.Mobile.BaseController.prototype;
    function UpdateProfileController(scope, accountService) {
        $base.constructor.call(this);
        this.set_Scope(scope);
        this._accountService = accountService;
    }
    var p = UpdateProfileController.prototype;
    p._accountService = null;
    p.init = function () {
        $base.init.call(this);
        this._accountService.getProfile(null, this.createHandler(Blade.del(this, function(response) {
            var defaultLanguage = this.get_ScopeData().model.Language;
            response.Data.Language = response.Data.Language || defaultLanguage;
            var defaultCountry = this.get_ScopeData().model.Country;
            response.Data.Country = response.Data.Country || defaultCountry;
            response.Data.State = response.Data.State || '';
            this.get_ScopeData().model = response.Data;
        })));
    };
    p.submit = function (modelName) {
        this._accountService.updateProfile(this.get_ScopeData().model, this.get_DefaultHandler());
    };
    return UpdateProfileController;
})();

DigitalBeacon.SiteBase.Mobile.BaseDetailsController = (function() {
    Blade.derive(BaseDetailsController, DigitalBeacon.SiteBase.Mobile.BaseController);
    var $base = DigitalBeacon.SiteBase.Mobile.BaseController.prototype;
    function BaseDetailsController(scope, state, location) {
        $base.constructor.call(this);
        this.set_Scope(scope);
        this.set_RouterState(state);
        this.set_Location(location);
    }
    var p = BaseDetailsController.prototype;
    p.get_ReturnToList = function() {
        return Blade.del(this, function(response) {
            if (response.Success) {
                this.hide(response);
            } else {
                DigitalBeacon.SiteBase.ControllerHelper.handleResponse(response, this.get_Scope());
            }
        });
    };
    p.get_SaveHandler = function() {
        return Blade.del(this, function(response) {
            if (response.Success) {
                this.detailsChanged();
                this.get_ScopeData().fileInput = null;
                this.getStateData('list.display').alerts = DigitalBeacon.SiteBase.ControllerHelper.getAlerts(response);
                this.get_RouterState().go('list.display', {
                    id: this.get_ScopeData().model.Id || response.Data
                });
            } else {
                DigitalBeacon.SiteBase.ControllerHelper.handleResponse(response, this.get_Scope());
            }
        });
    };
    p.init = function () {
        $base.init.call(this);
        if (this.get_RouterParams().id) {
            this.load(this.get_RouterParams().id);
        }
        if (this.get_RouterState().current.data && this.get_RouterState().current.data.alerts) {
            this.get_ScopeData().alerts = this.get_RouterState().current.data.alerts;
            this.get_RouterState().current.data.alerts = null;
        }
        this.show();
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
    p.hide = function (response) {
        response = (response !== undefined) ? response : null;
        this.get_Scope().$emit('hideDetails', response);
    };
    p.show = function () {
        this.get_Scope().$emit('showDetails');
    };
    p.detailsChanged = function () {
        this.get_Scope().$emit('detailsChanged');
    };
    p.load = function (id) {
    };
    p.save = function () {
    };
    p.$delete = function () {
    };
    p.cancel = function () {
        if (this.get_RouterState().is('list.edit')) {
            this.get_RouterState().go('list.display', {
                id: this.get_ScopeData().model.Id
            });
        } else {
            this.hide();
        }
    };
    p.submit = function (modelName) {
        this.save();
    };
    p.handleResponse = function (response) {
        DigitalBeacon.SiteBase.ControllerHelper.handleResponse(response, this.get_Scope());
        if (response.Success) {
            this.load(this.get_ScopeData().model.Id);
        }
    };
    return BaseDetailsController;
})();

DigitalBeacon.SiteBase.Mobile.BaseListController = (function() {
    Blade.derive(BaseListController, DigitalBeacon.SiteBase.Mobile.BaseController);
    var $base = DigitalBeacon.SiteBase.Mobile.BaseController.prototype;
    function BaseListController() {
        $base.constructor.call(this);
    }
    var p = BaseListController.prototype;
    p.get_ScopeData = function() {
        return this.data;
    };
    p.init = function () {
        $base.init.call(this);
        $.extend(this.data, {
            sortDirectionOptions: [new Option('Ascending', ''), new Option('Descending', '-DESC')],
            page: 1,
            pageSize: 10,
            pageCount: -1,
            footerHeight: 140,
            listVisible: true
        });
        this.get_ScopeData().listVisible = this.isListState();
        this.get_Scope().$on('hideDetails', Blade.del(this, function(evt, args) {
            this.showList(args);
        }));
        this.get_Scope().$on('showDetails', Blade.del(this, function() {
            this.hideList();
        }));
        this.get_Scope().$on('detailsChanged', Blade.del(this, function() {
            this.getStateData('list').refresh = true;
        }));
        this.get_Scope().$on('alerts', Blade.del(this, function(evt, args) {
            this.get_ScopeData().alerts = args;
        }));
        this.get_Scope().$watch(Blade.del(this, function() {
            return this.get_Location().url();
        }), Blade.del(this, function(url) {
            if (!this.get_ScopeData().listVisible && url && this.isListState()) {
                this.showList();
            }
        }));
    };
    p.search = function (requestMore) {
        requestMore = (requestMore !== undefined) ? requestMore : false;
    };
    p.loadMore = function () {
        if (this.get_ScopeData().page < this.get_ScopeData().pageCount) {
            this.get_ScopeData().page++;
            this.search(true);
        }
    };
    p.addNew = function () {
        this.clearAlerts();
        this.get_RouterState().go('list.new');
        this.get_ScopeData().model = {
        };
    };
    p.showDetails = function (id) {
        this.clearAlerts();
        this.get_RouterState().go('list.display', {
            id: id
        });
    };
    p.hideList = function () {
        this.get_ScopeData().listVisible = false;
        this.disableLoadMoreOnScroll();
    };
    p.showList = function (response) {
        response = (response !== undefined) ? response : null;
        this.get_ScopeData().listVisible = true;
        this.get_RouterState().go('list');
        if (this.get_ScopeData().pageCount < 0 || this.getStateData('list').refresh) {
            this.getStateData('list').refresh = false;
            this.search();
        }
        this.enableLoadMoreOnScroll();
        if (response) {
            DigitalBeacon.SiteBase.ControllerHelper.handleResponse(response, this.get_Scope());
        }
    };
    p.isListState = function () {
        return this.get_RouterState().current.name === 'list';
    };
    p.clearSearchText = function () {
        this.get_ScopeData().searchText = '';
        if (this.get_ScopeData().isFiltered) {
            this.search();
        }
        this.get_ScopeData().isFiltered = false;
        this.get_ScopeData().page = 1;
    };
    p.getSortValue = function () {
        return this.get_ScopeData().sortText + this.get_ScopeData().sortDirection;
    };
    p.enableLoadMoreOnScroll = function () {
        if (this.get_ScopeData().pageCount <= 1 || !this.get_ScopeData().listVisible) {
            return;
        }
        scrollTo(0, 0);
        $(self).on('scroll.sbClientListPanel', null, null, (Blade.del(this, function(e) {
            var w = $(self);
            var d = $(document);
            if (d.height() > w.height() && ((w.scrollTop() >= d.height() - w.height() - this.get_ScopeData().footerHeight) || (w.scrollTop() >= d.height() / 2))) {
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
        $base.constructor.call(this, scope, state, location);
        this._contactService = contactService;
    }
    var p = ContactDetailsController.prototype;
    p._contactService = null;
    p.get_ScopeData = function() {
        return this.data;
    };
    p.init = function () {
        $base.init.call(this);
        $.extend(this.data, {
            comment: {
            },
            isCollapsedCommentEditor: true
        });
    };
    p.handleResponse = function (response) {
        this.get_ScopeData().isOpenPhotoActions = false;
        if (response.Success) {
            this.detailsChanged();
        }
        $base.handleResponse.call(this, response);
    };
    p.load = function (id) {
        this.get_ScopeData().model = this._contactService.get(id, this.createHandler(Blade.del(this, function(response) {
            if (response.Id) {
                this.get_ScopeData().photoUrl = $.digitalbeacon.resolveUrl('~/contacts/{0}/photo?x={1}'.formatWith(response.Id, response.PhotoId));
            }
        })));
        this.getComments(id);
    };
    p.submit = function (modelName) {
        if (modelName === 'comment') {
            this.saveComment();
        } else {
            this.save();
        }
    };
    p.save = function () {
        if (this.get_HasFileInput()) {
            this._contactService.saveWithFileData(this.get_ScopeData().model.Id, this.get_ScopeData().model, this.get_Files(), this.get_SaveHandler());
        } else {
            this._contactService.save(this.get_ScopeData().model.Id, this.get_ScopeData().model, this.get_SaveHandler());
        }
    };
    p.$delete = function () {
        if (this.get_ScopeData().model.Id && confirm($.sb.localization.confirmText)) {
            this.detailsChanged();
            this._contactService.$delete(this.get_ScopeData().model.Id, this.get_ReturnToList());
        }
    };
    p.deletePhoto = function () {
        if (this.get_ScopeData().model.Id && confirm($.sb.localization.confirmText)) {
            this._contactService.deletePhoto(this.get_ScopeData().model.Id, this.get_ResponseHandler());
        }
    };
    p.rotatePhotoCounterclockwise = function () {
        if (this.get_ScopeData().model.Id && confirm($.sb.localization.confirmText)) {
            this._contactService.rotatePhotoCounterclockwise(this.get_ScopeData().model.Id, this.get_ResponseHandler());
        }
    };
    p.rotatePhotoClockwise = function () {
        if (this.get_ScopeData().model.Id && confirm($.sb.localization.confirmText)) {
            this._contactService.rotatePhotoClockwise(this.get_ScopeData().model.Id, this.get_ResponseHandler());
        }
    };
    p.getComments = function (contactId, setChanged) {
        setChanged = (setChanged !== undefined) ? setChanged : false;
        if (this.get_RouterState().is('list.display')) {
            this._contactService.getComments(contactId, this.createHandler(Blade.del(this, function(response) {
                this.get_ScopeData().comments = response.Data.Items;
                setTimeout(Blade.del(this, function() {
                    this.get_ScopeData().isCollapsedCommentEditor = true;
                    this.get_Scope().$apply();
                }), 0);
            })));
        }
        if (setChanged) {
            this.detailsChanged();
        }
    };
    p.newComment = function () {
        this.get_ScopeData().comment = {
        };
        this.resetForm('commentEditPanel', 'comment');
        this.get_ScopeData().isCollapsedCommentEditor = false;
    };
    p.editComment = function (commentId) {
        this.get_ScopeData().comment = this._contactService.getComment(commentId, Blade.del(this, function(response) {
            this.resetForm('commentEditPanel', 'comment');
            this.get_ScopeData().isCollapsedCommentEditor = false;
        }));
    };
    p.saveComment = function () {
        this.get_ScopeData().comment.ParentId = this.get_ScopeData().model.Id;
        this._contactService.saveComment(this.get_ScopeData().comment.Id, this.get_ScopeData().comment, this.createHandler(Blade.del(this, function(response) {
            this.getComments(this.get_ScopeData().model.Id, true)})));
    };
    p.cancelComment = function () {
        this.get_ScopeData().isCollapsedCommentEditor = true;
    };
    p.deleteComment = function (commentId) {
        if (this.get_ScopeData().model.Id && commentId && confirm($.sb.localization.confirmText)) {
            this.detailsChanged();
            this._contactService.deleteComment(commentId, this.createHandler(Blade.del(this, function(response) {
                this.getComments(this.get_ScopeData().model.Id)})));
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
    p.get_ScopeData = function() {
        return this.data;
    };
    p.init = function () {
        $base.init.call(this);
        this.get_ScopeData().sortTextOptions = [new Option('Last Name', 'LastName')];
        this.get_ScopeData().sortText = this.get_ScopeData().sortTextOptions[0].value;
        this.get_ScopeData().sortDirection = this.get_ScopeData().sortDirectionOptions[1].value;
        if (this.isListState()) {
            this.search();
        }
    };
    p.search = function (requestMore) {
        requestMore = (requestMore !== undefined) ? requestMore : false;
        if (!requestMore) {
            this.get_ScopeData().page = 1;
        }
        this.get_ScopeData().isFiltered = this.get_ScopeData().searchText && this.get_ScopeData().searchText.hasText();
        this._contactService.search({
            PageSize: this.get_ScopeData().pageSize,
            Page: this.get_ScopeData().page,
            SearchText: this.get_ScopeData().searchText,
            SortValue: this.getSortValue(),
            CommentTypeId: this.get_ScopeData().CommentTypeId,
            BirthMonth: this.get_ScopeData().BirthMonth,
            Inactive: this.get_ScopeData().Inactive
        }, (Blade.del(this, function(x) {
            this.handleResponse(x, requestMore)})));
    };
    p.handleResponse = function (response, isRequestForMore) {
        var c = null;
        var $c_enum = response.Data.GetEnumerator();
        while($c_enum.MoveNext()) {
            c = $c_enum.get_Current();
            if (c.PhotoId) {
                c.photoUrl = $.digitalbeacon.resolveUrl('~/contacts/{0}/thumbnail?x={1}'.formatWith(c.Id, c.PhotoId));
            }
        }
        if (isRequestForMore) {
            var c = null;
            var $c_enum = response.Data.GetEnumerator();
            while($c_enum.MoveNext()) {
                c = $c_enum.get_Current();
                this.get_ScopeData().contacts.push(c);
            }
        } else {
            this.get_ScopeData().contacts = response.Data;
            this.get_ScopeData().pageCount = Math.ceil(response.Total / this.get_ScopeData().pageSize);
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
        this.set_Resource(resource($.digitalbeacon.resolveUrl('~/contacts/:id/:action/json'), {
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
            },
            comments: {
                method: 'GET',
                params: {
                    action: 'comments'
                }
            }
        }));
        this._commentsResource = resource($.digitalbeacon.resolveUrl('~/contactComments/:id/:action/json'), {
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
            }
        });
    }
    var p = ContactService.prototype;
    p._http = null;
    p._commentsResource = null;
    p.saveWithFileData = function (id, model, files, responseHandler) {
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
    p.getComments = function (contactId, responseHandler) {
        responseHandler = (responseHandler !== undefined) ? responseHandler : null;
        this.get_Resource().comments({
            id: contactId
        }, responseHandler);
    };
    p.getComment = function (id, responseHandler) {
        responseHandler = (responseHandler !== undefined) ? responseHandler : null;
        return this._commentsResource.get({
            id: id
        }, responseHandler);
    };
    p.deleteComment = function (commentId, responseHandler) {
        responseHandler = (responseHandler !== undefined) ? responseHandler : null;
        this._commentsResource.$delete({
            id: commentId
        }, responseHandler);
    };
    p.saveComment = function (id, comment, responseHandler) {
        responseHandler = (responseHandler !== undefined) ? responseHandler : null;
        if (id) {
            this._commentsResource.update({
                id: id
            }, comment, responseHandler);
        } else {
            this._commentsResource.save(comment, responseHandler);
        }
    };
    Blade.impl(ContactService, 'DigitalBeacon.SiteBase.Mobile.IEntityService');
    return ContactService;
})();

DigitalBeacon.SiteBase.Mobile.Identity.RecoverUsernameController = (function() {
    Blade.derive(RecoverUsernameController, DigitalBeacon.SiteBase.Mobile.BaseController);
    var $base = DigitalBeacon.SiteBase.Mobile.BaseController.prototype;
    function RecoverUsernameController(scope, identityService) {
        $base.constructor.call(this);
        this.set_Scope(scope);
        this._identityService = identityService;
    }
    var p = RecoverUsernameController.prototype;
    p._identityService = null;
    p.submit = function (modelName) {
        this._identityService.recoverUsername(this.get_ScopeData().model, this.get_DefaultHandler());
    };
    return RecoverUsernameController;
})();

DigitalBeacon.SiteBase.Mobile.Identity.RegistrationController = (function() {
    Blade.derive(RegistrationController, DigitalBeacon.SiteBase.Mobile.BaseController);
    var $base = DigitalBeacon.SiteBase.Mobile.BaseController.prototype;
    function RegistrationController(scope, identityService) {
        $base.constructor.call(this);
        this.set_Scope(scope);
        this._identityService = identityService;
    }
    var p = RegistrationController.prototype;
    p._identityService = null;
    p.submit = function (modelName) {
        this._identityService.register(this.get_ScopeData().model, this.get_DefaultHandler());
    };
    return RegistrationController;
})();

DigitalBeacon.SiteBase.Mobile.Identity.ResetPasswordController = (function() {
    Blade.derive(ResetPasswordController, DigitalBeacon.SiteBase.Mobile.BaseController);
    var $base = DigitalBeacon.SiteBase.Mobile.BaseController.prototype;
    function ResetPasswordController(scope, identityService) {
        $base.constructor.call(this);
        this.set_Scope(scope);
        this._identityService = identityService;
    }
    var p = ResetPasswordController.prototype;
    p._identityService = null;
    p.submit = function (modelName) {
        if (this.get_ScopeData().model.PasswordConfirm !== this.get_ScopeData().model.Password) {
            this.setAlert($.sb.localization.passwordConfirmNotMatched);
            return;
        }
        if (this.get_ScopeData().model.Password === this.get_ScopeData().model.Username) {
            this.setAlert($.sb.localization.passwordInvalid);
            return;
        }
        this._identityService.resetPassword(this.get_ScopeData().model, this.createHandler(Blade.del(this, function(response) {
            if (response.Data.Step > 1) {
                angular.extend(this.get_ScopeData().model, response.Data);
                this.resetForm('resetPasswordPanel');
                setTimeout(Blade.del(this, function() {
                    $('#SecurityAnswer').focus();
                }), 0);
            }
        })));
    };
    p.back = function () {
        this.get_ScopeData().model.Step = 1;
        (this.get_Scope()['resetPasswordPanel']).$setDirty();
    };
    return ResetPasswordController;
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
    p.submit = function (modelName) {
        this._identityService.signIn(this.get_ScopeData().model, this.get_DefaultHandler());
    };
    return SignInController;
})();

angular.module('account', ['sitebase', 'accountService']).controller('updateProfileController', ['$scope', 'accountService', (function(scope, accountService) {
    DigitalBeacon.SiteBase.Mobile.BaseController.initScope(scope, new DigitalBeacon.SiteBase.Mobile.Account.UpdateProfileController(scope, accountService))})]).controller('changePasswordController', ['$scope', 'accountService', (function(scope, accountService) {
    DigitalBeacon.SiteBase.Mobile.BaseController.initScope(scope, new DigitalBeacon.SiteBase.Mobile.Account.ChangePasswordController(scope, accountService))})]).controller('changeSecurityQuestionController', ['$scope', 'accountService', (function(scope, accountService) {
    DigitalBeacon.SiteBase.Mobile.BaseController.initScope(scope, new DigitalBeacon.SiteBase.Mobile.Account.ChangeSecurityQuestionController(scope, accountService))})]);
angular.module('accountService', ['ngResource']).factory('accountService', ['$resource', (function(resource) {
    return resource($.digitalbeacon.resolveUrl('~/account/:operation/json'), {
    }, {
        getProfile: {
            method: 'GET',
            params: {
                operation: 'updateProfile'
            }
        },
        updateProfile: {
            method: 'POST',
            params: {
                operation: 'updateProfile'
            }
        },
        changePassword: {
            method: 'POST',
            params: {
                operation: 'changePassword'
            }
        },
        changeSecurityQuestion: {
            method: 'POST',
            params: {
                operation: 'changeSecurityQuestion'
            }
        }
    });
})]);
angular.module('contacts', ['sitebase', 'ui.router', 'contactService']).config(['$stateProvider', (function(stateProvider) {
    stateProvider.state('list', {
        url: $.digitalbeacon.resolveUrl('~/contacts'),
        templateUrl: $.digitalbeacon.resolveUrl('~/contacts/template'),
        controller: 'contactListController'
    }).state('list.new', {
        url: '/new',
        templateUrl: $.digitalbeacon.resolveUrl('~/contacts/new/template'),
        controller: 'contactDetailsController'
    }).state('list.display', {
        url: '/{id:[0-9]{1,9}}',
        templateUrl: $.digitalbeacon.resolveUrl('~/contacts/0/template'),
        controller: 'contactDetailsController'
    }).state('list.edit', {
        url: '/{id:[0-9]{1,9}}/edit',
        templateUrl: $.digitalbeacon.resolveUrl('~/contacts/0/edit/template'),
        controller: 'contactDetailsController'
    });
})]).controller('contactListController', ['$scope', '$state', '$location', 'contactService', (function(scope, state, location, contactService) {
    DigitalBeacon.SiteBase.Mobile.BaseController.initScope(scope, new DigitalBeacon.SiteBase.Mobile.Contacts.ContactListController(scope, state, location, contactService))})]).controller('contactDetailsController', ['$scope', '$state', '$location', 'contactService', (function(scope, state, location, contactService) {
    DigitalBeacon.SiteBase.Mobile.BaseController.initScope(scope, new DigitalBeacon.SiteBase.Mobile.Contacts.ContactDetailsController(scope, state, location, contactService))})]).run(['$state', function(state) {
    $.digitalbeacon.loadCssFile('~/resources/base/contacts/styles.css');
    state.transitionTo('list');
}]);
angular.module('identity', ['sitebase', 'identityService']).controller('signInController', ['$scope', 'identityService', (function(scope, identityService) {
    DigitalBeacon.SiteBase.Mobile.BaseController.initScope(scope, new DigitalBeacon.SiteBase.Mobile.Identity.SignInController(scope, identityService))})]).controller('registrationController', ['$scope', 'identityService', (function(scope, identityService) {
    DigitalBeacon.SiteBase.Mobile.BaseController.initScope(scope, new DigitalBeacon.SiteBase.Mobile.Identity.RegistrationController(scope, identityService))})]).controller('recoverUsernameController', ['$scope', 'identityService', (function(scope, identityService) {
    DigitalBeacon.SiteBase.Mobile.BaseController.initScope(scope, new DigitalBeacon.SiteBase.Mobile.Identity.RecoverUsernameController(scope, identityService))})]).controller('resetPasswordController', ['$scope', 'identityService', (function(scope, identityService) {
    DigitalBeacon.SiteBase.Mobile.BaseController.initScope(scope, new DigitalBeacon.SiteBase.Mobile.Identity.ResetPasswordController(scope, identityService))})]);
angular.module('identityService', ['ngResource']).factory('identityService', ['$resource', (function(resource) {
    return resource($.digitalbeacon.resolveUrl('~/identity/:operation/json'), {
    }, {
        signIn: {
            method: 'POST',
            params: {
                operation: 'signIn'
            }
        },
        register: {
            method: 'POST',
            params: {
                operation: 'register'
            }
        },
        recoverUsername: {
            method: 'POST',
            params: {
                operation: 'recoverUsername'
            }
        },
        resetPassword: {
            method: 'POST',
            params: {
                operation: 'resetPassword'
            }
        }
    });
})]);
angular.module('sitebase', ['ngSanitize', 'ui.bootstrap', 'ui.mask']).config(['$httpProvider', '$locationProvider', 'datepickerConfig', (function(httpProvider, locationProvider, datepickerConfig) {
    locationProvider.html5Mode(true);
    httpProvider.defaults.transformRequest.push(function(data) {
        $.sb.onAjaxStart();
        return data;
    });
    httpProvider.defaults.transformResponse.push(function(data) {
        $.sb.onAjaxEnd();
        return DigitalBeacon.Utils.convertDateStringsToDates(data);
    });
    datepickerConfig.showWeeks = false;
})]);
angular.module('contactService', ['ngResource']).factory('contactService', ['$http', '$resource', (function(http, resource) {
    return new DigitalBeacon.SiteBase.Mobile.Contacts.ContactService(http, resource);
})]);
