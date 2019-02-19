// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //
var DigitalBeacon;
(function (DigitalBeacon) {
    var SiteBase;
    (function (SiteBase) {
        var Editor = /** @class */ (function () {
            function Editor() {
                this._handleSiteRelativeUrls = true;
                this._defaultToTextBox = false;
            }
            Editor.Editor = function () {
                digitalbeacon.registerJQueryPlugin("sbEditor", (Func()(function (element, options) {
                    var editor = (dynamic), jQuery, Select;
                    (element).data("sbEditor");
                    if (!editor) {
                        new Editor(element, options);
                    }
                    return editor;
                })));
            };
            Editor.prototype.Editor = function (object, element, dynamic, options) {
                if (options != null) {
                    _handleSiteRelativeUrls = options.handleSiteRelativeUrls ?  ? _handleSiteRelativeUrls :  : ;
                    _defaultToTextBox = options.defaultToTextBox ?  ? _defaultToTextBox :  : ;
                }
                _textArea = jQuery.Select(element);
                _wrapperElement = _textArea.parent().get(0);
                _editorElement = jQuery.Select("table", _wrapperElement);
                _useEditorLink = jQuery.Select(".useEditor", _wrapperElement);
                _useTextBoxLink = jQuery.Select(".useTextBox", _wrapperElement);
                _useEditorLink.click(delegate(Event, e), {});
                _useTextBoxLink.click(delegate(Event, e), {});
                _textArea.data("sbEditor", this);
                if (!_defaultToTextBox) {
                    updateTelerikEditorValue();
                }
            };
            Editor.prototype.withTelerikEditor = function (Action, action) {
                if (_editor) {
                    action();
                    return;
                }
                var attempts = ((int)(_textArea.data("initAttempts") ?  ? 0 :  : )) + 1;
                if (attempts >= 10) {
                    digitalbeacon.log("Could not initialize Editor component for {0}.".formatWith(_textArea.attr("id")));
                    return;
                }
                dynamic;
                tEditor = _editorElement.data("tEditor");
                if (!tEditor) {
                    _textArea.data("initAttempts", attempts);
                    window.setTimeout((Action)(function () { return withTelerikEditor(action); }), 250);
                    return;
                }
                _editor = tEditor;
                action();
            };
            Editor.prototype.useEditor = function (Event, e) {
                updateTelerikEditorValue();
                toggleComponents(true);
                if (e != null) {
                    e.preventDefault();
                }
            };
            Editor.prototype.useTextBox = function (Event, e) {
                updateTextAreaValue();
                toggleComponents(false);
                if (e != null) {
                    e.preventDefault();
                }
            };
            Editor.prototype.toggleComponents = function (bool, useEditor) {
                _editorElement.toggle(useEditor);
                _useTextBoxLink.toggle(useEditor);
                _textArea.toggle(!useEditor);
                _useEditorLink.toggle(!useEditor);
            };
            Editor.prototype.updateTelerikEditorValue = function () {
                withTelerikEditor(function () {
                    var val = (string), _textArea, val;
                    ();
                    if (_handleSiteRelativeUrls) {
                        val = val.expandSiteRelativeText();
                    }
                    _editor.value(val);
                });
            };
            Editor.prototype.updateTextAreaValue = function () {
                withTelerikEditor(function () {
                    var val = (string), _editor, value;
                    ();
                    if (_handleSiteRelativeUrls) {
                        val = val.toSiteRelativeText();
                    }
                    _textArea.val(val);
                });
            };
            Editor.prototype.prepareForValidation = function () {
                if (_editorElement.)
                {
                    updateTextAreaValue();
                }
            };
            Editor.prototype.prepareForSubmission = function () {
                if (_editorElement.)
                {
                    updateTextAreaValue();
                }
                _textArea.val(digitalbeacon.htmlEncode((string), _textArea.val()));
            };
            Editor.prototype.value = function (string, val) {
                if (val === void 0) { val = null; }
                if (val) {
                    _textArea.val(val);
                    updateTelerikEditorValue();
                }
                else {
                    prepareForValidation();
                }
                return (string);
                _textArea.val();
            };
            Editor.prototype.encodedValue = function () {
                return digitalbeacon.htmlEncode(value());
            };
            return Editor;
        }());
        SiteBase.Editor = Editor;
    })(SiteBase = DigitalBeacon.SiteBase || (DigitalBeacon.SiteBase = {}));
})(DigitalBeacon || (DigitalBeacon = {}));
// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //
var DigitalBeacon;
(function (DigitalBeacon) {
    var SiteBase;
    (function (SiteBase) {
        /// <summary>
        /// Polls session status and redirects to login page when session expires
        /// </summary>
        var SessionAuditor = /** @class */ (function () {
            function SessionAuditor() {
                this._displayNotification = true;
            }
            Object.defineProperty(SessionAuditor.prototype, "authSessionUrl", {
                // public static SessionAuditor instance()
                // {
                // 	if (_instance == null)
                // 	{
                // 		_instance = new SessionAuditor();
                // 	}
                // 	return _instance;
                // }
                //
                // private SessionAuditor()
                // {
                // }
                get: function () {
                    return this._authSessionUrl;
                },
                set: function (value) {
                    this._authSessionUrl = value;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(SessionAuditor.prototype, "extendSessionUrl", {
                get: function () {
                    return this._extendSessionUrl;
                },
                set: function (value) {
                    this._extendSessionUrl = value;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(SessionAuditor.prototype, "signOutUrl", {
                get: function () {
                    return this._signOutUrl;
                },
                set: function (value) {
                    this._signOutUrl = value;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(SessionAuditor.prototype, "pollingInterval", {
                get: function () {
                    return this._pollingInterval;
                },
                set: function (value) {
                    this._pollingInterval = value;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(SessionAuditor.prototype, "notificationTime", {
                get: function () {
                    return this._notificationTime;
                },
                set: function (value) {
                    this._notificationTime = value;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(SessionAuditor.prototype, "displayNotification", {
                get: function () {
                    return this._displayNotification;
                },
                set: function (value) {
                    this._displayNotification = value;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(SessionAuditor.prototype, "notificationHeading", {
                get: function () {
                    return this._notificationHeading;
                },
                set: function (value) {
                    this._notificationHeading = value;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(SessionAuditor.prototype, "notificationMessage", {
                get: function () {
                    return this._notificationMessage;
                },
                set: function (value) {
                    this._notificationMessage = value;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(SessionAuditor.prototype, "extendButtonText", {
                get: function () {
                    return this._extendButtonText;
                },
                set: function (value) {
                    this._extendButtonText = value;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(SessionAuditor.prototype, "notificationTimerId", {
                get: function () {
                    return this._notificationTimerId;
                },
                set: function (value) {
                    this._notificationTimerId = value;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(SessionAuditor.prototype, "signOutTimerId", {
                get: function () {
                    return this._signOutTimerId;
                },
                set: function (value) {
                    this._signOutTimerId = value;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(SessionAuditor.prototype, "intervalTimerId", {
                get: function () {
                    return this._intervalTimerId;
                },
                set: function (value) {
                    this._intervalTimerId = value;
                },
                enumerable: true,
                configurable: true
            });
            SessionAuditor.prototype.start = function () {
                var _this = this;
                stop();
                this._intervalTimerId = setInterval(function () {
                    _this.checkSession();
                }, this.pollingInterval * 1000);
            };
            SessionAuditor.prototype.stop = function () {
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
            SessionAuditor.prototype.checkSession = function () {
                var _this = this;
                jQuery.post(this.authSessionUrl, null, function (data, status, xhr) {
                    _this.checkSessionOnSuccess(data);
                });
            };
            SessionAuditor.prototype.checkSessionOnSuccess = function (data) {
                var _this = this;
                var secondsRemaining = data;
                digitalbeacon.log(secondsRemaining + " seconds remaining...");
                if (this._displayNotification && this._notificationTimerId <= 0 && secondsRemaining < this.notificationTime + this.pollingInterval) {
                    this._notificationTimerId = window.setTimeout(function () {
                        _this.showNotification();
                    }, (secondsRemaining - this.notificationTime) * 1000);
                    this._signOutTimerId = window.setTimeout(function () {
                        _this.signOut();
                    }, secondsRemaining * 1000);
                    clearInterval(this._intervalTimerId);
                    this._intervalTimerId = 0;
                }
            };
            SessionAuditor.prototype.showNotification = function () {
                var _this = this;
                jQuery.post(this.authSessionUrl, null, function (data, status, xhr) {
                    var secondsRemaining = data;
                    if (secondsRemaining < _this.notificationTime + _this.pollingInterval) {
                        sitebase.displayMessage(_this.notificationMessage.formatWith(_this.notificationTime, _this.notificationTime / 60), _this.notificationHeading, "", _this.extendButtonText, function () {
                            _this.extendSession();
                        });
                    }
                    else {
                        // session was extended
                        _this.start();
                    }
                });
            };
            SessionAuditor.prototype.signOut = function () {
                var _this = this;
                jQuery.post(this.authSessionUrl, null, function (data, status, xhr) {
                    var secondsRemaining = data;
                    if (secondsRemaining < _this.pollingInterval) {
                        window.location.href = _this.signOutUrl;
                    }
                    else {
                        // session was extended
                        _this.start();
                    }
                });
            };
            SessionAuditor.prototype.extendSession = function () {
                jQuery.post(this.extendSessionUrl);
                this.start();
            };
            SessionAuditor.DefaultPollingInterval = 300;
            SessionAuditor.DefaultNotificationTime = 60;
            SessionAuditor.DefaultAuthSessionUrl = "~/.authSession.ashx";
            SessionAuditor.DefaultExtendSessionUrl = "~/account/extendSession";
            SessionAuditor.DefaultSignOutUrl = "~/identity/signOut";
            SessionAuditor.DefaultNotificationHeading = "Session Expiration";
            SessionAuditor.DefaultNotificationMessage = "Your session is set to expire in one minute.";
            SessionAuditor.DefaultExtendButtonText = "Extend Session";
            return SessionAuditor;
        }());
    })(SiteBase = DigitalBeacon.SiteBase || (DigitalBeacon.SiteBase = {}));
})(DigitalBeacon || (DigitalBeacon = {}));
// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //
$(document).ready(function () {
    $.sitebase.debug = false;
    $.sitebase.init();
});
(function ($) {
    /*-----------------------------------------------------------------------------
    sitebase singleton
    -----------------------------------------------------------------------------*/
    $.sitebase = new function () {
        this.debug = false;
        this.isMobile = document.isMobile || false;
        this.ajaxTimerId1 = 0;
        this.ajaxTimerId2 = 0;
        this.ajaxCounter = 0;
        this.showMessageSummaryAsModalBox = true;
        this.localization = {
            culture: 'en-US',
            closeLabel: 'Close',
            messageHeading: 'Message',
            errorHeading: 'Oops! Something went wrong...',
            errorText: 'An error occurred while processing this request.',
            loadingText: 'Loading',
            noDataText: 'No content available',
            confirmText: 'Are you sure?',
            formAlreadySubmitted: 'This form has already been submitted. Please refresh this page to attempt this request again.'
        };
        /*-----------------------------------------------------------------------------
        Performs global site initialization
        -----------------------------------------------------------------------------*/
        this.init = function () {
            this._addSignOutConfirmation();
            if (!this.isMobile) {
                this._initAjaxFeedback();
                this._setTooltipDefaults();
                this.displayMessageSummaryAsModalBox();
                this.addTooltips(); // decorate server-generated field validation error messages
            }
        };
        /*-----------------------------------------------------------------------------
        Log message to javascript console
        -----------------------------------------------------------------------------*/
        this.log = function (message) {
            $.digitalbeacon.log(message);
        };
        /*-----------------------------------------------------------------------------
        Display confirm dialog
        -----------------------------------------------------------------------------*/
        this.confirm = function (message) {
            if (message) {
                return confirm(message);
            }
            return confirm(this.localization.confirmText);
        };
        /*-----------------------------------------------------------------------------
        Display a message in a modal box
        -----------------------------------------------------------------------------*/
        this.displayMessage = function (message, heading, cssClass, additionalButtonText, additionalButtonAction) {
            if (!heading) {
                heading = $.sitebase.localization.messageHeading;
            }
            var additionalButtonText = additionalButtonText ?
                '<input type="submit" id="ok" class="ok" value="' + additionalButtonText + '" />' : '';
            $.sitebase.modalBox({
                content: '<h1>' + heading + '</h1><div class="message-panel">'
                    + (cssClass ? '<div class="' + cssClass + '">' : '<div>')
                    + message + '</div><div class="button-panel">' + additionalButtonText
                    + '<input type="submit" id="close" class="close-message-panel cancel" value="'
                    + $.sitebase.localization.closeLabel + '" /></div></div>',
                success: function () {
                    var close = $('#modalBox input.close-message-panel:last')
                        .click(function () { $.sitebase.closeModalBox(); })
                        .keyup(function (e) { if (e.keyCode == '27') {
                        return this.click();
                    } });
                    if (additionalButtonText && additionalButtonAction) {
                        $('#modalBox input.ok:last')
                            .click(function () { additionalButtonAction(); $.sitebase.closeModalBox(); });
                    }
                    setTimeout(function () { close.focus(); }, 100);
                },
                replace: false,
                clickOutsideToClose: true
            });
        };
        /*-----------------------------------------------------------------------------
        Display an error message in a modal box
        -----------------------------------------------------------------------------*/
        this.displayError = function (message, heading) {
            if (!heading) {
                heading = $.sitebase.localization.errorHeading;
            }
            this.displayMessage(message, heading, 'error-summary');
        };
        /*-----------------------------------------------------------------------------
        Display message summary as modal box
        -----------------------------------------------------------------------------*/
        this.displayMessageSummaryAsModalBox = function () {
            if ($.sitebase.showMessageSummaryAsModalBox) {
                var messagePanel = $('.message-panel:first');
                if (messagePanel.text()) {
                    messagePanel.hide();
                    this.displayMessage(messagePanel.html());
                }
            }
        };
        /*-----------------------------------------------------------------------------
        Checks for form double submission
        -----------------------------------------------------------------------------*/
        this.checkFormSubmission = function (form) {
            var $form = $(form);
            if ($form.data('sumbitted')) {
                $.sb.displayError($.sitebase.localization.formAlreadySubmitted);
                //$.sb.log('form has already been submitted');
                return false;
            }
            else {
                $form.data('sumbitted', true);
                return true;
            }
        };
        /*-----------------------------------------------------------------------------
        Resets form submission flag
        -----------------------------------------------------------------------------*/
        this.resetFormSubmission = function (form) {
            $(form).data('sumbitted', false);
        };
        /*-----------------------------------------------------------------------------
        Submit form via ajax
        -----------------------------------------------------------------------------*/
        this.ajaxSubmit = function (form) {
            if ($.sb.checkFormSubmission(form)) {
                $.post(form.action, $(form).serialize(), function (response) { $.sb.modalBox({ content: response }); });
            }
            return false;
        };
        /*-----------------------------------------------------------------------------
        Initialize client form validation
        -----------------------------------------------------------------------------*/
        this.initClientFormValidation = function (options) {
            if (window.mvcClientValidationMetadata) {
                var settings = {
                    debug: true,
                    highlight: function (element, errorClass) {
                        $.sitebase.addTooltips();
                        // trigger resize event to recenter modal content if necessary
                        $(window).resize();
                    },
                    submitHandler: function (form) {
                        if ($.sb.checkFormSubmission(form)) {
                            form.submit();
                        }
                    }
                };
                if (options) {
                    settings = $.extend(settings, options);
                }
                $().mvcValidate(window.mvcClientValidationMetadata, settings);
            }
        };
        /*-----------------------------------------------------------------------------
        Add tooltips for help text and field validation errors
        -----------------------------------------------------------------------------*/
        this.addTooltips = function () {
            $('.form-row-help').each(function () {
                $(this).show().find('.form-row-tip').bt($(this).text());
            });
            $('.field-validation-error').each(function () {
                if ($(this).text()) {
                    $(this).bt($(this).text());
                    $(this).text('').css('display', 'block');
                }
            });
        };
        /*-----------------------------------------------------------------------------
        Get url with partial diplay type parameter set
        -----------------------------------------------------------------------------*/
        this.getRenderPartialUrl = function (url) {
            return $.digitalbeacon.mergeParams(url, { renderType: 'partial' });
        };
        /*-----------------------------------------------------------------------------
        Get url with partial wrapped diplay type parameter set
        -----------------------------------------------------------------------------*/
        this.getRenderPartialWrappedUrl = function (url) {
            return $.digitalbeacon.mergeParams(url, { renderType: 'partialWrapped' });
        };
        /*-----------------------------------------------------------------------------
        Display content in a modal box
        -----------------------------------------------------------------------------*/
        this.modalBox = function (options) {
            $.digitalbeacon.modalBox(options);
        };
        /*-----------------------------------------------------------------------------
        Explicitly set modal box on close handler
        -----------------------------------------------------------------------------*/
        this.modalBoxOnClose = function (handler) {
            $.digitalbeacon.modalBoxOnClose(handler);
        };
        /*-----------------------------------------------------------------------------
        Close modal box
        -----------------------------------------------------------------------------*/
        this.closeModalBox = function () {
            $.digitalbeacon.closeModalBox();
        };
        /*-----------------------------------------------------------------------------
        Check to see if modal box is displayed
        -----------------------------------------------------------------------------*/
        this.isModalBoxVisible = function () {
            return $.digitalbeacon.isModalBoxVisible();
        };
        /*-----------------------------------------------------------------------------
        Get visible modal box
        -----------------------------------------------------------------------------*/
        this.getModalBox = function () {
            return $.digitalbeacon.getModalBox();
        };
        /*-----------------------------------------------------------------------------
        Show loading modal box
        -----------------------------------------------------------------------------*/
        this.showLoadingModalBox = function () {
            $.digitalbeacon.showLoadingModalBox();
        };
        /*-----------------------------------------------------------------------------
        Hide loading modal box
        -----------------------------------------------------------------------------*/
        this.hideLoadingModalBox = function () {
            $.digitalbeacon.hideLoadingModalBox();
        };
        /*-----------------------------------------------------------------------------
        Ajax start handler
        -----------------------------------------------------------------------------*/
        this.onAjaxStart = function () {
            $.sitebase.ajaxCounter++;
            if (!$.sitebase.$mask) {
                $.sitebase.$mask = $('#mask');
            }
            if ($.sitebase.ajaxTimerId1 || $.sitebase.ajaxTimerId2) {
                return;
            }
            $.sitebase.ajaxTimerId1 = setTimeout(function () {
                $.sitebase.$mask.fadeTo(0, 0.01); // enable the mask without really showing it
                //$mask.find('input').focus(); // capture focus to prevent input
            }, 250);
            $.sitebase.ajaxTimerId2 = setTimeout(function () {
                $.sitebase.showLoadingModalBox();
            }, 1000);
        };
        /*-----------------------------------------------------------------------------
        Ajax end handler
        -----------------------------------------------------------------------------*/
        this.onAjaxEnd = function () {
            $.sitebase.ajaxCounter--;
            if (!$.sitebase.$mask) {
                return;
            }
            if ($.sitebase.ajaxCounter <= 0) {
                if ($.sitebase.ajaxTimerId1 || $.sitebase.ajaxTimerId2) {
                    window.clearTimeout($.sitebase.ajaxTimerId1);
                    window.clearTimeout($.sitebase.ajaxTimerId2);
                    $.sitebase.ajaxTimerId1 = 0;
                    $.sitebase.ajaxTimerId2 = 0;
                }
                $.sitebase.ajaxCounter = 0;
                $.sitebase.$mask.fadeOut(0);
                $.sitebase.hideLoadingModalBox();
            }
        };
        /*-----------------------------------------------------------------------------
        Set defaults for tooltips
        -----------------------------------------------------------------------------*/
        this._setTooltipDefaults = function () {
            $.bt.options.fill = '#fff';
            $.bt.options.strokeStyle = '#000';
            $.bt.options.spikeLength = 15;
            $.bt.options.spikeGirth = 10;
            $.bt.options.padding = 8;
            $.bt.options.cornerRadius = 0;
            $.bt.options.shadow = true;
            $.bt.options.shadowColor = 'rgba(0,0,0,.5)';
            $.bt.options.shadowBlur = 8;
            $.bt.options.shadowOffsetX = 4;
            $.bt.options.shadowOffsetY = 4;
            $.bt.options.width = '300px';
            $.bt.options.shrinkToFit = true;
            $.bt.options.hoverIntentOpts = {
                interval: 100,
                timeout: 100
            };
        };
        /*-----------------------------------------------------------------------------
        Add confirmation prompt for sign out link
        -----------------------------------------------------------------------------*/
        this._addSignOutConfirmation = function () {
            $('a[href$="signOut"]').click(function () {
                if (confirm($(this).text() + '?')) {
                    return true;
                }
                else {
                    return false;
                }
            });
        };
        /*-----------------------------------------------------------------------------
        Initialize ajax feedback
        -----------------------------------------------------------------------------*/
        this._initAjaxFeedback = function () {
            $.ajaxSetup({ cache: false });
            this.$mask = $('#mask');
            this.$mask.ajaxStart(this.onAjaxStart);
            this.$mask.ajaxStop(this.onAjaxEnd);
            $(document).ajaxComplete(function (event, xmlHttpRequest, ajaxOptions) {
                if ($.sb.debug) {
                    $.sb.log(ajaxOptions.url);
                    $.sb.log(ajaxOptions.data);
                    $.sb.log(xmlHttpRequest.status);
                    $.sb.log(xmlHttpRequest.responseText);
                }
            });
            $(document).ajaxError(function (event, xmlHttpRequest, ajaxOptions, thrownError) {
                $.sb.log(xmlHttpRequest.status);
                $.sb.log(xmlHttpRequest.responseText);
                if (xmlHttpRequest.status == 403) {
                    window.location = window.location;
                }
                else {
                    $.sb.displayError($.sitebase.localization.errorText);
                }
            });
        };
    };
    /*-----------------------------------------------------------------------------
    Alias sitebase object
    -----------------------------------------------------------------------------*/
    $.sb = $.sitebase;
})(jQuery);
/*-----------------------------------------------------------------------------
Localized strings for modal box
-----------------------------------------------------------------------------*/
var modalboxLocalizedStrings = {
    messageCloseWindow: $.sitebase.localization.closeLabel,
    messageAjaxLoader: $.sitebase.localization.loadingText,
    errorMessageIfNoDataAvailable: $.sitebase.localization.noDataText,
    errorMessageXMLHttpRequest: $.sitebase.localization.errorText,
    errorMessageTextStatusError: $.sitebase.localization.errorText
};
$.digitalbeacon.modalBox.defaults.loadingText = $.sitebase.localization.loadingText;
$.digitalbeacon.modalBox.defaults.noDataText = $.sitebase.localization.noDataText;
//$.digitalbeacon.modalBox.defaults.width = 'auto';
//$.digitalbeacon.modalBox.defaults.width = '65em';
//$.digitalbeacon.modalBox.defaults.loadingWidth = '8em';
//# sourceMappingURL=DigitalBeacon.SiteBase.Scripts.js.map