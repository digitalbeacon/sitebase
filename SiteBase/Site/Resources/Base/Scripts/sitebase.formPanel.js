// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

/*-----------------------------------------------------------------------------
Initialize form panel
-----------------------------------------------------------------------------*/
(function($) {

	$.sb.formPanel = function(element, options) {
		this.element = element;
		this.deleteConfirmText = $.sb.localization.confirmText;
		$.extend(this, options);
		var formPanel = this;
		// save form
		this.form = $('form', this.element)[0];
		// list panel
		if (this.listPanelId) {
			var $listPanel = $(this.listPanelId);
			if ($listPanel.length == 1) {
				this.listPanelElement = $listPanel[0];
			}
		}
		// item sequencer
		if ($.sb.itemSequencers && this.itemSequencerKey) {
			this.itemSequencer = $.sb.itemSequencers[this.itemSequencerKey];
		}
		if (this.ajax) {
			$.sb.addTooltips();
			// delete
			if (this.deleteUrl) {
				$(this.deleteId, this.element).click(function() {
					formPanel.deleteItem();
					return false;
				});
			}
			// form target (for simulating ajax submit with file input)
			if (this.formTarget) {
				$('#' + this.formTarget, this.element).load(function() {
					var textarea = $(this).contents().find('textarea');
					if (textarea.length == 1) {
						formPanel.updateContent(textarea.text(), 'submit');
					}
				});
			}
			// cancel
			$(this.cancelId, this.element).click(function() {
				formPanel.cancel();
				return false;
			});
			// previous and next
			if (this.itemSequencer && this.itemSequencer.currentIndex >= 0) {
				var $previous = $(this.previousId, this.element).show();
				var $next = $(this.nextId, this.element).show();
				if (!this.itemSequencer.isFirst()) {
					if (formPanel.previousTooltip) {
						$previous.attr('title', formPanel.previousTooltip);
					}
					$previous.click(function() {
						formPanel.previous();
						return false;
					});
				}
				else {
					$previous.removeClass('link').addClass('disabled');
				}
				if (!this.itemSequencer.isLast()) {
					if (formPanel.nextTooltip) {
						$next.attr('title', formPanel.nextTooltip);
					}
					$next.click(function() {
						formPanel.next();
						return false;
					});
				}
				else {
					$next.removeClass('link').addClass('disabled');
				}
			}
		}
		this.validationOptions = {
			formId: this.form.id,
			submitHandler: function(form) { formPanel.submitForm(form); }
		};
		// init validation
		if (this.initValidation) {
			$.sb.initClientFormValidation(this.validationOptions);
		}
	}

	$.sb.formPanel.prototype = {
		prepareForm: function(form) { },
		updateContent: function(response, action) {
			if (this.listPanelElement && action != 'cancel' && action != 'previous' && action != 'next') {
				$(this.listPanelElement).trigger('refresh');
			}
			$.sb.modalBox({ content: response, width: this.modalBoxWidth, reposition: (action != 'previous' && action != 'next') });
			//$('#modalBox .modalBoxBodyContent').html(response);
		},
		cancel: function() {
			if ($.sb.isModalBoxVisible()) {
				$.sb.closeModalBox();
			}
			else {
				var formPanel = this;
				$.post(formPanel.form.action, { cancel: 'cancel' }, function(response) {
					formPanel.updateContent(response, 'cancel');
				});
			}
		},
		submitForm: function(form) {
			var formPanel = this;
			if ($.sb.checkFormSubmission(form)) {
				formPanel.prepareForm(form);
				if (formPanel.formTarget) {
					form.action = $.sb.getRenderPartialWrappedUrl(form.action);
					form.target = formPanel.formTarget;
					form.submit();
				}
				else if (formPanel.ajaxGet) {
					$.get(form.action, $(form).serialize(), function(response) {
						formPanel.updateContent(response, 'submit');
					});
				}
				else if (formPanel.ajax) {
					$.post(form.action, $(form).serialize(), function(response) {
						formPanel.updateContent(response, 'submit');
					});
				}
				else {
					form.submit();
				}
			}
			return false;
		},
		action: function(url, data, actionName) {
			var formPanel = this;
			if ($.sb.checkFormSubmission(this.form)) {
				$.post(url, data, function(response) {
					formPanel.updateContent(response, actionName);
				});
			}
		},
		deleteItem: function() {
			if (confirm(this.deleteConfirmText)) {
				var formPanel = this;
				$.ajax({
					url: this.deleteUrl,
					type: 'DELETE',
					success: function(response) {
						formPanel.updateContent(response, 'delete');
					}
				});
			}
		},
		previous: function() {
			var formPanel = this;
			this.itemSequencer.getPrevious(function(url) {
				$.post(url, null, function(response) {
					formPanel.updateContent(response, 'previous');
				});
			});
		},
		next: function() {
			var formPanel = this;
			this.itemSequencer.getNext(function(url) {
				$.post(url, null, function(response) {
					formPanel.updateContent(response, 'previous');
				});
			});
		},
		resetSubmitFlag: function() {
			$.sb.resetFormSubmission(this.form);
		}
	};

	$.fn.sbFormPanel = function(options) {
		options = $.extend({}, $.fn.sbFormPanel.defaults, options);
		return this.each(function() {
			if (!$(this).data('sbFormPanel')) {
				var formPanel = new $.sb.formPanel(this, options);
				$(this).data('sbFormPanel', formPanel);
			}
		});
	};

	$.fn.sbFormPanel.defaults = {
		cancelId: '#cancel',
		deleteId: '#delete',
		previousId: '#previous',
		nextId: '#next',
		deleteUrl: null,
		ajax: false,
		ajaxGet: false,
		initValidation: true,
		itemSequencerKey: null,
		formTarget: null,
		modalBoxWidth: null
	};

})(jQuery);
