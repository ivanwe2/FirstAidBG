using FirstAidBG_v._2.Data.Entities;
using FirstAidBG_v._2.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstAidBG_v._2.Data.Repositories
{
    public class AppUserRepository : BaseRepository<AppUser>, IAppUserRepository
    {
        public AppUserRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
