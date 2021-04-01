using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Neubel.Wow.Win.Authentication.Core.Model;
using Neubel.Wow.Win.Authentication.Infrastructure;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public class UserRepository : IUserRepository
    { 
        private readonly IConnectionFactory _connectionFactory;
        public UserRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        #region Public Methods
        public int Insert(User user, PasswordLogin passwordLogin)
        {
            var p = new DynamicParameters();
            p.Add("Id", 0, DbType.Int32, ParameterDirection.Output);
            p.Add("@UserName", user.UserName);
            p.Add("@FirstName", user.FirstName);
            p.Add("@LastName", user.LastName);
            p.Add("@Email", user.Email);
            p.Add("@Mobile", user.Mobile);
            p.Add("@Country", user.Country);
            p.Add("@ISDCode", user.ISDCode);
            p.Add("@TwoFactor", user.TwoFactor);
            p.Add("@Locked", user.Locked);
            p.Add("@IsActive", user.IsActive);
            p.Add("@EmailValidationStatus", user.EmailValidationStatus);
            p.Add("@MobileValidationStatus", user.MobileValidationStatus);
            p.Add("@OrgId", user.OrgId);
            p.Add("@AdminLevel", user.AdminLevel);

            string userInsertQuery = @"Insert into [User](UserName, FirstName, LastName, Email, Mobile, Country, ISDCode, TwoFactor, Locked, IsActive, EmailValidationStatus, MobileValidationStatus, OrgId, AdminLevel) 
                values (@UserName, @FirstName, @LastName, @Email, @Mobile, @Country, @ISDCode, @TwoFactor, @Locked, @IsActive, @EmailValidationStatus, @MobileValidationStatus, @OrgId, @AdminLevel);
                SELECT @Id = @@IDENTITY";

            string passwordLoginInsertQuery = @"Insert into [PasswordLogin](PasswordHash, PasswordSalt, UserId) 
                values (@PasswordHash, @PasswordSalt, @UserId)";

            using IDbConnection db = _connectionFactory.GetConnection;
            using var transaction = db.BeginTransaction();
            db.Execute(userInsertQuery, p, transaction);

            int insertedUserId = p.Get<int>("@Id");

            passwordLogin.UserId = insertedUserId;
            db.Execute(passwordLoginInsertQuery, passwordLogin, transaction);
            transaction.Commit();

            return insertedUserId;
        }
        public int Update(User user)
        {
            string query = @"update [User] Set 
                                UserName = @UserName, 
                                FirstName = @FirstName,
                                LastName = @LastName,
                                Email = @Email,
                                Mobile = @Mobile,
                                Country = @Country,
                                ISDCode = @ISDCode,
                                TwoFactor = @TwoFactor,
                                Locked = @Locked,
                                IsActive = @IsActive,
                                EmailValidationStatus = @EmailValidationStatus,
                                MobileValidationStatus = @MobileValidationStatus,
                                OrgId = @OrgId,
                                AdminLevel = @AdminLevel 
                            Where Id = @Id";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, user);
        }
        public List<User> Get()
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<User>("Select * From [User]").ToList();
        }
        public User Get(int id)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<User>("Select * From [User] where Id=@id", new {id}).FirstOrDefault();
        }

        public bool ActivateDeactivateUser(ActivateDeactivateUser activateDeactivateUser)
        {
            string query = @"update [User] Set 
                                IsActive = @IsActive
                            Where UserName = @UserName";

            using IDbConnection db = _connectionFactory.GetConnection;
            db.Execute(query, activateDeactivateUser);
            return true;
        }

        public bool Delete(int id)
        {
            string query = @"update [User] Set 
                                IsDeleted = @IsDeleted
                            Where Id = @Id";

            using IDbConnection db = _connectionFactory.GetConnection;
            db.Execute(query, new { IsDeleted = true, Id = id });
            return true;
        }

        #endregion
    }
}
