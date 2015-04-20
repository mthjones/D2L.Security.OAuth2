﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using D2L.Security.OAuth2.Scopes;

namespace D2L.Security.OAuth2 {
	public interface ID2LPrincipal {
		
		string UserId { get; }
		string TenantId { get; }
		
		PrincipalType Type { get; }

		IEnumerable<Scope> Scopes { get; }
		IEnumerable<Claim> AllClaims { get; }

		/// <summary>
		/// The expiration date of the access token provided with the request
		/// </summary>
		DateTime AccessTokenExpiry { get; }

		string AccessToken { get; }
		string AccessTokenId { get; }
	}
}
