
if(typeof DigitalBeacon == 'undefined') DigitalBeacon = {};

DigitalBeacon.ObjectExtensions = (function() {
    function ObjectExtensions() {
    }
    return ObjectExtensions;
})();
DigitalBeacon.ObjectExtensions.hasValue = function (obj) {
    return obj;
};

DigitalBeacon.StringBuilder = (function() {
    function StringBuilder() {
        this._parts = new Array(0);
    }
    var p = StringBuilder.prototype;
    p.cat = function (str) {
        this._parts.push(str);
        return this;
    };
    p.build = function () {
        return this._parts.join('');
    };
    return StringBuilder;
})();

DigitalBeacon.StringUtils = (function() {
    function StringUtils() {
    }
    return StringUtils;
})();
DigitalBeacon.StringUtils.DateRegex = new RegExp('\\/Date\\((\\-{0,1}\\d+)\\)\\/', 'gm');
DigitalBeacon.StringUtils.formatWith = function (format, arg1, arg2, arg3, arg4, arg5) {
    arg2 = (arg2 !== undefined) ? arg2 : null;
    arg3 = (arg3 !== undefined) ? arg3 : null;
    arg4 = (arg4 !== undefined) ? arg4 : null;
    arg5 = (arg5 !== undefined) ? arg5 : null;
    var s = format;
    var args = [arg1, arg2, arg3, arg4, arg5];
    if (arg2 && arg3 && arg4 && arg5) {
        args.length = 1;
    } else if (arg3 && arg4 && arg5) {
        args.length = 2;
    } else if (arg4 && arg5) {
        args.length = 3;
    } else if (arg5) {
        args.length = 4;
    }
    for(var i = 0; i < args.length; i++) {
        var reg = new RegExp('\\{' + i + '\\}', 'gm');
        s = s.replace(reg, '' + args[i]);
    }
    return s;
};
DigitalBeacon.StringUtils.hasText = function (str) {
    return $.trim(str).length > 0;
};
DigitalBeacon.StringUtils.toDate = function (dateStr) {
    if (!dateStr) {
        return null;
    }
    var intDateStr = dateStr.replace(DigitalBeacon.StringUtils.DateRegex, '$1');
    if (intDateStr) {
        return new Date(parseInt(intDateStr));
    }
    var intDateVal = Date.parse(dateStr);
    return intDateVal !== NaN ? new Date(intDateVal) : null;
};
DigitalBeacon.StringUtils.expandSiteRelativeText = function (text) {
    var regex = new RegExp('\"~/', 'gm');
    return DigitalBeacon.StringUtils.hasText(text) ? text.replace(regex, '\"' + ($.digitalbeacon.appContextPath === '/' ? '' : $.digitalbeacon.appContextPath) + '/') : text;
};
DigitalBeacon.StringUtils.toSiteRelativeText = function (text) {
    if ($.digitalbeacon.appContextPath === '/') {
        return text;
    }
    var regex = new RegExp('\"' + $.digitalbeacon.appContextPath + '/', 'gm');
    return (DigitalBeacon.StringUtils.hasText($.digitalbeacon.appContextPath) && DigitalBeacon.StringUtils.hasText(text)) ? text.replace(regex, '\"~/') : text;
};

DigitalBeacon.Utils = (function() {
    function Utils() {
    }
    return Utils;
})();
DigitalBeacon.Utils.mergeParams = function (url, args) {
    var key = null;
    var $key_enum = Object.keys(args).GetEnumerator();
    while($key_enum.MoveNext()) {
        key = $key_enum.get_Current();
        var regExp = new RegExp('({0})=([^\u0026]*)'.formatWith(key), 'gi');
        if (regExp.test(url)) {
            url = url.replace(regExp, '$1=' + args[key]);
        } else {
            var sb = new DigitalBeacon.StringBuilder();
            sb.cat(url);
            if (url.indexOf('?') < 0) {
                sb.cat('?');
            } else {
                sb.cat('\u0026');
            }
            url = sb.cat(key).cat('=').cat(encodeURIComponent(args[key])).build();
        }
    }
    return url;
};

String.prototype.formatWith = function(arg1, arg2, arg3, arg4, arg5) {
    var format = this;
    return DigitalBeacon.StringUtils.formatWith(format, arg1, arg2, arg3, arg4, arg5);
};
String.prototype.hasText = function() {
    var str = this;
    return DigitalBeacon.StringUtils.hasText(str);
};
String.prototype.toDate = function() {
    var str = this;
    return DigitalBeacon.StringUtils.toDate(str);
};
String.prototype.expandSiteRelativeText = function() {
    var str = this;
    return DigitalBeacon.StringUtils.expandSiteRelativeText(str);
};
String.prototype.toSiteRelativeText = function() {
    var str = this;
    return DigitalBeacon.StringUtils.toSiteRelativeText(str);
};
