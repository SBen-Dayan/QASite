using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA.Data
{
    public class UserRepository
    {
        public readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool Verify(string email, string password)
        {
            using var context = new QaDataContext(_connectionString);
            var userHashPassword = context.Users.Where(u => u.Email == email).Select(u => u.HashPassword).FirstOrDefault();
            if (userHashPassword == null)
            {
                return false;
            }
            return BCrypt.Net.BCrypt.Verify(password, userHashPassword);
        }

        public void InsertUser(User user, string password)
        {
            using var context = new QaDataContext(_connectionString);
            user.HashPassword = BCrypt.Net.BCrypt.HashPassword(password);
            context.Users.Add(user);
            context.SaveChanges();
        }

        public int GetUserIdByEmail(string email)
        {
            using var context = new QaDataContext(_connectionString);
            return context.Users.Where(u => u.Email == email).Select(u => u.Id).FirstOrDefault();
        }
    }
}
