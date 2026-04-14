using System.Security.Claims;

namespace GestionAbsences.Helpers
{
    public static class AuthorizationHelper
    {
        public static int GetCurrentUserId(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim!);
        }

        public static string GetCurrentUserRole(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }

        public static bool IsAdmin(ClaimsPrincipal user)
        {
            return GetCurrentUserRole(user) == "Admin";
        }

        public static bool IsProfesseur(ClaimsPrincipal user)
        {
            return GetCurrentUserRole(user) == "Professeur";
        }

        public static bool IsEtudiant(ClaimsPrincipal user)
        {
            return GetCurrentUserRole(user) == "Etudiant";
        }
    }
}
