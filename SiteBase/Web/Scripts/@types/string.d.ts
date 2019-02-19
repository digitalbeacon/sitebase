declare interface String {
    formatWith(arg1, arg2?, arg3?, arg4?, arg5?): string;
    hasText(): boolean;
    toDate(): Date;
    expandSiteRelativeText(): string;
    toSiteRelativeText(): string;
}
