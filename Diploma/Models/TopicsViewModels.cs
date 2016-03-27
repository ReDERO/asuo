using Diploma.DiplomaDb;
using Diploma.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diploma.Models
{
    public class TopicListItemViewModel : TranslatableViewModel
    {
        public TopicListItemViewModel(Topic topic)
            : base(topic)
        {
            this.Parent = topic.ParentId;
        }

        public int? Parent { get; set; }
    }

    public class TopicList : List<TopicListItemViewModel>
    {
        public TopicList(IEnumerable<Topic> set, User user = null)
        {
            if (user == null)
            {
                DontAdapt(set);
            }
            else
            {
                Adapt(set, user);
            }
        }

        private void DontAdapt(IEnumerable<Topic> set)
        {
            foreach (var topic in set)
            {
                base.Add(new TopicListItemViewModel(topic));
            }
        }

        private void Adapt(IEnumerable<Topic> set, User user)
        {
            var adapter = new Adapter();
            var adaptedList = adapter.AdaptList(set, user);

            foreach (var topic in adaptedList)
            {
                base.Add(new TopicListItemViewModel(topic.Item));
            }
        }
    }

    public class TopicDetails
    {
        public TopicDetails(IEnumerable<TranslatableViewModel> topics, IEnumerable<TranslatableViewModel> tasks)
        {

        }
    }
}