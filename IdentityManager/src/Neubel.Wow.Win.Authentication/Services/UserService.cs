using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Interfaces;
using Neubel.Wow.Win.Authentication.Core.Model;
using Neubel.Wow.Win.Authentication.Data.Repository;

namespace Neubel.Wow.Win.Authentication.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public int Insert(User user, string password)
        {
            PasswordLogin passwordLogin = Hasher.HashPassword(password);
            return _userRepository.Insert(user, passwordLogin);
        }

        public int Update(int id, User user)
        {
            User savedUser = _userRepository.Get(id);
            if (savedUser != null)
            {
                user.Id = id;
                if (!savedUser.Equals(user))
                   return _userRepository.Update(user);
            }

            return 0;
        }

        public List<User> Get()
        {
            return _userRepository.Get();
        }

        public User Get(int id)
        {
            return _userRepository.Get(id);
        }

        public bool ActivateDeactivateUser(ActivateDeactivateUser activateDeactivateUser)
        {
            return _userRepository.ActivateDeactivateUser(activateDeactivateUser);
        }

        public bool Delete(int id)
        {
            return _userRepository.Delete(id);
        }
    }
}
