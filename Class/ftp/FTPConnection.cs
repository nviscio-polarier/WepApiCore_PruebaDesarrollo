using FluentFTP;
using System.Diagnostics;
using System.Globalization;
using System.Net;

namespace WebApiCore.Class.ftp
{
    public enum FTPUser
    {
        files,
        amb,
        timbradomx
    }

    public class FTPConnection
    {


        private string URI { get; set; }
        private string user { get; set; }
        private string password { get; set; }

        public FTPConnection(FTPUser user = FTPUser.files, string path = "")
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build().GetSection("FTP_SAP");

            URI = config.GetSection("URI").Value;

            var appSettingsUser = config.GetSection("USERS").GetSection(user.ToString());
            this.user = appSettingsUser.GetSection("user").Value;
            password = appSettingsUser.GetSection("password").Value;
            URI += appSettingsUser.GetSection("path").Value + path;
        }

        static public async Task<AsyncFtpClient> CreateClientAndConnect(FTPUser user = FTPUser.files, string path = "") 
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build().GetSection("FTP_SAP");

            var host = config.GetSection("HOST").Value;
            var port = int.Parse(config.GetSection("PORT").Value);

            var appSettingsUser = config.GetSection("USERS").GetSection(user.ToString());
            var userString = appSettingsUser.GetSection("user").Value;
            var password = appSettingsUser.GetSection("password").Value;

            var client = new AsyncFtpClient(host, userString, password, port);
            await client.AutoConnect();

            if (!string.IsNullOrEmpty(path))
                await client.SetWorkingDirectory(path);

            return client;
        }

        public async Task<FtpWebResponse> UploadFile(string address, byte[] file)
        {
            try
            {
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(URI + address);

                ftpRequest.Credentials = new NetworkCredential(user, password);
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;

                using (Stream sw = ftpRequest.GetRequestStream())
                {
                    sw.Write(file, 0, file.Length);
                }

                return (FtpWebResponse)ftpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                throw new Exception("Error al subir el archivo al servidor FTP", ex);
            }
        }

        public async Task<FtpWebResponse> CreateDirectory(string address)
        {
            try
            {
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(URI + address);

                ftpRequest.Credentials = new NetworkCredential(user, password);
                ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;

                return (FtpWebResponse)ftpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                throw new Exception("Error al crear el directorio en el servidor FTP", ex);
            }
        }

        public async Task<FtpWebResponse> List(string address = "")
        {
            try
            {
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(URI + address);

                ftpRequest.Credentials = new NetworkCredential(user, password);
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                return (FtpWebResponse)ftpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                throw new Exception("Error al listar directorio en el servidor FTP", ex);
            }
        }

        public async Task<List<ListItem>> ListAndSerialize(string address = "")
        {
            try
            {
                var listFTP = await List(address);
                Stream responseStream = listFTP.GetResponseStream();
                StreamReader reader = new(responseStream);
                List<ListItem> files = new List<ListItem>();

                string line = reader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {

                    var data = line.Split(new char[] { ' ' }, 9, StringSplitOptions.RemoveEmptyEntries);

                    files.Add(new ListItem
                    {
                        Type = data[0][0] == 'd' ? ListItemType.Directory : ListItemType.File,
                        Permissions = data[0],
                        Owner = data[2],
                        Group = data[3],
                        Size = data[4],
                        LastModified = DateTime.TryParseExact($"{data[5]} {data[6]} {data[7]}", "MMM dd HH:mm", new CultureInfo("es-ES"), DateTimeStyles.None, out DateTime parsedDate) ? parsedDate : null,
                        Name = data[8],
                    });
                    line = reader.ReadLine();
                }

                reader.Close();

                return files;
            }
            catch (WebException ex)
            {
                throw new Exception("Error al listar directorio en el servidor FTP", ex);
            }
        }
    }
}
