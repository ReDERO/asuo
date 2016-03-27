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
using Diploma.Models;
using Diploma.Resources;

namespace Diploma.Controllers
{
    public class SkillPackagesController : BaseController
    {
        // GET: SkillPackages
        public async Task<ActionResult> Index()
        {
            ViewBag.AllowActions = await IsAllowed("SkillPackages", "Create");
            var skillPackages = await db.SkillPackages.ToListAsync();
            return View((new SkillPackageListViewModel(skillPackages, CurrentUser)));
        }

        // GET: SkillPackages/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SkillPackage skillPackage = await db.SkillPackages.FindAsync(id);
            if (skillPackage == null)
            {
                return HttpNotFound();
            }
            ViewBag.BreadCrumbs = new BreadCrumbsModel(Request.RequestContext, skillPackage);
            ViewBag.AllowActions = await IsAllowed("Tasks", "Create");
            ViewBag.Subscribed = CurrentUser.SubscribedSkillPackages.Contains(skillPackage);
            ViewBag.AchievedSkills = new SkillListViewModel(db.Users.Find(User.UserId).AchivedSkills);
            return View(new SkillPackageDetailsViewModel(skillPackage));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Subscribe(int id)
        {
            var skillPackage = await db.SkillPackages.FindAsync(id);
            var user = await db.Users.FindAsync(User.UserId);
            foreach(var skill in skillPackage.Skills)
            {
                if (!user.AchivedSkills.Contains(skill))
                {
                    var wishedSkill = await db.UserWishedSkills.FindAsync(skill.Id, user.Id);
                    if (wishedSkill != null)
                    {
                        wishedSkill.Requirement++;
                    }
                    else
                    {
                        db.UserWishedSkills.Add(new UserWishedSkill() { User = user, Skill = skill, Requirement = 1 });
                    }
                }
            }
            user.SubscribedSkillPackages.Add(skillPackage);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Unsubscribe(int id)
        {
            var skillPackage = await db.SkillPackages.FindAsync(id);
            var user = await db.Users.FindAsync(User.UserId);
            user.SubscribedSkillPackages.Remove(skillPackage);
            foreach(var skill in skillPackage.Skills)
            {
                var wishedSkill = await db.UserWishedSkills.FindAsync(skill.Id, user.Id);
                if (wishedSkill != null)
                {
                    wishedSkill.Requirement--;
                    if (wishedSkill.Requirement == 0)
                    {
                        db.UserWishedSkills.Remove(wishedSkill);
                    }
                }
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = id });
        }

        // GET: SkillPackages/Create
        public ActionResult Create()
        {
            return View(new SkillPackage() { AuthorId = User.UserId });
        }

        // POST: SkillPackages/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,AuthorId,DescriptionEN,DescriptionRU,DescriptionUA,Weight,CaptionEN,CaptionRU,CaptionUA")] SkillPackage skillPackage)
        {
            if (ModelState.IsValid)
            {
                if (skillPackage.IsValid())
                {
                    db.SkillPackages.Add(skillPackage);
                    await db.SaveChangesAsync();
                    return RedirectToAction("EditSkills", "Skills", new { entity = "skillPackage", objectId = skillPackage.Id });
                }
                ModelState.AddModelError("", Resource.CaptionsAreEmpty);
            }
            return View(skillPackage);
        }

        // GET: SkillPackages/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SkillPackage skillPackage = await db.SkillPackages.FindAsync(id);
            if (skillPackage == null)
            {
                return HttpNotFound();
            }
            ViewBag.BreadCrumbs = new BreadCrumbsModel(Request.RequestContext, skillPackage, "edit");
            return View(new SkillPackageEditViewModel(skillPackage));
        }

        // POST: SkillPackages/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,AuthorId,DescriptionEN,DescriptionRU,DescriptionUA,Weight,CaptionEN,CaptionRU,CaptionUA")] SkillPackage skillPackage)
        {
            if (ModelState.IsValid)
            {
                if (skillPackage.IsValid())
                {
                    db.Entry(skillPackage).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", Resource.CaptionsAreEmpty);
            }
            ViewBag.BreadCrumbs = new BreadCrumbsModel(Request.RequestContext, skillPackage, "edit");
            return View(new SkillPackageEditViewModel(skillPackage));
        }

        // GET: SkillPackages/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SkillPackage skillPackage = await db.SkillPackages.FindAsync(id);
            if (skillPackage == null)
            {
                return HttpNotFound();
            }
            return View(skillPackage);
        }

        // POST: SkillPackages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SkillPackage skillPackage = await db.SkillPackages.FindAsync(id);
            db.SkillPackages.Remove(skillPackage);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
