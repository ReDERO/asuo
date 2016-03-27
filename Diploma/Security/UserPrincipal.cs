using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Diploma.Security
{
    public class UserPrincipal : IPrincipal
    {
        public UserPrincipal(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int[] Roles { get; set; }
        public IIdentity Identity { get; private set; }

        public bool IsInRole(int roleId)
        {
            return Roles.Any(r => roleId == r);
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }

    public class UserPrincipalSerializeModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int[] Roles { get; set; }
    }
}