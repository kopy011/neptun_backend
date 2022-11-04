using neptun_backend.Context;
using neptun_backend.Entities;
using neptun_backend.UnitOfWork;

namespace neptun_backend.Services
{
    public interface ISemesterService: IAbstractService<Semester>
    {
    }

    public class SemesterService : AbstractService<Semester>, ISemesterService
    {
        public SemesterService(IUnitOfWork unitOfWork): base(unitOfWork)
        {
        }
    }
}
