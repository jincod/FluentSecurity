<%@ Page MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HomeView>" %>
<asp:Content ContentPlaceHolderID="TitleContent" runat="server">Getting started</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h1>Fluent Security</h1>
	
	<h2>Log in / Log out</h2>
	<ul>
		<%= Html.NavigationLink(Url.Action<AccountController>(x => x.LogInAsAdministrator()), "Log in as administrator", "li") %>
		<%= Html.NavigationLink(Url.Action<AccountController>(x => x.LogInAsPublisher()), "Log in as publisher", "li") %>
		<%= Html.NavigationLink(Url.Action<AccountController>(x => x.LogOut()), "Log out", "li") %>
	</ul>
	
	<h2>Examples</h2>
	<ul>
		<%= Html.NavigationLink(Url.Action<ExampleController>(x => x.DenyAnonymousAccess()), "Deny anonymous access", "li") %>
		<%= Html.NavigationLink(Url.Action<ExampleController>(x => x.DenyAuthenticatedAccess()), "Deny authenticated access", "li") %>
		<%= Html.NavigationLink(Url.Action<ExampleController>(x => x.RequireAdministratorRole()), "Require administrator role", "li") %>
		<%= Html.NavigationLink(Url.Action<ExampleController>(x => x.RequirePublisherRole()), "Require publisher role", "li") %>
		<%= Html.NavigationLink(Url.Action<ExampleController>(x => x.MissingConfiguration()), "Missing configuration", "li") %>
	</ul>
	
	<h3>Area examples</h3>
	<p>Log in to see all the actions available</p>
	<ul>
		<%= Html.NavigationLink(Url.AreaAction<FluentSecurity.SampleApplication.Areas.ExampleArea.Controllers.HomeController>(x => x.Index(), "ExampleArea"), "Index", "li") %>
		<%= Html.NavigationLink(Url.AreaAction<FluentSecurity.SampleApplication.Areas.ExampleArea.Controllers.HomeController>(x => x.PublishersOnly(), "ExampleArea"), "Publishers only", "li") %>
		<%= Html.NavigationLink(Url.AreaAction<FluentSecurity.SampleApplication.Areas.ExampleArea.Controllers.HomeController>(x => x.AdministratorsOnly(), "ExampleArea"), "Administrators only", "li") %>
	</ul>

	<h3>Example of route value based policy</h3>
	<p><%= Html.NavigationLink(Url.Action<AdminController>(x => x.ContextWithRouteValues(0), new { id = 38 }), "RouteValuePolicy - Id 38 - Result not cached") %></p>
	<p><%= Html.NavigationLink(Url.Action<AdminController>(x => x.ContextWithRouteValues(0), new { id = 38 }), "RouteValuePolicy - Id 38 - Result cached") %></p>
	<p><%= Html.NavigationLink(Url.Action<AdminController>(x => x.ContextWithRouteValues(0), new { id = 38 }), "RouteValuePolicy - Id 38 - Result cached") %></p>
	<p><%= Html.NavigationLink(Url.Action<AdminController>(x => x.ContextWithRouteValues(0), new { id = 37 }), "RouteValuePolicy - Id 37 - Result not cached") %></p>

	<h3>Examples of custom context based policies</h3>
	<p><%= Html.NavigationLink(Url.Action<AdminController>(x => x.CustomContext(0), new { id = 5 }), "CustomSecurityContext") %></p>
	<p><%= Html.NavigationLink(Url.Action<AdminController>(x => x.PrincipalContext()), "PrincipalSecurityContext") %></p>
	
	<h3>Testing of policy result cache</h3>
	<p><%= Html.NavigationLink(Url.Action<CacheController>(x => x.PolicyLevel()), "Policy level caching 1") %></p>
	<p><%= Html.NavigationLink(Url.Action<CacheController>(x => x.PolicyLevel()), "Policy level caching 2") %></p>
	<p><%= Html.NavigationLink(Url.Action<CacheController>(x => x.ControllerPolicyLevel()), "Controller policy level caching 1") %></p>
	<p><%= Html.NavigationLink(Url.Action<CacheController>(x => x.ControllerPolicyLevel()), "Controller policy level caching 2") %></p>
	<p><%= Html.NavigationLink(Url.Action<CacheController>(x => x.ControllerActionPolicyLevel()), "Controller action Policy level caching 1") %></p>
	<p><%= Html.NavigationLink(Url.Action<CacheController>(x => x.ControllerActionPolicyLevel()), "Controller action Policy level caching 2") %></p>

	<h2>What do I have</h2>
	<pre><%= Model.WhatDoIHave %></pre>
</asp:Content>