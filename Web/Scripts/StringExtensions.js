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
//# sourceMappingURL=StringExtensions.js.map