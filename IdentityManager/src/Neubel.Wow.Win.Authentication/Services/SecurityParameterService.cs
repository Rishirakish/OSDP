using System;
using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Common;
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
        public List<SecurityParameter> Get(SessionContext sessionContext)
        {
            try
            {
                return _securityParameterRepository.Get(sessionContext);
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
        public SecurityParameter Get(SessionContext sessionContext, int id)
        {
            try
            {
                return _securityParameterRepository.Get(sessionContext,id);
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
        public int Insert(SessionContext sessionContext, SecurityParameter securityParameter)
        {
            try
            {
                if (!Helpers.IsInOrganizationContext(sessionContext, securityParameter.OrgId))
                {
                    return 0;
                }
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
        public int Update(SessionContext sessionContext, int id, SecurityParameter securityParameter)
        {
            try
            {
                if (!Helpers.IsInOrganizationContext(sessionContext, securityParameter.OrgId))
                {
                    return 0;
                }

                SecurityParameter savedSecurityParameter = _securityParameterRepository.Get(sessionContext, id);
                if (savedSecurityParameter != null)
                {
                    securityParameter.Id = id;
                    if (!savedSecurityParameter.Equals(securityParameter))
                        return _securityParameterRepository.Update(securityParameter);
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
