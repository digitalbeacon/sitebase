// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //
var DigitalBeacon;
(function (DigitalBeacon) {
    var StringBuilder = /** @class */ (function () {
        function StringBuilder() {
        }
        StringBuilder.prototype.cat = function (str) {
            this._parts.push(str);
            return this;
        };
        StringBuilder.prototype.build = function () {
            return this._parts.join("");
        };
        return StringBuilder;
    }());
    DigitalBeacon.StringBuilder = StringBuilder;
})(DigitalBeacon || (DigitalBeacon = {}));
// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //
var DigitalBeacon;
(function (DigitalBeacon) {
    String.prototype.formatWith = function (arg1, arg2, arg3, arg4, arg5) {
        if (arg2 === void 0) { arg2 = null; }
        if (arg3 === void 0) { arg3 = null; }
        if (arg4 === void 0) { arg4 = null; }
        if (arg5 === void 0) { arg5 = null; }
        return DigitalBeacon.StringUtils.formatWith(this, arg1, arg2, arg3, arg4, arg5);
    };
    String.prototype.hasText = function () {
        return DigitalBeacon.StringUtils.hasText(this);
    };
    String.prototype.toDate = function () {
        return DigitalBeacon.StringUtils.toDate(this);
    };
    String.prototype.expandSiteRelativeText = function () {
        return DigitalBeacon.StringUtils.expandSiteRelativeText(this);
    };
    String.prototype.toSiteRelativeText = function () {
        return DigitalBeacon.StringUtils.toSiteRelativeText(this);
    };
})(DigitalBeacon || (DigitalBeacon = {}));
// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //
var DigitalBeacon;
(function (DigitalBeacon) {
    var StringUtils = /** @class */ (function () {
        function StringUtils() {
        }
        StringUtils.formatWith = function (format, arg1, arg2, arg3, arg4, arg5) {
            if (arg2 === void 0) { arg2 = null; }
            if (arg3 === void 0) { arg3 = null; }
            if (arg4 === void 0) { arg4 = null; }
            if (arg5 === void 0) { arg5 = null; }
            var s = format;
            var args = [arg1, arg2, arg3, arg4, arg5];
            if (arg2 && arg3 && arg4 && arg5) {
                args.length = 1;
            }
            else if (arg3 && arg4 && arg5) {
                args.length = 2;
            }
            else if (arg4 && arg5) {
                args.length = 3;
            }
            else if (arg5) {
                args.length = 4;
            }
            for (var i = 0; i < args.length; i++) {
                var reg = new RegExp("\\{" + i + "\\}", "gm");
                s = s.replace(reg, "" + args[i]);
            }
            return s;
        };
        StringUtils.hasText = function (str) {
            return jQuery.trim(str).length > 0;
        };
        StringUtils.isDateString = function (dateStr) {
            return StringUtils.DateRegex.test(dateStr);
        };
        StringUtils.toDate = function (dateStr) {
            if (!dateStr) {
                return null;
            }
            var intDateStr = dateStr.replace(StringUtils.DateRegex, "$1");
            if (intDateStr) {
                return new Date(parseInt(intDateStr));
            }
            var intDateVal = Date.parse(dateStr);
            return !isNaN(intDateVal) ? new Date(intDateVal) : null;
        };
        StringUtils.expandSiteRelativeText = function (text) {
            var regex = new RegExp("\"~/", "gm");
            return StringUtils.hasText(text) ? text.replace(regex, "\"" + ($.digitalbeacon.appContextPath == "/" ? "" : $.digitalbeacon.appContextPath) + "/") : text;
        };
        StringUtils.toSiteRelativeText = function (text) {
            if ($.digitalbeacon.appContextPath == "/") {
                return text;
            }
            var regex = new RegExp("\"" + $.digitalbeacon.appContextPath + "/", "gm");
            return (StringUtils.hasText($.digitalbeacon.appContextPath) && StringUtils.hasText(text)) ? text.replace(regex, "\"~/") : text;
        };
        StringUtils.DateRegex = new RegExp("\/Date\((\-{0,1}\d+)\)\/", "gm");
        return StringUtils;
    }());
    DigitalBeacon.StringUtils = StringUtils;
})(DigitalBeacon || (DigitalBeacon = {}));
// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //
var DigitalBeacon;
(function (DigitalBeacon) {
    var Utils = /** @class */ (function () {
        function Utils() {
        }
        Utils.mergeParams = function (url, args) {
            for (var key in Object.keys(args)) {
                var regExp = new RegExp("({0})=([^&]*)".formatWith(key), "gi");
                if (regExp.test(url)) {
                    url = url.replace(regExp, "$1=" + args[key]);
                }
                else {
                    var sb = new DigitalBeacon.StringBuilder();
                    sb.cat(url);
                    if (url.indexOf("?") < 0) {
                        sb.cat("?");
                    }
                    else {
                        sb.cat("&");
                    }
                    url = sb.cat(key).cat("=").cat(encodeURIComponent(args[key])).build();
                }
            }
            return url;
        };
        Utils.isString = function (obj) {
            return $.digitalbeacon.isOfType(obj, "string");
        };
        Utils.isObject = function (obj) {
            return obj != null && $.digitalbeacon.isOfType(obj, "object");
        };
        Utils.isDefined = function (obj) {
            return !$.digitalbeacon.isOfType(obj, "undefined");
        };
        Utils.convertDateStringsToDates = function (input, level) {
            if (level === void 0) { level = 0; }
            if (!Utils.isObject(input)) {
                return input;
            }
            for (var key in Object.keys(input)) {
                if (!input.hasOwnProperty(key))
                    continue;
                var value = input[key];
                if (Utils.isString(value) && DigitalBeacon.StringUtils.isDateString(value)) {
                    input[key] = DigitalBeacon.StringUtils.toDate(value);
                }
                else if (value && Utils.isObject(value) && level < 10) {
                    Utils.convertDateStringsToDates(value, level + 1);
                }
            }
            return input;
        };
        return Utils;
    }());
    DigitalBeacon.Utils = Utils;
})(DigitalBeacon || (DigitalBeacon = {}));
// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //
(function ($) {
    /*-----------------------------------------------------------------------------
    digitalbeacon singleton
    -----------------------------------------------------------------------------*/
    $.digitalbeacon = new function () {
        this.appContextPath = document.appContextPath || '';
        this.assetVersion = document.assetVersion || '';
        this.importedFiles = {};
        /*-----------------------------------------------------------------------------
        Log message to console
        -----------------------------------------------------------------------------*/
        this.log = function (message) {
            if (window.console && window.console.debug) {
                console.debug(message);
            }
        };
        /*-----------------------------------------------------------------------------
        Helper method to add jQuery plugins
        -----------------------------------------------------------------------------*/
        this.registerJQueryPlugin = function (name, fn) {
            // @ts-ignore
            $.fn[name] = function (options) {
                var component;
                var chain = this.each(function () {
                    var val = fn(this, options);
                    component = component || val;
                });
                if (options) {
                    return chain;
                }
                return component || chain;
            };
        };
        /*-----------------------------------------------------------------------------
        Dynamically load JavaScript file
        -----------------------------------------------------------------------------*/
        this.loadJsFile = function (jsFile) {
            if (this.importedFiles[jsFile]) {
                return;
            }
            var scriptElement = document.createElement('script');
            scriptElement.type = 'text/javascript';
            if (this.assetVersion) {
                scriptElement.src = this.mergeParams(this.resolveUrl(jsFile), { v: this.assetVersion });
            }
            else {
                scriptElement.src = this.resolveUrl(jsFile);
            }
            this.log('loaded: ' + scriptElement.src);
            $('head')[0].appendChild(scriptElement);
            this.importedFiles[jsFile] = true;
        };
        /*-----------------------------------------------------------------------------
        Dynamically load CSS file
        -----------------------------------------------------------------------------*/
        this.loadCssFile = function (cssFile) {
            if (this.importedFiles[cssFile]) {
                return;
            }
            var linkElement = document.createElement('link');
            linkElement.type = 'text/css';
            linkElement.rel = 'stylesheet';
            if (this.assetVersion) {
                linkElement.href = this.mergeParams(this.resolveUrl(cssFile), { v: this.assetVersion });
            }
            else {
                linkElement.href = this.resolveUrl(cssFile);
            }
            this.log('loaded: ' + linkElement.href);
            $('head')[0].appendChild(linkElement);
            this.importedFiles[cssFile] = true;
        };
        /*-----------------------------------------------------------------------------
        Html encode input
        -----------------------------------------------------------------------------*/
        this.htmlEncode = function (input) {
            return $('<div/>').text(input).html();
        };
        /*-----------------------------------------------------------------------------
        Html decode input
        -----------------------------------------------------------------------------*/
        this.htmlDecode = function (input) {
            return $('<div/>').html(input).text();
        };
        /*-----------------------------------------------------------------------------
        Resolve app context relative url
        -----------------------------------------------------------------------------*/
        this.resolveUrl = function (relativeUrl) {
            return relativeUrl.replace('~', this.appContextPath);
        };
        /*-----------------------------------------------------------------------------
        Merge parameters to url
        -----------------------------------------------------------------------------*/
        this.mergeParams = function (url, args) {
            if (DigitalBeacon.Utils) {
                return DigitalBeacon.Utils.mergeParams(url, args);
            }
            for (var key in args) {
                var regExp = new RegExp($.telerik.formatString('({0})=([^&]*)', key), 'gi');
                if (regExp.test(url)) {
                    url = url.replace(regExp, '$1=' + args[key]);
                }
                else {
                    var sb = new $.telerik.stringBuilder();
                    sb.cat(url);
                    if (url.indexOf('?') < 0) {
                        sb.cat('?');
                    }
                    else {
                        sb.cat('&');
                    }
                    url = sb.cat(key).cat('=').cat(encodeURIComponent(args[key])).string();
                }
            }
            return url;
        };
        /*-----------------------------------------------------------------------------
        Check if object is of a given type
        -----------------------------------------------------------------------------*/
        this.isOfType = function (obj, type) {
            return typeof obj === type;
        };
    };
    /*-----------------------------------------------------------------------------
    Add trim method to String object if not defined
    -----------------------------------------------------------------------------*/
    if (!String.prototype.trim) {
        String.prototype.trim = function () {
            return $.trim(this);
        };
    }
    /*-----------------------------------------------------------------------------
    Add indexOf method to Array object if not defined
    -----------------------------------------------------------------------------*/
    if (!Array.prototype.indexOf) {
        Array.prototype.indexOf = function (obj, fromIndex) {
            if (fromIndex == null) {
                fromIndex = 0;
            }
            else if (fromIndex < 0) {
                fromIndex = Math.max(0, this.length + fromIndex);
            }
            for (var i = fromIndex, j = this.length; i < j; i++) {
                if (this[i] === obj) {
                    return i;
                }
            }
            return -1;
        };
    }
})(jQuery);
// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //
(function ($) {
    if ($.fn.modalBox) {
        /*-----------------------------------------------------------------------------
        Display content in a modal box
        -----------------------------------------------------------------------------*/
        $.digitalbeacon.modalBox = function (options) {
            if (typeof options === "string") {
                options = { content: options };
            }
            if (!options.width && !options.nonPersistentWidth) {
                options.width = $.digitalbeacon.modalBox.defaults.width;
            }
            if (options.replace == undefined) {
                options.replace = true;
            }
            if (options.reposition == undefined) {
                options.reposition = true;
            }
            if (options.ajax || options.ajaxPost) {
                $.ajax({
                    url: options.ajaxPost ? options.ajaxPost : options.ajax,
                    type: options.ajaxPost ? 'POST' : 'GET',
                    data: options.data,
                    success: function (response) {
                        options.ajax = null;
                        options.ajaxPost = null;
                        options.content = response;
                        $.digitalbeacon.modalBox(options);
                    }
                });
            }
            else {
                var modalBox = $.digitalbeacon.getModalBox();
                if (modalBox.length) {
                    if (modalBox.hasClass('loading')) {
                        $.digitalbeacon._executeCloseHandler(modalBox, true);
                    }
                    var replaceLi = false;
                    if (options.replace) {
                        replaceLi = !($.digitalbeacon._executeCloseHandler(modalBox, false));
                    }
                    var modalBoxContent = modalBox.find('.modalBoxBodyContent');
                    var ul = modalBoxContent.find('> ul#modalBoxStack');
                    var $currentContents = modalBoxContent.find('> :not(ul#modalBoxStack)');
                    var updateElement = modalBoxContent;
                    var elementsToHide = void 0;
                    if (replaceLi) {
                        updateElement = ul.find('> li:last').data('modalBoxOptions', options);
                    }
                    else if (options.replace || $currentContents.length == 0) {
                        modalBox.data('modalBoxOptions', options);
                    }
                    else {
                        elementsToHide = $currentContents;
                        if (ul.length) {
                            elementsToHide = elementsToHide.add(ul.find('> li'));
                        }
                        else {
                            ul = $('<ul/>').attr('id', 'modalBoxStack').appendTo(modalBoxContent);
                        }
                        updateElement = $('<li/>').attr('id', 'modalBoxStackItem').data('modalBoxOptions', options).appendTo(ul);
                    }
                    var content = $.digitalbeacon.modalBox.defaults.noDataText;
                    if (options.content) {
                        content = options.content;
                    }
                    else if (options.element) {
                        content = $(options.element).html();
                    }
                    $.digitalbeacon._updateModalBoxContent(modalBox, updateElement, elementsToHide, content, options);
                }
                else {
                    var settings = {
                        killModalboxWithCloseButtonOnly: true,
                        minimalTopSpacingOfModalbox: 25
                    };
                    if (options.content) {
                        $.extend(settings, { directCall: { data: options.content } });
                    }
                    else if (options.element) {
                        $.extend(settings, { directCall: { element: options.element } });
                    }
                    else {
                        $.extend(settings, { directCall: { data: $.digitalbeacon.modalBox.defaults.noDataText } });
                    }
                    if (options.width && typeof options.width == 'number') {
                        $.extend(settings, { setWidthOfModalLayer: options.width });
                    }
                    else if (options.nonPersistentWidth && typeof options.nonPersistentWidth == 'number') {
                        $.extend(settings, { setWidthOfModalLayer: options.nonPersistentWidth });
                    }
                    if (options.clickOutsideToClose) {
                        $.extend(settings, { killModalboxWithCloseButtonOnly: !options.clickOutsideToClose });
                    }
                    if (options.success) {
                        $.extend(settings, { callFunctionAfterSuccess: options.success });
                    }
                    $.fn.modalBox(settings);
                    modalBox = $('#modalBox');
                    //modalBox.fadeTo(0, 0.01);
                    modalBox.addClass('visible');
                    if (options.width && typeof options.width == 'string' && options.width != 'auto') {
                        modalBox.css('width', options.width);
                        $.digitalbeacon.centerModalBox(modalBox);
                    }
                    else if (options.nonPersistentWidth
                        && typeof options.nonPersistentWidth == 'string'
                        && options.width != 'auto') {
                        modalBox.css('width', options.nonPersistentWidth);
                        $.digitalbeacon.centerModalBox(modalBox);
                    }
                    //modalBox.fadeTo(0, 1);
                    modalBox.data('modalBoxOptions', options);
                    var faderLayer = $('#modalBoxFaderLayer');
                    faderLayer.fadeTo(0, 0.01);
                    faderLayer.addClass('visible');
                    faderLayer.fadeTo(300, 0.4);
                    $('body').addClass("modalBoxOpen");
                }
            }
        };
        /*-----------------------------------------------------------------------------
        Modal box defaults
        -----------------------------------------------------------------------------*/
        $.digitalbeacon.modalBox.defaults = {
            loadingText: 'Loading...',
            noDataText: '<p>No content available</p>',
            width: 'auto',
            loadingWidth: 'auto'
        };
        /*-----------------------------------------------------------------------------
        Close modal box
        -----------------------------------------------------------------------------*/
        $.digitalbeacon.closeModalBox = function () {
            $.fn.modalBox.close();
        };
        /*-----------------------------------------------------------------------------
        Show loading modal box
        -----------------------------------------------------------------------------*/
        $.digitalbeacon.showLoadingModalBox = function (delay) {
            if (delay > 0) {
                $.digitalbeacon.modalBoxLoadingTimerId = setTimeout(function () { $.digitalbeacon.modalBoxLoading(0); }, delay);
            }
            else {
                $.digitalbeacon.modalBox({
                    content: '<div id="modalBoxAjaxLoader">' + $.digitalbeacon.modalBox.defaults.loadingText + '</div>',
                    nonPersistentWidth: $.digitalbeacon.modalBox.defaults.loadingWidth,
                    replace: false,
                    reposition: true
                });
                $.digitalbeacon.getModalBox().addClass('loading');
            }
        };
        /*-----------------------------------------------------------------------------
        Hide loading modalBox
        -----------------------------------------------------------------------------*/
        $.digitalbeacon.hideLoadingModalBox = function (modalBox) {
            if (!modalBox) {
                modalBox = $.digitalbeacon.getModalBox();
            }
            if (modalBox.length) {
                if (modalBox.hasClass('loading')) {
                    $.digitalbeacon.closeModalBox();
                    modalBox.removeClass('loading');
                }
            }
            if ($.digitalbeacon.modalBoxLoadingTimerId) {
                window.clearTimeout($.digitalbeacon.modalBoxLoadingTimerId);
                $.digitalbeacon.modalBoxLoadingTimerId = null;
            }
        };
        /*-----------------------------------------------------------------------------
        Get visible modal box
        -----------------------------------------------------------------------------*/
        $.digitalbeacon.getModalBox = function () {
            return $('body > #modalBox:visible');
        };
        /*-----------------------------------------------------------------------------
        Check to see if modal box is displayed
        -----------------------------------------------------------------------------*/
        $.digitalbeacon.isModalBoxVisible = function () {
            return $.digitalbeacon.getModalBox().length;
        };
        /*-----------------------------------------------------------------------------
        Center modal box
        -----------------------------------------------------------------------------*/
        $.digitalbeacon.centerModalBox = function (modalBox) {
            if (!modalBox) {
                modalBox = $.digitalbeacon.getModalBox();
            }
            if (modalBox.length) {
                $(window).resize();
            }
        };
        /*-----------------------------------------------------------------------------
        Explicitly set modal box on close handler
        -----------------------------------------------------------------------------*/
        $.digitalbeacon.modalBoxOnClose = function (handler) {
            setTimeout(function () {
                var modalBox = $.digitalbeacon.getModalBox();
                if (modalBox.length) {
                    var ul = modalBox.find('.modalBoxBodyContent > ul#modalBoxStack');
                    var container = modalBox;
                    if (ul.length) {
                        container = ul.find('> li:last');
                    }
                    var options = container.data('modalBoxOptions');
                    if (!options.close) {
                        options.close = handler;
                    }
                    else {
                        var proxiedHandler_1 = options.close;
                        options.close = function () {
                            handler();
                            proxiedHandler_1();
                        };
                    }
                }
            }, 0);
        };
        /*-----------------------------------------------------------------------------
        Update modal box content
        -----------------------------------------------------------------------------*/
        $.digitalbeacon._updateModalBoxContent = function (modalBox, elementToUpdate, elementsToHide, content, options) {
            var loading = modalBox.hasClass('loading');
            var reposition = options.reposition || loading;
            if (reposition) {
                modalBox.removeClass('visible');
                //modalBox.fadeTo(0, 0.01);
                if (options.width) {
                    modalBox.css('width', options.width);
                }
                else if (options.nonPersistentWidth) {
                    modalBox.css('width', options.nonPersistentWidth);
                }
            }
            elementToUpdate.html(content);
            if (elementsToHide) {
                elementsToHide.hide();
            }
            if (options.success) {
                options.success();
            }
            if (reposition) {
                $(window).resize();
                //modalBox.fadeTo(0, 1);
                modalBox.addClass('visible');
            }
            if (loading) {
                modalBox.removeClass('loading');
            }
        };
        /*-----------------------------------------------------------------------------
        Execute close handler for item on top of the stack
        -----------------------------------------------------------------------------*/
        $.digitalbeacon._executeCloseHandler = function (modalBox, removeItem, recenter) {
            if (!modalBox) {
                modalBox = $.digitalbeacon.getModalBox();
            }
            if (modalBox.length) {
                var modalBoxContent = modalBox.find('.modalBoxBodyContent');
                var lastItem = modalBoxContent.find('> :not(ul#modalBoxStack)');
                var ul = modalBoxContent.find('> ul#modalBoxStack');
                var options = void 0;
                if (ul.length) {
                    var li = ul.find('> li:last');
                    options = li.data('modalBoxOptions');
                    if (options.close) {
                        try {
                            options.close();
                        }
                        catch (ex) { }
                    }
                    if (removeItem) {
                        li.remove();
                        if (ul.find('> li').length) {
                            li = ul.find('> li:last');
                            options = li.data('modalBoxOptions');
                            if (options.width) {
                                modalBox.css('width', options.width);
                            }
                            li.show();
                        }
                        else {
                            ul.remove();
                            options = modalBox.data('modalBoxOptions');
                            if (options.width) {
                                modalBox.css('width', options.width);
                            }
                            lastItem.show();
                        }
                        if (recenter) {
                            modalBox.removeClass('visible');
                            $.digitalbeacon.centerModalBox(modalBox);
                            modalBox.addClass('visible');
                        }
                    }
                    else {
                        li.data('modalBoxOptions', null);
                    }
                    return false;
                }
                else {
                    options = modalBox.data('modalBoxOptions');
                    if (options && options.close) {
                        try {
                            options.close();
                        }
                        catch (ex) { }
                    }
                    if (removeItem) {
                        lastItem.remove();
                    }
                    else {
                        modalBox.data('modalBoxOptions', null);
                    }
                    return true;
                }
                return false;
            }
        };
        /*-----------------------------------------------------------------------------
        Add custom handler to modal box close operation
        -----------------------------------------------------------------------------*/
        var proxiedClose_1 = $.fn.modalBox.close;
        $.fn.modalBox.close = function (settings) {
            var modalBox = $.digitalbeacon.getModalBox();
            if ($.digitalbeacon._executeCloseHandler(modalBox, true, true)) {
                var faderLayer = $('#modalBoxFaderLayer');
                modalBox.fadeTo(0, 0.01);
                faderLayer.fadeTo(300, 0.01, function () { proxiedClose_1(settings); });
                $('body').removeClass("modalBoxOpen");
            }
        };
    }
})(jQuery);
// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //
(function ($) {
    $.fn.mvcValidate = function (mvcClientValidationMetadata, settings) {
        var config = {
            onkeyup: false
        };
        if (settings) {
            $.extend(config, settings);
        }
        var allFormOptions = mvcClientValidationMetadata;
        if (allFormOptions) {
            for (var i = 0; i < allFormOptions.length; i++) {
                if (allFormOptions[i].FormId == config.formId) {
                    var thisFormOptions = allFormOptions[i];
                    allFormOptions.splice(i, 1);
                    enableClientValidation(thisFormOptions, config);
                }
            }
        }
    };
    function formSettings(form, settings) {
        if (settings) {
            form.data('mvcValidate_settings', settings);
            return;
        }
        return form.data('mvcValidate_settings');
    }
    function createValidationOptions(validationFields) {
        var rulesObj = {};
        for (var i = 0; i < validationFields.length; i++) {
            var validationField = validationFields[i];
            var fieldName = validationField.FieldName;
            rulesObj[fieldName] = createRulesForField(validationField);
        }
        return rulesObj;
    }
    function createRulesForField(validationField) {
        var validationRules = validationField.ValidationRules;
        // hook each rule into jquery
        var rulesObj = {};
        for (var i = 0; i < validationRules.length; i++) {
            var thisRule = validationRules[i];
            switch (thisRule.ValidationType) {
                case "range":
                    applyValidator_Range(rulesObj, thisRule.ValidationParameters["min"], thisRule.ValidationParameters["max"]);
                    break;
                case "regex":
                    applyValidator_RegularExpression(rulesObj, thisRule.ValidationParameters["pattern"]);
                    break;
                case "required":
                    applyValidator_Required(rulesObj);
                    break;
                case "equalto":
                    applyValidator_EqualTo(rulesObj);
                    break;
                case "length":
                    applyValidator_StringLength(rulesObj, thisRule.ValidationParameters["max"]);
                    break;
                default:
                    applyValidator_Unknown(rulesObj, thisRule.ValidationType, thisRule.ValidationParameters);
                    break;
            }
        }
        return rulesObj;
    }
    function applyValidator_Range(object, min, max) {
        object["range"] = [min, max];
    }
    function applyValidator_RegularExpression(object, pattern) {
        object["regex"] = pattern;
    }
    function applyValidator_Required(object) {
        object["required"] = true;
    }
    function applyValidator_StringLength(object, maxLength) {
        object["maxlength"] = maxLength;
    }
    function applyValidator_EqualTo(object, validationParameters) {
        object["equalTo"] = validationParameters;
    }
    function applyValidator_Unknown(object, validationType, validationParameters) {
        object[validationType] = validationParameters;
    }
    function createFieldToValidationMessageMapping(validationFields) {
        var mapping = {};
        for (var i = 0; i < validationFields.length; i++) {
            var thisField = validationFields[i];
            mapping[thisField.FieldName] = "#" + thisField.ValidationMessageId;
        }
        return mapping;
    }
    function createErrorMessagesObject(validationFields) {
        var messagesObj = {};
        for (var i = 0; i < validationFields.length; i++) {
            var thisField = validationFields[i];
            var thisFieldMessages = {};
            messagesObj[thisField.FieldName] = thisFieldMessages;
            var validationRules = thisField.ValidationRules;
            for (var j = 0; j < validationRules.length; j++) {
                var thisRule = validationRules[j];
                if (thisRule.ErrorMessage) {
                    var jQueryValidationType = thisRule.ValidationType;
                    switch (thisRule.ValidationType) {
                        case "regularExpression":
                            jQueryValidationType = "regex";
                            break;
                        case "stringLength":
                            jQueryValidationType = "maxlength";
                            break;
                    }
                    if (thisRule.ErrorMessage != '--')
                        thisFieldMessages[jQueryValidationType] = thisRule.ErrorMessage;
                }
            }
        }
        return messagesObj;
    }
    function enableClientValidation(validationContext, settings) {
        // this represents the form containing elements to be validated
        var theForm = $("#" + validationContext.FormId);
        formSettings(theForm, settings);
        var fields = validationContext.Fields;
        var rulesObj = createValidationOptions(fields);
        var fieldToMessageMappings = createFieldToValidationMessageMapping(fields);
        var errorMessagesObj = createErrorMessagesObject(fields);
        if (settings.customRules) {
            for (var key in settings.customRules) {
                if (typeof (settings.customRules[key]) == 'string') {
                    var rule = {};
                    rule[settings.customRules[key]] = true;
                    rulesObj[key] = $.extend(rulesObj[key], rule);
                }
                else {
                    rulesObj[key] = $.extend(rulesObj[key], settings.customRules[key]);
                }
            }
        }
        if (settings.customMessages) {
            for (var key in settings.customMessages) {
                errorMessagesObj[key] = $.extend(errorMessagesObj[key], settings.customMessages[key]);
            }
        }
        var options = $.extend({
            errorClass: "input-validation-error",
            errorElement: "span",
            errorPlacement: function (error, element) {
                var messageSpan = fieldToMessageMappings[element.attr("name")];
                $(messageSpan).empty();
                $(messageSpan).removeClass("field-validation-valid");
                $(messageSpan).addClass("field-validation-error");
                error.removeClass("input-validation-error");
                error.attr("_for_validation_message", messageSpan);
                error.appendTo(messageSpan);
            },
            success: function (label) {
                var messageSpan = $(label.attr("_for_validation_message"));
                $(messageSpan).empty();
                $(messageSpan).addClass("field-validation-valid");
                $(messageSpan).removeClass("field-validation-error");
            },
            messages: errorMessagesObj,
            rules: rulesObj
        }, settings);
        if (validationContext.ValidationSummaryId && validationContext.ReplaceValidationSummary) {
            options = $.extend({
                errorContainer: "#" + validationContext.ValidationSummaryId,
                errorLabelContainer: "#" + validationContext.ValidationSummaryId + " ul:first",
                wrapper: "li",
                showErrors: function (errorMap, errorList) {
                    // Add error CSS class to user-input controls with errors
                    for (var i = 0; this.errorList[i]; i++) {
                        var element = this.errorList[i].element;
                        var messageSpan = $(fieldToMessageMappings[element.name]);
                        messageSpan.html(errorList[i].message);
                        messageSpan.removeClass("field-validation-valid").addClass("field-validation-error"); //.css("display", "none");
                        $("#" + element.id).addClass("input-validation-error");
                    }
                    // Remove error CSS class from user-input controls with zero validation errors
                    for (var i = 0; this.successList[i]; i++) {
                        var element = this.successList[i];
                        var messageSpan = fieldToMessageMappings[element.name];
                        $(messageSpan).addClass("field-validation-valid").removeClass("field-validation-error");
                        $("#" + element.id).removeClass("input-validation-error");
                    }
                    var errContainer = $(this.settings.errorContainer);
                    var errLabelContainer = $("ul:first", errContainer);
                    // this call will add the error messages the validation summary
                    this.defaultShowErrors();
                    // when server-side errors still exist in the Validation Summary, don't hide it
                    if (errLabelContainer.children("li").filter(function () { return $(this).css('display') != 'none'; }).length > 0) {
                        errContainer.addClass("validation-summary-errors").removeClass("validation-summary-valid").show();
                        errLabelContainer.css("display", "block");
                    }
                    else {
                        errContainer.addClass("validation-summary-valid").removeClass("validation-summary-errors").hide();
                    }
                    // trigger resize event to recenter modal content if necessary
                    $(window).resize();
                },
                messages: errorMessagesObj,
                rules: rulesObj
            }, settings);
        }
        else if (validationContext.ValidationSummaryId && !validationContext.ReplaceValidationSummary) {
            options = $.extend(options, {
                errorContainerStatic: "#" + validationContext.ValidationSummaryId,
                showErrors: function (errorMap, errorList) {
                    var errContainer = $(this.settings.errorContainerStatic);
                    var errLabelContainer = $("ul:first", errContainer);
                    // this call will add the error messages the validation summary
                    this.defaultShowErrors();
                    // when server-side errors still exist in the Validation Summary, don't hide it
                    if (errLabelContainer.children("li").filter(function () { return $(this).css('display') != 'none'; }).length > 0) {
                        errContainer.addClass("validation-summary-errors").removeClass("validation-summary-valid").show();
                        errLabelContainer.css("display", "block");
                    }
                    else {
                        errContainer.addClass("validation-summary-valid").removeClass("validation-summary-errors").hide();
                    }
                }
            });
        }
        // call the main jquery validate method
        theForm.validate(options);
    }
})(jQuery);
jQuery.validator.addMethod("regex", function (value, element, params) {
    if (this.optional(element)) {
        return true;
    }
    var match = new RegExp(params).exec(value);
    return (match && (match.index == 0) && (match[0].length == value.length));
});
jQuery.validator.addMethod("notEqualTo", function (value, element, param) {
    return value != $(param).val();
});
jQuery.validator.addMethod("integer", function (value, element) {
    return this.optional(element) || /^-?\d+$/.test(value);
}, "Please enter a valid integer.");
jQuery.validator.addMethod("currency", function (value, element) {
    return this.optional(element) || /^-?\$?((\d+)|(\d{1,3}(,\d{3})+))(\.\d{1,2})?$/.test(value);
}, "Please enter a valid currency value.");
jQuery.validator.addMethod("hasContent", function (value, element, param) {
    var hasContent = $(value).text().trim().length > 0;
    return param && hasContent;
});
if ($.telerik && $.telerik.datetime && $.telerik.datetime.parse) {
    jQuery.validator.addMethod("date", function (value, element, param) {
        return this.optional(element) || $.telerik.datetime.parse({ value: value, format: $.telerik.cultureInfo.shortDate });
    }, $.validator.messages.date);
    jQuery.validator.addMethod("dateRange", function (value, element, param) {
        var date = $.telerik.datetime.parse({ value: value, format: $.telerik.cultureInfo.shortDate });
        return date && date >= param[0] && date <= param[1];
    }, $.validator.messages.range);
    jQuery.validator.addMethod("minDate", function (value, element, param) {
        var date = $.telerik.datetime.parse({ value: value, format: $.telerik.cultureInfo.shortDate });
        return date && date >= param;
    }, $.validator.messages.min);
    jQuery.validator.addMethod("maxDate", function (value, element, param) {
        var date = $.telerik.datetime.parse({ value: value, format: $.telerik.cultureInfo.shortDate });
        return date && date <= param;
    }, $.validator.messages.min);
}
else {
    jQuery.validator.addMethod("dateRange", function (value, element, param) {
        var date = new Date(value);
        return date >= param[0] && date <= param[1];
    }, $.validator.messages.range);
    jQuery.validator.addMethod("minDate", function (value, element, param) {
        var date = new Date(value);
        return date >= param;
    }, $.validator.messages.min);
    jQuery.validator.addMethod("maxDate", function (value, element, param) {
        var date = new Date(value);
        return date <= param;
    }, $.validator.messages.min);
}
//# sourceMappingURL=DigitalBeacon.Scripts.js.map