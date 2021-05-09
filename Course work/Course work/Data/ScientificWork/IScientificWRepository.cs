using System.Collections.Generic;

namespace Course_work.ScientificWork
{
    public interface IScientificWRepository
    {
        public IEnumerable<ScientificWorkModel> GetAllWorksByAuthorId(int id);

        public ScientificWorkModel GetScientificWorkByWorkID(int id);

        public bool AddWork(ScientificWorkModel work);

        public bool UpdateWorkDocument(byte[] document, int workId);
    }
}
