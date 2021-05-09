using System.Collections.Generic;

namespace Course_work.Student
{
    public interface IStudentRepository
    {
        public IEnumerable<StudentModel> GetAllStudents();

        public StudentModel GetStudentByStudId(int id);

        public bool AddStudent(StudentModel student);

        public bool UpdateStudent(StudentModel upStudent);
    }
}
