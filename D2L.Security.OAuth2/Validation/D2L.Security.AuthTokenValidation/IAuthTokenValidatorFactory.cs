﻿namespace D2L.Security.AuthTokenValidation {
	public interface IAuthTokenValidatorFactory {
		IAuthTokenValidator Create();
	}
}
