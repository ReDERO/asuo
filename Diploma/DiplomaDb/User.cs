using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Diploma.DiplomaDb
{
    public class User
    {
        public User()
        {
            this.Roles = new HashSet<Role>();
            this.AchivedSkills = new HashSet<Skill>();
            this.WishedSkills = new HashSet<UserWishedSkill>();
            this.Tasks = new HashSet<Task>();
            this.CreatedSkillPackages = new HashSet<SkillPackage>();
            this.SubscribedSkillPackages = new HashSet<SkillPackage>();
            this.CompletedTasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        [Index(IsUnique=true)]
        [MaxLength(100)]
        public String Email { get; set; }
        [Required]
        public String Password { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Skill> AchivedSkills { get; set; }
        public virtual ICollection<UserWishedSkill> WishedSkills { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<SkillPackage> CreatedSkillPackages { get; set; }
        public virtual ICollection<SkillPackage> SubscribedSkillPackages { get; set; }
        public virtual ICollection<Task> CompletedTasks { get; set; }
    }
}