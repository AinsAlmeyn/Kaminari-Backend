using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Core.ContextInterfaces;
using WMS.Core.Entities.Database.MongoDb;
using WMS.Core.RepositoryInterfaces;
using WMS.DataAccess.RepositoryEntities;

namespace WMS.DataAccess.RepositoryArticles
{
    public class UserAnimeProfileRepository : GenericRepository<UserAnimeProfile>, IUserAnimeProfileRepository
    {
        public UserAnimeProfileRepository(IMongoDbContext _context) : base(_context)
        {
        }
    }
}