wwwwwwusing System;

namespace AuthenticationAndSec;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("SafeVault Authentication Demo");
        
        using var db = new DatabaseService("Data Source=safevault.db");
        
        // Create admin user if doesn't exist
        if (db.GetUserRole("admin") == null)
        {
            db.AddAdminUser("admin", "admin@safevault.com", "Admin@123");
            Console.WriteLine("Admin user created");
        }
        
        Console.WriteLine("\n=== Login ===");
        Console.Write("Username: ");
        var username = Console.ReadLine();
        
        Console.Write("Password: ");
        var password = Console.ReadLine();
        
        if (db.VerifyLogin(username, password))
        {
            Console.WriteLine($"\nWelcome {username}!");
            
            if (db.IsUserAdmin(username))
            {
                Console.WriteLine("\nADMIN MENU:");
                Console.WriteLine("1. View all users");
                Console.WriteLine("2. Reset user password");
                Console.WriteLine("3. Delete user");
            }
            else
            {
                Console.WriteLine("\nUSER MENU:");
                Console.WriteLine("1. View profile");
                Console.WriteLine("2. Change password");
            }
        }
        else
        {
            Console.WriteLine("Invalid credentials");
        }
    }
}
