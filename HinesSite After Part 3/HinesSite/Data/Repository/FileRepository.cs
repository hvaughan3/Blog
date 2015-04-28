#region Usings

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HinesSite.Data.Context;
using HinesSite.Helpers;
using HinesSite.Interface;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
#pragma warning disable 1570

#endregion

namespace HinesSite.Data.Repository {

    /// <summary>
    /// Handles all file related tasks
    /// </summary>
    public class FileRepository : IFileRepository, IDisposable {

        #region Properties

        private ApplicationDbContext _dbContext;
        private ILogger              _log;
        private Stopwatch            _timespan;

        #endregion

        #region Constructor

        /// <summary>
        /// Main constructor which sets the context
        /// </summary>
        public FileRepository(IUnitOfWork unitOfWork, ILogger log) {

            if(unitOfWork == null)
                throw new ArgumentNullException("unitOfWork", "unitOfWork is null in the FileRepository");
            if(log == null)
                throw new ArgumentNullException("log", "log is null in the FileRepository");

            _dbContext = unitOfWork.DbContext;
            _log       = log;
        }

        #endregion

        #region IFileRepository

        /// <summary>
        /// Save a file to an Azure Blob container
        /// </summary>
        /// <param name="filesToUpload">The files to save</param>
        /// <returns>Task<string> being the files' URLs</returns>
        public async Task<string[]> UploadFilesAsync(HttpPostedFileBase[] images) {

            #region Properties

            int                 arrayIndex     = 0;
            string[]            fileUrls       = null;
                                _timespan      = Stopwatch.StartNew();
            CloudStorageAccount storageAccount = StorageUtils.StorageAccount;

            // Create blob client and reference the container
            CloudBlobClient     blobClient     = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer  container      = blobClient.GetContainerReference("images");

            #endregion

            try {

                foreach(HttpPostedFileBase file in images) {

                    fileUrls[arrayIndex] = await SaveFileAsync(file, container);
                    arrayIndex++;
                    #region Logging

                    _timespan.Stop();
                    _log.TraceApi("Blob Service", "FileRepository.UploadFileAsync", _timespan.Elapsed, "filepath={0}", fileUrls[arrayIndex]);
                    _timespan.Reset();

                    #endregion
                }
            }
            catch(Exception ex) {
                #region Error Logging

                _log.Error(ex, "Error uploading file blob to storage");

                #endregion
            }

            // URL to be stored in the DB
            return fileUrls;
        }

        /// <summary>
        /// Called on program startup in App_Start of Global.asax to create an Azure Blob container if it doesn't exsit
        /// </summary>
        public async void CreateAndConfigureAsync() {

            try {

                #region Properties

                CloudStorageAccount storageAccount = StorageUtils.StorageAccount;

                // Create a blobClient and retrieve reference to files container
                CloudBlobClient     blobClient     = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer  container      = blobClient.GetContainerReference("images");

                #endregion

                // Create the "images" container if it doesn't already exist
                if(await container.CreateIfNotExistsAsync()) {

                    /*
                     * //Enable public access on the newly create "images" container (Turning off for now, so it defaults to private)
                     * await container.SetPermissionsAsync(new BlobContainerPermissions {
                     *     PublicAccess = BlobContainerPublicAccessType.Blob
                     * });
                     *
                     * #region Logging
                     *
                     * _log.Information("Successfully created Blob Storage Images Container and made it public");
                     *
                     * #endregion
                     */
                }
            }
            catch(Exception ex) {
                #region Error Logging

                _log.Error(ex, "Failure to Create or Configure images container in Blob Storage Service");

                #endregion
            }
        }

        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// Disposes of the DB context
        /// </summary>
        protected void Dispose(bool disposing) {

            if(disposing) {

                if(_dbContext != null) {

                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }

        /// <summary>
        /// Used to dispose the DB context
        /// </summary>
        public void Dispose() {

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Saves the files to an Azure Container
        /// </summary>
        /// <param name="file">The file to be saved</param>
        /// <param name="container">The Blob Container to be used</param>
        /// <returns>Task<string> which is the file's URL</returns>
        private async Task<string> SaveFileAsync(HttpPostedFileBase file, CloudBlobContainer container) {

            #region Properties

            // Create a unique name for the images we are about to upload
            string         fileName          = string.Format("file-{0}{1}", Guid.NewGuid(), Path.GetExtension(file.FileName));
            // Upload image to Blog Storage
            CloudBlockBlob blockBlob         = container.GetBlockBlobReference(fileName);
            blockBlob.Properties.ContentType = file.ContentType;

            #endregion

            using(Stream fileStream = file.InputStream) {
                await blockBlob.UploadFromStreamAsync(fileStream);
            }

            /*
             * // All 3 of the below methods were shown in different places to create the final full path string
             * // #1
             * // Convert to be HTTP based URI (default storage path is HTTPS)
             * UriBuilder uriBuilder = new UriBuilder(blockBlob.Uri) { Scheme = "http" };
             * return uriBuilder.ToString();
             * // #2
             * return blockBlob.Uri.ToString();
             * // #3
             * return String.Format("http://{0}{1}", blockBlob.Uri.DnsSafeHost, blockBlob.Uri.AbsolutePath);
             */

            return blockBlob.Uri.ToString();
        }

        #endregion
    }
}
