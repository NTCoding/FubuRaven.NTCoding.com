using System;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using NUnit.Framework;
using Rhino.Mocks;
using Web.Configuration;
using Web.Infrastructure.Validation;

namespace Web.Tests.Infrastructure
{
	[TestFixture]
	public class RemapFailedValidationFormValuesBehaviourTests
	{
		private IFubuRequest request;
		private RemapViewModel viewModel = null;
		private RemapFailedValidationFormValuesBehaviour<RemapInputModel> behaviour;
		private RemapInputModel inputModel;
		private IActionBehavior inner;

		[SetUp]
		public void SetUp()
		{
			request   = MockRepository.GenerateStub<IFubuRequest>();
			inner     = MockRepository.GenerateStub<IActionBehavior>();
			viewModel = new RemapViewModel();
			behaviour = new RemapFailedValidationFormValuesBehaviour<RemapInputModel>(request) {InnerBehavior = inner};

			request.Stub(r => r.Get(typeof(RemapViewModel))).Return(viewModel);
			request.Stub(r => r.Get<RemapViewModel>()).Return(viewModel);

			inputModel = new RemapInputModel
			{
				Name = "Jimmy Bogard",
				Age = 150,
				Height = 125
			};
		}

		[Test]
		public void Maps_properties_from_validated_input_model_to_view_model_when_validation_failed()
		{
			var validatedInputModel = new ValidatedInputModel<RemapInputModel>(inputModel){FailedValidation = true};
			
			request.Stub(r => r.Get<ValidatedInputModel<RemapInputModel>>()).Return(validatedInputModel);

			behaviour.Invoke();

			var vm = request.Get<RemapViewModel>();

			Assert.That(vm.Name, Is.EqualTo(inputModel.Name));
			Assert.That(vm.Age, Is.EqualTo(inputModel.Age));
			Assert.That(vm.Height, Is.EqualTo(inputModel.Height));
		}

		[Test]
		public void Does_not_map_properties_when_validation_is_successful()
		{
			var validatedInputModel = new ValidatedInputModel<RemapInputModel>(inputModel) { FailedValidation = false };

			request.Stub(r => r.Get<ValidatedInputModel<RemapInputModel>>()).Return(validatedInputModel);

			behaviour.Invoke();

			var vm = request.Get<RemapViewModel>();

			Assert.That(vm.Name, Is.Not.EqualTo(inputModel.Name));
			Assert.That(vm.Age, Is.Not.EqualTo(inputModel.Age));
			Assert.That(vm.Height, Is.Not.EqualTo(inputModel.Height));
		}

		[Test]
		public void Invokes_inner_behaviour_when_validation_is_success()
		{
			var validatedInputModel = new ValidatedInputModel<RemapInputModel>(inputModel) { FailedValidation = false };

			request.Stub(r => r.Get<ValidatedInputModel<RemapInputModel>>()).Return(validatedInputModel);

			behaviour.Invoke();

			inner.AssertWasCalled(i => i.Invoke());
		}

		[Test]
		public void Invokes_inner_behaviour_when_validation_fails()
		{
			var validatedInputModel = new ValidatedInputModel<RemapInputModel>(inputModel) { FailedValidation = false };

			request.Stub(r => r.Get<ValidatedInputModel<RemapInputModel>>()).Return(validatedInputModel);

			behaviour.Invoke();

			inner.AssertWasCalled(i => i.Invoke());
		}

		[Test]
		public void When_no_validated_input_model_in_request_continues_behaviour_chain()
		{
			request.Stub(r => r.Get<ValidatedInputModel<RemapInputModel>>()).Throw(new FubuException(1, ""));

			behaviour.Invoke();

			inner.AssertWasCalled(i => i.Invoke());
		}
	}

	public class RemapViewModel : RemapInputModel
	{
		
	}

	public class RemapInputModel
	{
		public string Name { get; set; }

		public int Age { get; set; }

		public int Height { get; set; }
	}
}