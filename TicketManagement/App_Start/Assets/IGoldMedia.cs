using System.Collections.Generic;
using System.IO;

/// <summary>
/// Summary description for IGoldMedia
/// </summary>
public interface IGoldMedia
{
    /// <summary>
    /// Upload file Stream to the directory <see cref="directoryName"/> in  Storage Media
    /// </summary>
    /// <param name="oldFileName">old FileName</param>
    /// <param name="directoryName">directory Name to upload</param>
    /// <param name="extName">File extension Name</param>
    /// <param name="streamContent">File Content stream</param>
    /// <param name="contentType">File contentType</param>
    /// <param name="isSystemFileName">File Name should be decided by System or not</param>
    /// <param name="isSystemModifiledFileName">File Name should be modified by System or not</param>
    /// <param name="overWrite">if File exists in given directory overWrite enable by System or not</param>
    /// <returns>Dictionary of (true, "Uploaded successfully") or (false, "Exception Message")</returns>
    Dictionary<bool, string> GoldMediaUpload(string oldFileName, string directoryName, string extName, Stream streamContent, string contentType, bool isSystemFileName = true, bool isSystemModifiledFileName = false, bool overWrite = false);

    /// <summary>
    /// Delete the file in given path in storage media
    /// </summary>
    /// <param name="path">Path</param>
    /// <returns>Dictionary of (true, "Uploaded successfully") or (false, "Exception Message")</returns>
    Dictionary<bool, string> GoldMediaDelete(string path);

    Stream GoldMediaDownload(string path);

    string MapPathToPublicUrl(string path);
}