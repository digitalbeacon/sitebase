// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

$(document).ready(function() {
	$.sitebase.debug = false;
	$.sitebase.init();
});

(function($) {
	/*-----------------------------------------------------------------------------
	sitebase singleton
	-----------------------------------------------------------------------------*/
	$.sitebase = new function() {
		this.debug = false;
		this.isMobile = document.isMobile || false;
		this.ajaxTimerId1 = 0;
		this.ajaxTimerId2 = 0;
		this.ajaxCounter = 0;
		this.showMessageSummaryAsModalBox = true;
		this.localization = {
			culture: 'en-US',
			closeLabel: 'Close',
			messageHeading: 'Message',
			errorHeading: 'Oops! Something went wrong...',
			errorText: 'An error occurred while processing this request.',
			loadingText: 'Loading',
			noDataText: 'No content available',
			confirmText: 'Are you sure?',
			formAlreadySubmitted: 'This form has already been submitted. Please refresh this page to attempt this request again.'
		};
		/*-----------------------------------------------------------------------------
		Performs global site initialization
		-----------------------------------------------------------------------------*/
		this.init = function() {
			this._addSignOutConfirmation();
			if (!this.isMobile)
			{
				this._initAjaxFeedback();
				this._setTooltipDefaults();
				this.displayMessageSummaryAsModalBox();
				this.addTooltips(); // decorate server-generated field validation error messages
			}
		};
		/*-----------------------------------------------------------------------------
		Log message to javascript console
		-----------------------------------------------------------------------------*/
		this.log = function(message) {
			$.digitalbeacon.log(message);
		};
		/*-----------------------------------------------------------------------------
		Display confirm dialog
		-----------------------------------------------------------------------------*/
		this.confirm = function(message) {
			if (message) {
				return confirm(message);
			}
			return confirm(this.localization.confirmText);
		};
		/*-----------------------------------------------------------------------------
		Display a message in a modal box
		-----------------------------------------------------------------------------*/
		this.displayMessage = function(message, heading, cssClass, additionalButtonText, additionalButtonAction) {
			if (!heading) {
				heading = $.sitebase.localization.messageHeading;
			}
			var additionalButtonText = additionalButtonText ? 
				'<input type="submit" id="ok" class="ok" value="' + additionalButtonText + '" />' : '';
			$.sitebase.modalBox({
				content: '<h1>' + heading + '</h1><div class="message-panel">'
						+ (cssClass ? '<div class="' + cssClass + '">' : '<div>')
						+ message + '</div><div class="button-panel">' + additionalButtonText 
						+ '<input type="submit" id="close" class="close-message-panel cancel" value="'
						+ $.sitebase.localization.closeLabel + '" /></div></div>',
				success: function() {
					var close = $('#modalBox input.close-message-panel:last')
						.click(function() { $.sitebase.closeModalBox(); })
						.keyup(function(e) { if (e.keyCode == '27') { return this.click(); } });
					if (additionalButtonText && additionalButtonAction)
					{
						$('#modalBox input.ok:last')
						.click(function() { additionalButtonAction(); $.sitebase.closeModalBox(); })
					}
					setTimeout(function() { close.focus(); }, 100);
				},
				replace: false,
				clickOutsideToClose: true
			});
		};
		/*-----------------------------------------------------------------------------
		Display an error message in a modal box
		-----------------------------------------------------------------------------*/
		this.displayError = function(message, heading) {
			if (!heading) {
				heading = $.sitebase.localization.errorHeading;
			}
			this.displayMessage(message, heading, 'error-summary');
		};
		/*-----------------------------------------------------------------------------
		Display message summary as modal box
		-----------------------------------------------------------------------------*/
		this.displayMessageSummaryAsModalBox = function() {
			if ($.sitebase.showMessageSummaryAsModalBox) {
				var messagePanel = $('.message-panel:first');
				if (messagePanel.text()) {
					messagePanel.hide();
					this.displayMessage(messagePanel.html());
				}
			}
		};
		/*-----------------------------------------------------------------------------
		Checks for form double submission
		-----------------------------------------------------------------------------*/
		this.checkFormSubmission = function(form) {
			var $form = $(form);
			if ($form.data('sumbitted')) {
				$.sb.displayError($.sitebase.localization.formAlreadySubmitted);
				//$.sb.log('form has already been submitted');
				return false;
			}
			else {
				$form.data('sumbitted', true);
				return true;
			}
		};
		/*-----------------------------------------------------------------------------
		Resets form submission flag
		-----------------------------------------------------------------------------*/
		this.resetFormSubmission = function(form) {
			$(form).data('sumbitted', false);
		};
		/*-----------------------------------------------------------------------------
		Submit form via ajax
		-----------------------------------------------------------------------------*/
		this.ajaxSubmit = function(form) {
			if ($.sb.checkFormSubmission(form)) {
				$.post(form.action, $(form).serialize(), function(response) { $.sb.modalBox({ content: response }); });
			}
			return false;
		};
		/*-----------------------------------------------------------------------------
		Initialize client form validation 
		-----------------------------------------------------------------------------*/
		this.initClientFormValidation = function(options) {
			if (window.mvcClientValidationMetadata) {
				var settings = {
					debug: true,
					highlight: function(element, errorClass) {
						$.sitebase.addTooltips();
						// trigger resize event to recenter modal content if necessary
						$(window).resize();
					},
					submitHandler: function(form) {
						if ($.sb.checkFormSubmission(form)) {
							form.submit();
						}
					}
				};
				if (options) {
					settings = $.extend(settings, options);
				}
				$().mvcValidate(window.mvcClientValidationMetadata, settings);
			}
		};
		/*-----------------------------------------------------------------------------
		Add tooltips for help text and field validation errors
		-----------------------------------------------------------------------------*/
		this.addTooltips = function() {
			$('.form-row-help').each(function() {
				$(this).show().find('.form-row-tip').bt($(this).text());
			});
			$('.field-validation-error').each(function() {
				if ($(this).text()) {
					$(this).bt($(this).text());
					$(this).text('').css('display', 'block');
				}
			});
		};
		/*-----------------------------------------------------------------------------
		Get url with partial diplay type parameter set
		-----------------------------------------------------------------------------*/
		this.getRenderPartialUrl = function(url) {
			return $.digitalbeacon.mergeParams(url, { renderType: 'partial' });
		};
		/*-----------------------------------------------------------------------------
		Get url with partial wrapped diplay type parameter set
		-----------------------------------------------------------------------------*/
		this.getRenderPartialWrappedUrl = function(url) {
			return $.digitalbeacon.mergeParams(url, { renderType: 'partialWrapped' });
		};
		/*-----------------------------------------------------------------------------
		Display content in a modal box
		-----------------------------------------------------------------------------*/
		this.modalBox = function(options) {
			$.digitalbeacon.modalBox(options);
		};
		/*-----------------------------------------------------------------------------
		Explicitly set modal box on close handler
		-----------------------------------------------------------------------------*/
		this.modalBoxOnClose = function(handler) {
			$.digitalbeacon.modalBoxOnClose(handler);
		};
		/*-----------------------------------------------------------------------------
		Close modal box
		-----------------------------------------------------------------------------*/
		this.closeModalBox = function() {
			$.digitalbeacon.closeModalBox();
		};
		/*-----------------------------------------------------------------------------
		Check to see if modal box is displayed
		-----------------------------------------------------------------------------*/
		this.isModalBoxVisible = function() {
			return $.digitalbeacon.isModalBoxVisible();
		};
		/*-----------------------------------------------------------------------------
		Get visible modal box
		-----------------------------------------------------------------------------*/
		this.getModalBox = function() {
			return $.digitalbeacon.getModalBox();
		};
		/*-----------------------------------------------------------------------------
		Show loading modal box
		-----------------------------------------------------------------------------*/
		this.showLoadingModalBox = function() {
			$.digitalbeacon.showLoadingModalBox();
		};
		/*-----------------------------------------------------------------------------
		Hide loading modal box
		-----------------------------------------------------------------------------*/
		this.hideLoadingModalBox = function() {
			$.digitalbeacon.hideLoadingModalBox();
		};
		/*-----------------------------------------------------------------------------
		Ajax start handler
		-----------------------------------------------------------------------------*/
		this.onAjaxStart = function () {
			$.sitebase.ajaxCounter++;
			if (!$.sitebase.$mask) {
				$.sitebase.$mask = $('#mask');
			}
			if ($.sitebase.ajaxTimerId1 || $.sitebase.ajaxTimerId2)
			{
				return;
			}
			$.sitebase.ajaxTimerId1 = setTimeout(function () {
				$.sitebase.$mask.fadeTo(0, 0.01); // enable the mask without really showing it
				//$mask.find('input').focus(); // capture focus to prevent input
			}, 250);
			$.sitebase.ajaxTimerId2 = setTimeout(function () {
				$.sitebase.showLoadingModalBox();
			}, 1000);
		};
		/*-----------------------------------------------------------------------------
		Ajax end handler
		-----------------------------------------------------------------------------*/
		this.onAjaxEnd = function () {
			$.sitebase.ajaxCounter--;
			if (!$.sitebase.$mask) { return; }
			if ($.sitebase.ajaxCounter <= 0)
			{
				if ($.sitebase.ajaxTimerId1 || $.sitebase.ajaxTimerId2)
				{
					window.clearTimeout($.sitebase.ajaxTimerId1);
					window.clearTimeout($.sitebase.ajaxTimerId2);
					$.sitebase.ajaxTimerId1 = 0;
					$.sitebase.ajaxTimerId2 = 0;
				}
				$.sitebase.ajaxCounter = 0;
				$.sitebase.$mask.fadeOut(0);
				$.sitebase.hideLoadingModalBox();
			}
		}
		/*-----------------------------------------------------------------------------
		Set defaults for tooltips
		-----------------------------------------------------------------------------*/
		this._setTooltipDefaults = function() {
			$.bt.options.fill = '#fff';
			$.bt.options.strokeStyle = '#000';
			$.bt.options.spikeLength = 15;
			$.bt.options.spikeGirth = 10;
			$.bt.options.padding = 8;
			$.bt.options.cornerRadius = 0;
			$.bt.options.shadow = true;
			$.bt.options.shadowColor = 'rgba(0,0,0,.5)';
			$.bt.options.shadowBlur = 8;
			$.bt.options.shadowOffsetX = 4;
			$.bt.options.shadowOffsetY = 4;
			$.bt.options.width = '300px';
			$.bt.options.shrinkToFit = true;
			$.bt.options.hoverIntentOpts = {
				interval: 100,
				timeout: 100
			};
		};
		/*-----------------------------------------------------------------------------
		Add confirmation prompt for sign out link
		-----------------------------------------------------------------------------*/
		this._addSignOutConfirmation = function() {
			$('a[href$="signOut"]').click(function() {
				if (confirm($(this).text() + '?')) {
					return true;
				}
				else {
					return false;
				}
			});
		};
		/*-----------------------------------------------------------------------------
		Initialize ajax feedback 
		-----------------------------------------------------------------------------*/
		this._initAjaxFeedback = function() {
			$.ajaxSetup({ cache: false });
			this.$mask = $('#mask');
			this.$mask.ajaxStart(this.onAjaxStart);
			this.$mask.ajaxStop(this.onAjaxEnd);
			$(document).ajaxComplete(function(event, xmlHttpRequest, ajaxOptions) {
				if ($.sb.debug) {
					$.sb.log(ajaxOptions.url);
					$.sb.log(ajaxOptions.data);
					$.sb.log(xmlHttpRequest.status);
					$.sb.log(xmlHttpRequest.responseText);
				}
			});
			$(document).ajaxError(function(event, xmlHttpRequest, ajaxOptions, thrownError) {
				$.sb.log(xmlHttpRequest.status);
				$.sb.log(xmlHttpRequest.responseText);
				if (xmlHttpRequest.status == 403) {
					window.location = window.location;
				}
				else {
					$.sb.displayError($.sitebase.localization.errorText);
				}
			});
		};
	};
	/*-----------------------------------------------------------------------------
	Alias sitebase object
	-----------------------------------------------------------------------------*/
	$.sb = $.sitebase;
})(jQuery);
/*-----------------------------------------------------------------------------
Localized strings for modal box
-----------------------------------------------------------------------------*/
var modalboxLocalizedStrings = {
	messageCloseWindow: $.sitebase.localization.closeLabel,
	messageAjaxLoader: $.sitebase.localization.loadingText,
	errorMessageIfNoDataAvailable: $.sitebase.localization.noDataText,
	errorMessageXMLHttpRequest: $.sitebase.localization.errorText,
	errorMessageTextStatusError: $.sitebase.localization.errorText
};
$.digitalbeacon.modalBox.defaults.loadingText = $.sitebase.localization.loadingText;
$.digitalbeacon.modalBox.defaults.noDataText = $.sitebase.localization.noDataText;
//$.digitalbeacon.modalBox.defaults.width = 'auto';
//$.digitalbeacon.modalBox.defaults.width = '65em';
//$.digitalbeacon.modalBox.defaults.loadingWidth = '8em';