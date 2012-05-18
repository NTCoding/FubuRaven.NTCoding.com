using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FubuCore.Reflection;
using FubuLocalization;
using FubuValidation;
using FubuValidation.Fields;
using Web.Utilities;

namespace Web.Infrastructure.Validation
{
	public class DenyPrefixRule : IFieldValidationRule
	{
		private readonly string prefix;

		public DenyPrefixRule(string prefix)
		{
			this.prefix = prefix;
		}

		public void Validate(Accessor accessor, ValidationContext context)
		{
			var items = (IEnumerable<StringWrapper>)accessor.GetValue(context.Target);

			if (items.Any(i => i.Text.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
			{
				var m = prefix + " is not a valid prefix for \""+ GetFormattedPropertyName(accessor)  + "\"";
				var em = StringToken.FromKeyString("", m);
				context.Notification.RegisterMessage(accessor, em);
			}
		}

		private string GetFormattedPropertyName(Accessor accessor)
		{
			return Regex.Replace(accessor.PropertyNames[0], @"(\B[A-Z])", @" $1");
		}
	}
}