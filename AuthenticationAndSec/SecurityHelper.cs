using System.Security.Cryptography;
using System.Text;

namespace AuthenticationAndSec;

public static class SecurityHelper
{
    public static string SanitizeInput(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;
            
        // Remove HTML/JS tags
        var sanitized = System.Text.RegularExpressions.Regex.Replace(
            input, 
            "<[^>]*(>|$)", 
            string.Empty, 
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            
        // Escape special characters
        return System.Net.WebUtility.HtmlEncode(sanitized);
    }
    
    public static bool ValidateEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;
            
        try {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch {
            return false;
        }
    }
    
    public static string HashPassword(string password, string salt = null)
    {
        using var sha256 = SHA256.Create();
        salt ??= GenerateRandomSalt();
        
        var saltedPassword = $"{salt}{password}";
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
        
        return $"{salt}:{Convert.ToBase64String(bytes)}";
    }
    
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        if (string.IsNullOrEmpty(hashedPassword))
            return false;
            
        var parts = hashedPassword.Split(':');
        if (parts.Length != 2)
            return false;
            
        var salt = parts[0];
        var newHash = HashPassword(password, salt);
        
        return newHash == hashedPassword;
    }
    
    private static string GenerateRandomSalt(int length = 32)
    {
        var randomBytes = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}
