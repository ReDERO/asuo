using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diploma.DiplomaDb
{
    public class Action : Translatable
    {
        public Action()
        {
            this.Roles = new HashSet<Role>();
        }
        public string Name { get; set; }

        public int EntityId { get; set; }
        public virtual Controller Entity { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}