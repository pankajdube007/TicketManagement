#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GoldTasks = System.Threading.Tasks;

#endregion Using

/// <summary>
/// Summary description for GoldMedia
/// </summary>
public class GoldMedia : IGoldMedia
{
    #region Initialization

    private readonly IMediaFileStore _mediaFileStore;

    #endregion Initialization

    #region Ctor

    public GoldMedia()
    {
        _mediaFileStore = BlobApi();
    }

    #endregion Ctor

    #region Method

    public Dictionary<bool, string> GoldMediaUpload(string oldFileName, string directoryName, string extName, Stream streamContent, string contentType, bool isSystemFileName = true, bool isSystemModifiledFileName = false, bool overWrite = false)
    {
        var rtnDic = new Dictionary<bool, string>();
        try
        {
            var _verifyAndCreateFileName = VerifyAndCreateFileName(oldFileName, directoryName, extName, isSystemFileName, isSystemModifiledFileName);

            if (_verifyAndCreateFileName.Keys.FirstOrDefault())
            {
                Upload(_verifyAndCreateFileName.Values.FirstOrDefault(), streamContent, contentType, overWrite);
                rtnDic.Add(true, _verifyAndCreateFileName.Values.FirstOrDefault());
            }
            else
            {
                rtnDic.Add(false, _verifyAndCreateFileName.Values.FirstOrDefault());
            }
        }
        catch (Exception ex)
        {
            rtnDic.Add(false, ex.Message);
        }
        return rtnDic;
    }

    public Dictionary<bool, string> GoldMediaDelete(string path)
    {
        var rtnDic = new Dictionary<bool, string>();
        try
        {
            if (Delete(path))
            {
                rtnDic.Add(true, "File Deleted Successfully.");
            }
            else
            {
                rtnDic.Add(false, "Unable to Delete the file.");
            }
        }
        catch (Exception ex)
        {
            rtnDic.Add(false, ex.Message);
        }
        return rtnDic;
    }

    public Stream GoldMediaDownload(string path)
    {
        Stream _stream = null;
        var _goldTask = GoldTasks.Task.Run(async () =>
        {
            try
            {
                _stream = await _mediaFileStore.GetFileStreamAsync(path);
            }
            catch (Exception ex)
            {
                throw new FileStoreException(string.Format("Error while getting the file in directory {0} : {1}.", path, ex.Message));
            }
        });
        _goldTask.Wait();

        return _stream;
    }

    public string MapPathToPublicUrl(string path)
    {
        return _mediaFileStore.MapPathToPublicUrl(path);
    }

    #endregion Method

    #region Utilities

    private Dictionary<bool, string> VerifyAndCreateFileName(string oldFileName, string directoryName, string extName, bool isSystemFileName, bool isSystemModifiledFileName)
    {
        string fileName = string.Empty, fileNameWithExt = string.Empty, filePathWithExt = string.Empty;
        var rtnStr = new Dictionary<bool, string>();

        if (isSystemFileName)
            fileName = Guid.NewGuid().ToString();
        else
            fileName = !isSystemModifiledFileName ? oldFileName : string.Format("{0}_{1}", oldFileName, DateTime.Now.ToString("dd_MMM_yyy_hh_mm_ss_fff"));

        fileNameWithExt = string.Format("{0}{1}", fileName, extName);
        filePathWithExt = string.Format("{0}/{1}", directoryName, fileNameWithExt);

        if (string.IsNullOrWhiteSpace(filePathWithExt))
            rtnStr.Add(false, "Given File Path should not be null or empty");
        else if (filePathWithExt.Length > 259)
            rtnStr.Add(false, "Oops!! Plz. change the file name. It's too long!.");
        else
            rtnStr.Add(true, filePathWithExt.ToLowerInvariant());

        return rtnStr;
    }

    private void Upload(string path, Stream stream, string contentType, bool overWrite)
    {
        // var _goldTask = GoldTasks.Task.Run(() =>
        //{
        try
        {
            _mediaFileStore.UploadFileFromStream(path.ToLowerInvariant(), stream, contentType, overWrite);
            //await _mediaFileStore.CreateFileFromStream(path.ToLowerInvariant(), stream, contentType, overWrite);
        }
        catch (Exception ex)
        {
            throw new FileStoreException(string.Format("Error while upload the file in directory {0} : {1}.", path, ex.Message));
        }
        //});
        // _goldTask.Wait();
    }

    private bool Delete(string path)
    {
        var result = false;
        var _goldTask = GoldTasks.Task.Run(async () =>
        {
            try
            {
                result = await _mediaFileStore.TryDeleteFileAsync(path.ToLowerInvariant());
            }
            catch (Exception ex)
            {
                throw new FileStoreException(string.Format("Error while deleting the file in directory {0} : {1}.", path, ex.Message));
            }
        });
        _goldTask.Wait();

        return result;
    }

    private static IMediaFileStore BlobApi()
    {
        var ai = BlobInitialization.Instance.GetMediaFileStore();
        return ai;
    }

    #endregion Utilities
}