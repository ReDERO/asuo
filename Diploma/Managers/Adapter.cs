using Diploma.DiplomaDb;
using Diploma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diploma.Managers
{
    public class AdaptedListItem<T>
    {
        public AdaptedListItem(T item)
        {
            this.Item = item;
        }

        /// <summary>
        /// Элемент, для которого подсчитывается количество желаемых навыков
        /// </summary>
        public T Item { get; private set; }
        
        /// <summary>
        /// Количество умений, которые пользователь уже получил
        /// </summary>
        public int CountOfAchieved { get; set; }

        /// <summary>
        /// Количество умений, которые пользователь желает получить
        /// </summary>
        //public int CountOfWished { get; set; }

        /// <summary>
        /// Количество умений, которых нет ни в одном из списков умений пользователя
        /// </summary>
        public int Count { get; set; }
    }

    public class Adapter
    {
        public List<AdaptedListItem<Subject>> AdaptList(IEnumerable<Subject> subjects, User user)
        {
            var temp = new List<AdaptedListItem<Subject>>();
            foreach (var subject in subjects)
            {
                var item = new AdaptedListItem<Subject>(subject);
                temp.Add(item);
                foreach (var topic in subject.Topics)
                {
                    foreach(var task in user.CompletedTasks)
                    {
                        if (topic.Tasks.Contains(task)) item.Count++;
                    }
                }
            }
            return temp.OrderByDescending(item => item.Count).ToList();
        }
        public List<AdaptedListItem<Topic>> AdaptList(IEnumerable<Topic> topics, User user)
        {
            var temp = new List<AdaptedListItem<Topic>>();
            foreach (var topic in topics)
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
                var item = new AdaptedListItem<Topic>(topic);
                temp.Add(item);
                foreach (var task in user.CompletedTasks)
                {
                    if (tasks.Contains(task)) item.Count++;
                }
            }
            return temp.OrderByDescending(item => item.Count).ToList();
        }
        public void AdaptList(IEnumerable<Task> tasks, User user, RecommendedListViewModel result)
        {
            var wishedSkills = user.WishedSkills.OrderByDescending(ws => ws.Requirement).ToList();
            foreach (var wishSkill in wishedSkills)
            {
                var temp = new List<AdaptedListItem<Task>>();
                foreach(var task in wishSkill.Skill.Tasks)
                {
                    var t = new AdaptedListItem<Task>(task)
                    {
                        CountOfAchieved = task.Skills.Intersect(user.AchivedSkills).ToList().Count,
                        Count = task.Skills.Count
                    };
                    temp.Add(t);
                }
                var current = new RecomendedTaskList(wishSkill.Skill);
                foreach (var groups in temp.OrderBy(s => s.Count).ToList().GroupBy(s => s.Count))
                {
                    foreach (var task in groups.OrderByDescending(s => s.CountOfAchieved).ToList())
                    {
                        current.Add(new RecomendedTaskListItem(task.Item));
                    }
                }
                result.Add(current);
            }
        }
        public List<AdaptedListItem<Task>> AdaptList(IEnumerable<Task> tasks, User user)
        {
            var temp = new List<AdaptedListItem<Task>>();
            var toMid = new List<AdaptedListItem<Task>>();
            var toEnd = new List<AdaptedListItem<Task>>();
            var completed = new List<AdaptedListItem<Task>>();
            foreach (var task in tasks)
            {
                var item = new AdaptedListItem<Task>(task);
                if (user.CompletedTasks.Contains(task))
                {
                    completed.Add(item);
                }
                else
                {
                    item.Count = task.Skills.Intersect(user.WishedSkills.Select(ws => ws.Skill)).Count();
                    if (item.Count != 0)
                    {
                        temp.Add(item);
                    }
                    else
                    {
                        item.Count = task.Skills.Except(user.AchivedSkills).Count();
                        if (item.Count != 0)
                        {
                            toMid.Add(item);
                        }
                        else
                        {
                            toEnd.Add(item);
                        }
                    }
                }
            }
            var tempResult = temp.OrderBy(item => item.Count).ToList();
            var midResult = toMid.OrderBy(item => item.Count).ToList();
            tempResult.AddRange(midResult);
            tempResult.AddRange(toEnd);
            tempResult.AddRange(completed);
            return tempResult;
        }
        public List<AdaptedListItem<SkillPackage>> AdaptList(IEnumerable<SkillPackage> skillPackages, User user)
        {
            var subscribed = new List<AdaptedListItem<SkillPackage>>();
            var temp = new List<AdaptedListItem<SkillPackage>>();
            var completed = new List<AdaptedListItem<SkillPackage>>();
            foreach (var skillPackage in skillPackages)
            {
                var item = new AdaptedListItem<SkillPackage>(skillPackage);
                item.Count = skillPackage.Skills.Except(user.AchivedSkills).Count();
                if (user.SubscribedSkillPackages.Contains(skillPackage))
                {
                    if (item.Count != 0)
                    {
                        subscribed.Add(item);
                    }
                    else
                    {
                        completed.Add(item);
                    }
                }
                else
                {
                    temp.Add(item);
                }
            }
            var subscribedResult = subscribed.OrderBy(item => item.Count).ToList();
            var tempResult = temp.OrderBy(item => item.Count).ToList();
            subscribedResult.AddRange(tempResult);
            subscribedResult.AddRange(completed);
            return subscribedResult;
        }
    }
}