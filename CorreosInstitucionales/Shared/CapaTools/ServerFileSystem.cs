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

            List<string> messages = [$"PATH: {Path.GetFullPath(".")}"];

            using (FileStream fs = new FileStream(filename, FileMode.CreateNew))
            {
                using (ZipArchive za = new ZipArchive(fs, ZipArchiveMode.Create, true))
                {
                    foreach (WebUtils.Link file in files)
                    {
                        z_filename = $"{root_directory}{file.Url}";

                        if (!File.Exists(z_filename))
                        {
                            messages.Add($"NO EXISTE EL ARCHIVO {z_filename}");
                            continue;
                        }

                        z_name = file.Name == "#" ? Path.GetFileName(file.Url) : file.Name;

                        za.CreateEntryFromFile(z_filename, z_name);
                    }

                    if (messages.Count > 0)
                    {
                        log = string.Join(Environment.NewLine, messages);

                        ZipArchiveEntry logfile = za.CreateEntry("mensajes.log");
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
