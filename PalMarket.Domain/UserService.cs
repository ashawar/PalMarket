using PalMarket.Common.Helpers;
using PalMarket.Domain.Contracts;
using PalMarket.Domain.Contracts.Repositories;
using PalMarket.Domain.Contracts.Services;
using PalMarket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Domain
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IStoreService Members

        public User Authenticate(string username, string password)
        {
            string hashedPassword = SecurityHelper.HashPassword(password);
            return userRepository.Authenticate(username, hashedPassword);
        }

        #endregion
    }
}
