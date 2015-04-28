#region Usings



#endregion
// ReSharper disable MissingXmlDoc

using System.Threading.Tasks;
using System.Web;

namespace HinesSite.Interface {

    public interface IFileRepository {

        Task<string[]> UploadFilesAsync(HttpPostedFileBase[] images);
        void CreateAndConfigureAsync();
    }
}
