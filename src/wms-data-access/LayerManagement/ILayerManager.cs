using System.Collections.Generic;
using wmsShared.Model;

namespace wmsDataAccess.LayerManagement
{
    public interface ILayerManager
    {
        List<string> GetLayerFieldsNames(string username, string layername);
        LayerDataResultModel GetLayerData(string username, string layername, LayerDataRequestModel layerDataRequestModel);
    }
}