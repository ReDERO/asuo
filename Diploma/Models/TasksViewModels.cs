using Diploma.DiplomaDb;
using Diploma.Managers;
using Diploma.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Diploma.Models
{
    public class TaskListItemViewModel : TranslatableViewModel
    {
        public TaskListItemViewModel(Task task)
            : base(task)
        {
            this.IsCompleted = false;
        }

        public bool IsCompleted { get; set; }
    }

    public class RecomendedTaskListItem : TranslatableViewModel
    {
        public RecomendedTaskListItem(Task task)
            : base(task)
        {
        }
    }

    public class RecomendedTaskList : List<RecomendedTaskListItem>
    {
        public RecomendedTaskList(Skill skill)
        {
            this.Skill = new SkillListItemViewModel(skill);
        }

        public TranslatableViewModel Skill { get; set; }
    }

    public class RecommendedListViewModel : List<RecomendedTaskList>
    {
        public RecommendedListViewModel(IEnumerable<Task> set, User user = null)
        {
            if (user != null)
            {
                Adapt(set, user);
            }
        }

        private void Adapt(IEnumerable<Task> set, User user)
        {
            var adapter = new Adapter();
            adapter.AdaptList(set.Where(t => t.Public || user.Tasks.Contains(t)), user, this);
        }
    }

    public class TaskListViewModel : List<TaskListItemViewModel>
    {
        public TaskListViewModel(Topic topic, User user = null)
        {
            var tasks = topic.Tasks.ToList();
            var subtopics = topic.Children.ToList();
            while (subtopics.Count != 0)
            {
                var subtopic = subtopics[0];
                if (subtopic.Children.Count != 0) subtopics.AddRange(subtopic.Children);
                if (subtopic.Tasks.Count != 0) tasks.AddRange(subtopic.Tasks);
                subtopics.RemoveAt(0);
            }
            CreateList(tasks, user);
        }

        private void CreateList(IEnumerable<Task> set, User user = null)
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

        private void Adapt(IEnumerable<Task> set, User user)
        {
            var adapter = new Adapter();
            var adaptedList = adapter.AdaptList(set.Where(t => t.Public || user.Tasks.Contains(t)), user);

            foreach (var task in adaptedList)
            {
                base.Add(new TaskListItemViewModel(task.Item));
            }
        }

        private void DontAdapt(IEnumerable<Task> set)
        {
            foreach (var task in set)
            {
                if (task.Public)
                {
                    base.Add(new TaskListItemViewModel(task));
                }
            }
        }
    }

    public class TaskEditViewModel : TranslatableExtended
    {
        public TaskEditViewModel(Task task)
        {
            this.AuthorId = task.AuthorId;
            this.CaptionEN = task.CaptionEN;
            this.CaptionRU = task.CaptionRU;
            this.CaptionUA = task.CaptionUA;
            this.DescriptionEN = task.DescriptionEN;
            this.DescriptionRU = task.DescriptionRU;
            this.DescriptionUA = task.DescriptionUA;
            this.Id = task.Id;
            this.InstructionEN = task.InstructionEN;
            this.InstructionRU = task.InstructionRU;
            this.InstructionUA = task.InstructionUA;
            this.Public = task.Public;
            this.SourceURL = task.SourceURL;
            this.TopicId = task.TopicId;
            this.Skills = new SkillListViewModel(task.Skills);
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
        public int AuthorId { get; set; }

        public IEnumerable<SkillListItemViewModel> Skills { get; set; }
    }

    public class TaskDetailsViewModel : TranslatableExtendedViewModel
    {
        public TaskDetailsViewModel(Task task)
            : base(task)
        {
            switch (Language)
            {
                case "en": Instruction = task.InstructionEN; break;
                case "ru": Instruction = task.InstructionRU; break;
                case "uk": Instruction = task.InstructionUA; break;
            }

            this.SourceURL = task.SourceURL;
            this.Author = task.Author;
            this.TopicId = task.TopicId;
        }

        public string SourceURL { get; set; }
        public string Instruction { get; set; }
        public User Author { get; set; }
        public int TopicId { get; set; }
    }
}