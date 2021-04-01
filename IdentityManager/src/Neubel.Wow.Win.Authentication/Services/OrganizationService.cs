using System;
using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Core.Interfaces;
using Neubel.Wow.Win.Authentication.Core.Model;
using Neubel.Wow.Win.Authentication.Data.Repository;

namespace Neubel.Wow.Win.Authentication.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly ILogger _logger;
        public OrganizationService(IOrganizationRepository organizationRepository, ILogger logger)
        {
            _organizationRepository = organizationRepository;
            _logger = logger;
        }

        #region Public Methods.
        public int Insert(Organization organization)
        {
            try
            {
                return _organizationRepository.Insert(organization);
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
        public int Update(int id, Organization organization)
        {
            try
            {
                Organization savedOrganization = _organizationRepository.Get(id);
                if (savedOrganization != null)
                {
                    organization.Id = id;
                    if (!savedOrganization.Equals(organization))
                        return _organizationRepository.Update(organization);
                }

                return _organizationRepository.Insert(organization);
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
        public List<Organization> Get()
        {
            try
            {
                return _organizationRepository.Get();
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
        public Organization Get(int id)
        {
            try
            {
                return _organizationRepository.Get(id);
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
        public bool Delete(int id)
        {
            try
            {
                return _organizationRepository.Delete(id);
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
