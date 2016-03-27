using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diploma.DiplomaDb
{
    public class Skill : TranslatableExtended
    {
        public Skill()
        {
            this.Owners = new HashSet<User>();
            this.Wishers = new HashSet<UserWishedSkill>();
            this.Tasks = new HashSet<Task>();
            this.SkillPackages = new HashSet<SkillPackage>();
        }

        public virtual ICollection<User> Owners { get; set; }
        public virtual ICollection<UserWishedSkill> Wishers { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<SkillPackage> SkillPackages { get; set; }
    }
}