using Neubel.Wow.Win.Authentication.Core.Model;
using System;
using System.Data;
using System.Linq;
using Dapper;
using Neubel.Wow.Win.Authentication.Infrastructure;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public AuthenticationRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public int LoginLog(LoginRequest loginRequest)
        {
            string query = @"Insert into [LoginLog](UserId, LoginDate, Status, UserName, PasswordHash, IPAddress, Browser, DeviceCode, DeviceName) 
                values (@Id, @LoginDate, @Status, @UserName, @PasswordHash, @IPAddress, @Browser, @DeviceCode, @DeviceName)";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, loginRequest);
        }
        public bool UpdatePasswordLogin(PasswordLogin passwordLogin)
        {
            string query = @"update [PasswordLogin] Set 
                                PasswordHash = @PasswordHash,
                                PasswordSalt = @PasswordSalt
                            Where UserId = @UserId";

            using IDbConnection db = _connectionFactory.GetConnection;
            db.Execute(query, passwordLogin);
            return true;
        }
        public bool LockUnlockUser(LockUnlockUser lockUnlockUser)
        {
            string query = @"update [User] Set 
                                Locked = @Locked
                            Where Id = @Id";
            string queryLockedLog = @"insert into [LockedLog] (LockedDate, UserId) values (@LockedDate, @UserId)";

            using IDbConnection db = _connectionFactory.GetConnection;
            using var transaction = db.BeginTransaction();
            db.Execute(query, lockUnlockUser, transaction);

            if (lockUnlockUser.Locked)
            {
                db.Execute(queryLockedLog, new { LockedDate = DateTime.Now, UserId = lockUnlockUser.Id }, transaction);
            }
            transaction.Commit();
            return true;
        }
        public PasswordLogin GetLoginPassword(string userName)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<PasswordLogin>("Select pl.* From [User] u inner join [PasswordLogin] pl on u.id=pl.userId where userName=@userName", new { userName }).FirstOrDefault();
        }
    }
}
