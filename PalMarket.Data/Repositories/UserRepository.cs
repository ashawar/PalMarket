using PalMarket.Data.Infrastructure;
using PalMarket.Domain.Contracts.Repositories;
using PalMarket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Data.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public User Authenticate(string username, string passwordHash)
        {
            return this.DbContext.Users.FirstOrDefault(a => a.Username.ToLower().Equals(username.ToLower()) && a.PasswordHash.Equals(passwordHash));
        }
    }
}
