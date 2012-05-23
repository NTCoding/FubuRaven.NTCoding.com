using System;
using FubuLocalization;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Runtime;
using FubuValidation;
using NUnit.Framework;
using Rhino.Mocks;
using Web.Configuration;
using Web.Infrastructure;
using Web.Infrastructure.Validation;

namespace Web.Tests.Infrastructure
{
	[TestFixture]
	public class Validation_behaviour_successful_validation_tests
	{
		private IFubuRequest request;
		private IValidator validator;
		private IPartialFactory partialFactory;
		private IActionBehavior innerBehaviour;
		private TestInputModel inputModel;
		private NTCodingValidationBehaviour<TestInputModel> behaviour;
		private Notification notificationWithNoErrors;

		[TestFixtureSetUp]
		public void SetUp()
		{
			request        = MockRepository.GenerateStub<IFubuRequest>();
			validator      = MockRepository.GenerateStub<IValidator>();
			partialFactory = MockRepository.GenerateStub<IPartialFactory>();
			innerBehaviour = MockRepository.GenerateStub<IActionBehavior>();
			inputModel     = new TestInputModel();

			request.Stub(r => r.Get<TestInputModel>()).Return(inputModel);
			behaviour = new NTCodingValidationBehaviour<TestInputModel>(request, validator, MockRepository.GenerateStub<IActionFinder>(), partialFactory);
			behaviour.InnerBehavior = innerBehaviour;

			notificationWithNoErrors = new Notification();
			validator.Stub(v => v.Validate(inputModel)).IgnoreArguments().Return(notificationWithNoErrors);

			behaviour.Invoke();
		}

		[Test]
		public void Invokes_inner_behaviour_when_no_validation_errors()
		{
			innerBehaviour.AssertWasCalled(b => b.Invoke());
		}

		[Test]
		public void Does_not_set_validation_notification_in_request()
		{
			request.AssertWasNotCalled(r => r.Set(notificationWithNoErrors));
		}

		[Test]
		public void Does_not_attempt_to_create_a_partial_for_redirecting_behaviour()
		{
			partialFactory.AssertWasNotCalled(p => p.BuildPartial(Arg<Type>.Is.Anything));
		}

		[Test]
		public void Sets_flag_on_validated_input_model_did_not_fail_validation()
		{
			var vim = (ValidatedInputModel<TestInputModel>)
				request.GetArgumentsForCallsMadeOn(r => r.Set(Arg<ValidatedInputModel<TestInputModel>>.Is.Anything))[0][0];

			Assert.That(vim.FailedValidation, Is.EqualTo(false));
		}
	}

	[TestFixture]
	public class Validation_behaviour_failed_validation_tests
	{

		private IFubuRequest request;
		private IValidator validator;
		private IPartialFactory partialFactory;
		private IActionBehavior innerBehaviour;
		private TestInputModel inputModel;
		private NTCodingValidationBehaviour<TestInputModel> behaviour;
		private Notification errorNotification;
		private IActionFinder actionFinder;
		private IActionBehavior partial;

		[TestFixtureSetUp]
		public void SetUp()
		{
			request        = MockRepository.GenerateStub<IFubuRequest>();
			validator      = MockRepository.GenerateStub<IValidator>();
			partialFactory = MockRepository.GenerateStub<IPartialFactory>();
			innerBehaviour = MockRepository.GenerateStub<IActionBehavior>();
			partial        = MockRepository.GenerateStub<IActionBehavior>();
			actionFinder   = MockRepository.GenerateStub<IActionFinder>();
			inputModel     = new TestInputModel();

			request.Stub(r => r.Get<TestInputModel>()).Return(inputModel);
			behaviour = new NTCodingValidationBehaviour<TestInputModel>(request, validator, actionFinder, partialFactory);
			behaviour.InnerBehavior = innerBehaviour;

			errorNotification = new Notification();
			errorNotification.RegisterMessage(new NotificationMessage(StringToken.FromKeyString("blah")));
			validator.Stub(v => v.Validate(inputModel)).Return(errorNotification);

			actionFinder.Stub(a => a.GetRequestModelTypeFor(Arg<TestInputModel>.Is.Anything)).Return(typeof (TestRequestModel));
			partialFactory.Stub(p => p.BuildPartial(typeof (TestRequestModel))).Return(partial);

			behaviour.Invoke();
		}

		[Test]
		public void Does_not_invoke_inner_behaviour()
		{
			innerBehaviour.AssertWasNotCalled(i => i.Invoke());
		}

		[Test]
		public void Sets_failed_validation_in_request()
		{
			request.AssertWasCalled(r => r.Set(errorNotification));
		}

		[Test]
		public void Redirects_to_get_action_using_partial()
		{
			partial.AssertWasCalled(p => p.Invoke());
		}

		[Test]
		public void Sets_validated_input_model_in_request()
		{
			var vim = (ValidatedInputModel<TestInputModel>)
				request.GetArgumentsForCallsMadeOn(r => r.Set(Arg<ValidatedInputModel<TestInputModel>>.Is.Anything))[0][0];

			Assert.That(vim.InputModel, Is.EqualTo(inputModel));
		}

		[Test]
		public void Sets_flag_on_validated_input_model_showing_failed_validation()
		{
			var vim = (ValidatedInputModel<TestInputModel>)
				request.GetArgumentsForCallsMadeOn(r => r.Set(Arg<ValidatedInputModel<TestInputModel>>.Is.Anything))[0][0];

			Assert.That(vim.FailedValidation, Is.EqualTo(true));
		}
	}

	public class TestRequestModel
	{
	}

	public class TestInputModel
	{
	}
}