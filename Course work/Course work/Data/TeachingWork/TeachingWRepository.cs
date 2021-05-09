using System.Collections.Generic;
using System.Linq;
using Dapper;
using Npgsql;

namespace Course_work.TeachingWork
{
    public class TeachingWRepository : ITeachingWRepository
    {
        readonly string connectionString =
            "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=Vsu;";

        public TeachingWorkModel GetTeachingWorkByTeachID(int id)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sqlGet = "SELECT id, teaching_plan, teacher_id " +
                    "FROM public.teaching_work " +
                    "WHERE teacher_id = @ID";
                string sqlCheck = "SELECT id FROM public.lecturer " +
                    "WHERE lecturer_id = @ID";

                db.Open();

                if (db.Query<string>(sqlCheck, new { ID = id }).FirstOrDefault() == null)
                {
                    return null;
                }

                return db.Query<TeachingWorkModel>(sqlGet, new { ID = id }).FirstOrDefault();
            }
        }

        public TeachingWorkModel GetTeachingWorkByWorkID(int id)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sqlGet = "SELECT id, teaching_plan, teacher_id " +
                    "FROM public.teaching_work " +
                    "WHERE teacher_id = @ID";

                db.Open();

                return db.Query<TeachingWorkModel>(sqlGet, new { ID = id }).FirstOrDefault();
            }
        }

        public bool AddWork(TeachingWorkModel work)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sql = "INSERT INTO public.teaching_work(teaching_plan, teacher_id) " +
                     "VALUES (@TPL, @TID)";
                string sqlCheck = "SELECT id FROM public.lecturer " +
                    "WHERE lecturer_id = @ID";

                db.Open();

                if (db.Query<string>(sqlCheck, new { ID = work.Id }).FirstOrDefault() == null)
                {
                    return false;
                }

                db.Execute(sql, new { 
                    TPL = work.TeachingPlan,
                    TID = work.TeacherId
                });

                return true;
            }
        }

        public bool UpdateWorkByWorkId(TeachingWorkModel updWork)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sqlCheck = "SELECT id FROM public.teaching_work " +
                    "WHERE id = @ID";
                string sql = "UPDATE public.teaching_work " +
                    "SET teaching_plan=@TPL " +
                    "WHERE  id = @ID";

                db.Open();

                if (db.Query<string>(sqlCheck, new { ID = updWork.Id }).FirstOrDefault() == null)
                {
                    return false;
                }

                db.Execute(sql, new
                {
                    TPL = updWork.TeachingPlan,
                    ID = updWork.Id
                });

                return true;
            }
        }
    }
}
