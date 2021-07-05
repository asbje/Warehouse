using Renci.SshNet;

namespace Warehouse.Modules.OutlookBookings.Service
{
    public class FTPClientHelper : SftpClient
    {
        private static string _path;

        public string Path { get { return _path; } }

        private static string _host;
        public string Host { get { return _host; } }


        public FTPClientHelper(string connectionString) : base(CreateConnectionInfo(connectionString))
        {

        }

        private static ConnectionInfo CreateConnectionInfo(string connectionString)
        {
            var user = "";
            var pass = "";
            int port = 0;
            foreach (var item in connectionString.Split(';'))
            {
                var pair = item.Split('=');
                if (pair.Length == 2)
                {
                    switch (pair[0].ToLower())
                    {
                        case "host": _host = pair[1]; break;
                        case "user": user = pair[1]; break;
                        case "pass": pass = pair[1]; break;
                        case "port": int.TryParse(pair[1], out port); break;
                        case "path": _path = pair[1]; break;
                    }
                }
            }

            if (user != null && pass != null)
                if (port > 0)
                    return new ConnectionInfo(_host, port, user, new PasswordAuthenticationMethod(user, pass));
                else
                    return new ConnectionInfo(_host, user, new PasswordAuthenticationMethod(user, pass));

            return default;
        }
    }
}
