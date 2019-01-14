using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Authetication.Models
{
    [DynamoDBTable("Users")]
    public class User
    {
        [DynamoDBHashKey]
        public string UserName { get; set; }

        public string UserId { get; set; }

        public string Password { get; set; }   
    }
}