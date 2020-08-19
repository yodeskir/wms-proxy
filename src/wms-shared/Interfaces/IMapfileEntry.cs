namespace wmsShared.Interfaces
{
    public interface IMapfileEntry
    {
        string id { get; set; }
        string name { get; }

        bool allowMultiplesInstances { get; set; }
        VALUETYPE valueType { get; }
    }

    public enum VALUETYPE
    {
        _block,
        _char,
        _string,
        _expression,
        _set,
        _int,
        _double,
        _double_range,
        _keyvalue,
        _yesno,
        _onoff,
        _truefalse,
        _extent,
        _filename,
        _color,
        _colorrange,
        _xy,
        _mimetype,
        _scaledenom,
        _attribute,
        _data,
        _func,
        _lyrref,
        _setpoints,
        _wkt,
        _proj,
        _processing,
        _symbol
    }
}
