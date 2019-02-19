// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

namespace DigitalBeacon.SiteBase
{
	export class Editor
	{
		private _wrapperElement : Element;
		private _textArea : JQueryStatic;
		private _editorElement : JQueryStatic;
		private _editor;
		private  _useEditorLink : JQueryStatic;
		private _useTextBoxLink : JQueryStatic;
		private _handleSiteRelativeUrls : boolean = true;
		private _defaultToTextBox : boolean = false;

		static Editor()
		{
			digitalbeacon.registerJQueryPlugin("sbEditor",
				(Func<object, object, object>)((element, options) =>
				{
					var editor = (dynamic)jQuery.Select(element).data("sbEditor");
					if (!editor)
					{
						new Editor(element, options);
					}
					return editor;
				}));
		}

		public Editor(object element, dynamic options)
		{
			if (options != null)
			{
				_handleSiteRelativeUrls = options.handleSiteRelativeUrls ?? _handleSiteRelativeUrls;
				_defaultToTextBox = options.defaultToTextBox ?? _defaultToTextBox;
			}

			_textArea = jQuery.Select(element);
			_wrapperElement = _textArea.parent().get(0);
			_editorElement = jQuery.Select("table", _wrapperElement);
			_useEditorLink = jQuery.Select(".useEditor", _wrapperElement);
			_useTextBoxLink = jQuery.Select(".useTextBox", _wrapperElement);

			_useEditorLink.click(delegate(Event e) { useEditor(e); });
			_useTextBoxLink.click(delegate(Event e) { useTextBox(e); });

			_textArea.data("sbEditor", this);

			if (!_defaultToTextBox)
			{
				updateTelerikEditorValue();
			}
		}

		private void withTelerikEditor(Action action)
		{
			if (_editor)
			{
				action();
				return;
			}
			var attempts = ((int)(_textArea.data("initAttempts") ?? 0)) + 1;
			if (attempts >= 10)
			{
				digitalbeacon.log("Could not initialize Editor component for {0}.".formatWith(_textArea.attr("id")));
				return;
			}
			dynamic tEditor = _editorElement.data("tEditor");
			if (!tEditor)
			{
				_textArea.data("initAttempts", attempts);
				window.setTimeout((Action)(() => withTelerikEditor(action)), 250);
				return;
			}
			_editor = tEditor;
			action();
		}

		private void useEditor(Event e)
		{
			updateTelerikEditorValue();
			toggleComponents(true);
			if (e != null)
			{
				e.preventDefault();
			}
		}

		private void useTextBox(Event e)
		{
			updateTextAreaValue();
			toggleComponents(false);
			if (e != null)
			{
				e.preventDefault();
			}
		}

		private void toggleComponents(bool useEditor)
		{
			_editorElement.toggle(useEditor);
			_useTextBoxLink.toggle(useEditor);
			_textArea.toggle(!useEditor);
			_useEditorLink.toggle(!useEditor);
		}

		private void updateTelerikEditorValue()
		{
			withTelerikEditor(() =>
			{
				var val = (string)_textArea.val();
				if (_handleSiteRelativeUrls)
				{
					val = val.expandSiteRelativeText();
				}
				_editor.value(val);
			});
		}

		private void updateTextAreaValue()
		{
			withTelerikEditor(() =>
			{
				var val = (string)_editor.value();
				if (_handleSiteRelativeUrls)
				{
					val = val.toSiteRelativeText();
				}
				_textArea.val(val);
			});
		}

		public void prepareForValidation()
		{
			if (_editorElement.@is(":visible"))
			{
				updateTextAreaValue();
			}
		}

		public void prepareForSubmission()
		{
			if (_editorElement.@is(":visible"))
			{
				updateTextAreaValue();
			}
			_textArea.val(digitalbeacon.htmlEncode((string)_textArea.val()));
		}

		public string value(string val = null)
		{
			if (val)
			{
				_textArea.val(val);
				updateTelerikEditorValue();
			}
			else
			{
				prepareForValidation();
			}
			return (string)_textArea.val();
		}

		public string encodedValue()
		{
			return digitalbeacon.htmlEncode(value());
		}
	}
}
