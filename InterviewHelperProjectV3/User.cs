using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewHelperProjectV3
{
    

    public class User
    {

        public int UserId { get; set; }


        public String UserName { get; set; }

        public String UserFullName { get; set; }

        public User(int userId, String userName, String userFull) 
        {
            this.UserId = userId;
            this.UserName = userName;
            this.UserFullName = userFull;
        }

    }
}
