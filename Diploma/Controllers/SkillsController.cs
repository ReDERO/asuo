using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Diploma.DiplomaDb;
using Diploma.Resources;
using Newtonsoft.Json;
using Diploma.Models;

namespace Diploma.Controllers
{
    public class SkillsController : BaseController
    {
        // GET: Skills
        public async Task<ActionResult> Index()
        {
            return View(await db.Skills.ToListAsync());
        }

        // GET: Skills/Details/5
        public async Task<ActionResult> Details(int? id, string returnURL)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = await db.Skills.FindAsync(id);
            if (skill == null)
            {
                return HttpNotFound();
            }
            ViewBag.AllowActions = await IsAllowed("Skills", "Create");
            ViewBag.BreadCrumbs = new BreadCrumbsModel(Request.RequestContext, skill, returnURL);
            ViewBag.ReturnURL = returnURL;
            return View(new SkillListItemViewModel(skill));
        }

        // GET: Skills/Create
        public ActionResult Create(string returnURL)
        {
            ViewBag.ReturnURL = returnURL;
            return View();
        }

        // POST: Skills/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,DescriptionEN,DescriptionRU,DescriptionUA,CaptionEN,CaptionRU,CaptionUA")] Skill skill, string returnURL)
        {
            if (ModelState.IsValid)
            {
                if (skill.IsValid())
                {
                    db.Skills.Add(skill);
                    await db.SaveChangesAsync();
                    return Redirect(returnURL);
                }
                ModelState.AddModelError("", Resource.CaptionsAreEmpty);
            }
            ViewBag.AllowActions = await IsAllowed("Skills", "Create");
            ViewBag.BreadCrumbs = new BreadCrumbsModel(Request.RequestContext, skill, returnURL);
            ViewBag.ReturnURL = returnURL;
            return View(skill);
        }

        // GET: Skills/Edit/5
        public async Task<ActionResult> Edit(int? id, string returnURL)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = await db.Skills.FindAsync(id);
            if (skill == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReturnURL = returnURL;
            ViewBag.BreadCrumbs = new BreadCrumbsModel(Request.RequestContext, skill, returnURL);
            return View(skill);
        }

        // POST: Skills/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,DescriptionEN,DescriptionRU,DescriptionUA,Weight,CaptionEN,CaptionRU,CaptionUA")] Skill skill, string returnURL)
        {
            if (ModelState.IsValid)
            {
                if (skill.IsValid())
                {
                    db.Entry(skill).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Details", new { id = skill.Id, returnURL = returnURL });
                }
                ModelState.AddModelError("", Resource.CaptionsAreEmpty);
            }
            ViewBag.ReturnURL = returnURL;
            ViewBag.BreadCrumbs = new BreadCrumbsModel(Request.RequestContext, skill, returnURL);
            return View(skill);
        }

        // GET: Skills/Delete/5
        public async Task<ActionResult> Delete(int? id, string returnURL)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = await db.Skills.FindAsync(id);
            if (skill == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReturnURL = returnURL;
            return View(skill);
        }

        // POST: Skills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id, string returnURL)
        {
            Skill skill = await db.Skills.FindAsync(id);
            db.Skills.Remove(skill);
            await db.SaveChangesAsync();
            return Redirect(returnURL);
        }

