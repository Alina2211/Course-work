
using System.Collections.Generic;

namespace Course_work.User
{
    public interface IUserRepository
    {
        public IEnumerable<UserModel> GetAllUsers();

        public UserModel GetUserById(int id);

        public UserModel GetUserByPassAndEmail(string pass, string email);

        public bool AddUser(UserModel user);

        public bool ChangeStatus(string newStatus, int id);

        public bool DeleteUser(int id);
    }
}
