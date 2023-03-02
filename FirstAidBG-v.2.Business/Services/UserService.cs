using FirstAidBG_v._2.Data;
using FirstAidBG_v._2.Data.Entities;
using System.Security.Claims;

namespace FirstAidBG_v._2.Business.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public AppUser GetUserByExternalProvider(string provider, string nameIdentifier)
        {
            var appUser = _context.AppUsers
                .Where(a => a.Provider == provider && a.NameIdentifier == nameIdentifier).FirstOrDefault();
            return appUser;
        }
        public AppUser GetUserByNameIdentifier(string nameIdentifier)
        {
            var appUser = _context.AppUsers
                 .Where(a => a.NameIdentifier == nameIdentifier).FirstOrDefault();
            return appUser;
        }
        public AppUser GetUserById(int id)
        {
            var appUser = _context.AppUsers.Find(id);
            return appUser;
        }

        public bool TryValidateUser(string username, string password, out List<Claim> claims)
        {
            claims = new List<Claim>();
            var appUser = _context.AppUsers
               .Where(a=> a.Username == username).Where(a=>a.Password == password).FirstOrDefault();
            if (appUser == null)
            {
                return false;
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, username));
                claims.Add(new Claim("username", username));
               
                claims.Add(new Claim(ClaimTypes.Email, appUser.Email));
                

                foreach (var item in appUser.RoleList)
                {
                    claims.Add(new Claim(ClaimTypes.Role, item));
                }
                return true;
	
            }
        }

        public AppUser AddNewUser(string provider, List<Claim> claims)
        {
            var appUser =new AppUser();
            appUser.Provider = provider;

            appUser.NameIdentifier = claims.GetClaim(ClaimTypes.NameIdentifier);
            //fix that make them be able to be null
            appUser.Username = claims.GetClaim(ClaimTypes.Name);
            appUser.Password = claims.GetClaim("password");
            
       
            appUser.Email = claims.GetClaim(ClaimTypes.Email);
            appUser.Roles = claims.GetClaim(ClaimTypes.Role);

            var entity = _context.AppUsers.Add(appUser);
            _context.SaveChanges();
            return entity.Entity;
        }
    }
    public static class Extensions
    {
        public static string GetClaim(this List<Claim> claims, string name)
        {
            string m = claims.FirstOrDefault(c => c.Type == name)?.Value;
            if (m == null)
            {
                return "";
            }
            return m;
        }
    }
}
