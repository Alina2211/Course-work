using System.ComponentModel.DataAnnotations;

namespace Course_work.Lecturer
{
    public class WallMessageModel
    {
        [Key]
        public int Id { get; set; }

        public int LecturerId { get; set; }

        //ссылка на яндекс диск (н-р), какая-либо новость для студентов
        public string Content { get; set; }
    }
}
