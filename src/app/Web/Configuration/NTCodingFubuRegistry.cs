﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.UI.Configuration;
using FubuMVC.Spark;
using FubuMVC.Validation;
using FubuValidation;
using FubuValidation.Fields;
using HtmlTags;
using Web.Endpoints;
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
        		.IgnoreMethodsNamed("get")
        		.IgnoreMethodsNamed("post")
        		.ConstrainToHttpMethod(x => x.Method.Name.Equals("Get"), "Get")
        		.ConstrainToHttpMethod(x => x.Method.Name.Equals("Post"), "Post")
        		.RootAtAssemblyNamespace();

        	Actions
        		.IncludeTypes(t => t.Namespace.Contains("Web.Endpoints") && t.Name.EndsWith("Endpoint"));

        	Views
        		.TryToAttach(x => x.by_ViewModel())
        		.RegisterActionLessViews(t => t.ViewModelType == typeof (Notification));

			// TODO - nice little blog post
        	Output.To<RenderImageNode>().WhenTheOutputModelIs<ImageModel>();


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
        	               x.Editors
        	               	.If(e => e.Accessor.Name.EndsWith("_BigText"))
        	               	.BuildBy(er => new HtmlTag("textarea"))
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
        	               	.If(e => e.Accessor.PropertyType.IsAssignableFrom(typeof (IDictionary<String, String>)))
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
    }

	public class RenderImageNode : OutputNode<ImageOutputBehaviour>
	{
		
	}

	public class ImageOutputBehaviour : BasicBehavior
	{
		private readonly IOutputWriter writer;
		private readonly IFubuRequest request;

		public ImageOutputBehaviour(IOutputWriter writer, IFubuRequest request) : base(PartialBehavior.Ignored)
		{
			this.writer = writer;
			this.request = request;
		}

		protected override DoNext performInvoke()
		{
			var bytes = File.ReadAllBytes(@"C:\Users\Administrator\Desktop\Servers.png");
			
			var response = HttpContext.Current.Response;
			response.BinaryWrite(bytes);
			response.ContentType = "image/png";

			return DoNext.Continue;
		}
	}
}