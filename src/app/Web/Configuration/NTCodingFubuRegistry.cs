using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Http;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Security;
using FubuMVC.Core.UI.Configuration;
using FubuMVC.Spark;
using FubuMVC.Validation;
using FubuValidation;
using HtmlTags;
using Web.Configuration.Behaviours.Output;
using Web.Endpoints;
using Web.Endpoints.HomepageModels;
using Web.Infrastructure;
using Web.Infrastructure.Authxx;
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

        	Output.To<RenderImageNode>().WhenTheOutputModelIs<ImageModel>();

			Services(x => x.ReplaceService<IAuthorizationFailureHandler, NTCodingAuthorizationFailureHandler>());


        	Policies
				.Add<NTCodingValidationConvention>()
				//.Add<RemapFormValuesOnValidationFailurePolicy>()
        		.WrapBehaviorChainsWith<RavenSessionBehaviour>()
        		.Add<AuthenticationConvention>();


			// TODO - move into conventions / policies? Put Html building logic into helpers
        	HtmlConvention(x =>
        	               x.Displays
        	               	.If(e => e.Accessor.Name.EndsWith("Html") && e.Accessor.PropertyType == typeof (String))
        	               	.BuildBy(er => new HtmlTag("div").AppendHtml(er.Value<String>())));

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
											var name = er.Accessor.Name;

											// TODO - duplicated logic for creating select lists
        	               	         		var tag = new HtmlTag("select");
											tag.Children.Add(new HtmlTag("option").Text(GetDefaultValue(er, name)).Attr("value", "-"));
        	               	         		var enumValues = Enum.GetValues(er.Accessor.PropertyType);
        	               	         		foreach (var enumValue in enumValues)
        	               	         		{
        	               	         			var option = new HtmlTag("option").Text(enumValue.ToString());
        	               	         			
        	               	         			if (GetSelectedValue(er, name) == enumValue.ToString())
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
        	               	         		string defaultValue = GetDefaultValue(er, name);

        	               	         		var dictionary = er.Value<IDictionary<String, String>>();
        	               	         		var tag = new HtmlTag("select").Attr("name", name);
        	               	         		tag.Children.Add(new HtmlTag("option").Text(defaultValue).Attr("value", "-"));
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

        	               	         		var list = new HtmlTag("ul");
        	               	         		tag.Children.Add(list);

        	               	         		var values = er.Value<IEnumerable<StringWrapper>>();
        	               	         		foreach (var s in values)
        	               	         		{

												// add a list item showing the value and a delete link
												var li = new HtmlTag("li");
        	               	         			li.Text(s.Text + "  ");

        	               	         			var deleteLInk = new HtmlTag("a")
        	               	         				.AddClass("listDelete")
        	               	         				.Text("delete")
        	               	         				.Attr("href", "#");

        	               	         			li.Children.Add(deleteLInk);

        	               	         			list.Children.Add(li);

												// add a hidden element so magic javascript can delete and manage indices
        	               	         			var hidden = new HtmlTag("input")
        	               	         				.Attr("type", "hidden")
        	               	         				.Attr("name", er.Accessor.PropertyNames[0])
        	               	         				.Attr("value", s.Text);

												tag.Children.Add(hidden);
        	               	         		}

        	               	         		return tag;
        	               	         	}));

			HtmlConvention(x =>
						x.Editors
						.If(e => e.Accessor.PropertyType.IsAssignableFrom(typeof(HttpPostedFileBase)))
						.BuildBy(er => new HtmlTag("input").Attr("type", "file"))
				);

			HtmlConvention(x =>
					x.Editors
					.If(e => e.Accessor.Name.Equals("Id"))
					.BuildBy(er => new HtmlTag("input").Attr("type", "hidden").Attr("value", er.Value<String>()))
				);

			this.UseSpark();
        }

		// TODO - this moves when the html conventions do
		// TODO - new convention for next blog post
    	private string GetSelectedValue(ElementRequest er, string name)
    	{
    		var selectedValueProperty = GetProperty(er, "Selected" + name);

    		var selectedValue = selectedValueProperty != null
    		                    	? GetValueOrDefault(er, selectedValueProperty) 
    		                    	: null;
    		return selectedValue;
    	}

		// TODO - another convention
    	private string GetDefaultValue(ElementRequest er, string name)
    	{
    		var defaultValueProperty = GetProperty(er, "Default" + name +"Text");

    		return defaultValueProperty != null
    		       	? GetValueOrDefault(er, defaultValueProperty)
    		       	: "-- Please Select --";
    	}

		private string GetValueOrDefault(ElementRequest er, PropertyInfo selectedValueProperty)
		{
			var value = selectedValueProperty.GetValue(er.Model, null);

			return value != null ? value.ToString() : null;
		}

    	private PropertyInfo GetProperty(ElementRequest er, string name)
		{
			return er.Model.GetType().GetProperty(name);
		}
    }

	//public class RemapFormValuesOnValidationFailurePolicy : IConfigurationAction
	//{
	//    public void Configure(BehaviorGraph graph)
	//    {
	//        graph
	//            .Actions()
	//            .Where(b => b.InputType() != null)
	//            .Each(chain => chain.AddAfter(new Wrapper(typeof(RemapFailedValidationFormValuesBehaviour<>).MakeGenericType(chain.OutputType().BaseType))));
	//    }
	//}

	//public class RemapFailedValidationFormValuesBehaviour<TInputModel> : BasicBehavior where  TInputModel : class
	//{
	//    private readonly IFubuRequest request;

	//    public RemapFailedValidationFormValuesBehaviour(IFubuRequest request) : base(PartialBehavior.Executes)
	//    {
	//        this.request = request;
	//    }

	//    protected override DoNext performInvoke()
	//    {
	//        FailedValidationInputModel<TInputModel> failedValidationContext = null;
	//        try
	//        {
	//            failedValidationContext = request.Get<FailedValidationInputModel<TInputModel>>();
	//        }
	//        catch (FubuException)
	//        {
	//            return DoNext.Continue;
	//        }

	//        if (failedValidationContext.IsForFailedValidation)
	//        {
	//            var viewModel = request.Get(GetViewModelType());

	//            MapProperties(failedValidationContext.InputModel, viewModel);

	//            request.Set(viewModel);
	//        }

	//        return DoNext.Continue;
	//    }

	//    protected override void afterInsideBehavior()
	//    {
			
	//    }

	//    private Type GetViewModelType()
	//    {
	//        return
	//            Assembly
	//                .GetAssembly(typeof (TInputModel))
	//                .GetTypes()
	//                .Where(t => t.IsSubclassOf(typeof (TInputModel)))
	//                .First();
	//    }

	//    private void MapProperties(object from, object to)
	//    {
	//        foreach (var pi in from.GetType().GetProperties())
	//        {
	//            var prop = to.GetType().GetProperties().SingleOrDefault(ti => ti.Name == pi.Name);
	//            if (prop != null)
	//            {
	//                prop.SetValue(to, pi.GetValue(from, new object[] { }), new object[] { });
	//            }
	//        }
	//    }
	//}

	public class NTCodingValidationConvention : IConfigurationAction
	{
		public void Configure(BehaviorGraph graph)
		{
			graph
				.Actions()
				.Where(b => b.InputType() != null)
				.Each(chain => chain.WrapWith(typeof (NTCodingValidationBehaviour<>).MakeGenericType(chain.InputType())));

		}
	}

	public class NTCodingValidationBehaviour<T> : IActionBehavior where T : class
	{
		private readonly IFubuRequest request;
		private readonly IValidator validator;
		private readonly IActionFinder actionFinder;
		private readonly IPartialFactory factory;

		public NTCodingValidationBehaviour(IFubuRequest request, IValidator validator, IActionFinder actionFinder, IPartialFactory factory)
		{
			this.request      = request;
			this.factory      = factory;
			this.validator    = validator;
			this.actionFinder = actionFinder;
		}

		public IActionBehavior InnerBehavior { get; set; }

		public void Invoke()
		{
			var inputModel = request.Get<T>();

			var notification = validator.Validate(inputModel);

			if (notification.IsValid())
				SetContextAndContinueChain(inputModel);
			else
				SetContextAndRedirectToGet(inputModel, notification);
		}

		private void SetContextAndContinueChain(T inputModel)
		{
			request.Set(new ValidatedInputModel<T>(inputModel) { FailedValidation = false });

			InnerBehavior.Invoke();
		}

		private void SetContextAndRedirectToGet(T inputModel, Notification notification)
		{
			request.Set(notification);
			request.Set(new ValidatedInputModel<T>(inputModel) { FailedValidation = true });

			factory.BuildPartial(actionFinder.GetRequestModelTypeFor(inputModel)).Invoke();
		}

		

		private void RedirectToGetWithErrors(T inputModel, Notification notification)
		{

			// TODO - still need to map the properties onto the request model from the input
			//        model in cases where request model has properties else get will fail
			//        This will also be conventional for special cases??


			//factory.BuildPartial(ac.InputType()).InvokePartial();
		}

		//private ActionCall GetActionCallForGetRequest(T inputModel)
		//{
		//    var targetNamespace = inputModel.GetType().Namespace;

		//    var getCall = graph
		//        .Behaviors
		//        .Where(chain => chain.FirstCall() != null && chain.FirstCall().HandlerType.Namespace == targetNamespace
		//                        && chain.Route.AllowedHttpMethods.Contains("GET"))
		//        .Select(chain => chain.FirstCall())
		//        .FirstOrDefault();

		//    return getCall;
		//}
		
		public void InvokePartial()
		{
			Invoke();
		}
	}

	public class ValidatedInputModel<T> where T : class 
	{
		public ValidatedInputModel(T inputModel)
		{
			InputModel = inputModel;
		}

		public T InputModel { get; private set; }

		public bool FailedValidation { get; set; }
	}
}