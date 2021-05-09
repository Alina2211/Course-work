using System.ComponentModel.DataAnnotations;

namespace Course_work.TeachingWork
{
    public class TeachingWorkModel
    {
        [Key]
        public int Id { get; set; }

        public int TeacherId { get; set; }

        public byte[] TeachingPlan { get; set; }
    }
}
