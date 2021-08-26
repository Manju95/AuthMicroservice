using System;
using System.Data;
using AuthMicroService.Interfaces;
using AuthMicroService.Models;
using Dapper;

namespace AuthMicroService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IDbConnection _Idbconnection;
        private ICryptoService _cryptoService;

        public AuthService(IDbConnection Idbconnection,ICryptoService cryptoService)
        {
            _Idbconnection = Idbconnection ?? throw new ArgumentNullException(nameof(Idbconnection));
            _cryptoService = cryptoService ?? throw new ArgumentNullException(nameof(cryptoService));
        }
        
        public User isUserExist(User user)
        {
            User userResult = null;
            string sqlcmd = "select * from users where username=@username";
            using (IDbConnection con = _Idbconnection)
            {
                con.Open();
                userResult = con.QueryFirstOrDefault<User>(sqlcmd, new { username = user.EmailId });
                con.Close();
            }
            return userResult;
        }

        public bool AreEqual(string dbHash,string plainPassword, string salt)
        {
            var UserPwdHash = _cryptoService.ComputeHash(plainPassword,salt);
            return UserPwdHash.Equals(dbHash);
        }

        public bool MatchUserCredentials(User user)
        {
            var userResult = isUserExist(user);
            if(userResult != null)
            {
                var isValidUser = AreEqual(userResult.Password,user.Password,userResult.Salt);
                if(isValidUser)
                {
                    return true;
                }
                else{
                    return false;
                }
            }
            return false;
        }

        public bool RegisterUser(User user)
        {
            string cmd = @"Insert into [dbo].[User] (FirstName,LastName,Email,Password,Salt) Values (@FirstName,@LastName,@Email,@Password,@Salt)";

            if(user != null && !string.IsNullOrEmpty(user.EmailId) && !string.IsNullOrEmpty(user.Password))
            {
                var salt = _cryptoService.GenerateSalt();
                var hashPassword = _cryptoService.ComputeHash(user.Password,salt);
                var param = new DynamicParameters();
                param.Add("@FirstName",user.FirstName);
                param.Add("@LastName",user.LastName);
                param.Add("@Email",user.EmailId);
                param.Add("@Password",hashPassword);
                param.Add("@Salt",salt);

                using (var con = _Idbconnection)
                {
                    con.Open();
                    var saveUser = con.Execute(cmd,param);
                    con.Close();

                    if(saveUser > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
    }
}