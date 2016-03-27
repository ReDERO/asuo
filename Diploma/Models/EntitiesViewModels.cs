using Diploma.DiplomaDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diploma.Models
{
    /*public class ActionViewModel : TranslatableViewModel
    {
        public ActionViewModel(Diploma.DiplomaDb.Action action, List<string> languages, bool isChild = false)
            : base(action, languages)
        {
            if (!isChild)
            {
                this.Entity = new EntityViewModel(action.Entity, languages, true);
                this.Roles = new HashSet<RoleViewModel>();

                action.Roles.AsParallel().ForAll(r => this.Roles.Add(new RoleViewModel(r, languages, true)));
            }
        }
        public virtual EntityViewModel Entity { get; set; }
        public virtual ICollection<RoleViewModel> Roles { get; set; }
    }

    public class EntityViewModel : TranslatableViewModel
    {
        public EntityViewModel(Entity entity, List<string> languages, bool isChild = false)
            : base(entity, languages)
        {
            if (!isChild)
            {
                this.Actions = new HashSet<ActionViewModel>();
                entity.Actions.AsParallel().ForAll(r => this.Actions.Add(new ActionViewModel(r, languages, true)));
            }
        }
        public virtual ICollection<ActionViewModel> Actions { get; set; }
    }

    public class RoleViewModel : TranslatableExtendedViewModel
    {
        public RoleViewModel(Role role, List<string> languages, bool isChild = false)
            : base(role, languages)
        {
            if (!isChild)
            {
                this.Actions = new HashSet<ActionViewModel>();
                role.Actions.AsParallel().ForAll(r => this.Actions.Add(new ActionViewModel(r, languages, true)));

                this.Users = new HashSet<UserViewModel>();
                role.Users.AsParallel().ForAll(r => this.Users.Add(new UserViewModel(r, languages, true)));
            }
        }
        public virtual ICollection<ActionViewModel> Actions { get { } }
        public virtual ICollection<UserViewModel> Users { get; set; }
    }

    public class UserViewModel
    {
        public UserViewModel(User user, List<string> languages, bool isChild = false)
        {
            if (!isChild)
            {
                this.Roles = new HashSet<RoleViewModel>();
                user.Roles.AsParallel().ForAll(r => this.Roles.Add(new RoleViewModel(r, languages, true)));
            }
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual ICollection<RoleViewModel> Roles { get; set; }
    }*/
}