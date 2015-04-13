﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D2L.Security.OAuth2.Caching;
using D2L.Security.OAuth2.Validation.Jwks;
using D2L.Security.OAuth2.Validation.Jwks.Data;

namespace D2L.Security.OAuth2.Validation {
	public static class AccessTokenValidatorFactory {
		
		public static IAccessTokenValidator Create( ICache cache = null ) {
			if( cache == null ) {
				cache = new NullCache();
			}

			IJwksProvider jwksProvider = new JwksProvider();
			IJwksProvider cacheJwksProvider = new CachedJwksProvider( cache, jwksProvider );
			
			IPublicKeyProvider tokenProvider = new PublicKeyProvider( cacheJwksProvider );
			IAccessTokenValidator validator = new AccessTokenValidator( tokenProvider );
			
			return validator;
		}
	}
}
