using Diploma.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Diploma.DiplomaDb
{
    public class Task : TranslatableExtended
    {
        public Task()
        {
            this.Skills = new HashSet<Skill>();
        }

        [Display(Name = "SourceURL", ResourceType = typeof(Resource))]
        public string SourceURL { get; set; }

        [Display(Name = "InstructionEN", ResourceType = typeof(Resource))]
        public string InstructionEN { get; set; }

        [Display(Name = "InstructionRU", ResourceType = typeof(Resource))]
        public string InstructionRU { get; set; }

        [Display(Name = "InstructionUA", ResourceType = typeof(Resource))]
        public string InstructionUA { get; set; }

        [Display(Name = "Access", ResourceType = typeof(Resource))]
        public bool Public { get; set; }
        
        public int TopicId { get; set; }
        public virtual Topic Topic { get; set; }

        public int AuthorId { get; set; }
        public virtual User Author { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }
        public virtual ICollection<User> FinishedUsers { get; set; }
    }
}