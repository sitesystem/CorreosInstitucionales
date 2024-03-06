using CorreosInstitucionales.Shared.Utils;
using System.IO.Compression;
using System.Text;

namespace CorreosInstitucionales.Shared.Utils
{
    public static class ServerFileSystem
    {
        public static string? WriteZip(string filename, List<WebUtils.Link> files, string root_directory = "")
        {
            string z_name = string.Empty;
            string z_filename = string.Empty;
            string? log = null;

            StringBuilder sb = new();
            bool guardar_log = false;

            using (FileStream fs = new FileStream(filename, FileMode.CreateNew))
            {
                using (ZipArchive za = new ZipArchive(fs, ZipArchiveMode.Create, true))
                {
                    foreach (WebUtils.Link file in files)
                    {
                        z_filename = $"{root_directory}{file.Url}";

                        if (!File.Exists(z_filename))
                        {
                            sb.AppendLine($"NO EXISTE EL ARCHIVO {z_filename}");
                            guardar_log = true;
                            continue;
                        }

                        z_name = file.Name == "#" ? Path.GetFileName(file.Url) : file.Name;

                        za.CreateEntryFromFile(z_filename, z_name);
                    }

                    if (guardar_log)
                    {
                        log = sb.ToString();

                        ZipArchiveEntry logfile = za.CreateEntry("log.txt");
                        using (Stream stream = logfile.Open())
                        {
                            using (StreamWriter sw = new StreamWriter(stream, Encoding.UTF8))
                            {
                                sw.Write(log);
                                sw.Flush();
                            }
                        }
                    }
                }

                fs.Position = 0;
            }

            return log;
        }
    }
}
