using Diploma.DiplomaDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Diploma.Models
{
    /// <summary>
    /// status data transfer object
    /// </summary>
    public class StatusDTO
    {
        /// <summary>
        /// jplist action: paging, filter, sort, etc.
        /// </summary>
        public string action { get; set; }

        /// <summary>
        /// jplist control name
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// jplist control type: drop-down, textbox, etc.
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// status related data
        /// </summary>
        public StatusDataDTO data { get; set; }
    }

    /// <summary>
    /// additional status data transfer object
    /// </summary>
    public class StatusDataDTO
    {
        #region "Common"

        /// <summary>
        /// jquery path or "default"
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// ignore regex
        /// </summary>
        public string ignore { get; set; }

        #endregion

        #region "Filtering"

        /// <summary>
        /// filter value
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// filter type: TextFilter, pathGroup, ..
        /// </summary>
        public string filterType { get; set; }

        /// <summary>
        /// list of jquery paths
        /// </summary>
        public List<string> pathGroup { get; set; }

        #endregion

        #region "Sorting"

        /// <summary>
        /// date time format
        /// </summary>
        public string dateTimeFormat { get; set; }

        /// <summary>
        /// sort order: asc/desc
        /// </summary>
        public string order { get; set; }

        #endregion

        #region "Pagination"

        /// <summary>
        /// items number - string value (it could be number or "all")
        /// </summary>
        public string number { get; set; }

        /// <summary>
        /// the current page index
        /// </summary>
        public int currentPage { get; set; }

        #endregion
    }

    /// <summary>
    /// This class creates SQL queries per jPList statuses from client side
    /// </summary>
    public class StatusQueries
    {
        private IEnumerable<Skill> SkillSet;
        public IEnumerable<SkillListItemExtendedViewModel> Skills { get; private set; }
        public int CountBeforePagination { get; private set; }
        /// <summary>
        /// status queries
        /// </summary>
        /// <param name="statuses"></param>
        /// <param name="set"></param>
        public StatusQueries(List<StatusDTO> statuses, DataContext db, List<Skill> checkedSkills)
        {
            this.SkillSet = new HashSet<Skill>();

            if (statuses != null)
            {
                FilterBySubjectAndTopic(statuses, db);

                this.Skills = new EditSkillsViewModel(this.SkillSet, checkedSkills);

                FilterByCaptionAndDescription(statuses);

                this.Skills = this.Skills.ToList();
                this.CountBeforePagination = this.Skills.Count();

                var pagingStatus = statuses.FirstOrDefault(status => status.action == "paging");
                if (pagingStatus != null)
                {
                    this.Paging(pagingStatus);
                }
            }
        }

        #region "Private Methods"

        private void FilterBySubjectAndTopic(List<StatusDTO> statuses, DataContext db)
        {
            var subjectStatus = statuses.FirstOrDefault(status => status.name == "subject-filter");
            if (subjectStatus != null)
            {
                int subjectId;
                if (!String.IsNullOrEmpty(subjectStatus.data.path) && Int32.TryParse(subjectStatus.data.path, out subjectId))
                {
                    var topicStatus = statuses.FirstOrDefault(status => status.name == "topic-filter");
                    if (subjectStatus != null)
                    {
                        int topicId;
                        if (!String.IsNullOrEmpty(topicStatus.data.path) && Int32.TryParse(topicStatus.data.path, out topicId))
                        {
                            db.Subjects.Find(subjectId).Topics.Where(topic => topic.Id == topicId).AsParallel().ForAll(topic =>
                                topic.Tasks.AsParallel().ForAll(task =>
                                    task.Skills.AsParallel().ForAll(skill =>
                                        ((HashSet<Skill>)this.SkillSet).Add(skill))));
                        }
                        else
                        {
                            var topics = db.Subjects.Find(subjectId).Topics;
                            foreach(var topic in topics)
                            {
                                foreach(var task in topic.Tasks)
                                {
                                    task.Skills.AsParallel().ForAll(skill =>
                                        ((HashSet<Skill>)this.SkillSet).Add(skill));
                                }
                            }
                        }
                    }
                }
                else
                {
                    this.SkillSet = db.Skills;
                }
            }
        }
        private void FilterByCaptionAndDescription(List<StatusDTO> statuses)
        {
            var captionStatus = statuses.FirstOrDefault(status => status.name == "caption-filter");
            if (captionStatus != null)
            {
                if (!String.IsNullOrEmpty(captionStatus.data.path) && !String.IsNullOrEmpty(captionStatus.data.value))
                {
                    this.Skills = this.Skills.Where(obj => obj.Caption.Contains(captionStatus.data.value));
                }
            }

            var descriptionStatus = statuses.FirstOrDefault(status => status.name == "desc-filter");
            if (descriptionStatus != null)
            {
                if (!String.IsNullOrEmpty(descriptionStatus.data.path) && !String.IsNullOrEmpty(descriptionStatus.data.value))
                {
                    this.Skills = this.Skills.Where(obj => obj.Description.Contains(descriptionStatus.data.value));
                }
            }

            var sortStatus = statuses.FirstOrDefault(status => status.action == "sort");
            if (sortStatus != null)
            {
                this.Sort(sortStatus);
            }
        }

        /// <summary>
        /// sort list by status
        /// </summary>
        /// <param name="status">the status object</param>
        private void Sort(StatusDTO status)
        {
            if (status != null && status.data != null && !String.IsNullOrEmpty(status.data.path))
            {
                switch (status.data.path)
                {
                    case ".caption":
                        {
                            if (!String.IsNullOrEmpty(status.data.order) && status.data.order.ToLower() == "desc")
                            {
                                this.Skills = this.Skills.OrderByDescending(obj => obj.Caption);
                                break;
                            }
                            this.Skills = this.Skills.OrderBy(obj => obj.Caption);
                            break;
                        }

                    case ".desc":
                        {
                            if (!String.IsNullOrEmpty(status.data.order) && status.data.order.ToLower() == "desc")
                            {
                                this.Skills = this.Skills.OrderByDescending(obj => obj.Description);
                                break;
                            }
                            this.Skills = this.Skills.OrderBy(obj => obj.Description);
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Apply pagination on list
        /// </summary>
        /// <param name="status">the status object</param>
        private void Paging(StatusDTO status)
        {
            int numberInt;

            if (status != null && status.data != null && Int32.TryParse(status.data.number, out numberInt) && this.Skills.Count() > numberInt)
            {
                this.Skills = this.Skills.Skip(status.data.currentPage * numberInt).Take(numberInt);
            }
        }

        #endregion
    }
}