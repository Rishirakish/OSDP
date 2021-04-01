using System;
using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Core.Interfaces;
using Neubel.Wow.Win.Authentication.Core.Model;
using Neubel.Wow.Win.Authentication.Data.Repository;

namespace Neubel.Wow.Win.Authentication.Services
{
    public class SecurityParameterService : ISecurityParameterService
    {
        private readonly ISecurityParameterRepository _securityParameterRepository;
        private readonly ILogger _logger;
        public SecurityParameterService(ISecurityParameterRepository securityParameterRepository, ILogger logger)
        {
            _securityParameterRepository = securityParameterRepository;
            _logger = logger;
        }

        #region Public Methods.
        public List<SecurityParameter> Get()
        {
            try
            {
                return _securityParameterRepository.Get();
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
        public SecurityParameter Get(int id)
        {
            try
            {
                return _securityParameterRepository.Get(id);
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
        public int Insert(SecurityParameter securityParameter)
        {
            try
            {
                return _securityParameterRepository.Insert(securityParameter);
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
        public int Update(int id, SecurityParameter updatedSecurityParameter)
        {
            try
            {
                SecurityParameter savedSecurityParameter = _securityParameterRepository.Get(id);
                if (savedSecurityParameter != null)
                {
                    updatedSecurityParameter.Id = id;
                    if (!savedSecurityParameter.Equals(updatedSecurityParameter))
                        return _securityParameterRepository.Update(updatedSecurityParameter);
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
        public bool Delete(int id)
        {
            try
            {
                return _securityParameterRepository.Delete(id);
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
