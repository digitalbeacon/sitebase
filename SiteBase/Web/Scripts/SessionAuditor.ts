// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

namespace DigitalBeacon.SiteBase {
    /// <summary>
    /// Polls session status and redirects to login page when session expires
    /// </summary>
    class SessionAuditor {
        
        public static readonly DefaultPollingInterval: number = 300;
        public static readonly DefaultNotificationTime: number = 60;

        private static readonly DefaultAuthSessionUrl: string = "~/.authSession.ashx";
        private static readonly DefaultExtendSessionUrl: string = "~/account/extendSession";
        private static readonly DefaultSignOutUrl: string = "~/identity/signOut";
        private static readonly DefaultNotificationHeading: string = "Session Expiration";
        private static readonly DefaultNotificationMessage: string = "Your session is set to expire in one minute.";
        private static readonly DefaultExtendButtonText: string = "Extend Session";

        // private static SessionAuditor _instance;

        private _authSessionUrl: string;
        private _extendSessionUrl: string;
        private _signOutUrl: string;
        private _intervalTimerId: number;
        private _notificationTimerId: number;
        private _signOutTimerId: number;
        private _pollingInterval: number;
        private _notificationTime: number;
        private _displayNotification: boolean = true;
        private _notificationHeading: string;
        private _notificationMessage: string;
        private _extendButtonText: string;

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

        get authSessionUrl(): string {
            return this._authSessionUrl;
        }

        set authSessionUrl(value: string) {
            this._authSessionUrl = value;
        }

        get extendSessionUrl(): string {
            return this._extendSessionUrl;
        }

        set extendSessionUrl(value: string) {
            this._extendSessionUrl = value;
        }

        get signOutUrl(): string {
            return this._signOutUrl;
        }

        set signOutUrl(value: string) {
            this._signOutUrl = value;
        }

        get pollingInterval(): number {
            return this._pollingInterval;
        }

        set pollingInterval(value: number) {
            this._pollingInterval = value;
        }

        get notificationTime(): number {
            return this._notificationTime;
        }

        set notificationTime(value: number) {
            this._notificationTime = value;
        }

        get displayNotification(): boolean {
            return this._displayNotification;
        }

        set displayNotification(value: boolean) {
            this._displayNotification = value;
        }

        get notificationHeading(): string {
            return this._notificationHeading;
        }

        set notificationHeading(value: string) {
            this._notificationHeading = value;
        }

        get notificationMessage(): string {
            return this._notificationMessage;
        }

        set notificationMessage(value: string) {
            this._notificationMessage = value;
        }

        get extendButtonText(): string {
            return this._extendButtonText;
        }

        set extendButtonText(value: string) {
            this._extendButtonText = value;
        }

        get notificationTimerId(): number {
            return this._notificationTimerId;
        }

        set notificationTimerId(value: number) {
            this._notificationTimerId = value;
        }

        get signOutTimerId(): number {
            return this._signOutTimerId;
        }

        set signOutTimerId(value: number) {
            this._signOutTimerId = value;
        }

        get intervalTimerId(): number {
            return this._intervalTimerId;
        }

        set intervalTimerId(value: number) {
            this._intervalTimerId = value;
        }

        public start() {
            stop();
            this._intervalTimerId = setInterval(() => {
                this.checkSession();
            }, this.pollingInterval * 1000);
        }

        public stop() {
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
        }

        checkSession() {
            jQuery.post(this.authSessionUrl, null, (data, status, xhr) => {
                this.checkSessionOnSuccess(data);
            });
        }

        checkSessionOnSuccess(data: number) {
            let secondsRemaining = data;
            digitalbeacon.log(secondsRemaining + " seconds remaining...");
            if (this._displayNotification && this._notificationTimerId <= 0 && secondsRemaining < this.notificationTime + this.pollingInterval) {
                this._notificationTimerId = window.setTimeout(() => {
                    this.showNotification();
                }, (secondsRemaining - this.notificationTime) * 1000);
                this._signOutTimerId = window.setTimeout(() => {
                    this.signOut();
                }, secondsRemaining * 1000);
                clearInterval(this._intervalTimerId);
                this._intervalTimerId = 0;
            }
        }

        showNotification() {
            jQuery.post(this.authSessionUrl, null, (data, status, xhr) => {
                var secondsRemaining = data;
                if (secondsRemaining < this.notificationTime + this.pollingInterval) {
                    sitebase.displayMessage(
                        this.notificationMessage.formatWith(this.notificationTime, this.notificationTime / 60),
                        this.notificationHeading, "", this.extendButtonText, () => {
                            this.extendSession();
                        });
                } else {
                    // session was extended
                    this.start();
                }
            });
        }

        signOut() {
            jQuery.post(this.authSessionUrl, null, (data, status, xhr) => {
                let secondsRemaining = data;
                if (secondsRemaining < this.pollingInterval) {
                    window.location.href = this.signOutUrl;
                } else {
                    // session was extended
                    this.start();
                }
            });
        }

        extendSession() {
            jQuery.post(this.extendSessionUrl);
            this.start();
        }
    }
}
