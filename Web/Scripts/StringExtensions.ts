// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

interface String {
    formatWith(arg1, arg2?, arg3?, arg4?, arg5?): string;
    hasText(): boolean;
    toDate(): Date;
    expandSiteRelativeText(): string;
    toSiteRelativeText(): string;
}

namespace DigitalBeacon {
    String.prototype.formatWith = function (arg1, arg2 = null, arg3 = null, arg4 = null, arg5 = null): string {
        return StringUtils.formatWith(this, arg1, arg2, arg3, arg4, arg5);
    };

    String.prototype.hasText = function (): boolean {
        return StringUtils.hasText(this);
    };

    String.prototype.toDate = function (): Date {
        return StringUtils.toDate(this);
    };

    String.prototype.expandSiteRelativeText = function (): string {
        return StringUtils.expandSiteRelativeText(this);
    };

    String.prototype.toSiteRelativeText = function (): string {
        return StringUtils.toSiteRelativeText(this);
    };
}
