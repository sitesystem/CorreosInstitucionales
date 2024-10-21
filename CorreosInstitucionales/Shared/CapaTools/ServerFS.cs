using System.IO.Compression;
using System.Text;

namespace CorreosInstitucionales.Shared.CapaTools
{
    public static class ServerFS
    {
        public const string Root = "wwwroot";

        public static string GetBaseDir(bool fullpath)
        {
            return fullpath ? Path.GetFullPath(Root) : string.Empty;
        }

        public static string ArchivoUsuario(int id_usuario, string archivo, bool ruta_absoluta = false)
        {
            return $"repositorio/usuarios/{id_usuario}/{id_usuario}_{archivo}";
        }

        public static string ArchivoRepositorio(int id_solicitud, string archivo, bool ruta_absoluta = false)
        {
            return $"repositorio/solicitudes-tickets/{id_solicitud}/{id_solicitud}_{archivo}";
        }

        public static bool SubirArchivo()
        {
            bool result = false;

            return result;
        }

        public static string? WriteZip(string filename, List<WebUtils.Link> files)
        {
            string z_name = string.Empty;
            string z_filename = string.Empty;
            string basedir = GetBaseDir(true);

            string? log = null;

            List<string> messages = []; // [$"PATH: {Path.GetFullPath(".")}"];

            using (FileStream fs = new($"{basedir}/{filename}", FileMode.CreateNew))
            {
                using (ZipArchive za = new(fs, ZipArchiveMode.Create, true))
                {
                    foreach (WebUtils.Link file in files)
                    {
                        z_filename = $"{basedir}/{file.Url}";

                        if (!File.Exists(z_filename))
                        {
                            messages.Add($"NO EXISTE EL ARCHIVO {z_filename}");
                            continue;
                        }

                        z_name = file.Name == "#" ? Path.GetFileName(file.Url) : file.Name;

                        za.CreateEntryFromFile(z_filename, z_name);

                        messages.Add($"{z_filename} -> {z_name}");
                    }

                    if (messages.Count > 0)
                    {
                        log = string.Join(Environment.NewLine, messages);

                        ZipArchiveEntry logfile = za.CreateEntry("mensajes.log");
                        using Stream stream = logfile.Open();
                        using StreamWriter sw = new(stream, Encoding.UTF8);
                        sw.Write(log);
                        sw.Flush();
                    }
                }

                fs.Position = 0;
            }

            return log;
        }
    }
}
