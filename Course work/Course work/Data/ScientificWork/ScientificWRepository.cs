using System.Collections.Generic;
using System.Linq;
using Dapper;
using Npgsql;

namespace Course_work.ScientificWork
{
    public class ScientificWRepository : IScientificWRepository
    {
        readonly string connectionString =
            "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=Vsu;";

        //author id = site user id
        public IEnumerable<ScientificWorkModel> GetAllWorksByAuthorId(int id)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sql = "SELECT id AS Id, mentor_id AS MentorID, author_id AS Author_id, " +
                    "content AS Content, doc AS Document " +
                    "FROM public.scientific_work " +
                    "WHERE author_id = @ID";

                db.Open();
                return db.Query<ScientificWorkModel>(sql, new { ID = id });
            }
        }

        public ScientificWorkModel GetScientificWorkByWorkID(int id)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sql = "SELECT id AS Id, mentor_id AS MentorID, author_id AS Author_id, " +
                    "content AS Content, doc AS Document " +
                    "FROM public.scientific_work " +
                    "WHERE id = @ID";

                db.Open();
                return db.Query<ScientificWorkModel>(sql, new { ID = id }).FirstOrDefault();
            }
        }

        public bool AddWork(ScientificWorkModel work)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sql = "INSERT INTO public.scientific_work(mentor_id, author_id, content, doc) " +
                    "VALUES (@MENTID, @AUTID, @CONT, @DOC)";
                string sqlCheckAuthor = "SELECT id FROM public.site_user WHERE id = @ID";
                string sqlCheckMentor = "SELECT lecturer_id FROM public.lecturer WHERE lecturer_id = @ID";

                db.Open();

                if (db.Query<string>(sqlCheckAuthor, new { ID = work.AuthorId}).FirstOrDefault() == null)
                {
                    return false;
                }

                if (db.Query<string>(sqlCheckMentor, new { ID = work.MentorId }).FirstOrDefault() == null)
                {
                    return false;
                }

                db.Execute(sql, new
                {
                    MENTID = work.MentorId,
                    AUTID = work.AuthorId,
                    CONT = work.Content,
                    DOC = work.Document
                });

                return true;
            }
        }

        public bool UpdateWorkDocument(byte[] document, int workId)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sqlCheck = "SELECT id FROM public.scientific_work " +
                    "WHERE id = @ID";
                string sqlUpd = "UPDATE public.scientific_work " +
                    "SET doc=@DOC " +
                    "WHERE id = @ID";

                db.Open();
                if (db.Query<string>(sqlCheck, new { ID = workId }).FirstOrDefault() == null)
                {
                    return false;
                }

                db.Execute(sqlUpd, new { ID = workId, DOC = document });

                return true;
            }
        }
    }
}
