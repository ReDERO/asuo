using Diploma.DiplomaDb;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diploma.Models
{
    public class UserEditViewModel
    {
        public UserEditViewModel(User user, List<Role> roleList)
        {
            this.Id = user.Id;
            this.Email = user.Email;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.CreateDate = user.CreateDate;

            var roles = new List<RoleListItemViewModel>();
            roleList.AsParallel().ForAll(r => roles.Add(new RoleListItemViewModel(r)));

            this.Roles = new List<SelectListItem>();
            foreach (var role in roles)
            {
                this.Roles.Add(new SelectListItem()
                {
                    Value = role.Id.ToString(),
                    Text = role.Caption,
                    Selected = user.Roles.Any(r => r.Id == role.Id)
                });
            }
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }

        public List<SelectListItem> Roles { get; set; }
    }
}