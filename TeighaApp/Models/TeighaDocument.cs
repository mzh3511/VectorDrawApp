using System;
using Teigha.DatabaseServices;
using Teigha.Runtime;

namespace Ranplan.iBuilding.TeighaApp.Models
{
    public class TeighaDocument : IDisposable
    {
        //private static Services _srvs;
        //private static HostApplicationServices _appsrv;

        public Database Database { get; private set; }

        public TeighaDocument()
        {
            //if (_srvs == null)
            //    _srvs = new Services();
            //if (_appsrv == null)
            //    _appsrv = new AppServices.HostAppServ();
        }

        public bool Load(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                if (Database != null)
                {
                    Database.Dispose();
                    Database = null;
                }
                return false;
            }
            try
            {
                Database = new Database(false, false);
                Database.ReadDwgFile(filePath, FileOpenMode.OpenForReadAndAllShare, true, "");
                Database.CloseInput(true);
                return true;
            }
            catch (System.Exception ex)
            {
                Database = null;
                return false;
            }
        }

        public void Dispose()
        {
            if (Database != null)
            {
                Database.Dispose();
                Database = null;
            }
            //if (_srvs != null)
            //{
            //    _srvs.Dispose();
            //    _srvs = null;
            //}
            //if (_appsrv != null)
            //{
            //    _appsrv.Dispose();
            //    _appsrv = null;
            //}
        }
    }
}