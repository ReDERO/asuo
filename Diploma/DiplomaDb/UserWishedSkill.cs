using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diploma.DiplomaDb
{
    public class UserWishedSkill
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int SkillId { get; set; }
        public virtual Skill Skill { get; set; }

        public int Requirement { get; set; }
    }
}