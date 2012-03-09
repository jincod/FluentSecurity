using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FluentSecurity.Policy;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("PolicyViolationHandlerSelectorSpec")]
	public class When_creating_a_PolicyViolationHandlerSelector
	{
		[Test]
		public void Should_throw_when_violation_handlers_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new PolicyViolationHandlerSelector((IEnumerable<IPolicyViolationHandler>) null));
		}
	}

	[TestFixture]
	[Category("PolicyViolationHandlerSelectorSpec")]
	public class When_selecting_a_policy_violation_handler
	{
		private IPolicyViolationHandlerSelector _violationHandler;

		[SetUp]
		public void SetUp()
		{
			var violationHandlers = Helpers.TestDataFactory.CreatePolicyViolationHandlers();
			_violationHandler = new PolicyViolationHandlerSelector(violationHandlers);
		}

		[Test]
		public void Should_return_handler_for_DenyAnonymousAccessPolicy()
		{
			// Arrange
			var exception = new PolicyViolationException<DenyAnonymousAccessPolicy>("Anonymous access denied");

			// Act
			var handler = _violationHandler.FindHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.TypeOf(typeof(DenyAnonymousAccessPolicyViolationHandler)));
		}

		[Test]
		public void Should_return_handler_for_DenyAuthenticatedAccessPolicy()
		{
			// Arrange
			var exception = new PolicyViolationException<DenyAuthenticatedAccessPolicy>("Authenticated access denied");

			// Act
			var handler = _violationHandler.FindHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.TypeOf(typeof(DenyAuthenticatedAccessPolicyViolationHandler)));
		}

		[Test]
		public void Should_not_return_handler_for_RequireRolePolicy()
		{
			// Arrange
			var exception = new PolicyViolationException<RequireRolePolicy>("Access denied");

			// Act
			var handler = _violationHandler.FindHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.Null);
		}
	}

	[TestFixture]
	[Category("PolicyViolationHandlerSelectorSpec")]
	public class When_selecting_a_policy_violation_handler_and_a_default_violation_handler_is_registered
	{
		private IPolicyViolationHandlerSelector _violationHandler;

		public class DefaultPolicyViolationHandler : IPolicyViolationHandler
		{
			public ActionResult Handle(PolicyViolationException exception)
			{
				return new EmptyResult();
			}
		}

		[SetUp]
		public void SetUp()
		{
			_violationHandler = new PolicyViolationHandlerSelector(new List<IPolicyViolationHandler>()
			{
				new DenyAnonymousAccessPolicyViolationHandler(new EmptyResult()),
				new DefaultPolicyViolationHandler()
			});
		}

		[Test]
		public void Should_return_handler_for_DenyAnonymousAccessPolicy()
		{
			// Arrange
			var exception = new PolicyViolationException<DenyAnonymousAccessPolicy>("Anonymous access denied");

			// Act
			var handler = _violationHandler.FindHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.TypeOf(typeof(DenyAnonymousAccessPolicyViolationHandler)));
		}

		[Test]
		public void Should_return_default_policy_violation_handler_when_policy_is_not_DenyAnonymousAccessPolicy()
		{
			// Arrange
			var exception = new PolicyViolationException<RequireRolePolicy>("Access denied");

			// Act
			var handler = _violationHandler.FindHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.TypeOf(typeof(DefaultPolicyViolationHandler)));
		}
	}
}