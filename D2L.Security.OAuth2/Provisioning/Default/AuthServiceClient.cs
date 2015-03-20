﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace D2L.Security.AuthTokenProvisioning.Default {
	
	public sealed class AuthServiceClient : IAuthServiceClient {

		private static AuthenticationHeaderValue m_authorization =
			new AuthenticationHeaderValue( "Basic", ToBase64( "lms.dev.d2l:lms_secret" ) );

		private readonly HttpClient m_client;
		private readonly bool m_disposeClient;
		private readonly Uri m_tokenProvisioningEndpoint;

		public AuthServiceClient(
			Uri tokenProvisioningEndpoint
		)
			: this(
			  httpClient: new HttpClient(),
			  disposeHttpClient: true,
			  tokenProvisioningEndpoint: tokenProvisioningEndpoint
			) { }

		public AuthServiceClient(
			HttpClient httpClient,
			Uri tokenProvisioningEndpoint,
			bool disposeHttpClient = true
		) {
			m_client = httpClient;
			m_disposeClient = disposeHttpClient;
			m_tokenProvisioningEndpoint = tokenProvisioningEndpoint;
		}

		async Task<IAccessToken> IAuthServiceClient.ProvisionAccessTokenAsync(
			string assertion,
			IEnumerable<Scope> scopes
		) {
			string requestBody = BuildFormContents( assertion, scopes );
			using( HttpResponseMessage response = await MakeRequest( requestBody ) ) {
				response.EnsureSuccessStatusCode();

				using( var resultStream = await response.Content.ReadAsStreamAsync() ) {
					IAccessToken accessToken = SerializationHelper.ExtractAccessToken( resultStream );
					return accessToken;
				}
			}
		}

		void IDisposable.Dispose() {
			if( m_disposeClient ) {
				m_client.Dispose();
			}
		}

		private Task<HttpResponseMessage> MakeRequest( string body ) {
			var request = new HttpRequestMessage( HttpMethod.Post, m_tokenProvisioningEndpoint );
			request.Content = new StringContent( body, Encoding.UTF8, "application/x-www-form-urlencoded" );
			
			// TODO: If we can solve US49562 we can get rid of this authorization header
			request.Headers.Authorization = m_authorization;

			return m_client.SendAsync( request );
		}

		private static string BuildFormContents( string assertion, IEnumerable<Scope> scopes ) {
			StringBuilder builder = new StringBuilder( "grant_type=" );
			builder.Append( Constants.AssertionGrant.GRANT_TYPE );

			builder.Append( "&assertion=" );
			builder.Append( assertion );

			var scopesString = String.Join( " ", scopes );
			scopesString = WebUtility.UrlEncode( scopesString );
			builder.Append( "&scope=" );
			builder.Append( scopesString );

			var result = builder.ToString();
			return result;
		}

		private static string ToBase64( string me ) {
			byte[] plainTextBytes = Encoding.UTF8.GetBytes( me );
			return Convert.ToBase64String( plainTextBytes );
		}

	}
}