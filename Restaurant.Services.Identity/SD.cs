﻿using Duende.IdentityServer.Models;

namespace Restaurant.Services.Identity
{
    public static class SD
    {
        public const string Admin= "Admin";
        public const string Customer = "Customer";

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes=>
            new List<ApiScope> { 
                new ApiScope("Restaurant","RestaurantServer"),
                new ApiScope(name: "read",   displayName: "Read your data."),
                new ApiScope(name: "write",  displayName: "Write your data."),
                new ApiScope(name: "delete", displayName: "Delete your data.")
            };  
        public static IEnumerable<Client>Clients=>
            new List<Client>
            {
                new Client
                {
                    ClientId="restaurant",
                    ClientSecrets= { new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes={"read","write","profile"}
                },

            }
    }
}
