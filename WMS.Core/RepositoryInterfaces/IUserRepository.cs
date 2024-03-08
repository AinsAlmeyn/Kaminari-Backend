using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Core.Entities.Database.MongoDb;

namespace WMS.Core.RepositoryInterfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
    }
}
