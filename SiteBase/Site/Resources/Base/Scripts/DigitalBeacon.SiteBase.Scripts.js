
if(typeof DigitalBeacon == 'undefined') DigitalBeacon = {};
if(!DigitalBeacon.SiteBase) DigitalBeacon.SiteBase = {};

DigitalBeacon.SiteBase.ApiResponse = (function() {
    function ApiResponse() {
    }
    var p = ApiResponse.prototype;
    p.Success = null;
    p.RedirectUrl = null;
    p.Message = null;
    p.ErrorMessage = null;
    p.ValidationErrors = null;
    return ApiResponse;
})();

DigitalBeacon.SiteBase.ApiResponseHelper = (function() {
    function ApiResponseHelper() {
    }
    return ApiResponseHelper;
})();
DigitalBeacon.SiteBase.ApiResponseHelper.handleResponse = function (obj, scope) {
    scope = (scope !== undefined) ? scope : null;
    var response = obj;
    if (response !== null) {
        if (response.RedirectUrl !== null) {
            location.assign(response.RedirectUrl);
        } else if (scope === null) {
            alert(response.Message || response.ErrorMessage || DigitalBeacon.SiteBase.ApiResponseHelper.toString(response.ValidationErrors));
        } else {
            var alerts = new Array(0);
            if (response.Message !== null) {
                alerts.push({
                    type: 'sucess',
                    msg: response.Message
                });
            }
            if (response.ErrorMessage !== null) {
                alerts.push({
                    type: 'danger',
                    msg: response.ErrorMessage
                });
            }
            var key = null;
            var $key_enum = Object.keys(response.ValidationErrors).GetEnumerator();
            while($key_enum.MoveNext()) {
                key = $key_enum.get_Current();
                var msg = null;
                var $msg_enum = response.ValidationErrors[key].GetEnumerator();
                while($msg_enum.MoveNext()) {
                    msg = $msg_enum.get_Current();
                    alerts.push({
                        type: 'danger',
                        msg: msg
                    });
                }
            }
            scope.alerts = alerts;
            scope.validationErrors = response.ValidationErrors || {
            };
        }
    }
};
DigitalBeacon.SiteBase.ApiResponseHelper.toString = function (validationErrors) {
    var s = '';
    var key = null;
    var $key_enum = Object.keys(validationErrors).GetEnumerator();
    while($key_enum.MoveNext()) {
        key = $key_enum.get_Current();
        s += (validationErrors[key]).join('\n');
        s += '\n';
    }
    return s;
};

DigitalBeacon.SiteBase.ControllerHelper = (function() {
    function ControllerHelper() {
    }
    return ControllerHelper;
})();
DigitalBeacon.SiteBase.ControllerHelper.getTemplateUrl = function (targetUrl) {
    return DigitalBeacon.Utils.mergeParams($.digitalbeacon.resolveUrl(targetUrl), {
        renderType: 'Template'
    });
};
DigitalBeacon.SiteBase.ControllerHelper.getJsonUrl = function (targetUrl) {
    return DigitalBeacon.Utils.mergeParams($.digitalbeacon.resolveUrl(targetUrl), {
        renderType: 'Json'
    });
};
DigitalBeacon.SiteBase.ControllerHelper.getApiUrl = function (targetUrl) {
    return DigitalBeacon.Utils.mergeParams($.digitalbeacon.resolveUrl(targetUrl), {
        renderType: 'Api'
    });
};

