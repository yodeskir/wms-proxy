using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;
using wmsDataAccess.UserManagement.Entities;
using WMSDataAccess.UserManagement.Entities;
using wmsShared.Model;

namespace WMSDataAccess.UserManagement
{
    public interface IUserManager
    {
        WMSUser GetUser(string username);
        Task<bool> CreateUser(string username, string email, string fullname, string salt, string hash);
        bool CreateUserShema(string username);

        List<WMSMaps> GetUserMaps(string username);
        List<WMSLayers> GetUserLayers(string username);
        List<WMSFields> GetUserLayerFields(string username, string[] layername);
        Dictionary<string, string> GetNumericUserLayerFields(string username, string layername);
        WMSMaps GetMap(int mapid);
        Task<bool> CreateUserLayer(ImportViewModel importViewModel);

        Task<bool> DeleteUserLayerAsync(string username, string layername);
        //object GetMVTTile(IMemoryCache cache, string username, string layername, double x, double y, int z);
        WMSLayers GetUserLayer(string username, string layername);
        Task<int> UpdateMapAsync(WMSMaps value);
        Task<int> LogMapStateAsync(string username, string mapname, string mapcontent);
        Task<int> LatLonToGeomAsync(string username, string layername, string projection, string[] latlon);
    }
}