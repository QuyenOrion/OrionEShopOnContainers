﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace OrionEShopOnContainer.Services.Identity.API
{
    public class MyClaim : IdentityResource
    {
        public MyClaim()
        {
            base.Name = "myclaim";
            base.DisplayName = "My Claim";
            base.Description = "My Claim";
            base.Emphasize = true;
            base.UserClaims = new List<string> { "myclaim" };
        }
    }
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new MyClaim(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            { new ApiScope("api1", "My API")};

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    
                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "api1" }
                },
                new Client
                {
                    ClientId = "web-shopping-http-aggregator",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    
                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    //// scopes that client has access to
                    //AllowedScopes = { }
                },
                new Client
                {
                    ClientId = "mvc",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    ClientUri = "http://localhost:5100",

                    AllowedGrantTypes = GrantTypes.Code,
                    AllowOfflineAccess = true,                    

                    // where to redirect to after login
                    RedirectUris = { "http://localhost:5100/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5100/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "myclaim",
                        "api1"
                    }
                },
                new Client
                {
                    ClientId = "webmvc",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    ClientUri = "https://www.orionproshop.store",

                    AllowedGrantTypes = GrantTypes.Code,
                    AllowOfflineAccess = true,                 

                    // where to redirect to after login
                    RedirectUris = { "https://www.orionproshop.store/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://www.orionproshop.store/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "myclaim",
                        "api1"
                    }
                }
            };
    }
}