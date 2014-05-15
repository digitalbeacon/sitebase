// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using System.ComponentModel;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;
using FluentValidation;
using FluentValidation.Attributes;

namespace DigitalBeacon.SiteBase.Models.Modules
{
	[Validator(typeof(EditModelValidator))]
	public class EditModel : BaseViewModel
	{
		private IList<ModuleSetting> _settings;
		private IList<CustomSetting> _customSettings;

		[ReadOnly(true)]
		public bool Global { get; set; }
		[ReadOnly(true)]
		public bool CreateInstance { get; set; }
		[ReadOnly(true)]
		public bool AllowMultiple { get; set; }
		[ReadOnly(true)]
		public IDictionary<long, ModuleSettingDefinitionEntity> Definitions { get; set; }

		public long Id { get; set; }

		[Required]
		[StringLength(ModuleEntity.NameMaxLength)]
		[LocalizedDisplayName("Modules.Name.Label")]
		public string Name { get; set; }

		[StringLength(ModuleEntity.UrlMaxLength)]
		[ManagedRegularExpression(WebConstants.AppContextRelativeUrlRegexKey, "Modules.Url.Invalid")]
		[LocalizedDisplayName("Modules.Url.Label")]
		public string Url { get; set; }

		// action attributes
		public string Cancel { get; set; }
		public string Delete { get; set; }

		[ReadOnly(true)]
		public IList<CustomSetting> CustomSettings
		{
			get
			{
				if (_customSettings == null)
				{
					_customSettings = new List<CustomSetting>();
				}
				return _customSettings; 
			}
		}

		public IList<ModuleSetting> Settings
		{
			get { return _settings; }
			set
			{
				_settings = value;
				// this code is here so that each setting can have all the
				// necessary information to perform validation using the
				// default model binder
				if (value != null && value.Count > 0 && Definitions != null)
				{
					foreach (var s in value)
					{
						var def = Definitions[s.DefinitionId];
						s.Name = def.Name;
						s.Required = def.Required;
						s.TypeId = (long)def.ModuleSettingType;
						s.Min = def.MinValue;
						s.Max = def.MaxValue;
					}
				}
			}
		}
	}

	public class EditModelValidator : BaseValidator<EditModel>
	{
		public EditModelValidator()
		{
			RuleFor(x => x.Settings).SetCollectionValidator(new ModuleSettingValidator());
		}
	}
}