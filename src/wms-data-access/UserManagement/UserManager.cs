using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wmsDataAccess.UserManagement.Entities;
using WMSDataAccess.UserManagement.DBContexts;
using WMSDataAccess.UserManagement.Entities;
using wmsShared.Model;

namespace WMSDataAccess.UserManagement
{
    public class UserManager : IUserManager
    {
        private readonly IUserDBContextFactory _userDBContextFactory;
        public UserManager(IUserDBContextFactory userDBContextFactory)
        {
            _userDBContextFactory = userDBContextFactory ?? throw new ArgumentNullException(nameof(userDBContextFactory));

        }

        public async Task<bool> CreateUser(string username, string email, string fullname, string clientname, string clientid, string salt, string hash)
        {
            using (var userContext = _userDBContextFactory.Create())
            {
                var wmsUser = new WMSUser
                {
                    username = username,
                    useremail = email,
                    userfullname = fullname,
                    salt = salt,
                    hashedpassword = hash
                };
                userContext.Users.Add(wmsUser);
                await userContext.SaveChangesAsync();
                CreateUserShema(username);
            }
            return true;
        }

        public bool CreateUserShema(string username)
        {

            using (var userContext = _userDBContextFactory.Create())
            using (var command = userContext.Database.GetDbConnection().CreateCommand())
            {
                using (var transaction = userContext.Database.BeginTransaction())
                {
                    try
                    {
                        command.CommandText = $"CREATE SCHEMA IF NOT EXISTS {username}";
                        userContext.Database.OpenConnection();
                        var result = command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }

            return true;
        }

        public WMSUser GetUser(string username)
        {
            using (var userContext = _userDBContextFactory.Create())
            {
                return userContext.Users.FirstOrDefault(u => u.username == username);
            }
        }

        
        public async Task<bool> CreateUserLayer(ImportViewModel importViewModel)
        {
            using (var userContext = _userDBContextFactory.Create())
            {
                var wmslayer = new WMSLayers
                {
                    username = importViewModel.UserName,
                    layername = importViewModel.LayerName,
                    datasourcename = importViewModel.DatasourceName,
                    layertype = importViewModel.UploadedLayerType.ToString(),
                    description = importViewModel.LayerDescription,
                    projection = importViewModel.Projection,
                    extent = importViewModel.Extent,
                    ispublic = bool.Parse(importViewModel.IsPublic)
                };
                userContext.wmsLayers.Add(wmslayer);

                await userContext.SaveChangesAsync();
            }
            return true;
        }

        public List<WMSLayers> GetUserLayers(string username)
        {
            using (var userContext = _userDBContextFactory.Create())
            {
                return userContext.wmsLayers.Where(u => u.username == username).ToList();
            }
        }

        public List<WMSMaps> GetUserMaps(string username)
        {
            using (var userContext = _userDBContextFactory.Create())
            {
                return userContext.Maps.Where(u => u.username == username).ToList();
            }
        }

        public WMSMaps GetMap(int mapid)
        {
            using (var userContext = _userDBContextFactory.Create())
            {

                return userContext.Maps.Where(l => l.id == mapid)?.FirstOrDefault();
            }
        }

        public List<WMSFields> GetUserLayerFields(string username, string[] layername)
        {
            var ret = new List<WMSFields>();
            using (var userContext = _userDBContextFactory.Create())
            using (var command = userContext.Database.GetDbConnection().CreateCommand())
            {
                var sb = new StringBuilder();
                foreach (var l in layername)
                {
                    sb.Append($"SELECT '{l}', column_name FROM information_schema.columns WHERE table_schema = '{username}'  AND lower(table_name) = '{l.ToLower()}' and column_name!='geom'");
                    sb.Append("UNION ALL ");
                }
                sb.Remove(sb.Length - 10, 10);
                command.CommandText = sb.ToString();
                userContext.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        var wmsField = new WMSFields();
                        wmsField.layername = result[0].ToString();
                        wmsField.field = result.GetString(result.GetOrdinal("column_name"));
                        ret.Add(wmsField);
                    }
                }
            }
            return ret;
        }

