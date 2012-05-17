using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using FubuCore.Reflection;
using FubuLocalization;
using FubuMVC.Core.Continuations;
using FubuValidation;
using FubuValidation.Fields;
using Model.About;
using Web.Endpoints.About;
using Web.Endpoints.About.LinkModels;
using Web.Utilities;

namespace Web.Endpoints.SiteManagement.About
{
	public class UpdateEndpoint
	{
		private readonly IAboutInfoUpdater updater;
		private readonly IAboutInfoRetriever retriever;

		public UpdateEndpoint(IAboutInfoUpdater updater, IAboutInfoRetriever retriever)
		{
			this.updater = updater;
			this.retriever = retriever;
		}

		public AboutViewModel Get(AboutRequestModel aboutRequestModel)
		{
			var af = retriever.GetAboutInfo();

			return new AboutViewModel
			       	{
			       		AboutText_BigText = af.AboutText,
						ThingsILikeUrls = af.ThingsILikeUrls.ToStringWrappers().ToList()
			       	};
		}

		public FubuContinuation Post(AboutUpdateModel model)
		{
			var dto = new AboutInfoDto {AboutText = model.AboutText_BigText, ThingsILikeUrls = model.ThingsILikeUrls.ToStrings()};
			updater.Update(dto);

			return FubuContinuation.RedirectTo<ViewEndpoint>(x => x.Get(new AboutLinkModel()));
		}
	}

	public class AboutRequestModel
	{
	}

	public class AboutViewModel : AboutUpdateModel
	{
		
	}

	public class AboutUpdateModel
	{
		public string AboutText_BigText { get; set; }

		[DenyPrefix(Prefix = "http://")]
		public IList<StringWrapper> ThingsILikeUrls { get; set; }
	}

	public class DenyPrefixAttribute : FieldValidationAttribute
	{
		public string Prefix { get; set; }

		public override IEnumerable<IFieldValidationRule> RulesFor(PropertyInfo property)
		{
			yield return new DenyPrefixRule(Prefix);
		}
	}

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