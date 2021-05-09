using System.Collections.Generic;
using System.Linq;
using Dapper;
using Npgsql;

namespace Course_work.Lecturer
{
    public class WallMessageRepository : IWallMessageRepository
    {
        readonly string connectionString =
            "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=Vsu;";

        public IEnumerable<WallMessageModel> GetAllPosts(int lecturerId)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sql = "SELECT lecturer_id AS LecturerId, news_id AS Id, " +
                    "content AS Content " +
                    "FROM public.lecturer_news " +
                    "WHERE lecturer_id = @LID";

                db.Open();

                return db.Query<WallMessageModel>(sql, new { LID = lecturerId });
            }
        }

        public IEnumerable<WallMessageModel> GetAllPostsBetween(int fromId, int toId)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                
                string sqlGet = "SELECT lecturer_id AS LecturerId, news_id AS Id, " +
                    "content AS Content " +
                    "FROM public.lecturer_news " +
                    "WHERE news_id BETWEEN @FIRST AND @LAST";

                db.Open();

                return db.Query<WallMessageModel>(sqlGet, new
                {
                    FIRST = fromId,
                    LAST = toId
                });
            }
        }

        public IEnumerable<WallMessageModel> GetLastFivePosts(int lecturerId)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sqlMaxId = "SELECT MAX(news_id) FROM public.lecturer_news " +
                    "WHERE lecturer_id = @LID";
                string sqlGet = "SELECT lecturer_id AS LecturerId, news_id AS Id, " +
                    "content AS Content " +
                    "FROM public.lecturer_news " +
                    "WHERE lecturer_id = @LID AND news_id BETWEEN @FIRST AND @LAST";

                db.Open();

                int maxId = db.Query<int>(sqlMaxId, new { LID = lecturerId }).FirstOrDefault();

                if (maxId <= 5)
                {
                    return GetAllPosts(lecturerId);
                } 
                else
                {
                    return db.Query<WallMessageModel>(sqlGet, new
                    {
                        LID = lecturerId,
                        FIRST = maxId - 5,
                        LAST = maxId
                    });
                }
            }
        }

        public bool AddPost(WallMessageModel news)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sqlAdd = "INSERT INTO public.lecturer_news(lecturer_id,  content) " +
                    "VALUES (@LID, @CONT)";

                db.Open();

                db.Execute(sqlAdd, new { LID = news.LecturerId, CONT = news.Content });

                return true;
            }
        }

        public bool DeletePost(int id)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sqlCheck = "SELECT news_id FROM public.lecturer_news " +
                    "WHERE news_id = @NID";
                string sqlDelete = "DELETE FROM public.lecturer_news " +
                    "WHERE news_id = @NID";

                db.Open();

                if (db.Query<string>(sqlCheck, new { NID = id }) == null) return false;

                db.Execute(sqlDelete, new { NID = id });
                return true;
            }
        }
    }
}
