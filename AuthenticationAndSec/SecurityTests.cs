using Xunit;

namespace AuthenticationAndSec.Tests;

public class SecurityTests
{
    [Fact]
    public void SanitizeInput_RemovesHtmlTags()
    {
        var input = "<script>alert('xss')</script>safe";
        var result = SecurityHelper.SanitizeInput(input);
        Assert.DoesNotContain("<script>", result);
        Assert.DoesNotContain("</script>", result);
        Assert.Contains("safe", result);
    }
    
    [Fact]
    public void ValidateEmail_AcceptsValidEmail()
    {
        Assert.True(SecurityHelper.ValidateEmail("test@example.com"));
    }
    
    [Fact]
    public void ValidateEmail_RejectsInvalidEmail()
    {
        Assert.False(SecurityHelper.ValidateEmail("invalid-email"));
    }
    
    [Fact]
    public void HashPassword_GeneratesDifferentHashesForSamePassword()
    {
        var password = "securePassword123";
        var hash1 = SecurityHelper.HashPassword(password);
        var hash2 = SecurityHelper.HashPassword(password);
        
        Assert.NotEqual(hash1, hash2);
    }
    
    [Fact]
    public void VerifyPassword_ValidatesCorrectPassword()
    {
        var password = "securePassword123";
        var hash = SecurityHelper.HashPassword(password);
        
        Assert.True(SecurityHelper.VerifyPassword(password, hash));
    }
    
    [Fact]
    public void DatabaseService_RejectsSqlInjection()
    {
        using var db = new DatabaseService("Data Source=:memory:");
        
        // Attempt SQL injection in username
        var maliciousInput = "admin'; DROP TABLE Users;--";
        
        // This should safely handle the input without executing malicious code
        db.AddUser(maliciousInput, "test@test.com", "password");
        
        // Verify table still exists and contains our test user
        var exists = db.VerifyLogin(maliciousInput, "password");
        Assert.True(exists);
    }
    
    [Fact]
    public void AddAdminUser_CreatesUserWithAdminRole()
    {
        using var db = new DatabaseService("Data Source=:memory:");
        
        db.AddAdminUser("admin", "admin@test.com", "password");
        
        var role = db.GetUserRole("admin");
        Assert.Equal("Admin", role);
    }

    [Fact]
    public void IsUserAdmin_ReturnsTrueForAdminUser()
    {
        using var db = new DatabaseService("Data Source=:memory:");
        
        db.AddAdminUser("admin", "admin@test.com", "password");
        
        Assert.True(db.IsUserAdmin("admin"));
    }

    [Fact]
    public void IsUserAdmin_ReturnsFalseForRegularUser()
    {
        using var db = new DatabaseService("Data Source=:memory:");
        
        db.AddUser("user", "user@test.com", "password");
        
        Assert.False(db.IsUserAdmin("user"));
    }
}
