using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Model.Seed
{
    public class InitRoleAdminSeed
    {
        public static void Init(QLNSContext context)
        {
            var AccountAdminName = "admin";
            var admin = context.Users.Where(x => x.UserName == AccountAdminName).FirstOrDefault();
            if (admin != null)
            {
                var listOperation = context.Operation.ToList();
            }

        }
    }
}
