using Amazon.DynamoDBv2.DataModel;

namespace Authetication.Models
{
    [DynamoDBTable("Tokens")]
    public class Token
    {
        [DynamoDBHashKey]
        public string UserId { get; set; }

        [DynamoDBRangeKey]
        public long TimeStamp { get; set; }

        public string token{ get; set; }

        public State State { get; set; }
    }
}