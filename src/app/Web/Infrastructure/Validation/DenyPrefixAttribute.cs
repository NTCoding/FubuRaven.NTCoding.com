using System.Collections.Generic;
using System.Reflection;
using FubuValidation.Fields;

namespace Web.Infrastructure.Validation
{
	public class DenyPrefixAttribute : FieldValidationAttribute
	{
		public string Prefix { get; set; }

		public override IEnumerable<IFieldValidationRule> RulesFor(PropertyInfo property)
		{
			yield return new DenyPrefixRule(Prefix);
		}
	}
}