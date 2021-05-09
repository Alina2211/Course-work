
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Npgsql;

namespace Course_work.User
{
    public class UserRepository : IUserRepository
    {
        readonly string connectionString =
           "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=Vsu;";

        public IEnumerable<UserModel> GetAllUsers()
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sql = "SELECT id AS Id, fio AS FIO, birth_date AS BirthDate, " +
                    "sex AS Sex, password AS Password, email AS Email, status AS Status " +
                    "FROM public.site_user";

                db.Open();
                return db.Query<UserModel>(sql);
            }
        }

        public UserModel GetUserById(int id)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sql = "SELECT id AS Id, fio AS FIO, birth_date AS BirthDate, " +
                    "sex AS Sex, password AS Password, email AS Email, status AS Status " +
                    "FROM public.site_user " +
                    "WHERE id = @ID";

                db.Open();
                return db.Query<UserModel>(sql, new { ID = id }).FirstOrDefault();
            }
        }

        public UserModel GetUserByPassAndEmail(string pass, string email)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sql = "SELECT id AS Id, fio AS FIO, birth_date AS BirthDate, " +
                    "sex AS Sex, password AS Password, email AS Email, status AS Status " +
                    "FROM public.site_user " +
                    "WHERE password = @PAS AND email = @EM";

                db.Open();
                return db.Query<UserModel>(sql, new { PAS = pass, EM = email}).FirstOrDefault();
            }
        }

        public bool AddUser(UserModel user)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sqlCheck = "SELECT id FROM public.site_user WHERE email = @EM";
                string sqlInsert = "INSERT INTO public.site_user(fio, birth_date, sex, password, email) " +
                    "VALUES (@FIO, @BDATE, @SEX, @PAS, @EM)";


                if (db.Query<string>(sqlCheck, new { EM = user.Email }).FirstOrDefault() != null)
                {
                    return false;
                }

                db.Execute(sqlInsert, new
                {
                    FIO = user.FIO,
                    BDATE = user.BirthDate,
                    SEX = user.Sex,
                    PAS = user.Password,
                    EM = user.Email
                });

                return true;
            }
        }

        public bool ChangeStatus(string newStatus, int id)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sqlCheck = "SELECT id FROM public.site_user WHERE id = @ID";
                string sqlUpd = "UPDATE public.site_user SET status = @NSTAT " +
                    "WHERE id = @ID";

                if (db.Query<string>(sqlCheck, new { ID = id }).FirstOrDefault() == null)
                {
                    return false;
                }

                if (newStatus != "lecturer" && newStatus != "student") return false;

                db.Open();
                db.Execute(sqlUpd, new { ID = id, NSTAT = newStatus });

                return true;
            }
        }

        

        public bool DeleteUser(int id)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sqlCheck = "SELECT id FROM public.site_user WHERE id = @ID";
                string sqlGetStatus = "SELECT status FROM public.site_user WHERE id = @ID";
                string sqlDeleteUser = "DELETE FROM public.site_user WHERE id = @ID";
                string sqlDeleteLecturer = "DELETE FROM public.lecturer WHERE user_id = @ID";
                string sqlDeleteTeachingWork = "DELETE FROM public.teaching_work WHERE teacher_id = @ID";
                string sqlDeleteLecturerNews = "DELETE FROM public.lecturer WHERE lecturer_id = @ID";
                string sqlDeleteStudent = "DELETE FROM public.student WHERE user_id = @ID";

                if (db.Query<string>(sqlCheck, new { ID = id }).FirstOrDefault() == null)
                {
                    return false;
                }

                string status = db.Query<string>(sqlGetStatus, new { ID = id }).FirstOrDefault();

                if (status == "undefined")
                {
                    db.Execute(sqlDeleteUser, new { ID = id });
                    return true;
                }

                if (status == "student")
                {
                    db.Execute(sqlDeleteStudent, new { ID = id });
                    db.Execute(sqlDeleteUser, new { ID = id });
                    return true;
                }

                if (status == "lecturer")
                {
                    db.Execute(sqlDeleteLecturerNews, new { ID = id });
                    db.Execute(sqlDeleteTeachingWork, new { ID = id });
                    db.Execute(sqlDeleteLecturer, new { ID = id });
                    db.Execute(sqlDeleteUser, new { ID = id });
                    return true;
                }

                //т.е. если статус не из вышеописанных
                return false;
            }
        }
    }
}
