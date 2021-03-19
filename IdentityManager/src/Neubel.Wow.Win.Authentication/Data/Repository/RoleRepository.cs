using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Neubel.Wow.Win.Authentication.Core.Model;
using Neubel.Wow.Win.Authentication.Infrastructure;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public RoleRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        #region Public Methods
        public int Insert(Role role)
        {
            string query = @"Insert into [Role](Name, Level) 
                values (@Name, @Level)";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, role);
        }
        public int Update(Role role)
        {
            string query = @"update [Role] Set 
                                Name = @Name, 
                                Level = @Level
                            Where Id = @Id";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, role);
        }
        public List<Role> Get()
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<Role>("Select * From [Role]").ToList();
        }
        public Role Get(int id)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<Role>("Select * From [Role] where Id=@id", new { id }).FirstOrDefault();
        }

        public List<string> Get(string userName)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            string query = @"Select r.Name from [User] u 
                                inner join [UserRole] ur on u.id = ur.userId
                                inner join [Role] r on r.Id = ur.RoleId
                             where u.UserName=@userName";

            return db.Query<string>(query, new { userName }).ToList();
        }

        public bool Delete(int id)
        {
            string query = @"update [Role] Set 
                                IsDeleted = @IsDeleted
                            Where Id = @Id";

            using IDbConnection db = _connectionFactory.GetConnection;
            db.Execute(query, new { IsDeleted = true, Id = id });
            return true;
        }
        #endregion
    }
}
