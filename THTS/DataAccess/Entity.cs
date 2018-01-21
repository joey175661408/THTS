using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace THTS.DataAccess
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int IsDelete { get; set; }
    }
}
