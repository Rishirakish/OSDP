using Neubel.Wow.Win.Authentication.Core.Model;
using System;
using System.Collections.Generic;
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

        #region Public Methods.
        public int SaveLoginToken(LoginToken loginToken)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            int userId = db.Query<int>(@"Select u.Id From [User] u Where u.UserName = @UserName", new { loginToken.UserName}).FirstOrDefault();
            loginToken.UserId = userId;
            int loginTokenUserId = db.Query<int>(@"Select userId From [LoginToken] Where  UserId = @userId", new { userId }).FirstOrDefault();

            string query = loginTokenUserId > 0 ?
                @"update [LoginToken] Set 
                    AccessToken = @AccessToken,
                    RefreshToken = @RefreshToken,
                    AccessTokenExpiry = @AccessTokenExpiry,
                    DeviceCode = @DeviceCode,
                    DeviceName = @DeviceName,
                    RefreshTokenExpiry = @RefreshTokenExpiry
                  Where UserId = @UserId"
                :
                @"Insert into [LoginToken](UserId, AccessToken, RefreshToken, AccessTokenExpiry, DeviceCode, DeviceName, RefreshTokenExpiry) 
                values (@UserId, @AccessToken, @RefreshToken, @AccessTokenExpiry, @DeviceCode, @DeviceName, @RefreshTokenExpiry)";

            return db.Execute(query, loginToken);
        }
        public int UpdateAccessToken(RefreshedAccessToken refreshedAccessToken)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            int userId = db.Query<int>(@"Select u.Id From [User] u Where u.UserName = @UserName", new { refreshedAccessToken.UserName }).FirstOrDefault();
            refreshedAccessToken.UserId = userId;

            int loginTokenUserId = db.Query<int>(@"Select userId From [LoginToken] Where  UserId = @userId", new { userId }).FirstOrDefault();

            if (loginTokenUserId > 0)
            {
                string query = @"update [LoginToken] Set 
                                UserId = @UserId,
                                AccessToken = @AccessToken,
                                AccessTokenExpiry = @AccessTokenExpiry,
                                DeviceCode = @DeviceCode,
                                DeviceName = @DeviceName
                              Where UserId = @UserId";

                return db.Execute(query, refreshedAccessToken);
            }
            return 0;
        }
        public int PasswordChangeLog(PasswordLogin passwordLogin)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            string query = @"Insert into [PasswordLog](UserId, PasswordHash, PasswordSalt, ChangeDate) 
                values (@UserId, @PasswordHash, @PasswordSalt, @ChangeDate)";

            return db.Execute(query, new { passwordLogin.UserId, passwordLogin.PasswordHash, passwordLogin.PasswordSalt, ChangeDate = DateTime.Now});
        }
        public int LoginTokenLog(LoginToken loginToken)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            string query = @"Insert into [LoginTokenLog](UserId, AccessToken, RefreshToken, AccessTokenExpiry, DeviceCode, DeviceName, RefreshTokenExpiry) 
                values (@UserId, @AccessToken, @RefreshToken, @AccessTokenExpiry, @DeviceCode, @DeviceName, @RefreshTokenExpiry)";

            return db.Execute(query, loginToken);
        }
        public int LoginTokenLogForRefreshToken(RefreshedAccessToken refreshedAccessToken)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            string query = @"Insert into [LoginTokenLog](UserId, AccessToken, AccessTokenExpiry, DeviceCode, DeviceName) 
                values (@UserId, @AccessToken, @AccessTokenExpiry, @DeviceCode, @DeviceName)";

            return db.Execute(query, refreshedAccessToken);
        }
        public int LoginLog(LoginRequest loginRequest)
        {
            string query = @"Insert into [LoginLog](UserId, LoginDate, Status, UserName, PasswordHash, IPAddress, Browser, DeviceCode, DeviceName) 
                values (@Id, @LoginDate, @Status, @UserName, @PasswordHash, @IPAddress, @Browser, @DeviceCode, @DeviceName)";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, loginRequest);
        }
        public List<LoginHistory> GetLoginHistory(int userId)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<LoginHistory>("Select * From [LoginLog] where UserId=@UserId", new { userId }).ToList();
        }
        public int LockedUserLog(LockUnlockUser lockUnlockUser)
        {
            string query = @"insert into [LockedLog] (LockedDate, UserId) values (@LockedDate, @UserId)";
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, new { LockedDate = DateTime.Now, UserId = lockUnlockUser.Id });
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

            using IDbConnection db = _connectionFactory.GetConnection;
            db.Execute(query, lockUnlockUser);
            return true;
        }
        public bool UpdateMobileConfirmationStatus(int userId, bool mobileValidationStatus)
        {
            string query = @"update [User] Set
                                MobileValidationStatus = @MobileValidationStatus
                            Where Id = @Id";

            using IDbConnection db = _connectionFactory.GetConnection;
            db.Execute(query, new {Id = userId, MobileValidationStatus = mobileValidationStatus });
            return true;
        }
        public bool UpdateEmailConfirmationStatus(int userId, bool emailValidationStatus)
        {
            string query = @"update [User] Set
                                EmailValidationStatus = @EmailValidationStatus
                            Where Id = @Id";

            using IDbConnection db = _connectionFactory.GetConnection;
            db.Execute(query, new { Id = userId, EmailValidationStatus = emailValidationStatus });
            return true;
        }
        public PasswordLogin GetLoginPassword(string userName)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<PasswordLogin>("Select pl.* From [User] u inner join [PasswordLogin] pl on u.id=pl.userId where userName=@userName", new { userName }).FirstOrDefault();
        }
        public int SaveOtp(UserValidationOtp userValidationOtp)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            int userId = db.Query<int>(@"Select u.UserId [UserValidationOtp] Where u.UserId = @UserId", new { userValidationOtp.UserId }).FirstOrDefault();

            string query = userId > 0 ?
                @"update [UserValidationOtp] Set 
                    otp = @otp,
                    OtpGeneratedTime = @OtpGeneratedTime,
                    OtpAuthenticatedTime = @OtpAuthenticatedTime,
                    Status = @Status,
                    Type = @Type,
                    OrgId = @OrgId,
                    Where UserId = @UserId"
                :
                @"Insert into [UserValidationOtp](UserId, otp, OtpGeneratedTime, OtpAuthenticatedTime, Status, Type, OrgId) 
                values (@UserId, @otp, @OtpGeneratedTime, @OtpAuthenticatedTime, @Status, @Type, @OrgId)";

            return db.Execute(query, userValidationOtp);
        }
        public UserValidationOtp GetOtp(int userId)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<UserValidationOtp>("Select * From [UserValidationOtp] where UserId=@UserId", new { userId }).FirstOrDefault();
        }
        #endregion
    }
}
