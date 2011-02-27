using System;
using System.Collections.Generic;
using FluentSecurity.Policy;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("PolicyViolationHandlerSelectorSpec")]
	public class When_creating_a_PolicyViolationHandlerSelector
	{
		private ISecurityConfiguration _configuration;
		private IEnumerable<IPolicyViolationHandler> _violationHandlers;

		[SetUp]
		public void SetUp()
		{
			_configuration = Helpers.TestDataFactory.CreateValidSecurityConfiguration();
			_violationHandlers = Helpers.TestDataFactory.CreatePolicyViolationHandlers();
		}

		[Test]
		public void Should_throw_when_configuration_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new PolicyViolationHandlerSelector(null, _violationHandlers));
		}

		[Test]
		public void Should_throw_when_violation_handlers_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new PolicyViolationHandlerSelector(_configuration, null));
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
			var configuration = Helpers.TestDataFactory.CreateValidSecurityConfiguration();
			var violationHandlers = Helpers.TestDataFactory.CreatePolicyViolationHandlers();

			_violationHandler = new PolicyViolationHandlerSelector(configuration, violationHandlers);
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
}