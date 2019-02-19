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
            return StringUtils.hasText(text) ? text.replace(regex, "\"" + (digitalbeacon.appContextPath == "/" ? "" : digitalbeacon.appContextPath) + "/") : text;
        };
        StringUtils.toSiteRelativeText = function (text) {
            if (digitalbeacon.appContextPath == "/") {
                return text;
            }
            var regex = new RegExp("\"" + digitalbeacon.appContextPath + "/", "gm");
            return (StringUtils.hasText(digitalbeacon.appContextPath) && StringUtils.hasText(text)) ? text.replace(regex, "\"~/") : text;
        };
        StringUtils.DateRegex = new RegExp("\/Date\((\-{0,1}\d+)\)\/", "gm");
        return StringUtils;
    }());
    DigitalBeacon.StringUtils = StringUtils;
})(DigitalBeacon || (DigitalBeacon = {}));
//# sourceMappingURL=StringUtils.js.map