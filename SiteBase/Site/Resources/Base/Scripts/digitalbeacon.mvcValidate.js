// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

(function ($) {

	$.fn.mvcValidate = function (mvcClientValidationMetadata, settings) {
		var config = {
			onkeyup: false
		};
		if (settings) {
			$.extend(config, settings);
		}
		var allFormOptions = mvcClientValidationMetadata;
		if (allFormOptions) {
			for (var i = 0; i < allFormOptions.length; i++) {
				if (allFormOptions[i].FormId == config.formId) {
					var thisFormOptions = allFormOptions[i];
					allFormOptions.splice(i, 1);
					enableClientValidation(thisFormOptions, config);
				}
			}
		}
	};

	function formSettings(form, settings) {
		if (settings) {
			form.data('mvcValidate_settings', settings);
			return;
		}
		return form.data('mvcValidate_settings');
	}

	function createValidationOptions(validationFields) {
		var rulesObj = {};
		for (var i = 0; i < validationFields.length; i++) {
			var validationField = validationFields[i];
			var fieldName = validationField.FieldName;
			rulesObj[fieldName] = createRulesForField(validationField);
		}
		return rulesObj;
	}

	function createRulesForField(validationField) {
		var validationRules = validationField.ValidationRules;
		// hook each rule into jquery
		var rulesObj = {};
		for (var i = 0; i < validationRules.length; i++) {
			var thisRule = validationRules[i];
			switch (thisRule.ValidationType) {
				case "range":
					applyValidator_Range(rulesObj, thisRule.ValidationParameters["min"], thisRule.ValidationParameters["max"]);
					break;

				case "regex":
					applyValidator_RegularExpression(rulesObj, thisRule.ValidationParameters["pattern"]);
					break;

				case "required":
					applyValidator_Required(rulesObj);
					break;

				case "equalto":
					applyValidator_EqualTo(rulesObj);
					break;

				case "length":
					applyValidator_StringLength(rulesObj, thisRule.ValidationParameters["max"]);
					break;

				default:
					applyValidator_Unknown(rulesObj, thisRule.ValidationType, thisRule.ValidationParameters);
					break;
			}
		}
		return rulesObj;
	}

	function applyValidator_Range(object, min, max) {
		object["range"] = [min, max];
	}

	function applyValidator_RegularExpression(object, pattern) {
		object["regex"] = pattern;
	}

	function applyValidator_Required(object) {
		object["required"] = true;
	}

	function applyValidator_StringLength(object, maxLength) {
		object["maxlength"] = maxLength;
	}

	function applyValidator_EqualTo(object, validationParameters) {
		object["equalTo"] = validationParameters;
	}

	function applyValidator_Unknown(object, validationType, validationParameters) {
		object[validationType] = validationParameters;
	}

	function createFieldToValidationMessageMapping(validationFields) {
		var mapping = {};
		for (var i = 0; i < validationFields.length; i++) {
			var thisField = validationFields[i];
			mapping[thisField.FieldName] = "#" + thisField.ValidationMessageId;
		}
		return mapping;
	}

	function createErrorMessagesObject(validationFields) {
		var messagesObj = {};

		for (var i = 0; i < validationFields.length; i++) {
			var thisField = validationFields[i];
			var thisFieldMessages = {};
			messagesObj[thisField.FieldName] = thisFieldMessages;
			var validationRules = thisField.ValidationRules;

			for (var j = 0; j < validationRules.length; j++) {
				var thisRule = validationRules[j];
				if (thisRule.ErrorMessage) {
					var jQueryValidationType = thisRule.ValidationType;
					switch (thisRule.ValidationType) {
						case "regularExpression":
							jQueryValidationType = "regex";
							break;

						case "stringLength":
							jQueryValidationType = "maxlength";
							break;
					}

					if (thisRule.ErrorMessage != '--')
						thisFieldMessages[jQueryValidationType] = thisRule.ErrorMessage;
				}
			}
		}
		return messagesObj;
	}

	function enableClientValidation(validationContext, settings) {
		// this represents the form containing elements to be validated
		var theForm = $("#" + validationContext.FormId);
		formSettings(theForm, settings);

		var fields = validationContext.Fields;
		var rulesObj = createValidationOptions(fields);
		var fieldToMessageMappings = createFieldToValidationMessageMapping(fields);
		var errorMessagesObj = createErrorMessagesObject(fields);

		if (settings.customRules) {
			for (var key in settings.customRules) {
				if (typeof (settings.customRules[key]) == 'string') {
					var rule = {};
					rule[settings.customRules[key]] = true;
					rulesObj[key] = $.extend(rulesObj[key], rule)
				}
				else {
					rulesObj[key] = $.extend(rulesObj[key], settings.customRules[key]);
				}
			}
		}

		if (settings.customMessages) {
			for (var key in settings.customMessages) {
				errorMessagesObj[key] = $.extend(errorMessagesObj[key], settings.customMessages[key]);
			}
		}

		var options = $.extend({
			errorClass: "input-validation-error",
			errorElement: "span",
			errorPlacement: function (error, element) {
				var messageSpan = fieldToMessageMappings[element.attr("name")];
				$(messageSpan).empty();
				$(messageSpan).removeClass("field-validation-valid");
				$(messageSpan).addClass("field-validation-error");
				error.removeClass("input-validation-error");
				error.attr("_for_validation_message", messageSpan);
				error.appendTo(messageSpan);
			},
			success: function (label) {
				var messageSpan = $(label.attr("_for_validation_message"));
				$(messageSpan).empty();
				$(messageSpan).addClass("field-validation-valid");
				$(messageSpan).removeClass("field-validation-error");
			},
			messages: errorMessagesObj,
			rules: rulesObj
		}, settings);


		if (validationContext.ValidationSummaryId && validationContext.ReplaceValidationSummary) {
			options = $.extend({
				errorContainer: "#" + validationContext.ValidationSummaryId,
				errorLabelContainer: "#" + validationContext.ValidationSummaryId + " ul:first",
				wrapper: "li",
				showErrors: function (errorMap, errorList) {
					// Add error CSS class to user-input controls with errors
					for (var i = 0; this.errorList[i]; i++) {
						var element = this.errorList[i].element;
						var messageSpan = $(fieldToMessageMappings[element.name]);
						messageSpan.html(errorList[i].message);
						messageSpan.removeClass("field-validation-valid").addClass("field-validation-error"); //.css("display", "none");
						$("#" + element.id).addClass("input-validation-error");
					}
					// Remove error CSS class from user-input controls with zero validation errors
					for (var i = 0; this.successList[i]; i++) {
						var element = this.successList[i];
						var messageSpan = fieldToMessageMappings[element.name];
						$(messageSpan).addClass("field-validation-valid").removeClass("field-validation-error");
						$("#" + element.id).removeClass("input-validation-error");
					}

					var errContainer = $(this.settings.errorContainer);
					var errLabelContainer = $("ul:first", errContainer);
					// this call will add the error messages the validation summary
					this.defaultShowErrors();
					// when server-side errors still exist in the Validation Summary, don't hide it
					if (errLabelContainer.children("li").filter(function () { return $(this).css('display') != 'none'; }).length > 0) {
						errContainer.addClass("validation-summary-errors").removeClass("validation-summary-valid").show();
						errLabelContainer.css("display", "block");
					}
					else {
						errContainer.addClass("validation-summary-valid").removeClass("validation-summary-errors").hide();
					}
					// trigger resize event to recenter modal content if necessary
					$(window).resize();
				},
				messages: errorMessagesObj,
				rules: rulesObj
			}, settings);
		}
		else if (validationContext.ValidationSummaryId && !validationContext.ReplaceValidationSummary) {
			options = $.extend(options, {
				errorContainerStatic: "#" + validationContext.ValidationSummaryId,
				showErrors: function (errorMap, errorList) {
					var errContainer = $(this.settings.errorContainerStatic);
					var errLabelContainer = $("ul:first", errContainer);
					// this call will add the error messages the validation summary
					this.defaultShowErrors();
					// when server-side errors still exist in the Validation Summary, don't hide it
					if (errLabelContainer.children("li").filter(function () { return $(this).css('display') != 'none'; }).length > 0) {
						errContainer.addClass("validation-summary-errors").removeClass("validation-summary-valid").show();
						errLabelContainer.css("display", "block");
					}
					else {
						errContainer.addClass("validation-summary-valid").removeClass("validation-summary-errors").hide();
					}
				}
			});
		}
		// call the main jquery validate method
		theForm.validate(options);
	}
})(jQuery);

