using System;
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
        private readonly ISecurityParameterRepository _securityParameterRepository;
        public UserService(IUserRepository userRepository, ISecurityParameterRepository securityParameterRepository)
        {
            _userRepository = userRepository;
            _securityParameterRepository = securityParameterRepository;
        }

        #region Public Methods.
        public bool Insert(User user, string password)
        {
            try
            {
                var passwordPolicy = _securityParameterRepository.Get(user.OrgId);
                if (Helpers.ValidatePassword(password, passwordPolicy))
                {
                    PasswordLogin passwordLogin = Hasher.HashPassword(password);
                    _userRepository.Insert(user, passwordLogin);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                //TODO: log exception here.
                return false;
            }
        }
        public int Update(int id, User user)
        {
            try
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
            catch (Exception ex)
            {
                //TODO: log exception here.
                return 0;
            }
        }
        public List<User> Get()
        {
            try
            {
                return _userRepository.Get();
            }
            catch (Exception ex)
            {
                //TODO: log exception here.
                return null;
            }
        }
        public User Get(int id)
        {
            try
            {
                return _userRepository.Get(id);
            }
            catch (Exception ex)
            {
                //TODO: log exception here.
                return null;
            }
        }
        public bool ActivateDeactivateUser(ActivateDeactivateUser activateDeactivateUser)
        {
            try
            {
                return _userRepository.ActivateDeactivateUser(activateDeactivateUser);
            }
            catch (Exception ex)
            {
                //TODO: log exception here.
                return false;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                return _userRepository.Delete(id);
            }
            catch (Exception ex)
            {
                //TODO: log exception here.
                return false;
            }
        }
        #endregion
    }
}
