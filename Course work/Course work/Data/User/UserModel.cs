using System.ComponentModel.DataAnnotations;

namespace Course_work.User
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        public string FIO { get; set; }

        public string BirthDate { get; set; }

        public string Sex { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Status { get; set; }
    }
}
