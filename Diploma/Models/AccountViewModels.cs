using System.ComponentModel.DataAnnotations;
using Diploma.Resources;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Web;
using Diploma.DiplomaDb;

namespace Diploma.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(Resource))]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof(Resource))]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FirstNameRequired")]
        [Display(Name = "FirstName", ResourceType = typeof(Resource))]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(Resource))]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "EmailRequired")]
        [Display(Name = "Email", ResourceType = typeof(Resource))]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PasswordRequired")]
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PasswordRequirements")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resource))]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "ConfirmPasswordCompare")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RoleRequired")]
        [Display(Name = "Role", ResourceType = typeof(Resource))]
        public int Role { get; set; }
    }

    public class UserProfileViewModel
    {
        public UserProfileViewModel(User user)
        {
            this.Id = user.Id;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;

            this.Skills = new List<SkillListItemViewModel>();
            foreach(var skill in user.AchivedSkills)
            {
                this.Skills.Add(new SkillListItemViewModel(skill));
            }
        }

        public int Id { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(Resource))]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(Resource))]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        public List<SkillListItemViewModel> Skills { get; set; }
    }

    public class EditAccountViewModel
    {
        public EditAccountViewModel() { }
        public EditAccountViewModel(User user)
        {
            this.Id = user.Id;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
        }

        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FirstNameRequired")]
        [Display(Name = "FirstName", ResourceType = typeof(Resource))]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(Resource))]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "EmailRequired")]
        [Display(Name = "Email", ResourceType = typeof(Resource))]
        [EmailAddress]
        public string Email { get; set; }
    }
}