        public Dictionary<string, string> GetNumericUserLayerFields(string username, string layername)
        {
            var ret = new Dictionary<string, string>();
            using (var userContext = _userDBContextFactory.Create())
            using (var command = userContext.Database.GetDbConnection().CreateCommand())
            {
                var sb = $"SELECT * FROM {username}.{layername} LIMIT 1";

                command.CommandText = sb;
                userContext.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        for(var i=0;i<result.FieldCount;i++)
                        {
                            ret.Add(result.GetName(i), result.GetValue(i).ToString());
                        }
                        
                    }
                }
            }
            return ret;
        }

        public async Task<bool> DeleteUserLayerAsync(string username, string layername)
        {

            using (var userContext = _userDBContextFactory.Create())
            using (var command = userContext.Database.GetDbConnection().CreateCommand())
            {
                using (var transaction = userContext.Database.BeginTransaction())
                {
                    try
                    {
                        var wmslayer = userContext.wmsLayers.FirstOrDefault(u => u.username == username && u.layername == layername);
                        userContext.wmsLayers.Remove(wmslayer);
                        await userContext.SaveChangesAsync();

                        command.CommandText = $"DROP TABLE IF EXISTS {username}.{wmslayer.datasourcename}";
                        userContext.Database.OpenConnection();
                        var result = command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }

            return true;
        }

        public async Task<bool> CreateUser(string username, string email, string fullname, string salt, string hash)
        {
            using (var userContext = _userDBContextFactory.Create())
            {
                var wmuser = new WMSUser();
                wmuser.username = username;
                wmuser.useremail = email;
                wmuser.userfullname = fullname;
                wmuser.salt = salt;
                wmuser.hashedpassword = hash;
                userContext.Users.Add(wmuser);
                await userContext.SaveChangesAsync();
            }
            return true;
        }

        public WMSLayers GetUserLayer(string username, string layername)
        {
            using (var userContext = _userDBContextFactory.Create())
            {
                return userContext.wmsLayers.FirstOrDefault(u => u.username == username && u.layername == layername);
            }
        }

        public async Task<int> UpdateMapAsync(WMSMaps value)
        {
            using (var userContext = _userDBContextFactory.Create())
            {
                var map = userContext.Maps.FirstOrDefault(m => m.id == value.id);
                map.center = value.center;
                map.zoom = value.zoom;
                return await userContext.SaveChangesAsync();
            }
        }

        public async Task<int> LogMapStateAsync(string username, string mapname, string mapcontent)
        {
            using (var userContext = _userDBContextFactory.Create())
            {
                var maplogs = userContext.MapsLog.Where(l=>l.username.Equals(username, StringComparison.InvariantCultureIgnoreCase) && l.mapname.Equals(mapname, StringComparison.InvariantCultureIgnoreCase)).ToList();
                if(maplogs.Count > 9)
                {
                    userContext.MapsLog.Remove(maplogs.First());
                    await userContext.SaveChangesAsync();
                }
                var log = new WMSMapsLog();
                log.logdate = DateTime.UtcNow;
                log.username = username;
                log.mapname = mapname;
                log.mapcontent = mapcontent;
                userContext.MapsLog.Add(log);
                return await userContext.SaveChangesAsync();
            }
        }

        public async Task<int> LatLonToGeomAsync(string username, string layername, string projection, string[] latlon)
        {
            var result = 0;
            using (var userContext = _userDBContextFactory.Create())
            using (var command = userContext.Database.GetDbConnection().CreateCommand())
            {
                using (var transaction = userContext.Database.BeginTransaction())
                {
                    try
                    {
                        command.CommandText = $"SELECT AddGeometryColumn ('{username}', '{layername.ToLower()}','geom',{projection},'POINT',2);";
                        command.CommandText += $"UPDATE {username}.{layername.ToLower()} SET geom = ST_SetSRID(ST_MakePoint(cast({latlon[1]} as numeric), cast({latlon[0]} as numeric)), {projection});";
                        userContext.Database.OpenConnection();
                        result += await command.ExecuteNonQueryAsync();
                        transaction.Commit();
                        return result;
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

        }

        //public object GetMVTTile(IMemoryCache cache, string username, string layername, double x, double y, int z)
        //{
        //    using (var userContext = _userDBContextFactory.Create())
        //    using (var command = userContext.Database.GetDbConnection().CreateCommand())
        //    {

        //        try
        //        {
        //            var mvtt = new MVTTile(256, cache);
        //            var b = mvtt.bbox(x, y, z, false, null);
        //            command.CommandText = $"SELECT ST_AsMVT(q, '{layername}', 4096, 'geom') FROM (SELECT id, ST_AsMVTGeom( geom, ST_MakeEnvelope({b.ll.x}, {b.ll.y}, {b.ur.x}, {b.ur.y}, 4326), 4096, 256,false) geom FROM {username}.{layername} c) q";
        //            userContext.Database.OpenConnection();
        //            using (var result = command.ExecuteReader())
        //            {
        //                result.Read();
        //                return result[0];
        //            }
        //        }
        //        catch
        //        {
        //            return null;
        //        }
        //    }
        //}
    }
}
