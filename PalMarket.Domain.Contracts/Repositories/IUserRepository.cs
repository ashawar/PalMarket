using PalMarket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Domain.Contracts.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User Authenticate(string username, string passwordHash);
    }
}
