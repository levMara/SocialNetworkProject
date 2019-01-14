using System;
using System.Collections.Generic;
using System.Text;
using Authetication.Db;
using Authetication.Models;
using Jose;

namespace BL.Manageres
{
    public class TokenService
    {
        private readonly DynamoService _dynamoService;

        public TokenService()
        {
            _dynamoService = new DynamoService();
        }

        public Token Get(string userId, long timeStamp)
        {

            Token t = _dynamoService.Get<Token>(userId, timeStamp);
            return t;
        }

        public Token Add(Token newToken)
        {
            _dynamoService.Add<Token>(newToken);
            return newToken;
        }
    }
}