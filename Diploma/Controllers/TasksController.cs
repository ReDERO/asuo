using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Diploma.DiplomaDb;
using Diploma.Resources;
using Diploma.Models;

namespace Diploma.Controllers
{
    public class TasksController : BaseController
    {
        public ActionResult Recommended()
        {
            ViewBag.BreadCrumbs = new BreadCrumbsModel(Request.RequestContext, "recommended");
            return View((new RecommendedListViewModel(db.Tasks, CurrentUser)));
        }

        // GET: Tasks
        public async System.Threading.Tasks.Task<ActionResult> Index(int id)
        {
            var topic = await db.Topics.FindAsync(id);
            ViewBag.TopicId = id;
            ViewBag.BreadCrumbs = new BreadCrumbsModel(Request.RequestContext, topic);
            ViewBag.AllowActions = await IsAllowed("Tasks", "Create");
            return View((new TaskListViewModel(topic, CurrentUser)));
        }

        // GET: Tasks/Details/5
        public async System.Threading.Tasks.Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = await db.Tasks.FindAsync(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.BreadCrumbs = new BreadCrumbsModel(Request.RequestContext, task);
            ViewBag.AllowActions = await IsAllowed("Tasks", "Create");
            ViewBag.IsCompleted = (User != null) ? CurrentUser.CompletedTasks.Contains(task) : false;
            
            return View(new TaskDetailsViewModel(task));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Done(int id)
        {
            var task = await db.Tasks.FindAsync(id);
            var user = await db.Users.FindAsync(User.UserId);
            foreach (var skill in task.Skills)
            {
                var wishedSkill = await db.UserWishedSkills.FindAsync(skill.Id, user.Id);
                if (wishedSkill != null) db.UserWishedSkills.Remove(wishedSkill);
                user.AchivedSkills.Add(skill);
            }
            user.CompletedTasks.Add(task);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = task.Id });

        }

        // GET: Tasks/Create
        public ActionResult Create(int id)
        {
            return View(new Task() { TopicId = id, AuthorId = User.UserId });
        }

        // POST: Tasks/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Create([Bind(Include = "Id,SourceURL,Instruction,Public,TopicId,AuthorId,DescriptionEN,DescriptionRU,DescriptionUA,Weight,CaptionEN,CaptionRU,CaptionUA")] Task task, string Access)
        {
            switch (Access)
            {
                case "on": task.Public = true; break;
                case "off": task.Public = false; break;
            }
            if (ModelState.IsValid)
            {
                if (task.IsValid())
                {
                    db.Tasks.Add(task);
                    await db.SaveChangesAsync();
                    return RedirectToAction("EditSkills", "Skills", new { entity = "task", objectId = task.Id });
                }
                ModelState.AddModelError("", Resource.CaptionsAreEmpty);
            }
            return View(task);
        }

        // GET: Tasks/Edit/5
        public async System.Threading.Tasks.Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = await db.Tasks.FindAsync(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsMySkills = (User != null && User.UserId == task.AuthorId);
            //ViewBag.TopicId = new SelectList(db.Topics, "Id", "CaptionEN", task.TopicId);
            ViewBag.BreadCrumbs = new BreadCrumbsModel(Request.RequestContext, task, "edit");
            return View(new TaskEditViewModel(task));
        }

        // POST: Tasks/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Edit([Bind(Include = "Id,SourceURL,Instruction,Public,TopicId,AuthorId,DescriptionEN,DescriptionRU,DescriptionUA,Weight,CaptionEN,CaptionRU,CaptionUA")] Task task, string Access)
        {
            switch (Access)
            {
                case "on": task.Public = true; break;
                case "off": task.Public = false; break;
            }
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { id = task.TopicId });
            }
            ViewBag.IsMySkills = (User != null && User.UserId == task.AuthorId);
            ViewBag.TopicId = new SelectList(db.Topics, "Id", "CaptionEN", task.TopicId);
            ViewBag.BreadCrumbs = new BreadCrumbsModel(Request.RequestContext, task, "edit");
            return View(new TaskEditViewModel(task));
        }

        // GET: Tasks/Delete/5
        public async System.Threading.Tasks.Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = await db.Tasks.FindAsync(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> DeleteConfirmed(int id)
        {
            Task task = await db.Tasks.FindAsync(id);
            db.Tasks.Remove(task);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new { id = task.TopicId });
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
