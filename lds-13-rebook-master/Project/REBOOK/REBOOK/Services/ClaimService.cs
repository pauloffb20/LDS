using System.Security.Claims;

namespace REBOOK.Services
{
    public class ClaimService
    {
        /// <summary>
        /// Método para obter o id do utilizador através do token
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <returns>Id do User</returns>
        public static int? GetIdFromClaimIdentity(ClaimsIdentity claimsIdentity)
        {
            foreach (var claim in claimsIdentity.Claims)
            {
                if (claim.Type.Equals(ClaimTypes.SerialNumber))
                {
                    return int.Parse(claim.Value);
                }
            }

            return null;
        }
    }
}