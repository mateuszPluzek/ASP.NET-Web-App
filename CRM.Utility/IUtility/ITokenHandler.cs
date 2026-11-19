using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Utility.IUtility
{
    public interface ITokenHandler
    {
        public string GenerateJwtToken(List<Claim> claims);
    }
}
