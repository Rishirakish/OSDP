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
        public OrganizationService(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
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
                //TODO: log exception here.
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
                //TODO: log exception here.
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
                //TODO: log exception here.
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
                //TODO: log exception here.
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
                //TODO: log exception here.
                return false;
            }
        }
        #endregion
    }
}
