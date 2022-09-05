// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

/*-----------------------------------------------------------------------------
Initialize list panel with search
-----------------------------------------------------------------------------*/
(function ($) {

	$.sb.listPanel = function (element, options) {
		this.element = element;
		this.deleteConfirmText = $.sb.localization.confirmText;
		this.isRefreshing = false;
		$.extend(this, options);
		var listPanel = this;
		var $grid = $(this.gridId, this.element);
		if (listPanel.newActionUrl) {
			if (listPanel.newTooltip) {
				$grid.find('th.' + this.newClass).attr('title', listPanel.newTooltip);
			}
			$('.' + this.newClass, this.element).click(function () {
				listPanel.addItem();
				return false;
			});
		}
		this.search = $(this.searchId, this.element);
		if (this.search.length) {
			this.searchText = $(this.searchTextId, this.element);
			this.clearSearch = $(this.clearSearchId, this.element);
			if (!this.searchText.val() || this.searchText.val() == this.defaultSearchText) {
				this.clearSearch.hide()
			}
			this.searchText.focus(function () {
				if (this.value == listPanel.defaultSearchText) {
					$(this).removeClass('empty');
					this.value = '';
				}
			});
			this.searchText.blur(function () {
				if (!this.value || this.value == listPanel.defaultSearchText) {
					$(this).addClass('empty');
					this.value = listPanel.defaultSearchText;
				}
			});
			this.searchText.blur();
			this.clearSearch.click(function () {
				listPanel.searchText.val('');
				listPanel.searchText.blur();
				listPanel.search.click();
				listPanel.clearSearch.hide();
				return false;
			});
			this.search.click(function () {
				var searchTextValue = listPanel.searchText.val().trim();
				if (searchTextValue == listPanel.defaultSearchText) {
					searchTextValue = '';
				}
				if (listPanel.alwaysApplySearch || searchTextValue || listPanel.clearSearch.is(':visible') || this.form.elements.length > 1) {
					listPanel.applySearch();
				}
				if (searchTextValue) {
					listPanel.clearSearch.show();
				}
				else {
					listPanel.clearSearch.hide();
				}
				return false;
			});
			this.searchText.keypress(function (event) {
				if (event.keyCode == '13') {
					listPanel.search.click();
					return false;
				}
			});
		}
		$(this.element).bind('refresh', function (e) {
			if (listPanel.isRefreshing) {
				return;
			}
			listPanel.isRefreshing = true;
			$.sb.log(listPanel.element.id + ' refreshing...');
			listPanel.gridRefresh();
		});
		this.gridPostInit();
	}

	$.sb.listPanel.prototype = {

		gridPostInit: function () {
			var listPanel = this;
			var $grid = $(this.gridId, this.element);
			// save reference to grid
			this.grid = $grid.data('tGrid');
			if (!this.grid) {
				var attempts = ($grid.data('initAttempts') || 0) + 1;
				if (attempts >= 10)
				{
					$.sb.log('tGrid not initialized for ' + this.element.id);
				}
				else {
					$grid.data('initAttempts', attempts);
					// wait for tGrid to be initialized
					setTimeout(function () { listPanel.gridPostInit(); }, 250);
					return;
				}
			}
			// save original ajax select url
			if (this.grid.ajax) {
				this.gridAjaxUrl = this.grid.ajax.selectUrl;
			}
			// item sequencer
			if (this.itemSequencerKey) {
				var headers = $grid.find('th:not(.t-hierarchy-cell)');
				for (var i = 0; i < headers.length; i++) {
					if ($(headers[i]).hasClass(this.selectClass)) {
						this.itemSequencer = new $.sb.itemSequencer(this, $(this.grid.columns[i].format.replace('{0}', 0)).attr('href').replace('0', '{0}'));
						$.sb.itemSequencers[this.itemSequencerKey] = this.itemSequencer;
						break;
					}
				}
			}
			// perform localization
			if ($.sb.localization.culture != 'en-US') {
				this.grid.updatePager();
			}
			// add handlers
			if (!this.grid.onDataBound) {
				this.grid.onDataBound = function () { listPanel.gridDataBound() };
				$grid.bind('dataBound', this.grid.onDataBound);
				// call data bound for first-time initialization
				this.gridDataBound();
			}
			// remove no data row
			//$('tbody tr.t-no-data', this.grid.element).remove();
		},

		gridDataBound: function () {
			var listPanel = this;
			var $grid = $(this.gridId, this.element);
			if (this.enableSelect) {
				var $selectCells = $grid.find('.' + this.selectClass);
				// item sequencer
				if (this.itemSequencer) {
					var index = 0;
					$grid.find('.' + this.selectClass + ' a')
						.click(function (e) {
							e.preventDefault();
						})
						.each(function () {
							listPanel.itemSequencer.addItem(listPanel.grid.currentPage, index++, $(this).attr('data-id'));
						});
					this.itemSequencer.pages.push(this.grid.currentPage);
					if (this.itemSequencer.queuedAction) {
						var url = this.itemSequencer.getCurrent();
						if (url) {
							$.sb.log('executing queued action for: ' + url);
							this.itemSequencer.queuedAction(url);
						}
						else {
							$.sb.log('could not find previous or next item in sequencer');
						}
						this.itemSequencer.queuedAction = null;
					}
				}
				$grid.find('tbody tr').unbind('click.sbListPanel').bind('click.sbListPanel', function (e) {
					var lastSelection = $grid.data('last-selection');
					if (lastSelection && new Date().getTime() - lastSelection < 500) {
						// prevent double click
						return false;
					}
					else {
						if (!($(e.target).hasClass('t-icon'))) {
							$grid.data('last-selection', new Date().getTime());
							if (listPanel.selectItem) {
								listPanel.selectItem(this);
							}
							else {
								listPanel.defaultSelectItem(this);
							}
							if (!listPanel.maintainSelection) {
								return false;
							}
						}
						else if (!listPanel.maintainSelection) {
							var $this = $(this);
							setTimeout(function () { $this.removeClass('t-state-selected'); }, 0);
						}
					}
				});
			}
			if (this.enableDelete) {
				var deleteLinks = $grid.find('tbody .' + this.deleteClass + ' a');
				if (listPanel.deleteTooltip) {
					deleteLinks.attr('title', listPanel.deleteTooltip);
				}
				deleteLinks.unbind('click.sbListPanel').bind('click.sbListPanel', function () {
					listPanel.gridRowDelete(this);
					return false;
				});
			}
			if ((this.grid.total == undefined && this.grid.$rows().length) || this.grid.total || $(this.clearSearchId + ':visible', this.element).length) {
				if (this.enableNoItemsMessage) {
					$grid.show();
					$('.no-items', this.element).hide();
				}
				if (this.hideSearchPanelWhenEmpty) {
					$('.' + this.searchPanelClass, this.element).show();
				}
			}
			else {
				if (this.enableNoItemsMessage) {
					$grid.hide();
					$('.no-items', this.element).show();
				}
				if (this.hideSearchPanelWhenEmpty) {
					$('.' + this.searchPanelClass, this.element).hide();
				}
			}
			if (this.grid.total) {
				$('.t-pager', this.grid.element).show();
			}
			else {
				$('.t-pager', this.grid.element).hide();
				if (this.noMatchesText) {
					$('.t-status-text', this.grid.element).text(this.noMatchesText);
				}
			}
			$(this.element).trigger('dataBound');
			if (this.itemSequencer && this.itemSequencer.initiatedPaging) {
				this.itemSequencer.initiatedPaging = false;
			}
			else {
				$(window).resize();
			}
			this.isRefreshing = false;
		},

		gridRowDelete: function (link) {
			if (confirm(this.deleteConfirmText)) {
				this.removeItem(link);
			}
		},

		gridRefresh: function () {
			this.grid.ajaxRequest();
			// item sequencer
			if (this.itemSequencer) {
				this.itemSequencer.reset();
			}
		},

		getAugmentedUrl: function (baseUrl) {
			if (this.augmentParams) {
				return $.digitalbeacon.mergeParams(baseUrl, this.augmentParams);
			}
			return baseUrl;
		},

		applySearch: function () {
			this.grid.currentPage = 1;
			if (this.search.length) {
				var searchForm = this.search[0].form;
				var params = {};
				for (var i = 0; i < searchForm.elements.length; i++) {
					if (searchForm.elements[i].name == this.searchText.attr('name')
                        && searchForm.elements[i].value == this.defaultSearchText) {
						params[searchForm.elements[i].name] = '';
					}
					else {
						params[searchForm.elements[i].name] = searchForm.elements[i].value;
					}
				}
				this.grid.ajax.selectUrl = $.digitalbeacon.mergeParams(this.gridAjaxUrl, params);
			}
			else {
				this.grid.ajax.selectUrl = this.gridAjaxUrl;
			}
			this.gridRefresh();
		},

		defaultSelectItem: function (row) {
			var $selectLinks = $('.' + this.selectClass + ' a', row);
			if ($selectLinks.length) {
				// item sequencer
				if (this.itemSequencer) {
					this.itemSequencer.setCurrent($selectLinks.attr('data-id'));
				}
				$.sb.modalBox({ ajax: this.getAugmentedUrl($selectLinks.attr('href')), replace: this.selectReplacesList, width: this.modalBoxWidth, anchorTop: this.anchorSelectModal });
			}
		},

		addItem: function () {
			if (this.itemSequencer) {
				this.itemSequencer.currentIndex = -1;
			}
			$.sb.modalBox({
				ajax: this.getAugmentedUrl(this.newActionUrl),
				replace: false,
				width: this.modalBoxWidth
			});
		},

		removeItem: function (link) {
			var listPanel = this;
			$.ajax({
				url: link.href,
				type: 'DELETE',
				success: function (data) {
					if (listPanel.grid.$rows().length == 1 && listPanel.grid.currentPage > 1) {
						listPanel.grid.pageTo(listPanel.grid.currentPage - 1);
					}
					else {
						listPanel.gridRefresh();
					}
					$.sb.modalBox({ content: data, replace: false });
				}
			});
		},

		resetAlternatingRows: function () {
			var rows = this.grid.$rows();
			for (var i = 0; i < rows.length; i++) {
				if (i % 2 == 1) {
					$(rows[i]).addClass('t-alt');
				}
				else {
					$(rows[i]).removeClass('t-alt');
				}
			}
		}

	};

	$.fn.sbListPanel = function (options) {
		options = $.extend({}, $.fn.sbListPanel.defaults, options);
		return this.each(function () {
			if (!$(this).data('sbListPanel')) {
				var listPanel = new $.sb.listPanel(this, options);
				$(this).data('sbListPanel', listPanel);
			}
		});
	};

	$.fn.sbListPanel.defaults = {
		gridId: '#grid',
		enableSelect: true,
		maintainSelection: false,
		selectReplacesList: false,
		selectClass: 'select',
		newId: '#new',
		newClass: 'new',
		enableDelete: true,
		deleteClass: 'delete',
		searchPanelClass: 'search-panel',
		searchId: '#search',
		searchTextId: '#searchText',
		clearSearchId: '#clearSearch',
		alwaysApplySearch: false,
		hideSearchPanelWhenEmpty: false,
		itemSequencerKey: null,
		customGridRowSelect: null,
		enableNoItemsMessage: true,
		defaultSearchText: '',
		augmentParams: null,
		modalBoxWidth: null,
		anchorSelectModal: false
	};

	$.sb.itemSequencer = function (listPanel, urlFormat) {
		this.keys = {};
		this.pages = [];
		this.currentIndex = -1;
		this.listPanel = listPanel;
		this.urlFormat = urlFormat;
	}

	$.sb.itemSequencer.prototype = {

		reset: function () {
			this.keys = {};
			this.pages = [];
			this.currentIndex = -1;
		},
		getUrl: function (key) {
			return this.listPanel.getAugmentedUrl(this.urlFormat.replace('{0}', key));
		},
		addItem: function (currentPage, currentIndex, key) {
			this.keys[(currentPage - 1) * this.listPanel.grid.pageSize + currentIndex] = key;
		},
		setCurrent: function (key) {
			this.currentIndex = this._getIndex(key);
		},
		getCurrent: function () {
			if (!this.currentIndex >= 0) {
				return this.getUrl(this.keys[this.currentIndex]);
			}
			return null;
		},
		isFirst: function () {
			return this.currentIndex == 0;
		},
		isLast: function () {
			return this.currentIndex == this.listPanel.grid.total - 1;
		},
		getNext: function (fn) {
			if (!this.isLast()) {
				this.currentIndex++;
				if (this.keys[this.currentIndex]) {
					fn(this.getUrl(this.keys[this.currentIndex]));
				}
				else {
					this.queuedAction = fn;
				}
				this._getPage(this._getCurrentPage());
			}
		},
		getPrevious: function (fn) {
			if (!this.isFirst()) {
				this.currentIndex--;
				if (this.keys[this.currentIndex]) {
					fn(this.getUrl(this.keys[this.currentIndex]));
				}
				else {
					this.queuedAction = fn;
				}
				this._getPage(this._getCurrentPage());
			}
		},
		_getPage: function (page) {
			if (page < 1 || page > this.listPanel.grid.totalPages(this.listPanel.grid.total)) {
				return;
			}
			if (this.listPanel.grid.currentPage != page) {
				this.initiatedPaging = true;
				this.listPanel.grid.pageTo(page);
			}
		},
		_getIndex: function (key) {
			for (var x in this.keys) {
				if (this.keys[x] == key) {
					return x;
				}
			}
			return -1;
		},
		_getCurrentPage: function () {
			return Math.ceil((this.currentIndex + 1) / this.listPanel.grid.pageSize);
		}
	};

	$.sb.itemSequencers = {};

	if ($.fn.tGrid) {
		/*-----------------------------------------------------------------------------
		Redefine tGrid method to not perform initial ajaxRequest when there is no data
		-----------------------------------------------------------------------------*/
		var tGridDefaults = $.fn.tGrid.defaults;
		$.fn.tGrid = function (options) {
			var $t = $.telerik;
			return $t.create(this, {
				name: 'tGrid',
				init: function (element, options) {
					return new $t.grid(element, options);
				},
				options: options,
				success: function (grid) {
					//if (grid.$tbody.find('tr.t-no-data').length)
					//    grid.ajaxRequest();
				}
			});
		};
		$.fn.tGrid.defaults = tGridDefaults;
	}
})(jQuery);
