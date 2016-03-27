using Diploma.DiplomaDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diploma.Models
{
    public class SkillListItemViewModel : TranslatableExtendedViewModel
    {
        public SkillListItemViewModel(Skill skill)
            :base(skill)
        {
        }
    }

    public class SkillListViewModel : List<SkillListItemViewModel>
    {
        public SkillListViewModel(IEnumerable<Skill> set)
        {
            foreach (var skill in set) base.Add(new SkillListItemViewModel(skill));
        }
    }

    public class SkillListItemExtendedViewModel : SkillListItemViewModel
    {
        public SkillListItemExtendedViewModel(Skill skill)
            : base(skill)
        {
        } 

        public bool Added { get; set; }
    }

    public class EditSkillsViewModel : List<SkillListItemExtendedViewModel>
    {
        public EditSkillsViewModel(IEnumerable<Skill> skills, List<Skill> userSkills)
        {
            foreach(var skill in skills)
            {
                base.Add(new SkillListItemExtendedViewModel(skill) { Added = userSkills.Exists(s => s.Equals(skill)) });
            }
        }
    }

    public class EditSkillsSkillsViewModel
    {
        public EditSkillsSkillsViewModel(User user, List<Skill> skillList)
        {
            this.Id = user.Id;

            var skills = new List<SkillListItemViewModel>();
            foreach(var skill in skillList) skills.Add(new SkillListItemViewModel(skill));

            this.Skills = new List<SelectListItem>();
            foreach (var role in skills)
            {
                this.Skills.Add(new SelectListItem()
                {
                    Value = role.Id.ToString(),
                    Text = role.Caption,
                    Selected = user.AchivedSkills.Any(r => r.Id == role.Id)
                });
            }
        }

        public EditSkillsSkillsViewModel(Task task, List<Skill> skillList)
        {
            this.Id = task.Id;

            var skills = new List<SkillListItemViewModel>();
            foreach (var skill in skillList) skills.Add(new SkillListItemViewModel(skill));

            this.Skills = new List<SelectListItem>();
            foreach (var role in skills)
            {
                this.Skills.Add(new SelectListItem()
                {
                    Value = role.Id.ToString(),
                    Text = role.Caption,
                    Selected = task.Skills.Any(r => r.Id == role.Id)
                });
            }
        }

        public EditSkillsSkillsViewModel(SkillPackage skillPackage, List<Skill> skillList)
        {
            this.Id = skillPackage.Id;

            var skills = new List<SkillListItemViewModel>();
            foreach (var skill in skillList) skills.Add(new SkillListItemViewModel(skill));

            this.Skills = new List<SelectListItem>();
            foreach (var role in skills)
            {
                this.Skills.Add(new SelectListItem()
                {
                    Value = role.Id.ToString(),
                    Text = role.Caption,
                    Selected = skillPackage.Skills.Any(r => r.Id == role.Id)
                });
            }
        }

        public int Id { get; set; }
        public List<SelectListItem> Skills { get; set; }
    }
}