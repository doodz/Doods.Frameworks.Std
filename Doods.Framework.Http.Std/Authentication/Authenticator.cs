﻿using System.Collections.Generic;
using Doods.Framework.ApiClientBase.Std.Authentication;
using RestSharp.Authenticators;

namespace Doods.Framework.Http.Std.Authentication
{
    internal class Authenticator
    {
        public Authenticator(Credentials credentials)
        {
            Credentials = credentials;
            Authenticators = new Dictionary<AuthenticationType, IAuthenticator>
            {
                {AuthenticationType.Anonymous, new AnonymousAuthenticator()},
                {AuthenticationType.Basic, new HttpBasicAuthenticator(credentials.Login, credentials.Password)},
                { AuthenticationType.SimpleHttpHeader,new SimpleHttpHeaderAuthenticator(credentials.Login, credentials.Password)},
                {
                    AuthenticationType.Simple,
                    new SimpleAuthenticator("login", credentials.Login, "password", credentials.Password)
                },
            };


            CreatedAuthenticator = Authenticators[credentials.AuthenticationType];
        }

        public IAuthenticator CreatedAuthenticator { get; private set; }

        public Credentials Credentials { get; private set; }

        private Dictionary<AuthenticationType, IAuthenticator> Authenticators { get; set; }
    }
}