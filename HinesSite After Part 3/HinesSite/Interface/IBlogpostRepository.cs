#region Usings

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using HinesSite.Models;

#endregion
// ReSharper disable MissingXmlDoc

namespace HinesSite.Interface {

    public interface IBlogpostRepository {

        IEnumerable<Blogpost> GetBlogposts();
        Task<Blogpost> GetBlogpostById(int? blogpostId);

        Task SetRowVersion(Blogpost blogpostToUpdate, byte[] rowVersion);

        Task InsertBlogpost(Blogpost blogpost, IEnumerable<HttpPostedFileBase> images);
        Task UpdateBlogpost(Blogpost blogpost);
        Task DeleteBlogpost(Blogpost blogpost);

        Task Save();
    }
}