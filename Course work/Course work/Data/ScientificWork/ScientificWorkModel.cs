using System.ComponentModel.DataAnnotations;

namespace Course_work.ScientificWork
{
    public class ScientificWorkModel
    {
        [Key]
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public int MentorId { get; set; }

        public string Content { get; set; }

        public byte[] Document { get; set; }
    }
}
