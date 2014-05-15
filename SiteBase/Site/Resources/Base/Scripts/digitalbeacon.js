// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

(function($) {
    /*-----------------------------------------------------------------------------
    digitalbeacon singleton
    -----------------------------------------------------------------------------*/
	$.digitalbeacon = new function() {
		this.appContextPath = document.appContextPath || '';
        this.importedFiles = {};
        /*-----------------------------------------------------------------------------
        Log message to console
        -----------------------------------------------------------------------------*/
        this.log = function(message) {
            if (window.console && window.console.debug) {
                console.debug(message);
            }
        };
		/*-----------------------------------------------------------------------------
        Helper method to add jQuery plugins
        -----------------------------------------------------------------------------*/
        this.registerJQueryPlugin = function (name, fn) {
        	$.fn[name] = function (options) {
        		var component;
        		var chain = this.each(function () {
        			var val = fn(this, options);
        			component = component || val;
        		});
        		if (options) {
        			return chain;
        		}
        		return component || chain;
        	};
        };
		/*-----------------------------------------------------------------------------
        Dynamically load JavaScript file
        -----------------------------------------------------------------------------*/
        this.loadJsFile = function(jsFile) {
            if (this.importedFiles[jsFile]) {
                return;
            }
            var scriptElement = document.createElement('script');
            scriptElement.type = 'text/javascript';
            scriptElement.src = this.resolveUrl(jsFile);
            this.log('loaded: ' + scriptElement.src);
            $('head')[0].appendChild(scriptElement);
            this.importedFiles[jsFile] = true;
        };
        /*-----------------------------------------------------------------------------
        Dynamically load CSS file
        -----------------------------------------------------------------------------*/
        this.loadCssFile = function(cssFile) {
            if (this.importedFiles[cssFile]) {
                return;
            }
            var linkElement = document.createElement('link');
            linkElement.type = 'text/css';
            linkElement.rel = 'stylesheet';
            linkElement.href = this.resolveUrl(cssFile);
            this.log('loaded: ' + linkElement.href);
            $('head')[0].appendChild(linkElement);
            this.importedFiles[cssFile] = true;
        };
        /*-----------------------------------------------------------------------------
        Html encode input
        -----------------------------------------------------------------------------*/
        this.htmlEncode = function(input) {
            return $('<div/>').text(input).html();
        };
        /*-----------------------------------------------------------------------------
        Html decode input
        -----------------------------------------------------------------------------*/
        this.htmlDecode = function(input) {
            return $('<div/>').html(input).text();
        };
        /*-----------------------------------------------------------------------------
        Resolve app context relative url
        -----------------------------------------------------------------------------*/
        this.resolveUrl = function(relativeUrl) {
            return relativeUrl.replace('~', this.appContextPath);
        };
        /*-----------------------------------------------------------------------------
        Merge parameters to url
        -----------------------------------------------------------------------------*/
        this.mergeParams = function(url, args) {
            for (var key in args) {
                var regExp = new RegExp($.telerik.formatString('({0})=([^&]*)', key), 'gi');
                if (regExp.test(url)) {
                    url = url.replace(regExp, '$1=' + args[key]);
                }
                else {
                    var sb = new $.telerik.stringBuilder();
                    sb.cat(url);
                    if (url.indexOf('?') < 0) {
                        sb.cat('?');
                    }
                    else {
                        sb.cat('&');
                    }
                    url = sb.cat(key).cat('=').cat(encodeURIComponent(args[key])).string();
                }
            }
            return url;
        };
    };
    /*-----------------------------------------------------------------------------
    Add trim method to String object if not defined
    -----------------------------------------------------------------------------*/
    if (!String.prototype.trim) {
        String.prototype.trim = function() {
            return $.trim(this);
        }
    }
    /*-----------------------------------------------------------------------------
    Add indexOf method to Array object if not defined
    -----------------------------------------------------------------------------*/
    if (!Array.prototype.indexOf) {
        Array.prototype.indexOf = function(obj, fromIndex) {
            if (fromIndex == null) {
                fromIndex = 0;
            }
            else if (fromIndex < 0) {
                fromIndex = Math.max(0, this.length + fromIndex);
            }
            for (var i = fromIndex, j = this.length; i < j; i++) {
                if (this[i] === obj) {
                    return i;
                }
            }
            return -1;
        };
    }
})(jQuery);