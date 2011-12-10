using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using FubuMVC.Core;
using FubuMVC.Core.UI.Configuration;
using FubuMVC.Spark;
using FubuMVC.Validation;
using FubuValidation;
using HtmlTags;
using Web.Configuration.Behaviours.Output;
using Web.Endpoints;
using Web.Endpoints.HomepageModels;
using Web.Infrastructure.Behaviours;
using Web.Utilities;

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
				.IgnoreClassSuffix("Index")
        		.IgnoreMethodsNamed("get")
        		.IgnoreMethodsNamed("post")
        		.ConstrainToHttpMethod(x => x.Method.Name.Equals("Get"), "Get")
        		.ConstrainToHttpMethod(x => x.Method.Name.Equals("Post"), "Post")
        		.RootAtAssemblyNamespace()
        		.HomeIs<IndexEndpoint>(e => e.Get(new HomepageLinkModel()));

        	Actions
        		.IncludeTypes(t => t.Namespace.Contains("Web.Endpoints") && t.Name.EndsWith("Endpoint"));

        	Views
        		.TryToAttach(x => x.by_ViewModel())
        		.RegisterActionLessViews(t => t.ViewModelType == typeof (Notification));

			// TODO - nice little blog post
        	Output.To<RenderImageNode>().WhenTheOutputModelIs<ImageModel>();


			//this.Validation(validation =>
			//                    {
			//                        validation
			//                            .Actions
			//                            .Include(call => call.HasInput && call.InputType().Name.Contains("Input"));

			//                        validation
			//                            .Failures
			//                            .If(f => f.InputType() != null && f.InputType().Name.Contains("Input"))
			//                            .TransferBy<HandlerModelDescriptor>();
			//                    });

        	Policies
        		.WrapBehaviorChainsWith<RavenSessionBehaviour>();

			// TODO - move into conventions / policies? Put Html building logic into helpers
        	HtmlConvention(x =>
        	               x.Labels
        	               	.If(e => e.Accessor.Name.EndsWith("_BigText"))
        	               	.BuildBy(er => new HtmlTag("label").Text(er.Accessor.Name.Replace("_BigText", "")))
        		);

        	HtmlConvention(x =>
        	               x.Displays
        	               	.If(e => e.Accessor.PropertyType.IsAssignableFrom(typeof (IEnumerable<String>)))
        	               	.BuildBy(er =>
        	               	         	{
        	               	         		var list = new HtmlTag("ul");
        	               	         		foreach (var item in er.Value<IEnumerable<String>>())
        	               	         		{
        	               	         			list.Append(new HtmlTag("li", t => t.Text(item)));
        	               	         		}

        	               	         		return list;
        	               	         	}
        	               	));

			HtmlConvention(x =>
						x.Displays
						.If(e => e.Accessor.PropertyType.IsAssignableFrom(typeof(ImageDisplayModel)))
						.BuildBy(er =>
						{
							// TODO - New convention to blog about - mention how we are making url's irrelevant to the internals of the application
							var model = er.Value<ImageDisplayModel>();
							var urlForImage = String.Format("/Image?id={0}&width={1}&height={2}", model.Id, model.Width, model.Height);
							return new HtmlTag("img").Attr("src", urlForImage);
						}
							));

        	HtmlConvention(x =>
        	               x.Editors
        	               	.If(e => e.Accessor.Name.EndsWith("_BigText"))
        	               	.BuildBy(er => new HtmlTag("textarea").Text(er.Value<String>()))
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
        	               	         			var option = new HtmlTag("option").Text(enumValue.ToString());
												if (GetSelectedValue(er, er.Accessor.Name) == enumValue.ToString())
												{
													option.Attr("selected", "selected");
												}

												tag.Children.Add(option);
        	               	         		}

        	               	         		return tag;
        	               	         	})
        		);

        	HtmlConvention(x =>
        	               x.Editors
        	               	.If(e => e.Accessor.PropertyType.IsAssignableFrom(typeof (IDictionary<String, String>)))
        	               	.BuildBy(er =>
        	               	         	{
											// TODO - getting ugly - make this a class / method itself

        	               	         		string name = er.Accessor.PropertyNames[0].Substring(0, er.Accessor.PropertyNames[0].Length - 1);


        	               	         		string selectedValue = GetSelectedValue(er, name);


        	               	         		var dictionary = er.Value<IDictionary<String, String>>();
        	               	         		var tag = new HtmlTag("select").Attr("name", name);
        	               	         		foreach (var item in dictionary)
        	               	         		{
        	               	         			var option = new HtmlTag("option").Text(item.Value).Attr("value", item.Key);

												// TODO - new convention for next blog post
												if (selectedValue != null && selectedValue == item.Value)
												{
													option.Attr("selected", "selected");
												}

        	               	         			tag.Children.Add(option);
        	               	         		}

        	               	         		return tag;
        	               	         	})
        		);

        	HtmlConvention(x =>
        	               x.Editors
        	               	.If(e => e.Accessor.PropertyType.IsAssignableFrom(typeof (IList<StringWrapper>)))
        	               	.BuildBy(er =>
        	               	         	{
        	               	         		var tag = new HtmlTag("div").AddClass("hasHiddenGroup");
											
											tag.Children.Add(
												new HtmlTag("input")
												.Attr("type", "text")
												.Attr("name", er.Accessor.Name)
												);
											
											tag.Children.Add(
												new HtmlTag("a")
												.Attr("href", "#")
												.Text("add")
												.AddClass("addItem")
												);

        	               	         		tag.Children.Add(new HtmlTag("ul"));

        	               	         		return tag;
        	               	         	}));

			HtmlConvention(x =>
						x.Editors
						.If(e => e.Accessor.PropertyType.IsAssignableFrom(typeof(HttpPostedFileBase)))
						.BuildBy(er => new HtmlTag("input").Attr("type", "file"))
				);

			this.UseSpark();
        }

		// TODO - this moves when the html conventions do
		// TODO - new convention for next blog post
    	private string GetSelectedValue(ElementRequest er, string name)
    	{
    		var selectedValueProperty = er.Model.GetType().GetProperty("Selected" + name);

    		var selectedValue = selectedValueProperty != null
    		                    	? selectedValueProperty.GetValue(er.Model, null).ToString() 
    		                    	: null;
    		return selectedValue;
    	}
    }
}