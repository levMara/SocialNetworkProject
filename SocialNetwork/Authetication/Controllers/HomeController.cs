using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Authetication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var user = new Document
            //{
            //    ["username/Token"] = "levi",
            //    ["userId"] = "123"
            //};
            //AmazonDynamoDBConfig dbConfing = new AmazonDynamoDBConfig();
            //var client = new AmazonDynamoDBClient(dbConfing);
            //Table authenticationTable = Table.LoadTable(client, "AuthenticationDb");
            //var users = authenticationTable.PutItem(user);

            //ViewBag.Title = "Home Page";

            return View();
        }
    }
}
