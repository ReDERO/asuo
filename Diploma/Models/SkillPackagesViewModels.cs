using Diploma.DiplomaDb;
using Diploma.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diploma.Models
{
    public enum SkillPackageState { Done, InProgress, NotStarted }
    public class SkillPackageListItemViewModel : TranslatableViewModel
    {
        public SkillPackageListItemViewModel(SkillPackage skillPackage)
            : base(skillPackage)
        {
            State = SkillPackageState.NotStarted;
        }

        public SkillPackageState State { get; set; }
    }

    public class SkillPackageListViewModel : List<SkillPackageListItemViewModel>
    {
        public SkillPackageListViewModel(IEnumerable<SkillPackage> set, User user)
        {
            if(user == null)
            {
                DontAdapt(set);
            }
            else
            {
                Adapt(set, user);
            }
        }

        private void Adapt(IEnumerable<SkillPackage> set, User user)
        {
            var adapter = new Adapter();
            var adaptList = adapter.AdaptList(set, user);

            foreach(var skillPackage in adaptList)
            {
                var sp = new SkillPackageListItemViewModel(skillPackage.Item);
                if (user.SubscribedSkillPackages.Contains(skillPackage.Item))
                {
                    if (skillPackage.Count == 0)
                    {
                        sp.State = SkillPackageState.Done;
                    }
                    else
                    {
                        sp.State = SkillPackageState.InProgress;
                    }
                }

                base.Add(sp);
            }
        }

        private void DontAdapt(IEnumerable<SkillPackage> set)
        {
            foreach (var skillPackage in set)
            {
                base.Add(new SkillPackageListItemViewModel(skillPackage));
            }
        }
    }

    public class SkillPackageDetailsViewModel : TranslatableExtendedViewModel
    {
        public SkillPackageDetailsViewModel(SkillPackage skillPackage)
            : base(skillPackage)
        {
            this.Author = skillPackage.Author;
            this.Skills = new SkillListViewModel(skillPackage.Skills);
        }

        public User Author { get; set; }
        public IEnumerable<SkillListItemViewModel> Skills { get; private set; }
    }

    public class SkillPackageEditViewModel : TranslatableExtended
    {
        public SkillPackageEditViewModel(SkillPackage skillPackage)
        {
            this.AuthorId = skillPackage.AuthorId;
            this.CaptionEN = skillPackage.CaptionEN;
            this.CaptionRU = skillPackage.CaptionRU;
            this.CaptionUA = skillPackage.CaptionUA;
            this.DescriptionEN = skillPackage.DescriptionEN;
            this.DescriptionRU = skillPackage.DescriptionRU;
            this.DescriptionUA = skillPackage.DescriptionUA;
            this.Id = skillPackage.Id;
            this.Skills = new SkillListViewModel(skillPackage.Skills);
        }

        public int AuthorId { get; set; }

        public IEnumerable<SkillListItemViewModel> Skills { get; private set; }
    }
}