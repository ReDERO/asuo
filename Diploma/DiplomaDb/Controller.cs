using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diploma.DiplomaDb
{
    public class Controller : Translatable
    {
        public Controller()
        {
            this.Actions = new HashSet<Action>();
        }
        public string Name { get; set; }

        public virtual ICollection<Action> Actions { get; set; }
    }
}