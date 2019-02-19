// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

namespace DigitalBeacon {
    export class Utils {
        public static mergeParams(url: string, args): string {
            for (let key in Object.keys(args)) {
                var regExp = new RegExp("({0})=([^&]*)".formatWith(key), "gi");
                if (regExp.test(url)) {
                    url = url.replace(regExp, "$1=" + args[key]);
                } else {
                    var sb = new StringBuilder();
                    sb.cat(url);
                    if (url.indexOf("?") < 0) {
                        sb.cat("?");
                    } else {
                        sb.cat("&");
                    }
                    url = sb.cat(key).cat("=").cat(encodeURIComponent(args[key])).build();
                }
            }
            return url;
        }

        public static isString(obj): boolean {
            return $.digitalbeacon.isOfType(obj, "string");
        }

        public static isObject(obj): boolean {
            return obj != null && $.digitalbeacon.isOfType(obj, "object");
        }

        public static isDefined(obj): boolean {
            return !$.digitalbeacon.isOfType(obj, "undefined");
        }

        public static convertDateStringsToDates(input, level: number = 0) {
            if (!Utils.isObject(input)) {
                return input;
            }
            for (var key in Object.keys(input)) {
                if (!input.hasOwnProperty(key)) continue;
                var value = input[key];
                if (Utils.isString(value) && StringUtils.isDateString(value)) {
                    input[key] = StringUtils.toDate(value);
                } else if (value && Utils.isObject(value) && level < 10) {
                    Utils.convertDateStringsToDates(value, level + 1);
                }
            }
            return input;
        }
    }
}
