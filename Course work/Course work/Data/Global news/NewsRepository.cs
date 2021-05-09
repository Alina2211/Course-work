
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Npgsql;

namespace Course_work.Global_news
{
    public class NewsRepository : INewsRepository
    {

        readonly string connectionString =
           "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=Vsu;";

        public IEnumerable<NewsModel> GetAllNews()
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
               string sql = "SELECT id AS Id, content AS Content, " +
                    "picture AS PictureBytes, doc AS DocBytes, " +
                    "picture_name AS PictureName, doc_name AS DocName " +
                    "FROM public.global_news";

               db.Open();
               return db.Query<NewsModel>(sql);
            }
        }

        public bool AddNews(NewsModel news)
        {

            using (var db = new NpgsqlConnection(connectionString))
            {
                string sql1 = "INSERT INTO public.global_news(content, picture, doc, picture_name, doc_name) " +
                    "VALUES(@CN, @PIC, @DOC, @PICN, @DOCN)";
                string sql2 = "INSERT INTO public.global_news(content, doc, doc_name) " +
                    "VALUES(@CN, @DOC, @DOCN)";
                string sql3 = "INSERT INTO public.global_news(content, picture, picture_name) " +
                    "VALUES(@CN, @PIC,@PICN)";
                string sql4 = "INSERT INTO public.global_news(content) " +
                    " VALUES(@CON) ";



                if (news.DocName == null && news.PictureName == null)
                {
                    db.Open();
                    db.Execute(sql4, new { CON = news.Content });
                    return true;
                }
                else if (news.DocName == null && news.PictureName != null)
                {
                    db.Open();
                    db.Execute(sql3, new { CON = news.Content, PIC = news.PictureBytes, PICN = news.PictureName });
                    return true;
                }
                else if (news.DocName != null && news.PictureName == null)
                {
                    db.Open();
                    db.Execute(sql2, new { CON = news.Content, DOC = news.DocBytes, DOCN = news.DocName });
                    return true;
                }
                else if (news.DocName != null && news.PictureName != null)
                {
                    db.Open();
                    db.Execute(sql1,
                        new
                        {
                            CON = news.Content,
                            DOC = news.DocBytes,
                            DOCN = news.DocName,
                            PIC = news.PictureBytes,
                            PICN = news.PictureName
                        });
                    return true;
                }
                else return false;

            }
        }

        public IEnumerable<NewsModel> GetAllNewsBetween(int fromId, int toId)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sql = "SELECT id AS Id, content AS Content, " +
                    "picture AS PictureBytes, doc AS DocBytes, " +
                    "picture_name AS PictureName, doc_name AS DocName " +
                    "FROM public.global_news " +
                    "WHERE id BETWEEN @FIRST AND @LAST";

                db.Open();
                return db.Query<NewsModel>(sql, new { FIRST = fromId, LAST = toId });
            }
        }

        public IEnumerable<NewsModel> GetLastTenNews()
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
              
                string sql1 = "SELECT MAX(id) FROM public.global_news";

                int lastId = db.Query<int>(sql1).FirstOrDefault();

                int firstId = lastId <= 10 ? firstId = 1 : lastId - 10;

               
                db.Open();
                return GetAllNewsBetween(firstId, lastId);
            }
        }

        public bool DeleteNews(int id)
        {
            using (var db = new NpgsqlConnection(connectionString))
            {
                string sql1 = "SELECT id FROM public.global_news WHERE id = @ID";
                string sql2 = "DELETE FROM public.global_news WHERE id = @ID";


                db.Open();

                string check = db.Query<string>(sql1, new { ID = id }).FirstOrDefault();

                if (db.Query<string>(sql1, new { ID = id }).FirstOrDefault() == null)
                {
                    return false;
                }

                db.Execute(sql2, new { ID = id });

                return true;
            }
        }
    }
}
