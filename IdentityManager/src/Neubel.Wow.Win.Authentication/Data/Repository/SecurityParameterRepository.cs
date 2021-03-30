using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Neubel.Wow.Win.Authentication.Core.Model;
using Neubel.Wow.Win.Authentication.Infrastructure;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public class SecurityParameterRepository : ISecurityParameterRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public SecurityParameterRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        #region Public Methods.
        public List<SecurityParameter> Get()
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<SecurityParameter>("Select * From [PasswordPolicy]").ToList();
        }

        public SecurityParameter Get(int orgId)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<SecurityParameter>("Select * From [PasswordPolicy] where OrgId=@orgId", new { orgId }).FirstOrDefault();
        }

        public int Insert(SecurityParameter securityParameter)
        {
            string query = @"Insert into [PasswordPolicy](MinCaps, MinSmallChars, MinSpecialChars, MinNumber, MinLength, AllowUserName, DisAllPastPassword, DisAllowedChars, ChangeIntervalDays, OrgId) 
                values (@MinCaps, @MinSmallChars, @MinSpecialChars, @MinNumber, @MinLength, @AllowUserName, @DisAllPastPassword, @DisAllowedChars, @ChangeIntervalDays, @OrgId)";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, securityParameter);
        }

        public int Update(SecurityParameter updatedSecurityParameter)
        {
            string query = @"update [PasswordPolicy] Set 
                                MinCaps = @MinCaps, 
                                MinSmallChars = @MinSmallChars,
                                MinSpecialChars = @MinSpecialChars,
                                MinNumber = @MinNumber,
                                MinLength = @MinLength,
                                AllowUserName = @AllowUserName,
                                DisAllPastPassword = @DisAllPastPassword,
                                DisAllowedChars = @DisAllowedChars,
                                ChangeIntervalDays = @ChangeIntervalDays,
                                OrgId = @OrgId
                            Where Id = @Id";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, updatedSecurityParameter);
        }
        public bool Delete(int id)
        {
            string query = @"update [PasswordPolicy] Set
                                IsDeleted = @IsDeleted
                            Where Id = @Id";

            using IDbConnection db = _connectionFactory.GetConnection;
            db.Execute(query, new { IsDeleted = true, Id = id });
            return true;
        }
        #endregion
    }
}
