using System.Collections.Generic;

namespace Course_work.TeachingWork
{
    public interface ITeachingWRepository
    {

        public TeachingWorkModel GetTeachingWorkByTeachID(int id);

        public TeachingWorkModel GetTeachingWorkByWorkID(int id);

        public bool AddWork(TeachingWorkModel work);

        public bool UpdateWorkByWorkId(TeachingWorkModel updWork);
    }
}
