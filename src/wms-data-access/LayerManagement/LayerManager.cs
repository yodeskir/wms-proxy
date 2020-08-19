using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using wmsShared.Model;

namespace wmsDataAccess.LayerManagement
{
    public class LayerManager : ILayerManager
    {
        private readonly IOptions<MapServerRuntimeOptions> _runtimeOptions;

        public LayerManager(IOptions<MapServerRuntimeOptions> runtimeOptions)
        {
            _runtimeOptions = runtimeOptions;
        }

        public LayerDataResultModel GetLayerData(string username, string layername, LayerDataRequestModel dtRequest)
        {
            var result = new LayerDataResultModel();
            result.Fields = GetLayerFieldsNames(username, layername);
            var sqlWhere = " ";
            var whereFilter = "";
            if (!string.IsNullOrEmpty(dtRequest.FilterBy) && !string.IsNullOrEmpty(dtRequest.FilterOpType) && !string.IsNullOrEmpty(dtRequest.FilterValue)) {
                whereFilter = resolveWhereOperation(dtRequest.FilterBy, dtRequest.FilterOpType, dtRequest.FilterValue);
                sqlWhere = $" AND {whereFilter}";
            }
            string sql = $"SELECT {string.Join(',', result.Fields)} FROM {username}.{layername} WHERE ID > {dtRequest.RowStart} {sqlWhere} ORDER BY ID ASC LIMIT 101 ";

            using (var connection = new NpgsqlConnection(_runtimeOptions.Value.ConnectionString))
            {
                var cmd = new NpgsqlCommand(sql);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                connection.Open();
                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (dbReader.Read()) {
                    var fieldValues = new List<object>();
                    foreach (string field in result.Fields)
                    {
                        var ordinal = dbReader.GetOrdinal(field);
                        var dataType = dbReader.GetDataTypeName(ordinal);
                        if (!dataType.Equals("public.geometry"))
                            fieldValues.Add(dbReader[field]);
                    }
                    result.Data.Add(fieldValues);
                }
                dbReader.Close();
            }
            return result;
        }

        private string resolveWhereOperation(string filterBy, string filterOpType, string filterValue)
        {
            switch (filterOpType) {
                case "contains":
                    return $" {filterBy}::text LIKE '%{filterValue}%' ";
                case "notContains":
                    return $" {filterBy}::text NOT LIKE '%{filterValue}%' ";
                case "equals":
                    return $" {filterBy}::text LIKE '{filterValue}' ";
                case "notEqual":
                    return $" {filterBy}::text NOT LIKE '{filterValue}' ";
                case "startsWith":
                    return $" {filterBy}::text LIKE '{filterValue}%' ";
                case "endsWith":
                    return $" {filterBy}::text LIKE '%{filterValue}' ";
                default:
                    return $" {filterBy}::text LIKE '%{filterValue}%' ";
            }
        }

        public List<string> GetLayerFieldsNames(string username, string layername)
        {
            var result = new List<string>();
            string sql = $"SELECT column_name FROM information_schema.columns WHERE table_schema = '{username}' AND table_name = '{layername}' AND (data_type not like 'USER-DEFINED' OR column_name not like 'geom')";
            using (var connection = new NpgsqlConnection(_runtimeOptions.Value.ConnectionString))
            {
                var cmd = new NpgsqlCommand(sql);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                connection.Open();
                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (dbReader.Read())
                {
                    result.Add(dbReader[0].ToString());
                }
                dbReader.Close();
            }
            return result;
        }
    }
}
