// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.Util;
using DigitalBeacon.Web.Validation;
using FluentValidation;
using FluentValidation.Attributes;

namespace DigitalBeacon.SiteBase.Models.Modules
{
	[Validator(typeof(ModuleSettingValidator))]
	public class ModuleSetting
	{
		public long Id { get; set; }
		public long TypeId { get; set; }
		public string Name { get; set; }
		public bool Required { get; set; }
		public bool Default { get; set; }
		public string Value { get; set; }
		public string Subject { get; set; }
		public double? Min { get; set; }
		public double? Max { get; set; }
		public long DefinitionId { get; set; }

		public ModuleSettingType Type
		{
			get { return (ModuleSettingType)TypeId; }
		}

		public double? NumberValue
		{
			get { return Value.ToDouble(); }
			set { Value = value.HasValue ? value.Value.ToString() : null; }
		}

		public DateTime? DateValue
		{
			get { return Value.ToDate(); }
			//get { return Value.HasText() ? new DateTime(Convert.ToInt64(Value)) : (DateTime?)null; }
			set { Value = value.HasValue ? value.Value.Ticks.ToString() : null; }
		}

		public DateTime? DateMin
		{
			get { return Min.HasValue ? new DateTime(Convert.ToInt64(Min.Value)) : (DateTime?)null; }
			set { Min = value.HasValue ? value.Value.Ticks : (long?)null; }
		}

		public DateTime? DateMax
		{
			get { return Max.HasValue ? new DateTime(Convert.ToInt64(Max.Value)) : (DateTime?)null; }
			set { Max = value.HasValue ? value.Value.Ticks : (long?)null; }
		}

		public bool BooleanValue
		{
			get { return Value.HasText() ? TextUtil.IsEqualIgnoreCase(Value, Boolean.TrueString) : false; }
			set { Value = TextUtil.IsEqualIgnoreCase(value, Boolean.TrueString) ? Boolean.TrueString : Boolean.FalseString; }
		}

		public Int32? IntegerValue
		{
			get { return Value.ToInt32(); }
			set { Value = value.HasValue ? value.Value.ToString() : null; }
		}

		public Int32? IntegerMin
		{
			get { return Min.HasValue ? Convert.ToInt32(Min.Value) : (Int32?)null; }
			set { Min = value.HasValue ? value.Value : (int?)null; }
		}

		public Int32? IntegerMax
		{
			get { return Max.HasValue ? Convert.ToInt32(Max.Value) : (Int32?)null; }
			set { Max = value.HasValue ? value.Value : (int?)null; }
		}

		public Decimal? DecimalValue
		{
			get { return Value.ToDecimal(); }
			set { Value = value.HasValue ? value.Value.ToString() : null; }
		}

		public Decimal? DecimalMin
		{
			get { return Min.HasValue ? Convert.ToDecimal(Min.Value) : (Decimal?)null; }
			set { Min = value.HasValue ? Convert.ToDouble(value.Value) : (double?)null; }
		}

		public Decimal? DecimalMax
		{
			get { return Max.HasValue ? Convert.ToDecimal(Max.Value) : (Decimal?)null; }
			set { Max = value.HasValue ? Convert.ToDouble(value.Value) : (double?)null; }
		}
	}

	public class ModuleSettingValidator : BaseValidator<ModuleSetting>
	{
		public ModuleSettingValidator()
		{
			RuleFor(x => x.Subject)
				.NotNullOrBlank()
				.When(x => x.Required && x.Type == ModuleSettingType.MessageTemplate)
				.WithLocalizedMessage("Modules.Error.Subject.Required", x => x.Name);
			RuleFor(x => x.Value)
				.Cascade(FluentValidation.CascadeMode.StopOnFirstFailure)
				.NotNullOrBlank()
				.When(x => x.Required)
				.Unless(x => x.Type == ModuleSettingType.Custom || x.Type == ModuleSettingType.Boolean)
				.WithLocalizedMessage("Validation.Error.Required", x => x.Name)
				.Must((x, y) => x.NumberValue.HasValue)
				.When(x => x.Type == ModuleSettingType.Number && x.Value.HasText())
				.WithLocalizedMessage("Validation.Error.Number", x => x.Name)
				.Must((x, y) => x.IntegerValue.HasValue)
				.When(x => x.Type == ModuleSettingType.Integer && x.Value.HasText())
				.WithLocalizedMessage("Validation.Error.Integer", x => x.Name)
				.Must((x, y) => x.DecimalValue.HasValue)
				.When(x => x.Type == ModuleSettingType.Currency && x.Value.HasText())
				.WithLocalizedMessage("Validation.Error.Currency", x => x.Name)
				.Must((x, y) => x.DateValue.HasValue)
				.When(x => x.Type == ModuleSettingType.Date && x.Value.HasText())
				.WithLocalizedMessage("Validation.Error.Date", x => x.Name)
				.Must((x, y) => x.DateValue.Value >= x.DateMin.Value && x.DateValue.Value <= x.DateMax.Value)
				.When(x => x.Type == ModuleSettingType.Date && x.DateValue.HasValue && x.DateMin.HasValue && x.DateMax.HasValue)
				.WithLocalizedMessage("Validation.Error.Range", x => x.Name, x => x.DateMin.Value.ToShortDateString(), x => x.DateMax.Value.ToShortDateString())
				.Must((x, y) => x.DateValue.Value >= x.DateMin.Value)
				.When(x => x.Type == ModuleSettingType.Date && x.DateValue.HasValue && x.DateMin.HasValue)
				.WithLocalizedMessage("Validation.Error.DateMin", x => x.Name, x => x.DateMin.Value.ToShortDateString())
				.Must((x, y) => x.DateValue.Value <= x.DateMax.Value)
				.When(x => x.Type == ModuleSettingType.Date && x.DateValue.HasValue && x.DateMax.HasValue)
				.WithLocalizedMessage("Validation.Error.DateMax", x => x.Name, x => x.DateMax.Value.ToShortDateString())
				.Must((x, y) => x.NumberValue.Value >= x.Min.Value && x.NumberValue.Value <= x.Max.Value)
				.When(x => x.NumberValue.HasValue && x.Min.HasValue && x.Max.HasValue)
				.WithLocalizedMessage("Validation.Error.Range", x => x.Name, x => x.Min.Value, x => x.Max.Value)
				.Must((x, y) => x.NumberValue.Value >= x.Min.Value)
				.When(x => x.NumberValue.HasValue && x.Min.HasValue)
				.WithLocalizedMessage("Validation.Error.Min", x => x.Name, x => x.Min.Value)
				.Must((x, y) => x.NumberValue.Value <= x.Max.Value)
				.When(x => x.NumberValue.HasValue && x.Max.HasValue)
				.WithLocalizedMessage("Validation.Error.Max", x => x.Name, x => x.Max.Value);
		}
	}
}