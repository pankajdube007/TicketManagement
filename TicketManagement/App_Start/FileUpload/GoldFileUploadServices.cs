#region using

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

#endregion using

/// <summary>
/// Summary description for GoldFileUploadServices
/// </summary>
public class GoldFileUploadServices
{
    #region fields

    private string GOLDUSER = string.Empty;
    private string BASEDIRECTORY = string.Empty;
    private string DirectoryName = string.Empty;
    private Dictionary<bool, string> retStr = null;

    #endregion fields

    #region Ctor

    public GoldFileUploadServices()
    {
        BASEDIRECTORY = ConfigurationManager.AppSettings["BaseDirctory"];
        GOLDUSER = '@' + ConfigurationManager.AppSettings["BaseUser"];
        retStr = new Dictionary<bool, string>();
    }

    #endregion Ctor

    #region Methods

    /// <summary>
    /// Upload a file to Gold directory
    /// </summary>
    /// <param name="goldFileUploadArgs"></param>
    /// <returns></returns>
    public Dictionary<bool, string> GoldFileUpload(GoldFileUploadArgs goldFileUploadArgs)
    {
        retStr.Clear();
        DirectoryName = Path.Combine(BASEDIRECTORY, goldFileUploadArgs.FolderName);

        if (!string.IsNullOrWhiteSpace(DirectoryName))
        {
            //var directoryPermission = SetDirectoryFilesRights(BASEDIRECTORY);
            //if (directoryPermission == null)
            //{
            //    retStr.Add(false, "Unable to create proper permmision to create a directory");
            //    return retStr;
            //}
            //if (!directoryPermission.Keys.FirstOrDefault())
            //    return directoryPermission;

            retStr.Clear();
            if (Directory.Exists(DirectoryName))
                retStr = FileUpload(new FileUploadArgs(goldFileUploadArgs.FileName, DirectoryName, goldFileUploadArgs.ExtName, goldFileUploadArgs.IsSystemFileName, goldFileUploadArgs.IsSystemModifiledFileName, goldFileUploadArgs.StreamToUpload));
            else
            {
                Directory.CreateDirectory(DirectoryName);
                retStr = FileUpload(new FileUploadArgs(goldFileUploadArgs.FileName, DirectoryName, goldFileUploadArgs.ExtName, goldFileUploadArgs.IsSystemFileName, goldFileUploadArgs.IsSystemModifiledFileName, goldFileUploadArgs.StreamToUpload));
            }
        }
        else
        {
            retStr.Add(false, "Given Directory should not be null or empty");
        }
        return retStr;
    }

    #endregion Methods

    #region Properties

    /// <summary>
    /// File upload agruments
    /// </summary>
    private struct FileUploadArgs
    {
        private string _FileName;
        private string _DirectoryName;
        private string _ExtName;
        private bool _IsSystemFileName;
        private bool _IsSystemModifiledFileName;
        private Stream _StreamContent;

        /// <summary>
        /// Summary for FileUploadArgs
        /// </summary>
        public FileUploadArgs(string fileName, string directoryName, string extName, bool isSystemFileName, bool isSystemModifiledFileName, Stream streamContent)
        {
            _FileName = fileName;
            _DirectoryName = directoryName;
            _ExtName = extName;
            _IsSystemFileName = isSystemFileName;
            _IsSystemModifiledFileName = isSystemModifiledFileName;
            _StreamContent = streamContent;
        }

        public string FileName
        {
            get
            {
                return _FileName;
            }
        }

        public string DirectoryName
        {
            get
            {
                return _DirectoryName;
            }
        }

        public string ExtName
        {
            get
            {
                return _ExtName;
            }
        }

        public bool IsSystemFileName
        {
            get
            {
                return _IsSystemFileName;
            }
        }

        public bool IsSystemModifiledFileName
        {
            get
            {
                return _IsSystemModifiledFileName;
            }
        }

        public Stream StreamContent
        {
            get
            {
                return _StreamContent;
            }
        }
    }

    #endregion Properties

    #region Utilities

