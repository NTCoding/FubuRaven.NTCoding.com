using System.Linq;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using GenericEnumerableExtensions = System.Collections.Generic.GenericEnumerableExtensions;

namespace Web.Infrastructure.Validation
{
	public class NTCodingValidationConvention : IConfigurationAction
	{
		public void Configure(BehaviorGraph graph)
		{
			GenericEnumerableExtensions.Each<ActionCall>(graph
				                  	.Actions()
				                  	.Where(b => b.InputType() != null), chain => chain.WrapWith(typeof (NTCodingValidationBehaviour<>).MakeGenericType(chain.InputType())));

		}
	}
}