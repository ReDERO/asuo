using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diploma.DiplomaDb
{
    public class Subject : Translatable
    {
        public Subject()
        {
            this.Topics = new HashSet<Topic>();
        }

        public virtual ICollection<Topic> Topics { get; set; }
    }
}