DigitalBeacon.SiteBase.Editor = (function() {
    function Editor(element, options) {
        if (options !== null) {
            this._handleSiteRelativeUrls = options.handleSiteRelativeUrls || this._handleSiteRelativeUrls;
            this._defaultToTextBox = options.defaultToTextBox || this._defaultToTextBox;
        }
        this._textArea = $(element);
        this._wrapperElement = this._textArea.parent().get(0);
        this._editorElement = $('table', this._wrapperElement);
        this._useEditorLink = $('.useEditor', this._wrapperElement);
        this._useTextBoxLink = $('.useTextBox', this._wrapperElement);
        this._useEditorLink.click(Blade.del(this, function(e) {
            this.useEditor(e);
        }));
        this._useTextBoxLink.click(Blade.del(this, function(e) {
            this.useTextBox(e);
        }));
        this._textArea.data('sbEditor', this);
        if (!this._defaultToTextBox) {
            this.updateTelerikEditorValue();
        }
    }
    var p = Editor.prototype;
    p._wrapperElement = null;
    p._textArea = null;
    p._editorElement = null;
    p._editor = null;
    p._useEditorLink = null;
    p._useTextBoxLink = null;
    p._handleSiteRelativeUrls = true;
    p._defaultToTextBox = false;
    p.withTelerikEditor = function (action) {
        if (this._editor) {
            action();
            return;
        }
        var attempts = ((this._textArea.data('initAttempts') || 0)) + 1;
        if (attempts >= 10) {
            $.digitalbeacon.log('Could not initialize Editor component for {0}.'.formatWith(this._textArea.attr('id')));
            return;
        }
        var tEditor = this._editorElement.data('tEditor');
        if (!tEditor) {
            this._textArea.data('initAttempts', attempts);
            setTimeout((Blade.del(this, function() {
                this.withTelerikEditor(action)})), 250);
            return;
        }
        this._editor = tEditor;
        action();
    };
    p.useEditor = function (e) {
        this.updateTelerikEditorValue();
        this.toggleComponents(true);
        if (e !== null) {
            e.preventDefault();
        }
    };
    p.useTextBox = function (e) {
        this.updateTextAreaValue();
        this.toggleComponents(false);
        if (e !== null) {
            e.preventDefault();
        }
    };
    p.toggleComponents = function (useEditor) {
        this._editorElement.toggle(useEditor);
        this._useTextBoxLink.toggle(useEditor);
        this._textArea.toggle(!useEditor);
        this._useEditorLink.toggle(!useEditor);
    };
    p.updateTelerikEditorValue = function () {
        this.withTelerikEditor(Blade.del(this, function() {
            var val = this._textArea.val();
            if (this._handleSiteRelativeUrls) {
                val = val.expandSiteRelativeText();
            }
            this._editor.value(val);
        }));
    };
    p.updateTextAreaValue = function () {
        this.withTelerikEditor(Blade.del(this, function() {
            var val = this._editor.value();
            if (this._handleSiteRelativeUrls) {
                val = val.toSiteRelativeText();
            }
            this._textArea.val(val);
        }));
    };
    p.prepareForValidation = function () {
        if (this._editorElement.is(':visible')) {
            this.updateTextAreaValue();
        }
    };
    p.prepareForSubmission = function () {
        if (this._editorElement.is(':visible')) {
            this.updateTextAreaValue();
        }
        this._textArea.val($.digitalbeacon.htmlEncode(this._textArea.val()));
    };
    p.value = function (val) {
        val = (val !== undefined) ? val : null;
        if (val) {
            this._textArea.val(val);
            this.updateTelerikEditorValue();
        } else {
            this.prepareForValidation();
        }
        return this._textArea.val();
    };
    p.encodedValue = function () {
        return $.digitalbeacon.htmlEncode(this.value());
    };
    return Editor;
})();