jQuery.validator.addMethod("regex", function(value, element, params) {
	if (this.optional(element)) {
		return true;
	}
	var match = new RegExp(params).exec(value);
	return (match && (match.index == 0) && (match[0].length == value.length));
});

jQuery.validator.addMethod("notEqualTo", function(value, element, param) {
	return value != $(param).val();
});

jQuery.validator.addMethod("integer", function(value, element) {
	return this.optional(element) || /^-?\d+$/.test(value);
}, "Please enter a valid integer.");

jQuery.validator.addMethod("currency", function(value, element) {
	return this.optional(element) || /^-?\$?((\d+)|(\d{1,3}(,\d{3})+))(\.\d{1,2})?$/.test(value);
}, "Please enter a valid currency value.");

jQuery.validator.addMethod("hasContent", function (value, element, param) {
	var hasContent = $(value).text().trim().length > 0;
	return param && hasContent;
});

if ($.telerik && $.telerik.datetime && $.telerik.datetime.parse) {
	jQuery.validator.addMethod("date", function(value, element, param) {
		return this.optional(element) || $.telerik.datetime.parse({ value: value, format: $.telerik.cultureInfo.shortDate });
	}, $.validator.messages.date);
	
	jQuery.validator.addMethod("dateRange", function(value, element, param) {
		var date = $.telerik.datetime.parse({ value: value, format: $.telerik.cultureInfo.shortDate });
		return date && date >= param[0] && date <= param[1];
	}, $.validator.messages.range);

	jQuery.validator.addMethod("minDate", function(value, element, param) {
		var date = $.telerik.datetime.parse({ value: value, format: $.telerik.cultureInfo.shortDate });
		return date && date >= param;
	}, $.validator.messages.min);

	jQuery.validator.addMethod("maxDate", function(value, element, param) {
		var date = $.telerik.datetime.parse({ value: value, format: $.telerik.cultureInfo.shortDate });
		return date && date <= param;
	}, $.validator.messages.min);
}
else {
	jQuery.validator.addMethod("dateRange", function(value, element, param) {
		var date = new Date(value);
		return date >= param[0] && date <= param[1];
	}, $.validator.messages.range);

	jQuery.validator.addMethod("minDate", function(value, element, param) {
		var date = new Date(value);
		return date >= param;
	}, $.validator.messages.min);

	jQuery.validator.addMethod("maxDate", function(value, element, param) {
		var date = new Date(value);
		return date <= param;
	}, $.validator.messages.min);
}