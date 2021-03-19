using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Core.Interfaces;
using Neubel.Wow.Win.Authentication.Core.Model;
using Neubel.Wow.Win.Authentication.Data.Repository;

namespace Neubel.Wow.Win.Authentication.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public int Insert(Role role)
        {
            return _roleRepository.Insert(role);
        }

        public int Update(int id, Role role)
        {
            Role savedRole = _roleRepository.Get(id);
            if (savedRole != null)
            {
                role.Id = id;
                if (!savedRole.Equals(role))
                    return _roleRepository.Update(role);
            }

            return _roleRepository.Insert(role);
        }

        public List<Role> Get()
        {
            return _roleRepository.Get();
        }

        public Role Get(int id)
        {
            return _roleRepository.Get(id);
        }

        public List<string> Get(string userName)
        {
            return _roleRepository.Get(userName);
        }

        public bool Delete(int id)
        {
            return _roleRepository.Delete(id);
        }
    }
}
