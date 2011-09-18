using System;
using System.Collections.Generic;
using FubuMVC.Core;
using FubuMVC.Core.UI.Configuration;
using FubuMVC.Spark;
using FubuMVC.Validation;
using FubuValidation;
using FubuValidation.Fields;
using HtmlTags;

namespace Web.Configuration
{
    public class NTCodingFubuRegistry : FubuRegistry
    {
        public NTCodingFubuRegistry()
        {
            IncludeDiagnostics(true);

            Routes
				.IgnoreNamespaceText("Endpoints")
				.IgnoreClassSuffix("Endpoint")
				.IgnoreMethodsNamed("get")
				.IgnoreMethodsNamed("post")
				.ConstrainToHttpMethod(x => x.Method.Name.Equals("Get"), "Get")
				.ConstrainToHttpMethod(x => x.Method.Name.Equals("Post"), "Post")
                .RootAtAssemblyNamespace();

        	Actions
        		.IncludeTypes(t => t.Namespace.Contains("Web.Endpoints") && t.Name.EndsWith("Endpoint"));

			Views
        		.TryToAttach(x => x.by_ViewModel())
				.RegisterActionLessViews(t => t.ViewModelType == typeof(Notification));

			this.Validation(validation =>
								{
									validation
										.Actions
										.Include(call => call.HasInput && call.InputType().Name.Contains("Input"));

									validation
										.Failures
										.If(f => f.InputType() != null && f.InputType().Name.Contains("Input"))
										.TransferBy<HandlerModelDescriptor>();
								});

        	HtmlConvention(x =>
        	                    x.Editors
        						  .If(e => e.Accessor.Name.EndsWith("_BigText"))
        	                      .BuildBy(er => new HtmlTag("textarea"))
						);

        	HtmlConvention(x =>
        					   x.Labels
        	               		.If(e => e.Accessor.Name.EndsWith("_BigText"))
        	               		.BuildBy(er => new HtmlTag("label").Text(er.Accessor.Name.Replace("_BigText", "")))
						);

			HtmlConvention(x =>
							x.Editors
							  .If(e => e.Accessor.PropertyType.IsEnum)
							  .BuildBy(er =>
						           		{
						           			var tag = new HtmlTag("select");
						           			var enumValues = Enum.GetValues(er.Accessor.PropertyType);
						           			foreach (var enumValue in enumValues)
						           			{
						           				tag.Children.Add(new HtmlTag("option").Text(enumValue.ToString()));
						           			}

						           			return tag;
						           		})
						);

			HtmlConvention(x => 
							x.Editors
								.If(e => e.Accessor.PropertyType.IsAssignableFrom(typeof(IDictionary<String,String>)))
								.BuildBy(er =>
								         	{
								         		var dictionary = er.Value<IDictionary<String, String>>();
								         		string name = er.Accessor.PropertyNames[0].Substring(0, er.Accessor.PropertyNames[0].Length - 1);
								         		var tag = new HtmlTag("select").Attr("name", name);
								         		foreach (var item in dictionary)
								         		{
								         			tag.Children.Add(new HtmlTag("option")
														.Text(item.Value)
														.Attr("value", item.Key)
														);
								         		}

								         		return tag;
								         	})
						);


        	this.UseSpark();
        }
    }
}