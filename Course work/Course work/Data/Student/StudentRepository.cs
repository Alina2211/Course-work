using System.Collections.Generic;
using System.Linq;
using Dapper;
using Npgsql;

namespace Course_work.Student
{
    public class StudentRepository : IStudentRepository
    {

        readonly string connectionString =
           "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=Vsu;";

        public IEnumerable<StudentModel> GetAllStudents()
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sql = "SELECT student_id AS Id, course AS Course, " +
                     "status AS Status, user_id AS UserId " +
                     "FROM public.student";

                db.Open();
                return db.Query<StudentModel>(sql);
            }
        }

        public StudentModel GetStudentByStudId(int id)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sql = "SELECT student_id AS Id, course AS Course, " +
                     "status AS Status, user_id AS UserId " +
                     "FROM public.student " +
                     "WHERE student_id = @ID";

                db.Open();
                return db.Query<StudentModel>(sql, new { ID = id }).FirstOrDefault();
            }
        }

        public bool AddStudent(StudentModel  student)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sql = "INSERT INTO public.student(course, user_id, status) " +
                "VALUES (@CRS, @USID, @ST)";
                string sqlCheck = "SELECT id FROM public.site_user " +
                    "WHERE id = ID";
                string sqlCheckStatus = "SELECT status FROM public.site_user " +
                    "WHERE id = ID";

                db.Open();

                if (db.Query<string>(sqlCheck, new { ID = student.UserId }).FirstOrDefault() == null)
                {
                    return false;
                }

                if (db.Query<string>(sqlCheckStatus, new { ID = student.UserId }).FirstOrDefault() != "student")
                {
                    return false;
                }

                db.Execute(sql, new { CRS = student.Course, USID = student.UserId, ST = student.Status });

                return true;
                
            }
            
        }

        public bool UpdateStudent(StudentModel upStudent)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string updAll = "UPDATE public.student " +
                    "SET course=@CRS, status=@STS " +
                    "WHERE student_id = @ID";
                string updCourse = "UPDATE public.student " +
                    "SET course=@CRS " +
                    "WHERE student_id = @ID";
                string updStatus = "UPDATE public.student " +
                    "SET status=@STS " +
                    "WHERE student_id = @ID";
                string sqlCheck = "SELECT id FROM public.student " +
                    "WHERE student_id = @ID";

                db.Open();

                if (upStudent.Status == null && upStudent.Course == 0)
                {
                    return false;
                }

                if (db.Query<string>(sqlCheck, new { ID = upStudent.UserId }).FirstOrDefault() == null)
                {
                    return false;
                }

                if (upStudent.Status != null && upStudent.Course != 0)
                {
                    db.Execute(updAll, new { CRS = upStudent.Course, STS = upStudent.Status, ID = upStudent.Id });
                } 
                else if (upStudent.Status != null && upStudent.Course == 0)
                {
                    db.Execute(updStatus, new { STS = upStudent.Status, ID = upStudent.Id });
                }
                else if (upStudent.Status == null && upStudent.Course != 0)
                {
                    db.Execute(updCourse, new { CRS = upStudent.Course, ID = upStudent.Id });
                }


                return true;

            }
        }
    }
}
