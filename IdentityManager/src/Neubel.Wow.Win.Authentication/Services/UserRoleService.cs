using System;
using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Core.Interfaces;
using Neubel.Wow.Win.Authentication.Core.Model;
using Neubel.Wow.Win.Authentication.Data.Repository.Interfaces;

namespace Neubel.Wow.Win.Authentication.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly ILogger _logger;

        public UserRoleService(IUserRoleRepository userRoleRepository, ILogger logger)
        {
            _userRoleRepository = userRoleRepository;
            _logger = logger;
        }

        public int Add(UserRole userRole)
        {
            try
            {
                return _userRoleRepository.Add(userRole);
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

        public bool Delete(int id)
        {
            try
            {
                return _userRoleRepository.Delete(id);
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

        public List<UserRole> Get()
        {
            try
            {
                return _userRoleRepository.Get();
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

        public List<UserRole> Get(int userId)
        {
            try
            {
                return _userRoleRepository.Get(userId);
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
    }
}