DigitalBeacon.SiteBase.SessionAuditor = (function() {
    function SessionAuditor() {
    }
    var p = SessionAuditor.prototype;
    p._authSessionUrl = null;
    p._extendSessionUrl = null;
    p._signOutUrl = null;
    p._intervalTimerId = null;
    p._notificationTimerId = null;
    p._signOutTimerId = null;
    p._pollingInterval = null;
    p._notificationTime = null;
    p._displayNotification = true;
    p._notificationHeading = null;
    p._notificationMessage = null;
    p._extendButtonText = null;
    p.get_pollingInterval = function() {
        return this._pollingInterval > 0 ? this._pollingInterval : DigitalBeacon.SiteBase.SessionAuditor.DefaultPollingInterval;
    };
    p.set_pollingInterval = function(value) {
        this._pollingInterval = value;
    };
    p.get_notificationTime = function() {
        return this._notificationTime > 0 ? this._notificationTime : DigitalBeacon.SiteBase.SessionAuditor.DefaultNotificationTime;
    };
    p.set_notificationTime = function(value) {
        this._notificationTime = value;
    };
    p.get_displayNotification = function() {
        return this._displayNotification;
    };
    p.set_displayNotification = function(value) {
        this._displayNotification = value;
    };
    p.get_authSessionUrl = function() {
        return $.digitalbeacon.resolveUrl(this._authSessionUrl || DigitalBeacon.SiteBase.SessionAuditor.DefaultAuthSessionUrl);
    };
    p.set_authSessionUrl = function(value) {
        this._authSessionUrl = value;
    };
    p.get_extendSessionUrl = function() {
        return $.digitalbeacon.resolveUrl(this._extendSessionUrl || DigitalBeacon.SiteBase.SessionAuditor.DefaultExtendSessionUrl);
    };
    p.set_extendSessionUrl = function(value) {
        this._extendSessionUrl = value;
    };
    p.get_signOutUrl = function() {
        return $.digitalbeacon.resolveUrl(this._signOutUrl || DigitalBeacon.SiteBase.SessionAuditor.DefaultSignOutUrl);
    };
    p.set_signOutUrl = function(value) {
        this._signOutUrl = value;
    };
    p.get_notificationHeading = function() {
        return this._notificationHeading || DigitalBeacon.SiteBase.SessionAuditor.DefaultNotificationHeading;
    };
    p.set_notificationHeading = function(value) {
        this._notificationHeading = value;
    };
    p.get_notificationMessage = function() {
        return this._notificationMessage || DigitalBeacon.SiteBase.SessionAuditor.DefaultNotificationMessage;
    };
    p.set_notificationMessage = function(value) {
        this._notificationMessage = value;
    };
    p.get_extendButtonText = function() {
        return this._extendButtonText || DigitalBeacon.SiteBase.SessionAuditor.DefaultExtendButtonText;
    };
    p.set_extendButtonText = function(value) {
        this._extendButtonText = value;
    };
    p.start = function () {
        this.stop();
        this._intervalTimerId = setInterval((Blade.del(this, function() {
            this.checkSession();
        })), this.get_pollingInterval() * 1000);
    };
    p.stop = function () {
        if (this._intervalTimerId > 0) {
            clearInterval(this._intervalTimerId);
            this._intervalTimerId = 0;
        }
        if (this._notificationTimerId > 0) {
            clearTimeout(this._notificationTimerId);
            this._notificationTimerId = 0;
        }
        if (this._signOutTimerId > 0) {
            clearTimeout(this._signOutTimerId);
            this._signOutTimerId = 0;
        }
    };
    p.checkSession = function () {
        $.post(this.get_authSessionUrl(), null, Blade.del(this, function(data, status, xhr) {
            this.checkSessionOnSuccess(data);
        }));
    };
    p.checkSessionOnSuccess = function (data) {
        var secondsRemaining = data;
        $.digitalbeacon.log(secondsRemaining + ' seconds remaining...');
        if (this._displayNotification && this._notificationTimerId <= 0 && secondsRemaining < this.get_notificationTime() + this.get_pollingInterval()) {
            this._notificationTimerId = setTimeout((Blade.del(this, function() {
                this.showNotification();
            })), (secondsRemaining - this.get_notificationTime()) * 1000);
            this._signOutTimerId = setTimeout((Blade.del(this, function() {
                this.signOut();
            })), secondsRemaining * 1000);
            clearInterval(this._intervalTimerId);
            this._intervalTimerId = 0;
        }
    };
    p.showNotification = function () {
        $.post(this.get_authSessionUrl(), null, Blade.del(this, function(data, status, xhr) {
            var secondsRemaining = data;
            if (secondsRemaining < this.get_notificationTime() + this.get_pollingInterval()) {
                $.sb.displayMessage(this.get_notificationMessage().formatWith(this.get_notificationTime(), this.get_notificationTime() / 60), this.get_notificationHeading(), '', this.get_extendButtonText(), Blade.del(this, function() {
                    this.extendSession();
                }));
            } else {
                this.start();
            }
        }));
    };
    p.signOut = function () {
        $.post(this.get_authSessionUrl(), null, Blade.del(this, function(data, status, xhr) {
            var secondsRemaining = data;
            if (secondsRemaining < this.get_pollingInterval()) {
                location.href = this.get_signOutUrl();
            } else {
                this.start();
            }
        }));
    };
    p.extendSession = function () {
        $.post(this.get_extendSessionUrl());
        this.start();
    };
    return SessionAuditor;
})();
DigitalBeacon.SiteBase.SessionAuditor.DefaultPollingInterval = 300;
DigitalBeacon.SiteBase.SessionAuditor.DefaultNotificationTime = 60;
DigitalBeacon.SiteBase.SessionAuditor.DefaultAuthSessionUrl = '~/.authSession.ashx';
DigitalBeacon.SiteBase.SessionAuditor.DefaultExtendSessionUrl = '~/account/extendSession';
DigitalBeacon.SiteBase.SessionAuditor.DefaultSignOutUrl = '~/identity/signOut';
DigitalBeacon.SiteBase.SessionAuditor.DefaultNotificationHeading = 'Session Expiration';
DigitalBeacon.SiteBase.SessionAuditor.DefaultNotificationMessage = 'Your session is set to expire in one minute.';
DigitalBeacon.SiteBase.SessionAuditor.DefaultExtendButtonText = 'Extend Session';
DigitalBeacon.SiteBase.SessionAuditor._instance = null;
DigitalBeacon.SiteBase.SessionAuditor.instance = function () {
    if (DigitalBeacon.SiteBase.SessionAuditor._instance === null) {
        DigitalBeacon.SiteBase.SessionAuditor._instance = new DigitalBeacon.SiteBase.SessionAuditor();
    }
    return DigitalBeacon.SiteBase.SessionAuditor._instance;
};

$.digitalbeacon.registerJQueryPlugin('sbEditor', (function(element, options) {
    var editor = $(element).data('sbEditor');
    if (!editor) {
        new DigitalBeacon.SiteBase.Editor(element, options);
    }
    return editor;
}));
