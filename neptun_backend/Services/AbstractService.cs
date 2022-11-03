using neptun_backend.Entities;
using neptun_backend.UnitOfWork;

namespace neptun_backend.Services
{
    public class AbstractService
    {
        protected IUnitOfWork unitOfWork;

        public AbstractService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
    }
}
