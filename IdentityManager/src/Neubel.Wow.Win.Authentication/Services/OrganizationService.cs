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

        public int Insert(Organization organization)
        {
            return _organizationRepository.Insert(organization);
        }

        public int Update(int id, Organization organization)
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

        public List<Organization> Get()
        {
            return _organizationRepository.Get();
        }

        public Organization Get(int id)
        {
            return _organizationRepository.Get(id);
        }
        public bool Delete(int id)
        {
            return _organizationRepository.Delete(id);
        }
    }
}
