using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialBL.Dal
{
    class Neo4jClient
    {
        //singlton????
        GraphClient _client;
        string _uri = "http://ec2-18-221-16-181.us-east-2.compute.amazonaws.com:7474/db/data";
        string _user = "neo4j";
        string _pass = "lm770";

        public Neo4jClient()
        {
            _client = new GraphClient(new Uri(_uri), _user, _pass);
            _client.Connect();
        }


        public void CreateRelation()
        {
            //_client.Cypher
            //    .Match("(u1:User)", "(u2:User)")
            //    .Where((User u1) => u1.Id == userId)
            //    .AndWhere((User u2) => u2.Id == otherUserId)
            //    .Create("(u1)-[:Follow]->(u2)")
            //    .ExecuteWithoutResults();


            //_client.Cypher
            //    .Match("t)
        }
    }
}
