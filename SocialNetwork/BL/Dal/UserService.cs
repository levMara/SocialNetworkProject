using Authetication.Db;
using Authetication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Dal
{
    class UserService
    {
        //interface
        private readonly DynamoService _dynamoService;

        public UserService()
        {
            _dynamoService = new DynamoService();
        }

        public User Get(string userName)
        {
            return _dynamoService.Get<User>(userName);
        }

        public User Add(User newUser)
        {
            _dynamoService.Add<User>(newUser);
            return newUser;
        }

        public User Update(User updateUser)
        {
            _dynamoService.Update<User>(updateUser);
            return updateUser;
        }

    }
}
