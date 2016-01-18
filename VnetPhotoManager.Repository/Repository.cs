using System.Data;
using VnetPhotoManager.Infrastructure;

namespace VnetPhotoManager.Repository
{
    public abstract class Repository<T>
    {
        protected string ConnectionString;

        protected Repository()
        {
            ConnectionString = AppConfiguration.ConnectionString;
        }
        protected abstract T BuildFromRecord(IDataRecord record);
    }
}
