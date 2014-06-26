// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

(function($) {
	if ($.fn.modalBox) {

		/*-----------------------------------------------------------------------------
		Display content in a modal box
		-----------------------------------------------------------------------------*/
		$.digitalbeacon.modalBox = function(options) {
			if (typeof options === "string") {
				options = { content: options };
			}
			if (!options.width && !options.nonPersistentWidth) {
				options.width = $.digitalbeacon.modalBox.defaults.width;
			}
			if (options.replace == undefined) {
				options.replace = true;
			}
			if (options.reposition == undefined) {
				options.reposition = true;
			}
			if (options.ajax || options.ajaxPost) {
				$.ajax({
					url: options.ajaxPost ? options.ajaxPost : options.ajax,
					type: options.ajaxPost ? 'POST' : 'GET',
					data: options.data,
					success: function(response) {
						options.ajax = null;
						options.ajaxPost = null;
						options.content = response;
						$.digitalbeacon.modalBox(options);
					}
				});
			}
			else {
				var modalBox = $.digitalbeacon.getModalBox();
				if (modalBox.length) {
					if (modalBox.hasClass('loading')) {
						$.digitalbeacon._executeCloseHandler(modalBox, true);
					}
					var replaceLi = false;
					if (options.replace) {
						replaceLi = !($.digitalbeacon._executeCloseHandler(modalBox, false));
					}
					var modalBoxContent = modalBox.find('.modalBoxBodyContent');
					var ul = modalBoxContent.find('> ul#modalBoxStack');
					var $currentContents = modalBoxContent.find('> :not(ul#modalBoxStack)');
					var updateElement = modalBoxContent;
					var elementsToHide;
					if (replaceLi) {
						updateElement = ul.find('> li:last').data('modalBoxOptions', options);
					}
					else if (options.replace || $currentContents.length == 0) {
						modalBox.data('modalBoxOptions', options);
					}
					else {
						elementsToHide = $currentContents;
						if (ul.length) {
							elementsToHide = elementsToHide.add(ul.find('> li'));
						}
						else {
							ul = $('<ul/>').attr('id', 'modalBoxStack').appendTo(modalBoxContent);
						}
						updateElement = $('<li/>').attr('id', 'modalBoxStackItem').data('modalBoxOptions', options).appendTo(ul);
					}
					var content = $.digitalbeacon.modalBox.defaults.noDataText;
					if (options.content) {
						content = options.content;
					}
					else if (options.element) {
						content = $(options.element).html();
					}
					$.digitalbeacon._updateModalBoxContent(modalBox, updateElement, elementsToHide, content, options);
				}
				else {
					var settings = {
						killModalboxWithCloseButtonOnly: true,
						minimalTopSpacingOfModalbox: 25
					};
					if (options.content) {
						$.extend(settings, { directCall: { data: options.content} });
					}
					else if (options.element) {
						$.extend(settings, { directCall: { element: options.element} });
					}
					else {
						$.extend(settings, { directCall: { data: $.digitalbeacon.modalBox.defaults.noDataText} });
					}
					if (options.width && typeof options.width == 'number') {
						$.extend(settings, { setWidthOfModalLayer: options.width });
					}
					else if (options.nonPersistentWidth && typeof options.nonPersistentWidth == 'number') {
						$.extend(settings, { setWidthOfModalLayer: options.nonPersistentWidth });
					}
					if (options.clickOutsideToClose) {
						$.extend(settings, { killModalboxWithCloseButtonOnly: !options.clickOutsideToClose });
					}
					if (options.success) {
						$.extend(settings, { callFunctionAfterSuccess: options.success });
					}
					$.fn.modalBox(settings);
					modalBox = $('#modalBox');
					//modalBox.fadeTo(0, 0.01);
					modalBox.addClass('visible');
					if (options.width && typeof options.width == 'string' && options.width != 'auto') {
						modalBox.css('width', options.width);
						$.digitalbeacon.centerModalBox(modalBox);
					}
					else if (options.nonPersistentWidth
								&& typeof options.nonPersistentWidth == 'string'
								&& options.width != 'auto') {
						modalBox.css('width', options.nonPersistentWidth);
						$.digitalbeacon.centerModalBox(modalBox);
					}
					//modalBox.fadeTo(0, 1);
					modalBox.data('modalBoxOptions', options);
					var faderLayer = $('#modalBoxFaderLayer');
					faderLayer.fadeTo(0, 0.01);
					faderLayer.addClass('visible');
					faderLayer.fadeTo(300, 0.4);
					$('body').addClass("modalBoxOpen");
				}
			}
		};
		/*-----------------------------------------------------------------------------
		Modal box defaults
		-----------------------------------------------------------------------------*/
		$.digitalbeacon.modalBox.defaults = {
			loadingText: 'Loading...',
			noDataText: '<p>No content available</p>',
			width: 'auto',
			loadingWidth: 'auto'
		};
		/*-----------------------------------------------------------------------------
		Close modal box
		-----------------------------------------------------------------------------*/
		$.digitalbeacon.closeModalBox = function() {
			$.fn.modalBox.close();
		};
		/*-----------------------------------------------------------------------------
		Show loading modal box
		-----------------------------------------------------------------------------*/
		$.digitalbeacon.showLoadingModalBox = function(delay) {
			if (delay > 0) {
				$.digitalbeacon.modalBoxLoadingTimerId = setTimeout(function() { $.digitalbeacon.modalBoxLoading(0); }, delay);
			}
			else {
				$.digitalbeacon.modalBox({
					content: '<div id="modalBoxAjaxLoader">' + $.digitalbeacon.modalBox.defaults.loadingText + '</div>',
					nonPersistentWidth: $.digitalbeacon.modalBox.defaults.loadingWidth,
					replace: false,
					reposition: true
				});
				$.digitalbeacon.getModalBox().addClass('loading');
			}
		};
		/*-----------------------------------------------------------------------------
		Hide loading modalBox
		-----------------------------------------------------------------------------*/
		$.digitalbeacon.hideLoadingModalBox = function(modalBox) {
			if (!modalBox) {
				modalBox = $.digitalbeacon.getModalBox();
			}
			if (modalBox.length) {
				if (modalBox.hasClass('loading')) {
					$.digitalbeacon.closeModalBox();
					modalBox.removeClass('loading');
				}
			}
			if ($.digitalbeacon.modalBoxLoadingTimerId) {
				window.clearTimeout($.digitalbeacon.modalBoxLoadingTimerId);
				$.digitalbeacon.modalBoxLoadingTimerId = null;
			}
		};
		/*-----------------------------------------------------------------------------
		Get visible modal box
		-----------------------------------------------------------------------------*/
		$.digitalbeacon.getModalBox = function() {
			return $('body > #modalBox:visible');
		};
		/*-----------------------------------------------------------------------------
		Check to see if modal box is displayed
		-----------------------------------------------------------------------------*/
		$.digitalbeacon.isModalBoxVisible = function() {
			return $.digitalbeacon.getModalBox().length;
		};
		/*-----------------------------------------------------------------------------
		Center modal box
		-----------------------------------------------------------------------------*/
		$.digitalbeacon.centerModalBox = function(modalBox) {
			if (!modalBox) {
				modalBox = $.digitalbeacon.getModalBox();
			}
			if (modalBox.length) {
			$(window).resize();
			}
		};
		/*-----------------------------------------------------------------------------
		Explicitly set modal box on close handler
		-----------------------------------------------------------------------------*/
		$.digitalbeacon.modalBoxOnClose = function(handler) {
			setTimeout(function() {
				var modalBox = $.digitalbeacon.getModalBox();
				if (modalBox.length) {
					var ul = modalBox.find('.modalBoxBodyContent > ul#modalBoxStack');
					var container = modalBox;
					if (ul.length) {
						container = ul.find('> li:last');
					}
					var options = container.data('modalBoxOptions');
					if (!options.close) {
						options.close = handler;
					}
					else {
						var proxiedHandler = options.close;
						options.close = function() {
							handler();
							proxiedHandler();
						};
					}
				}
			}, 0);
		};
		/*-----------------------------------------------------------------------------
		Update modal box content
		-----------------------------------------------------------------------------*/
		$.digitalbeacon._updateModalBoxContent = function(modalBox, elementToUpdate, elementsToHide, content, options) {
			var loading = modalBox.hasClass('loading');
			var reposition = options.reposition || loading;
			if (reposition) {
				modalBox.removeClass('visible');
				//modalBox.fadeTo(0, 0.01);
				if (options.width) {
					modalBox.css('width', options.width);
				}
				else if (options.nonPersistentWidth) {
					modalBox.css('width', options.nonPersistentWidth);
				}
			}
			elementToUpdate.html(content);
			if (elementsToHide) {
				elementsToHide.hide();
			}
			if (options.success) {
				options.success();
			}
			if (reposition) {
				$(window).resize();
				//modalBox.fadeTo(0, 1);
				modalBox.addClass('visible');
			}
			if (loading) {
				modalBox.removeClass('loading');
			}
		};
		/*-----------------------------------------------------------------------------
		Execute close handler for item on top of the stack
		-----------------------------------------------------------------------------*/
		$.digitalbeacon._executeCloseHandler = function(modalBox, removeItem, recenter) {
			if (!modalBox) {
				modalBox = $.digitalbeacon.getModalBox();
			}
			if (modalBox.length) {
				var modalBoxContent = modalBox.find('.modalBoxBodyContent');
				var lastItem = modalBoxContent.find('> :not(ul#modalBoxStack)');
				var ul = modalBoxContent.find('> ul#modalBoxStack');
				var options;
				if (ul.length) {
					var li = ul.find('> li:last');
					options = li.data('modalBoxOptions');
					if (options.close) {
						try {
							options.close();
						}
						catch (ex) { }
					}
					if (removeItem) {
						li.remove();
						if (ul.find('> li').length) {
							li = ul.find('> li:last');
							options = li.data('modalBoxOptions');
							if (options.width) {
								modalBox.css('width', options.width);
							}
							li.show();
						}
						else {
							ul.remove();
							options = modalBox.data('modalBoxOptions');
							if (options.width) {
								modalBox.css('width', options.width);
							}
							lastItem.show();
						}
						if (recenter) {
							modalBox.removeClass('visible');
							$.digitalbeacon.centerModalBox(modalBox);
							modalBox.addClass('visible');
						}
					}
					else {
						li.data('modalBoxOptions', null);
					}
					return false;
				}
				else {
					options = modalBox.data('modalBoxOptions');
					if (options && options.close) {
						try {
							options.close();
						}
						catch (ex) { }
					}
					if (removeItem) {
						lastItem.remove();
					}
					else {
						modalBox.data('modalBoxOptions', null);
					}
					return true;
				}
				return false;
			}
		};
		/*-----------------------------------------------------------------------------
		Add custom handler to modal box close operation 
		-----------------------------------------------------------------------------*/
		var proxiedClose = $.fn.modalBox.close;
		$.fn.modalBox.close = function(settings) {
			var modalBox = $.digitalbeacon.getModalBox();
			if ($.digitalbeacon._executeCloseHandler(modalBox, true, true)) {
				var faderLayer = $('#modalBoxFaderLayer');
				modalBox.fadeTo(0, 0.01);
				faderLayer.fadeTo(300, 0.01, function() { proxiedClose(settings); });
				$('body').removeClass("modalBoxOpen");
			}
		};
	}
})(jQuery);