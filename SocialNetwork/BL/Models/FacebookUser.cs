using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    [DynamoDBTable("FacebookUsers")]
    public class FacebookUser
    {
        [DynamoDBHashKey]
        public string FacebookId { get; set; }

        public string UserId { get; set; }

    }
}

