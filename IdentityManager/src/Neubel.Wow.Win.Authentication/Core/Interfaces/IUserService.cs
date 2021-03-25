using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Core.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Insert user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool Insert(User user, string password);

        /// <summary>
        /// Update user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        int Update(int id, User user);
        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns></returns>
        List<User> Get();
        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User Get(int id);
        bool ActivateDeactivateUser(ActivateDeactivateUser activateDeactivateUser);
        bool Delete(int id);
    }
}
