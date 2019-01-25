
namespace BL.Dal
{
    using Authetication.Db;
    using Authetication.Models;
    using global::BL.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace BL.Dal
    {
        class FacebookUserService
        {
            //interface
            private readonly DynamoService _dynamoService;

            public FacebookUserService()
            {
                _dynamoService = new DynamoService();
            }

            public FacebookUser Get(string facebookId)
            {
                try
                {
                    return _dynamoService.Get<FacebookUser>(facebookId);
                }
                catch (Exception ex)
                { throw ex; }
            }

            public FacebookUser Add(FacebookUser newUser)
            {
                _dynamoService.Add<FacebookUser>(newUser);
                return newUser;
            }

            public void Update(FacebookUser newUser)
            {
                _dynamoService.Update<FacebookUser>(newUser);
            }


        }
    }
}
