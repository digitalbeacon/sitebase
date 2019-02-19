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
            return digitalbeacon.isOfType(obj, "string");
        };
        Utils.isObject = function (obj) {
            return obj != null && digitalbeacon.isOfType(obj, "object");
        };
        Utils.isDefined = function (obj) {
            return !digitalbeacon.isOfType(obj, "undefined");
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
//# sourceMappingURL=Utils.js.map