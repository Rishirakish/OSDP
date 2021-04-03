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
        private readonly ILogger _logger;

        public UserService(IUserRepository userRepository, ISecurityParameterRepository securityParameterRepository, ILogger logger)
        {
            _userRepository = userRepository;
            _securityParameterRepository = securityParameterRepository;
            _logger = logger;
        }

        #region Public Methods.
        public RequestResult<bool> Insert(SessionContext sessionContext, User user, string password)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            try
            {
                if (!Helpers.IsInOrganizationContext(sessionContext, user.OrgId))
                {
                    var error = new ValidationMessage { Reason = "You can only register users in your organization", Severity = ValidationSeverity.Error};
                    validationMessages.Add(error);
                    return new RequestResult<bool>(false, validationMessages);
                }

                var passwordPolicy = _securityParameterRepository.Get(sessionContext, user.OrgId);
                var validatePassword = Helpers.ValidatePassword(password, passwordPolicy);
                if (validatePassword.IsSuccessful)
                {
                    PasswordLogin passwordLogin = Hasher.HashPassword(password);
                    _userRepository.Insert(user, passwordLogin);
                    return new RequestResult<bool>(true);
                }

                return new RequestResult<bool>(false, validatePassword.ValidationMessages);
            }
            catch (Exception ex)
            {
                _logger.LogException(new ExceptionLog
                {
                    ExceptionDate = DateTime.Now,
                    ExceptionMsg = ex.Message,
                    ExceptionSource = ex.Source,
                    ExceptionType = "UserService",
                    FullException = ex.StackTrace
                });
                return new RequestResult<bool>(false);
            }
        }
        public int Update(SessionContext sessionContext, int id, User user)
        {
            try
            {
                if (!Helpers.IsInOrganizationContext(sessionContext, user.OrgId))
                {
                    return 0;
                }

                User savedUser = _userRepository.Get(sessionContext, id);
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
                _logger.LogException(new ExceptionLog
                {
                    ExceptionDate = DateTime.Now,
                    ExceptionMsg = ex.Message,
                    ExceptionSource = ex.Source,
                    ExceptionType = "UserService",
                    FullException = ex.StackTrace
                });
                return 0;
            }
        }
        public List<User> Get(SessionContext sessionContext)
        {
            try
            {
                return _userRepository.Get(sessionContext);
            }
            catch (Exception ex)
            {
                _logger.LogException(new ExceptionLog
                {
                    ExceptionDate = DateTime.Now,
                    ExceptionMsg = ex.Message,
                    ExceptionSource = ex.Source,
                    ExceptionType = "UserService",
                    FullException = ex.StackTrace
                });
                return null;
            }
        }
        public User Get(SessionContext sessionContext, int id)
        {
            try
            {
                return _userRepository.Get(sessionContext, id);
            }
            catch (Exception ex)
            {
                _logger.LogException(new ExceptionLog
                {
                    ExceptionDate = DateTime.Now,
                    ExceptionMsg = ex.Message,
                    ExceptionSource = ex.Source,
                    ExceptionType = "UserService",
                    FullException = ex.StackTrace
                });
                return null;
            }
        }
        public bool ActivateDeactivateUser(SessionContext sessionContext, ActivateDeactivateUser activateDeactivateUser)
        {
            try
            {
                return _userRepository.ActivateDeactivateUser(activateDeactivateUser);
            }
            catch (Exception ex)
            {
                _logger.LogException(new ExceptionLog
                {
                    ExceptionDate = DateTime.Now,
                    ExceptionMsg = ex.Message,
                    ExceptionSource = ex.Source,
                    ExceptionType = "UserService",
                    FullException = ex.StackTrace
                });
                return false;
            }
        }
        public bool Delete(SessionContext sessionContext, int id)
        {
            try
            {
                return _userRepository.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogException(new ExceptionLog
                {
                    ExceptionDate = DateTime.Now,
                    ExceptionMsg = ex.Message,
                    ExceptionSource = ex.Source,
                    ExceptionType = "UserService",
                    FullException = ex.StackTrace
                });
                return false;
            }
        }

        #endregion

    }
}