        //
        // GET: /Account/EditSkills
        public async Task<ActionResult> EditSkills(string entity, int objectId)
        {
            ViewBag.Entity = entity;
            ViewBag.ObjectId = objectId;
            ViewBag.Subjects = new SubjectList(db.Subjects, CurrentUser);
            ViewBag.Topics = new TopicList(db.Topics, CurrentUser);

            switch (entity)
            {
                case "user":
                    {
                        ViewBag.Description = Resource.EditUserSkillsDescription;
                        var user = await db.Users.FindAsync(objectId);
                        ViewBag.BreadCrumbs = new BreadCrumbsModel(Request.RequestContext, user, "skills");
                        return View(new EditSkillsSkillsViewModel(user, db.Skills.ToList()));
                    }

                case "task":
                    {
                        ViewBag.Description = Resource.EditTaskSkillsDescription;
                        var task = await db.Tasks.FindAsync(objectId);
                        ViewBag.BreadCrumbs = new BreadCrumbsModel(Request.RequestContext, task, "skills");
                        return View(new EditSkillsSkillsViewModel(task, db.Skills.ToList()));
                    }

                case "skillPackage":
                    {
                        ViewBag.Description = Resource.EditSkillPackageSkillsDescription;
                        var skillPackage = await db.SkillPackages.FindAsync(objectId);
                        ViewBag.BreadCrumbs = new BreadCrumbsModel(Request.RequestContext, skillPackage, "skills");
                        return View(new EditSkillsSkillsViewModel(skillPackage, db.Skills.ToList()));
                    }
            }

            throw new Exception("Сущность не найдена!");
        }

        [Authorize]
        [HttpPost]
        public ActionResult ObjectSkills(string entity, int objectId, string returnURL)
        {
            ViewBag.ReturnURL = returnURL;
            ViewBag.Entity = entity;
            ViewBag.ObjectId = objectId;
            List<Skill> checkedSkills;

            switch (entity)
            {
                case "user":
                    {
                        var user = db.Users.Find(objectId);
                        checkedSkills = user.AchivedSkills.ToList();
                    } break;
                
                case "task":
                    {
                        var task = db.Tasks.Find(objectId);
                        checkedSkills = task.Skills.ToList();
                    } break;

                case "skillPackage":
                    {
                        var skillPackage = db.SkillPackages.Find(objectId);
                        checkedSkills = skillPackage.Skills.ToList();
                    } break;

                default: throw new Exception("Сущность не найдена!");
            }
            var statuses = Request.Form.Get("statuses");

            //decode the url
            statuses = Server.UrlDecode(statuses);
            var statusesList = JsonConvert.DeserializeObject<List<StatusDTO>>(statuses);

            //init status queries provider
            var statusQueries = new StatusQueries(statusesList, db, checkedSkills);

            ViewBag.Count = statusQueries.CountBeforePagination;
            return PartialView(statusQueries.Skills);
        }

        public ActionResult AddSkill(string entity, int objectId, int skillId)
        {
            switch (entity)
            {
                case "user":
                    {
                        var user = db.Users.Find(objectId);
                        var skill = db.Skills.Find(skillId);
                        user.AchivedSkills.Add(skill);
                        db.SaveChanges();
                    }
                    break;

                case "task":
                    {
                        var task = db.Tasks.Find(objectId);
                        var skill = db.Skills.Find(skillId);
                        task.Skills.Add(skill);
                        db.SaveChanges();
                    }
                    break;

                case "skillPackage":
                    {
                        var skillPackage = db.SkillPackages.Find(objectId);
                        var skill = db.Skills.Find(skillId);
                        skillPackage.Skills.Add(skill);
                        db.SaveChanges();
                    }
                    break;
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveSkill(string entity, int objectId, int skillId)
        {
            switch (entity)
            {
                case "user":
                    {
                        var user = db.Users.Find(objectId);
                        var skill = db.Skills.Find(skillId);
                        user.AchivedSkills.Remove(skill);
                        db.SaveChanges();
                    }
                    break;

                case "task":
                    {
                        var task = db.Tasks.Find(objectId);
                        var skill = db.Skills.Find(skillId);
                        task.Skills.Remove(skill);
                        db.SaveChanges();
                    }
                    break;

                case "skillPackage":
                    {
                        var skillPackage = db.SkillPackages.Find(objectId);
                        var skill = db.Skills.Find(skillId);
                        skillPackage.Skills.Remove(skill);
                        db.SaveChanges();
                    }
                    break;
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
