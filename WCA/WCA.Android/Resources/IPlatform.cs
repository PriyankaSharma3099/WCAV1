using System;
using Microsoft.Identity.Client;

namespace WCA.Droid.Resources
{
	
        public interface IPlatform
        {
            IPublicClientApplication GetIdentityClient(string applicationId);
        }
    
}

