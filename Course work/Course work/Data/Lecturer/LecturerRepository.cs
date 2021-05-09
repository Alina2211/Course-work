using System.Collections.Generic;
using System.Linq;
using Dapper;
using Npgsql;

namespace Course_work.Lecturer
{
    public class LecturerRepository : ILecturerRepository
    {
        readonly string connectionString =
            "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=Vsu;";

        public IEnumerable<LecturerModel> GetAllLecturers()
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sql = "SELECT lecturer_id AS Id, achivements AS Achivments, " +
                    "publications_list AS Publications, teaching_info AS TeachingInfo, " +
                    "user_id AS UserId, photo AS Photo" +
                    "FROM public.lecturer";

                db.Open();
                return db.Query<LecturerModel>(sql);
            }
        }

        public LecturerModel GetLecturerByLecId(int id)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sql = "SELECT lecturer_id AS Id, achivements AS Achivments, " +
                    "publications_list AS Publications, teaching_info AS TeachingInfo, " +
                    "user_id AS UserId, photo AS Photo" +
                    "FROM public.lecturer " +
                    "WHERE lecturer_id = @LID";

                db.Open();
                return db.Query<LecturerModel>(sql, new { LID = id }).FirstOrDefault();
            }
        }

        public bool AddLecturer(LecturerModel lecturer)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sqlInsert = "INSERT INTO public.lecturer(achivements, " +
                    "publications_list, teaching_info, user_id, photo) " +
                    "VALUES (@ACH, @PUBL, @TEACIN, @USID, @PH)";
                string sqlCheck = "SELECT id FROM public.site_user " +
                    "WHERE id = @ID";
                string sqlCheckStatus = "SELECT status FROM public.site_user " +
                    "WHERE id = @ID";

                db.Open();

                if (db.Query<string>(sqlCheck, new { ID = lecturer.UserId }).FirstOrDefault() == null)
                {
                    return false;
                }

                string t = db.Query<string>(sqlCheckStatus, new { ID = lecturer.UserId }).FirstOrDefault();
                if (db.Query<string>(sqlCheckStatus, new { ID = lecturer.UserId }).FirstOrDefault() != "lecturer")
                {
                    return false;
                }

                db.Execute(sqlInsert, new { 
                    ACH = lecturer.Achivements,
                    PUBL = lecturer.Publications,
                    TEACIN = lecturer.TeachingInfo,
                    USID = lecturer.UserId,
                    PH = lecturer.Photo
                });

                return true;
            }
        }

        public bool UpdateLecturerAchivements(string achivs, int lecId)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sqlCheck = "SELECT lecturer_id FROM public.lecturer " +
                    "WHERE lecturer_id = @ID";
                string sqlUpd = "UPDATE public.lecturer SET achivements=@ACH WHERE lecturer_id = @ID";

                db.Open();

                if (db.Query<string>(sqlCheck, new { ID = lecId }).FirstOrDefault() == null)
                {
                    return false;
                }

                db.Execute(sqlUpd, new { ID = lecId, ACH = achivs });

                return true;
            }
        }

        public bool UpdateLecturerPublications(string publ, int lecId)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sqlCheck = "SELECT lecturer_id FROM public.lecturer " +
                    "WHERE lecturer_id = @ID";
                string sqlUpd = "UPDATE public.lecturer SET publications_list=@PBL WHERE lecturer_id = @ID";

                db.Open();

                if (db.Query<string>(sqlCheck, new { ID = lecId }).FirstOrDefault() == null)
                {
                    return false;
                }

                db.Execute(sqlUpd, new { ID = lecId, PBL = publ });

                return true;
            }
        }

        public bool UpdateLecturerTeachingInfo(string teacInf, int lecId)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sqlCheck = "SELECT lecturer_id FROM public.lecturer " +
                    "WHERE lecturer_id = @ID";
                string sqlUpd = "UPDATE public.lecturer SET teaching_info=@TIN WHERE lecturer_id = @ID";

                db.Open();

                if (db.Query<string>(sqlCheck, new { ID = lecId }).FirstOrDefault() == null)
                {
                    return false;
                }

                db.Execute(sqlUpd, new { ID = lecId, TIN = teacInf });

                return true;

            }
        }

        public bool UpdateLecturerPhoto(byte[] photo, int lecId)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sqlCheck = "SELECT lecturer_id FROM public.lecturer " +
                    "WHERE lecturer_id = @ID";
                string sqlUpd = "UPDATE public.lecturer SET photo=@PH WHERE lecturer_id = @ID";

                db.Open();

                if (db.Query<string>(sqlCheck, new { ID = lecId }).FirstOrDefault() == null)
                {
                    return false;
                }

                db.Execute(sqlUpd, new { ID = lecId, PH = photo });

                return true;

            }
        }
    }
}
