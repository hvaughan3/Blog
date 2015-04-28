#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HinesSite.Models;

#endregion
// ReSharper disable MissingXmlDoc

namespace HinesSite.Interface {

    public interface ITagRepository {

        Task<List<Tag>> GetTags(Func<IQueryable<Tag>, IQueryable<Tag>> orderBy = null);
        Task<Tag> GetTagById(int? tagId);

        Task SetRowVersion(Tag tagToUpdate, byte[] rowVersion);

        Task InsertTag(Tag tag);
        Task UpdateTag(Tag tag);
        Task DeleteTag(Tag tag);

        Task Save();
    }
}