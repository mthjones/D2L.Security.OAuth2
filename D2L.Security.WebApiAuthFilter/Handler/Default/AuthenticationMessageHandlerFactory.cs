﻿using System;
using System.Net.Http;
using System.Web.Http;
using SimpleLogInterface;

namespace D2L.Security.WebApiAuthFilter.Handler.Default {

	/// <summary>
	/// A factory that creates authentication delegating message handlers for use with Web API.
	/// </summary>
	public sealed class AuthenticationMessageHandlerFactory : IAuthenticationMessageHandlerFactory {

		/// <summary>
		/// Creates an authentication delegating message handler for use with Web API.
		/// </summary>
		/// <param name="httpConfiguration">The Web API configuration object.</param>
		/// <param name="authenticationEndpoint">The endpoint of the auth service.</param>
		/// <param name="verifyCsrf">If true, CSRF validation will also be performed.</param>
		/// <param name="logProvider">Log provider. If no logging, pass NullLogProvider.Instance.</param>
		/// <returns>A delegating message handler that performs authentication, and optionally does CSRF validation.</returns>
		public DelegatingHandler Create(
			HttpConfiguration httpConfiguration,
			Uri authenticationEndpoint,
			bool verifyCsrf,
			ILogProvider logProvider
			) {

			return new AuthenticationMessageHandler(
				httpConfiguration,
				authenticationEndpoint,
				verifyCsrf,
				logProvider
				);
		}
	}
}
