using System;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNet.Identity;

namespace ListPlanner_GamGam.Extensions
{


    public class HsmClaimTypes
    {
        public const string ProviderImageUrl = "ProviderImageUrl";
        public const string ProviderUserName = "ProviderUserName";
    }
    /// <summary>
    /// Extensions making it easier to get the user name/user id claims off of an identity
    /// 
    /// </summary>
    public static class HsmIdentityExtensions
    {
        
        public static string GetImageUrl(this IIdentity identity)
        {
            if (identity == null)
                throw new ArgumentNullException(nameof(identity));

            var claimsIdentity = identity as ClaimsIdentity;
            if (claimsIdentity != null)
                return IdentityExtensions.FindFirstValue(claimsIdentity, HsmClaimTypes.ProviderImageUrl);
            return (string)null;
        }

        public static string GetEmail(this IIdentity identity)
        {
            if (identity == null)
                throw new ArgumentNullException(nameof(identity));

            var claimsIdentity = identity as ClaimsIdentity;
            if (claimsIdentity != null)
                return IdentityExtensions.FindFirstValue(claimsIdentity, ClaimTypes.Email);            
            return (string)null;
        }
        public static string GetName(this IIdentity identity)
        {
            if (identity == null)
                throw new ArgumentNullException(nameof(identity));

            var claimsIdentity = identity as ClaimsIdentity;
            if (claimsIdentity != null)
                return IdentityExtensions.FindFirstValue(claimsIdentity, HsmClaimTypes.ProviderUserName);
            return (string)null;
        }


        /// <summary>
        /// Return the claim value for the first claim with the specified type if it exists, null otherwise
        /// 
        /// </summary>
        /// <param name="identity"/><param name="claimType"/>
        /// <returns/>
        public static string FindFirstValue(this ClaimsIdentity identity, string claimType)
        {
            if (identity == null)
                throw new ArgumentNullException("identity");
            Claim first = identity.FindFirst(claimType);
            if (first == null)
                return (string)null;
            return first.Value;
        }
    }
}