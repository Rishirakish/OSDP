using System;
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

        #region Public Methods.
        public int Insert(Role role)
        {
            try
            {
                return _roleRepository.Insert(role);
            }
            catch (Exception ex)
            {
                //TODO: log exception here.
                return 0;
            }
        }
        public int Update(int id, Role role)
        {
            try
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
            catch (Exception ex)
            {
                //TODO: log exception here.
                return 0;
            }
        }
        public List<Role> Get()
        {
            try
            {
                return _roleRepository.Get();
            }
            catch (Exception ex)
            {
                //TODO: log exception here.
                return null;
            }
        }
        public Role Get(int id)
        {
            try
            {
                return _roleRepository.Get(id);
            }
            catch (Exception ex)
            {
                //TODO: log exception here.
                return null;
            }
        }
        public List<string> Get(string userName)
        {
            try
            {
                return _roleRepository.Get(userName);
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
                return _roleRepository.Delete(id);
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
