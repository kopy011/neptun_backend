using neptun_backend.Context;
using neptun_backend.Entities;
using neptun_backend.UnitOfWork;

namespace neptun_backend.Services
{
    public interface ISemesterService
    {
        IEnumerable<Semester> getAll();
    }

    public class SemesterService : AbstractService, ISemesterService
    {
        public SemesterService(IUnitOfWork unitOfWork): base(unitOfWork)
        {
        }

        public IEnumerable<Semester> getAll()
        {
            return unitOfWork.GetRepository<Semester>().GetAll();
        }
    }
}
