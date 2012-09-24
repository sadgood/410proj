using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using System.Diagnostics;


namespace TDPPM
{
    public class Compress
    {
        //利用SharpZipLib压缩
        /// <summary>
        /// 压缩文件夹，zip文件和文件夹同名
        /// </summary>
        /// <param name="dirPath">文件夹路径</param>
        /// <param name="zipFilePath">zip文件名，如果未空，则与文件夹同名</param>
        /// <returns></returns>
        public static bool ZipFileDictory(string dirPath,string zipFilePath)
        {
            string error = "";
            if (string.IsNullOrEmpty(dirPath))
            {
                error = "要压缩的文件夹不能为空";
                return false;
            }
            if (!Directory.Exists(dirPath))
            {
                error = "要压缩的文件夹不存在";
                return false;
            }
            //压缩文件名为空时使用文件夹名+.zip
            if (string.IsNullOrEmpty(zipFilePath))
            {
                if (dirPath.EndsWith("//"))
                {
                    dirPath = dirPath.Substring(0,dirPath.Length-1);
                }
                zipFilePath = dirPath + ".3dppm";
            }
            try
            {
                string[] fileNames = Directory.GetFiles(dirPath);
                using (ZipOutputStream s = new ZipOutputStream(File.Create(zipFilePath)))
                {
                    s.SetLevel(9);
                    byte[] buffer=new byte[4096];
                    foreach (string file in fileNames)
                    {
                        ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                        entry.DateTime = DateTime.Now;
                        s.PutNextEntry(entry);
                        using (FileStream fs = File.OpenRead(file))
                        {
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                s.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }
                    }
                    s.Finish();
                    s.Close();
                }
            }
            catch(Exception ex)
            {
                error = ex.Message;
                return false;
            }
            return true;
        }
        //利用SharpZipLib解压
        /// <summary>
        /// 解压zip文件,文件夹和zip文件同名
        /// </summary>
        /// <param name="zipFilePath">zip文件路径</param>
        /// <param name="unZipDir">解压文件存放路径,为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹</param>
        /// <returns></returns>
        public static bool UnZipFileDictory(string zipFilePath,string unZipDir)
        {
            string error = "";
            if (zipFilePath == string.Empty)
            {
                error = "压缩文件不能为空！";
                return false;
            }            

            try
            {
                if (!File.Exists(zipFilePath))
                {
                    error = "压缩文件不存在！";
                    return false;
                }
                ////解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹
                if (string.IsNullOrEmpty(unZipDir))
                {
                    unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
                }
                if (!unZipDir.EndsWith("//"))
                {
                    unZipDir += "//";
                }
                if (!Directory.Exists(unZipDir))
                {
                    Directory.CreateDirectory(unZipDir);
                }
                else
                {
                    Directory.Delete(unZipDir, true);
                    Directory.CreateDirectory(unZipDir);
                }

                using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
                {
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);
                        if (directoryName.Length > 0)
                        {
                            Directory.CreateDirectory(unZipDir+directoryName);
                        }
                        if (!directoryName.EndsWith("//"))
                        {
                            directoryName += "//";
                        }
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            using (FileStream streamWriter = File.Create(unZipDir + theEntry.Name))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }//while
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
            return true;
        }
        //利用rar.exe解压
        public static bool UnRAR(string RarEXE, string RarPatch, string UnRarPath)
        {
            bool flag = false;
            try
            {
                Directory.CreateDirectory(UnRarPath);
                String the_Info = "e \"" + RarPatch + "\" \"" + UnRarPath + "\" -y";
                ProcessStartInfo the_StartInfo = new ProcessStartInfo();
                the_StartInfo.FileName = RarEXE;
                the_StartInfo.Arguments = the_Info;
                the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                the_StartInfo.WorkingDirectory = "";   
                Process the_Process = new Process();
                the_Process.StartInfo = the_StartInfo;
                the_Process.Start();
                the_Process.WaitForExit();
                if (the_Process.HasExited)
                {
                    flag = true;
                }
                the_Process.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }
        //利用rar.exe压缩
        public static bool RAR(string RarEXE, string TargetRar, string FolderPath)
        {
            bool flag = false;
            try
            {
                String the_Info = "a -ep -y \"" + TargetRar + "\" \"" + FolderPath + "\\*\" -y";
                ProcessStartInfo the_StartInfo = new ProcessStartInfo();
                the_StartInfo.FileName = RarEXE;
                the_StartInfo.Arguments = the_Info;
                the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                the_StartInfo.WorkingDirectory = "";   
                Process the_Process = new Process();
                the_Process.StartInfo = the_StartInfo;
                the_Process.Start();
                the_Process.WaitForExit();
                if (the_Process.HasExited)
                {
                    flag = true;
                }
                the_Process.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

    }
}
