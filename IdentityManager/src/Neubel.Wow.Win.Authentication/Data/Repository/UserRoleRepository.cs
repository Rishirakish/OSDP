using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Neubel.Wow.Win.Authentication.Core.Model;
using Neubel.Wow.Win.Authentication.Data.Repository.Interfaces;
using Neubel.Wow.Win.Authentication.Infrastructure;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public UserRoleRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<UserRole> Get()
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<UserRole>(@"Select ur.UserId, u.UserName, ur.RoleId, r.Name as RoleName  From [User] u
                                            inner join [UserRole] ur on u.id=ur.userId
                                            inner join [Role] r on ur.roleId=r.id").ToList();
        }
        public List<UserRole> Get(int userId)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<UserRole>(@"Select ur.UserId, u.UserName, ur.RoleId, r.Name as RoleName  From [User] u
                                            inner join [UserRole] ur on u.id=ur.userId
                                            inner join [Role] r on ur.roleId=r.id
                                           where ur.userId=@userId", new{userId}).ToList();
        }
        public int Add(UserRole userRole)
        {
            string query = @"Insert into [UserRole](UserId, RoleId) values (@UserId, @RoleId)";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, userRole);
        }
        public bool Delete(int id)
        {
            string query = @"update [UserRole] Set 
                                IsDeleted = @IsDeleted
                            Where Id = @Id";

            using IDbConnection db = _connectionFactory.GetConnection;
            db.Execute(query, new { IsDeleted = true, Id = id });
            return true;
        }
    }
}
