#region Usings

using System;
using HinesSite.Data.Context;
using HinesSite.Data.Repository;
using HinesSite.Models;

#endregion

namespace HinesSite.Interface {

    /// <summary>
    /// UnitOfWork Interface class
    /// </summary>
    public interface IUnitOfWork : IDisposable {

        /// <summary>
        /// DB context
        /// </summary>
        ApplicationDbContext DbContext { get; }

        /// <summary>
        /// Method for saving to the DB using the context
        /// </summary>
        void Save();

    }
}