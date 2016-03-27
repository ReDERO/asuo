using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diploma.DiplomaDb
{
    public class SkillPackage : TranslatableExtended
    {
        public SkillPackage()
        {
            this.Skills = new HashSet<Skill>();
            this.Subscribers = new HashSet<User>();
        }

        public int AuthorId { get; set; }
        public virtual User Author { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }
        public virtual ICollection<User> Subscribers { get; set; }
    }
}