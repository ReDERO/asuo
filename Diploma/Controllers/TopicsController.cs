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
    public class TopicsController : BaseController
    {
        // GET: Topics
        public async Task<ActionResult> Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = await db.Subjects.FindAsync(id);
            if (subject == null)
            {
                return HttpNotFound();
            }

            var topics = subject.Topics.Where(t => t.ParentId == null).ToList();
            ViewBag.BreadCrumbs = new BreadCrumbsModel(Request.RequestContext, subject);
            ViewBag.Subject = new SubjectListItemViewModel(subject);
            ViewBag.AllowActions = await IsAllowed("Topics", "Create");

            return View(new TopicList(topics, CurrentUser));
        }

        // GET: Topics/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = await db.Topics.FindAsync(id);
            if (topic == null)
            {
                return HttpNotFound();
            }

            ViewBag.BreadCrumbs = new BreadCrumbsModel(Request.RequestContext, topic);
            ViewBag.Subject = new SubjectListItemViewModel(topic.Subject);
            ViewBag.Topic = new TopicListItemViewModel(topic);
            ViewBag.AllowActions = await IsAllowed("Topics", "Create");

            return View(new TopicList(topic.Children.ToList(), CurrentUser));
        }

        // GET: Topics/Create
        public ActionResult Create(int subjectId, int? topicId)
        {
            return View(new Topic() { AuthorId = User.UserId, SubjectId = subjectId, ParentId = topicId });
        }

        // POST: Topics/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,SubjectId,AuthorId,ParentId,Weight,CaptionEN,CaptionRU,CaptionUA")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                if (topic.IsValid())
                {
                    db.Topics.Add(topic);
                    await db.SaveChangesAsync();

                    if (topic.ParentId == null)
                    {
                        return RedirectToAction("Index", new { id = topic.SubjectId });
                    }
                    else
                    {
                        return RedirectToAction("Details", new { id = topic.ParentId });
                    }
                }
                ModelState.AddModelError("", Resource.CaptionsAreEmpty);
            }
            return View(topic);
        }

        // GET: Topics/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = await db.Topics.FindAsync(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Topics/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,SubjectId,AuthorId,ParentId,Weight,CaptionEN,CaptionRU,CaptionUA")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topic).State = EntityState.Modified;
                await db.SaveChangesAsync();
                if (topic.ParentId == null)
                {
                    return RedirectToAction("Index", new { id = topic.SubjectId });
                }
                else
                {
                    return RedirectToAction("Details", new { id = topic.ParentId });
                }
            }
            return View(topic);
        }

        // GET: Topics/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = await db.Topics.FindAsync(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Topic topic = await db.Topics.FindAsync(id);
            db.Topics.Remove(topic);
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
