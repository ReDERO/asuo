using Diploma.DiplomaDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Diploma.Models
{
    public class BreadCrumbItem
    {
        public string Caption { get; set; }
        public string URL { get; set; }
    }

    public class BreadCrumbsModel : List<BreadCrumbItem>
    {
        private UrlHelper url;

        public BreadCrumbsModel(RequestContext context, string startPoint)
        {
            url = new UrlHelper(context);
            AddStartPoint(startPoint);
        }

        public BreadCrumbsModel(RequestContext context, Subject subject)
        {
            url = new UrlHelper(context);
            AddSubject(subject);
        }

        public BreadCrumbsModel(RequestContext context, Topic topic)
        {
            url = new UrlHelper(context);
            AddTopics(topic);
        }

        public BreadCrumbsModel(RequestContext context, Skill skill, string returnURL)
        {
            url = new UrlHelper(context);
            switch (context.RouteData.Values["action"].ToString())
            {
                case "Details":
                    {
                        base.Add(new BreadCrumbItem()
                        {
                            Caption = (new SkillListItemViewModel(skill)).Caption,
                            URL = url.Action("Details", "Skills", new { id = skill.Id, returnURL = returnURL })
                        });
                    }
                    break;

                case "Edit":
                    {
                        base.Add(new BreadCrumbItem()
                        {
                            Caption = Resources.Resource.Edit,
                            URL = ""
                        });
                        base.Add(new BreadCrumbItem()
                        {
                            Caption = (new SkillListItemViewModel(skill)).Caption,
                            URL = url.Action("Details", "Skills", new { id = skill.Id, returnURL = returnURL })
                        });
                    }
                    break;
            }

            base.Add(new BreadCrumbItem()
            {
                Caption = Resources.Resource.Skills,
                URL = returnURL
            });
        }

        public BreadCrumbsModel(RequestContext context, Task task, string part = "default")
        {
            url = new UrlHelper(context);
            switch (part)
            {
                case "skills":
                    {
                        base.Add(new BreadCrumbItem()
                        {
                            Caption = Resources.Resource.Skills,
                            URL = ""
                        });
                        base.Add(new BreadCrumbItem()
                        {
                            Caption = Resources.Resource.Edit,
                            URL = url.Action("Edit", "Tasks", new { id = task.Id })
                        });
                        base.Add(new BreadCrumbItem()
                        {
                            Caption = (new TaskListItemViewModel(task)).Caption,
                            URL = url.Action("Details", "Tasks", new { id = task.Id })
                        });
                    }
                    break;

                case "edit":
                    {
                        base.Add(new BreadCrumbItem()
                        {
                            Caption = Resources.Resource.Edit,
                            URL = url.Action("Edit", "Tasks", new { id = task.Id })
                        });
                        base.Add(new BreadCrumbItem()
                        {
                            Caption = (new TaskListItemViewModel(task)).Caption,
                            URL = url.Action("Details", "Tasks", new { id = task.Id })
                        });
                    }
                    break;

                default: AddTask(task); break;
            }
        }

        public BreadCrumbsModel(RequestContext context, SkillPackage skillPackage, string part = "default")
        {
            url = new UrlHelper(context);
            switch(part)
            {
                case "skills":
                    {
                        base.Add(new BreadCrumbItem()
                        {
                            Caption = Resources.Resource.Skills,
                            URL = ""
                        });
                        base.Add(new BreadCrumbItem()
                        {
                            Caption = Resources.Resource.Edit,
                            URL = url.Action("Edit", "SkillPackages", new { id = skillPackage.Id })
                        });
                        base.Add(new BreadCrumbItem()
                        {
                            Caption = (new SkillPackageListItemViewModel(skillPackage)).Caption,
                            URL = url.Action("Details", "SkillPackages", new { id = skillPackage.Id })
                        });
                    }
                    break;

                case "edit":
                    {
                        base.Add(new BreadCrumbItem()
                        {
                            Caption = Resources.Resource.Edit,
                            URL = url.Action("Edit", "SkillPackages", new { id = skillPackage.Id })
                        });
                        base.Add(new BreadCrumbItem()
                        {
                            Caption = (new SkillPackageListItemViewModel(skillPackage)).Caption,
                            URL = url.Action("Details", "SkillPackages", new { id = skillPackage.Id })
                        });
                    }
                    break;

                default: AddSkillPackage(skillPackage); break;
            }
        }

        public BreadCrumbsModel(RequestContext context, User user, string part = "default")
        {
            url = new UrlHelper(context);

            switch (part)
            {
                case "skills":
                    {
                        base.Add(new BreadCrumbItem()
                        {
                            Caption = Resources.Resource.Skills,
                            URL = ""
                        });
                        base.Add(new BreadCrumbItem()
                        {
                            Caption = Resources.Resource.MyProfile,
                            URL = url.Action("UserProfile", "Account", new { id = user.Id })
                        });
                    }
                    break;

                case "edit":
                    {
                        base.Add(new BreadCrumbItem()
                        {
                            Caption = Resources.Resource.Edit,
                            URL = url.Action("Edit", "Account", new { id = user.Id })
                        });
                        base.Add(new BreadCrumbItem()
                        {
                            Caption = Resources.Resource.MyProfile,
                            URL = url.Action("UserProfile", "Account", new { id = user.Id })
                        });
                    }
                    break;
            }
        }

        private void AddSkillPackage(SkillPackage skillPackage)
        {
            var sp = new SkillPackageListItemViewModel(skillPackage);
            base.Add(new BreadCrumbItem() { Caption = sp.Caption, URL = url.Action("Detail", "SkillPackages", new { id = sp.Id }) });
            this.AddStartPoint("skillPackages");
        }

        private void AddTask(Task task)
        {
            var t = new TaskListItemViewModel(task);
            var topicShort = new TopicListItemViewModel(task.Topic);
            base.Add(new BreadCrumbItem() { Caption = t.Caption, URL = url.Action("Detail", "Tasks", new { id = t.Id }) });
            base.Add(new BreadCrumbItem() { Caption = topicShort.Caption, URL = url.Action("Index", "Tasks", new { id = topicShort.Id }) });
            this.AddTopics(task.Topic.Parent);
        }

        private void AddTopics(Topic topic)
        {
            var top = topic;
            do
            {
                var topicShort = new TopicListItemViewModel(top);
                base.Add(new BreadCrumbItem() { Caption = topicShort.Caption, URL = url.Action("Details", "Topics", new { id = topicShort.Id }) });
                top = top.Parent;
            }
            while (top != null);
            AddSubject(topic.Subject);
        }

        private void AddSubject(Subject subject)
        {
            var subj = new SubjectListItemViewModel(subject);
            base.Add(new BreadCrumbItem() { Caption = subj.Caption, URL = url.Action("Index", "Topics", new { id = subj.Id }) });
            this.AddStartPoint("subjects");
        }

        private void AddStartPoint(string start)
        {
            switch (start)
            {
                case "subjects":
                    {
                        base.Add(new BreadCrumbItem()
                        {
                            Caption = Resources.Resource.Subjects,
                            URL = url.Action("Index", "Subjects")
                        });
                    }
                    break;

                case "skillPackages":
                    {
                        base.Add(new BreadCrumbItem()
                        {
                            Caption = Resources.Resource.SkillPackages,
                            URL = url.Action("Index", "SkillPackages")
                        });
                    }
                    break;

                case "recommended":
                    {
                        base.Add(new BreadCrumbItem()
                        {
                            Caption = Resources.Resource.Recommended,
                            URL = url.Action("Recommended", "Tasks")
                        });
                    }
                    break;
            }
        }
    }
}