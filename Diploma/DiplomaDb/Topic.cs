using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diploma.DiplomaDb
{
    public class Topic : Translatable
    {
        public Topic()
        {
            this.Children = new HashSet<Topic>();
            this.Tasks = new HashSet<Task>();
        }

        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        public int AuthorId { get; set; }
        public virtual User Author { get; set; }

        public int? ParentId { get; set; }
        public virtual Topic Parent { get; set; }
        public virtual ICollection<Topic> Children { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}