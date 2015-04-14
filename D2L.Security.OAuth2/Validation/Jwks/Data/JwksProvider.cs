﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace D2L.Security.OAuth2.Validation.Jwks.Data {
	internal sealed class JwksProvider : IJwksProvider {
		
		async Task<JwksResponse> IJwksProvider.RequestJwksAsync( Uri endpoint, bool skipCache ) {

			// TODO: control httpclient creation?
			using( var httpClient = new HttpClient() ) {

				using( HttpResponseMessage response = await httpClient.GetAsync( endpoint ).ConfigureAwait( false ) ) {
					response.EnsureSuccessStatusCode();
					string jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait( false );
					return new JwksResponse(
						fromCache: false,
						jwksJson: jsonResponse
					);
				}

			}
		}
	}
}
