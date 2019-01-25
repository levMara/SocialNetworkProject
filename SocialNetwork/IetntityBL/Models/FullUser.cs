using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityBL.Models
{
    [DynamoDBTable("FullUsers")]
    public class FullUser
    {
        [DynamoDBHashKey]
        public string UserId { get; set; }

        public DateTime BirthDate { get; set; }

        public string City { get; set; }

        public string FullName { get; set; }

        public string WorkPlace { get; set; }
    }
}
