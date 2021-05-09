using System.ComponentModel.DataAnnotations;

namespace Course_work.Global_news
{
    public class NewsModel
    {
        [Key]
        public int Id { get; set; }

        public string Content { get; set; }

        public byte[] PictureBytes { get; set; }

        public string PictureName { get; set; }

        public byte[] DocBytes { get; set; }

        public string DocName { get; set; }
    }
}
