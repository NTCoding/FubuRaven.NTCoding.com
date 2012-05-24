using System.Linq;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using Web.Configuration;
using GenericEnumerableExtensions = System.Collections.Generic.GenericEnumerableExtensions;

namespace Web.Infrastructure.Validation
{
	public class RemapFormValuesOnValidationFailurePolicy : IConfigurationAction
	{
		public void Configure(BehaviorGraph graph)
		{
			GenericEnumerableExtensions.Each<ActionCall>(graph
				                  	.Actions()
				                  	.Where(b => b.InputType() != null), chain => chain.AddAfter(new Wrapper(typeof(RemapFailedValidationFormValuesBehaviour<>).MakeGenericType(chain.OutputType().BaseType))));
		}
	}
}