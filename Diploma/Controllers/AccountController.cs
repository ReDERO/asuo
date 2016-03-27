using Diploma.DiplomaDb;
using Diploma.Models;
using Diploma.Resources;
using Diploma.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Diploma.Controllers
{
    public class AccountController : BaseController
    {
        private void CreateSession(User user, bool rememberMe = false)
        {
            var roles = user.Roles.Select(r => r.Id).ToArray();

            UserPrincipalSerializeModel serializeModel = new UserPrincipalSerializeModel();
            serializeModel.UserId = user.Id;
            serializeModel.FirstName = user.FirstName;
            serializeModel.LastName = user.LastName;
            serializeModel.Roles = roles;

            string userData = JsonConvert.SerializeObject(serializeModel);
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                     1,
                     user.Email,
                     DateTime.Now,
                     DateTime.Now.AddMinutes((rememberMe) ? 10080 : 1440),
                     rememberMe,
                     userData);

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(faCookie);
        }

        //
        // GET: /Account/Login
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(u => u.Email == model.Email && u.Password == model.Password).FirstOrDefault();
                if (user != null)
                {
                    CreateSession(user, model.RememberMe);
                    return Redirect(returnUrl);
                }

                ModelState.AddModelError("", Resource.LoginError);
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: Account/LogOut
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut(string returnUrl)
        {
            FormsAuthentication.SignOut();
            return Redirect(returnUrl);
        }

        //
        // GET: Account/Register
        public ActionResult Register(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            var roles = new List<RoleListItemViewModel>();
            db.Roles.AsParallel().ForAll(r => roles.Add(new RoleListItemViewModel(r)));
            ViewBag.Roles = new SelectList(roles, "Id", "Caption");
            return View();
        }

        //
        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    Email = model.Email,
                    Password = model.Password,
                    FirstName = model.FirstName,
                    LastName = model.FirstName,
                    CreateDate = DateTime.UtcNow
                };
                user.Roles.Add(db.Roles.Find(model.Role));
                db.Users.Add(user);
                try
                {
                    await db.SaveChangesAsync();
                    CreateSession(user);
                    return RedirectToAction("EditSkills", "Skills", new { entity = "user", objectId = user.Id });
                }
                catch (DbUpdateException e)
                {
                    ModelState.AddModelError("Email", Resource.EmailAlreadyExists);
                }
            }
            ViewBag.ReturnUrl = returnUrl;
            // Появление этого сообщения означает наличие ошибки; повторное отображение формы
            return View(model);
        }

        //
        // GET: /Account/UserProfile
        public async Task<ActionResult> UserProfile(int id)
        {
            ViewBag.IsMyProfile = (User != null && User.UserId == id);
            var user = await db.Users.FindAsync(id);
            return View(new UserProfileViewModel(user));
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var userView = new EditAccountViewModel(user);

            return View(userView);
        }

        // POST: Users/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Email,FirstName,LastName")] EditAccountViewModel userView)
        {
            if (ModelState.IsValid)
            {
                var user = await db.Users.FindAsync(userView.Id);
                user.Email = userView.Email;
                user.FirstName = userView.FirstName;
                user.LastName = userView.LastName;
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("UserProfile", new { id = user.Id });
            }
            return View(userView);
        }
    }
}