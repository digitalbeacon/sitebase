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
//# sourceMappingURL=StringBuilder.js.map