using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Authetication.Models;
using BL.Exceptiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Authetication.Db
{
    public class DynamoService
    {
        AmazonDynamoDBClient _client;
        DynamoDBContextConfig _conf;

        public DynamoService()
        {
            _client = new AmazonDynamoDBClient();

            _conf = new DynamoDBContextConfig
            {
                ConsistentRead = true,
                Conversion = DynamoDBEntryConversion.V2
            };


        }

        public void Add<T>(T item) where T : new()
        {
            using (DynamoDBContext context = new DynamoDBContext(_client, _conf))
            {
                context.Save(item);
            }
        }

        public T Get<T>(string key) where T : class
        {
            using (DynamoDBContext context = new DynamoDBContext(_client, _conf))
            {
                return context.Load<T>(key);
            }
        }

        public T Get<T>(string key, long range) where T : class
        {
            using (DynamoDBContext context = new DynamoDBContext(_client, _conf))
            {
                T t = context.Load<T>(key, range);
                return t;
            }
        }

        //????
        public void Update<T>(T item) where T : class
        {
            using (DynamoDBContext context = new DynamoDBContext(_client, _conf))
            {
                T savedItem = context.Load(item);

                if (savedItem == null)
                {
                    throw new EntityNotExistsException("The item does not exist in the Table");
                }

                context.Save(item);
            }
        }

        public void Delete<T>(string key, string range)
        {
            using (DynamoDBContext context = new DynamoDBContext(_client, _conf))
            {
                var savedItem = context.Load<T>(key, range);

                if (savedItem == null)
                {
                    throw new EntityNotExistsException("The item does not exist in the Table");
                }

                context.Delete(savedItem);
            }

        }
        









    }

}