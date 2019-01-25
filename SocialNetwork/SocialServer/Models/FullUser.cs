
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialServer.Models
{
    public class FullUser
    {
        public string UserId { get; set; }

        public DateTime BirthDate { get; set; }

        public string City { get; set; }

        public string FullName { get; set; }

        public string WorkPlace { get; set; }
    }
}