    /// <summary>
    /// Add the Read/Write permission to the given directory Path
    /// </summary>
    /// <param name="directoryPathName">directory Full Path</param>
    /// <returns>true/false as Dictionary<bool, string> with proper message</returns>
    private Dictionary<bool, string> SetDirectoryFilesRights(string directoryPathName)
    {
        bool isCurrentUserRightsExists = false;
        retStr.Clear();
        try
        {
            var directoryInfo = new DirectoryInfo(directoryPathName);
            var directorySecurity = directoryInfo.GetAccessControl();
            var accsssRule = directorySecurity.GetAccessRules(true, true, typeof(NTAccount));
            var currentUserIdentity = WindowsIdentity.GetCurrent();

            foreach (AuthorizationRule rule in accsssRule)
            {
                if (rule.IdentityReference.Value.Equals(currentUserIdentity.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    if ((((FileSystemAccessRule)rule).FileSystemRights & FileSystemRights.Read) > 0 && (((FileSystemAccessRule)rule).FileSystemRights & FileSystemRights.Write) > 0)
                    {
                        isCurrentUserRightsExists = true;
                        retStr.Add(true, "Directory exist with proper permission.");
                        break;
                    }
                }
            }
            if (!isCurrentUserRightsExists)
            {
                var fileSystemRule = new FileSystemAccessRule(currentUserIdentity.Name,
                                                  FileSystemRights.Read | FileSystemRights.Write,
                                                  InheritanceFlags.ObjectInherit |
                                                  InheritanceFlags.ContainerInherit,
                                                  PropagationFlags.None,
                                                  AccessControlType.Allow);

                directorySecurity.AddAccessRule(fileSystemRule);
                directoryInfo.SetAccessControl(directorySecurity);
                retStr.Add(true, "Successfully rights added");
            }
        }
        catch (Exception ex)
        {
            retStr.Add(false, "Oops! Error,When creating permission to the directory.");
        }
        return retStr;
    }

    /// <summary>
    /// upload sream data to server
    /// </summary>
    /// <param name="fileUploadArgs"></param>
    /// <returns>true/false as Dictionary<bool, string> with proper message</returns>
    private Dictionary<bool, string> FileUpload(FileUploadArgs fileUploadArgs)
    {
        string FileName = string.Empty, FileNameWithExt = string.Empty, FilePathWithExt = string.Empty;
        retStr.Clear();

        if (!fileUploadArgs.IsSystemFileName)
            FileName = !fileUploadArgs.IsSystemModifiledFileName ? fileUploadArgs.FileName : string.Format("{0}_{1}", fileUploadArgs.FileName, DateTime.Now.ToString("dd_MMM_yyy_hh_mm_ss_fff"));
        else
            FileName = Guid.NewGuid().ToString();

        FileNameWithExt = string.Format("{0}{1}", FileName, fileUploadArgs.ExtName);
        FilePathWithExt = string.Format("{0}/{1}", fileUploadArgs.DirectoryName, FileNameWithExt);

        if (string.IsNullOrWhiteSpace(FilePathWithExt))
            retStr.Add(false, "Given File Path should not be null or empty");
        else if (FilePathWithExt.Length > 259)
            retStr.Add(false, "Oops!! Plz. change the file name. It's too long!.");
        else if (File.Exists(FilePathWithExt))
            retStr.Add(false, "Given File already exist in the directory");
        else
        {
            int bytesRead = 0;
            const int length = 256;
            byte[] buffer = new byte[length];
            try
            {
                using (var fs = new FileStream(FilePathWithExt, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (var stream = fileUploadArgs.StreamContent)
                    {
                        do
                        {
                            bytesRead = stream.Read(buffer, 0, length);
                            //fs.Write(buffer, 0, bytesRead);
                            var t = System.Threading.Tasks.Task.Factory.FromAsync(fs.BeginWrite, fs.EndWrite, buffer, 0, bytesRead, null);
                            t.Wait();
                        }
                        while (bytesRead == length);

                        stream.Flush();
                        stream.Close();
                    }
                    fs.Flush();
                    fs.Close();
                }
                retStr.Add(true, string.Format("{0}~{1}", FileNameWithExt, FilePathWithExt));
            }
            catch (Exception)
            {
                retStr.Add(false, "Unable to write the file into directory!.");
            }
        }
        return retStr;
    }

    #endregion Utilities
}