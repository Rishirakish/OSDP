using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Core.Interfaces;
using Neubel.Wow.Win.Authentication.Core.Model;
using Neubel.Wow.Win.Authentication.Data.Repository;

namespace Neubel.Wow.Win.Authentication.Services
{
    public class SecurityParameterService : ISecurityParameterService
    {
        private readonly ISecurityParameterRepository _securityParameterRepository;
        public SecurityParameterService(ISecurityParameterRepository securityParameterRepository)
        {
            _securityParameterRepository = securityParameterRepository;
        }
        
        public List<SecurityParameter> Get()
        {
            return _securityParameterRepository.Get();
        }

        public SecurityParameter Get(int id)
        {
            return _securityParameterRepository.Get(id);
        }

        public int Insert(SecurityParameter securityParameter)
        {
            return _securityParameterRepository.Insert(securityParameter);
        }

        public int Update(int id, SecurityParameter updatedSecurityParameter)
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
        public bool Delete(int id)
        {
            return _securityParameterRepository.Delete(id);
        }
    }
}
