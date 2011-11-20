using System;
using System.Linq;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	public abstract class AssemblyScannerAssemblySpecification
	{
		protected static void Because(Action<ConfigurationExpression> configurationExpression)
		{
			// Arrange
			var expression = TestDataFactory.CreateValidConfigurationExpression();
			expression.CreateContext(new RootConfigurationExpression());
			configurationExpression(expression);
			AssertAllControllerActionsHasContainer(expression);
		}

		private static void AssertAllControllerActionsHasContainer(ConfigurationExpression configurationExpression)
		{
			var policyContainers = configurationExpression.PolicyContainers;

			Assert.That(policyContainers.Count(), Is.EqualTo(12));
			var blog = NameHelper<BlogController>.Controller();
			var admin = NameHelper<AdminController>.Controller();
			var root = NameHelper<TestData.AssemblyScannerControllers.RootController>.Controller();
			var include = NameHelper<TestData.AssemblyScannerControllers.Include.IncludedController>.Controller();
			var exclude = NameHelper<TestData.AssemblyScannerControllers.Exclude.ExcludedController>.Controller();

			Assert.That(policyContainers.GetContainerFor(blog, "Index"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(blog, "ListPosts"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(blog, "AddPost"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(blog, "EditPost"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(blog, "DeletePost"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(blog, "AjaxList"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(admin, "Index"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(admin, "LogIn"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(admin, "LogOut"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(root, "Index"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(include, "Index"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(exclude, "Index"), Is.Not.Null);
		}
	}
}