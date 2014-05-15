// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Html;
using System.Collections;
using jQueryLib;


namespace DigitalBeacon.SiteBase
{
	/// <summary>
	/// Polls session status and redirects to login page when session expires
	/// </summary>
	public class SessionAuditor
	{
		public const int DefaultPollingInterval = 300;
		public const int DefaultNotificationTime = 60;

		private const string DefaultAuthSessionUrl = "~/.authSession.ashx";
		private const string DefaultExtendSessionUrl = "~/account/extendSession";
		private const string DefaultSignOutUrl = "~/identity/signOut";
		private const string DefaultNotificationHeading = "Session Expiration";
		private const string DefaultNotificationMessage = "Your session is set to expire in one minute.";
		private const string DefaultExtendButtonText = "Extend Session";

		private static SessionAuditor _instance;

		private string _authSessionUrl;
		private string _extendSessionUrl;
		private string _signOutUrl;
		private int _intervalTimerId;
		private int _notificationTimerId;
		private int _signOutTimerId;
		private int _pollingInterval;
		private int _notificationTime;
		private bool _displayNotification = true;
		private string _notificationHeading;
		private string _notificationMessage;
		private string _extendButtonText;

		public static SessionAuditor instance()
		{
			if (_instance == null)
			{
				_instance = new SessionAuditor();
			}
			return _instance;
		}

		private SessionAuditor()
		{
		}

		public int pollingInterval
		{
			get { return _pollingInterval > 0 ? _pollingInterval : DefaultPollingInterval; }
			set { _pollingInterval = value; }
		}

		public int notificationTime
		{
			get { return _notificationTime > 0 ? _notificationTime : DefaultNotificationTime; }
			set { _notificationTime = value; }
		}

		public bool displayNotification
		{
			get { return _displayNotification; }
			set { _displayNotification = value; }
		}

		public string authSessionUrl
		{
			get { return digitalbeacon.resolveUrl((dynamic)_authSessionUrl || DefaultAuthSessionUrl); }
			set { _authSessionUrl = value; }
		}

		public string extendSessionUrl
		{
			get { return digitalbeacon.resolveUrl((dynamic)_extendSessionUrl || DefaultExtendSessionUrl); }
			set { _extendSessionUrl = value; }
		}

		public string signOutUrl
		{
			get { return digitalbeacon.resolveUrl((dynamic)_signOutUrl || DefaultSignOutUrl); }
			set { _signOutUrl = value; }
		}

		public string notificationHeading
		{
			get { return (dynamic)_notificationHeading || DefaultNotificationHeading; }
			set { _notificationHeading = value; }
		}

		public string notificationMessage
		{
			get { return (dynamic)_notificationMessage || DefaultNotificationMessage; }
			set { _notificationMessage = value; }
		}

		public string extendButtonText
		{
			get { return (dynamic)_extendButtonText || DefaultExtendButtonText; }
			set { _extendButtonText = value; }
		}

		public void start()
		{
			stop();
			_intervalTimerId = window.setInterval((Action)(() => { checkSession(); }), pollingInterval * 1000);
		}

		public void stop()
		{
			if (_intervalTimerId > 0)
			{
				window.clearInterval(_intervalTimerId);
				_intervalTimerId = 0;
			}
			if (_notificationTimerId > 0)
			{
				window.clearTimeout(_notificationTimerId);
				_notificationTimerId = 0;
			}
			if (_signOutTimerId > 0)
			{
				window.clearTimeout(_signOutTimerId);
				_signOutTimerId = 0;
			}
		}

		private void checkSession()
		{
			jQuery.post(authSessionUrl, null, (data, status, xhr) => { checkSessionOnSuccess(data); });
		}

		private void checkSessionOnSuccess(object data)
		{
			var secondsRemaining = (int)data;
			digitalbeacon.log(secondsRemaining + " seconds remaining...");
			if (_displayNotification && _notificationTimerId <= 0 && secondsRemaining < notificationTime + pollingInterval)
			{
				_notificationTimerId = window.setTimeout((Action)(() => { showNotification(); }), (secondsRemaining - notificationTime) * 1000);
				_signOutTimerId = window.setTimeout((Action)(() => { signOut(); }), secondsRemaining * 1000);
				window.clearInterval(_intervalTimerId);
				_intervalTimerId = 0;
			}
		}

		private void showNotification()
		{
			jQuery.post(authSessionUrl, null, (data, status, xhr) =>
			{
				var secondsRemaining = (int)data;
				if (secondsRemaining < notificationTime + pollingInterval)
				{
					sitebase.displayMessage(
						notificationMessage.formatWith(notificationTime, notificationTime / 60),
						notificationHeading, "", extendButtonText, () => { extendSession(); });
				}
				else
				{
					// session was extended
					start();
				}
			});
		}

		private void signOut()
		{
			jQuery.post(authSessionUrl, null, (data, status, xhr) =>
			{
				int secondsRemaining = (int)data;
				if (secondsRemaining < pollingInterval)
				{
					window.location.href = signOutUrl;
				}
				else
				{
					// session was extended
					start();
				}
			});
		}

		private void extendSession()
		{
			jQuery.post(extendSessionUrl);
			start();
		}
	}
}
