using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Diploma.DiplomaDb
{
    public class Role : TranslatableExtended
    {
        public Role()
        {
            this.Actions = new HashSet<Action>();
            this.Users = new HashSet<User>();
        }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Action> Actions { get; set; }
    }
}