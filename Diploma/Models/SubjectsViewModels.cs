using Diploma.DiplomaDb;
using Diploma.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diploma.Models
{
    public class SubjectListItemViewModel : TranslatableViewModel
    {
        public SubjectListItemViewModel(Subject subject)
            : base(subject)
        {
        }
    }

    public class SubjectList : List<SubjectListItemViewModel>
    {
        public SubjectList(IEnumerable<Subject> set, User user = null)
        {
            if (user == null)
                DontAdapt(set);
            else
                Adapt(set, user);
        }

        private void DontAdapt(IEnumerable<Subject> set)
        {
            foreach (var subject in set)
            {
                base.Add(new SubjectListItemViewModel(subject));
            }
        }

        private void Adapt(IEnumerable<Subject> set, User user)
        {
            var adapter = new Adapter();
            var adaptedList = adapter.AdaptList(set, user);

            foreach (var subject in adaptedList)
            {
                base.Add(new SubjectListItemViewModel(subject.Item));
            }
        }
    }
}