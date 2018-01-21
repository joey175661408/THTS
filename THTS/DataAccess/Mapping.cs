using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace THTS.DataAccess
{
    public class Mapping
    {
        public class UserMap : EntityTypeConfiguration<User>
        {
            public UserMap()
            {
            }
        }
        
    }
}
