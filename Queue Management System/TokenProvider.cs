using Microsoft.IdentityModel.Tokens;
using Queue_Management_System.Models;
using Queue_Management_System.QueueDbContext;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Queue_Management_System
{
    public class TokenProvider
    {
        QueueDbConnectionDbContext _context;

        public TokenProvider(QueueDbConnectionDbContext context)
        {
            _context = context;
        }

        public string LoginUser(string UserName, string Password)
        {


            string username = UserName;
            string pass = Password;



            var user = _context.Users.SingleOrDefault(x => x.UserName == username && x.Password == pass);

            //Authenticate User, Check if its a registered user in DB  - JRozario
            if (user == null)
                return null;

            var key = Encoding.ASCII.GetBytes("YourKey-2374-OFFKDI940NG7:56753253-tyuw-5769-0921-kfirox29zoxv");

            var JWToken = new JwtSecurityToken(
                issuer: "",
                audience: "",
                claims: GetUserClaims(user),
                notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,

                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );
            var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
            var strusername = user.UserName;
            return token;
        }
        private IEnumerable<Claim> GetUserClaims(Users user)
        {
            List<Claim> claims = new List<Claim>();
            Claim _claim;
            _claim = new Claim(ClaimTypes.Name, user.Id.ToString());
            claims.Add(_claim);
            _claim = new Claim(ClaimTypes.Role, user.UserGroup);
            claims.Add(_claim);

            if (user.UserGroup != null)
            {
                _claim = new Claim(user.UserGroup, user.UserGroup);
                claims.Add(_claim);
            }
            return claims.AsEnumerable<Claim>();
        }
    }
}

