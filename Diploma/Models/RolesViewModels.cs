using Diploma.DiplomaDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diploma.Models
{
    public class RoleListItemViewModel : TranslatableExtendedViewModel
    {
        public RoleListItemViewModel(Role role)
            : base(role)
        {
        }
    }
    public class RoleDetailViewModel : TranslatableExtendedViewModel
    {
        public RoleDetailViewModel(Role role)
            : base(role)
        {
        }
    }
}