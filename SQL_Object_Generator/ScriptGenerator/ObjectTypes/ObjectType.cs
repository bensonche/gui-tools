using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using BC.ScriptGenerator.Model;

namespace BC.ScriptGenerator
{
    abstract public class ObjectType
    {
        abstract public string Name { get; }
        abstract public string CountQuery { get; }
        abstract public string DefinitionQuery { get; }
        abstract public string FileBody { get; }
        public bool IncludeSchemaInFilename = true;

        public int Count;

        public string Remaining
        {
            get
            {
                return Count < 0 ? "Done" : Count.ToString();
            }
        }

        public void SetInitialCount(SqlConnection con)
        {
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = CountQuery;
            cmd.CommandType = CommandType.Text;

            Count = (int)cmd.ExecuteScalar();
        }

        public virtual async Task<List<DbObjectResult>> GetDbOjectAsync(string connectionString)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                await con.OpenAsync();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = con,
                    CommandType = CommandType.Text,
                    CommandText = DefinitionQuery
                };

                adapter.SelectCommand = cmd;

                var table = new DataTable();

                adapter.Fill(table);

                var raw = from t in table.AsEnumerable()
                          select new
                          {
                              Name = t.Field<string>("name"),
                              Definition = t.Field<string>("definition"),
                              Schema = t.Field<string>("schema"),
                              PermissionType = t.Field<string>("PermissionType"),
                              PermissionName = t.Field<string>("PermissionName"),
                              GranteeName = t.Field<string>("GranteeName")
                          };

                var grouped = from r in raw
                              group r by new { r.Name, r.Definition, r.Schema } into g
                              select g;

                List<DbObjectResult> resultList = new List<DbObjectResult>();

                foreach (var obj in grouped)
                {
                    DbObjectResult result = new DbObjectResult();
                    result.Name = obj.Key.Name;
                    result.Definition = obj.Key.Definition;
                    result.Schema = obj.Key.Schema;

                    bool hasPermissions = true;
                    StringBuilder sb = new StringBuilder();
                    foreach (var permission in obj.OrderBy(x => x.PermissionType).ThenBy(x => x.PermissionName).ThenBy(x => x.GranteeName))
                    {
                        if (permission.PermissionType == null)
                        {
                            hasPermissions = false;
                            break;
                        }

                        sb.AppendLine(string.Format("{0} {1} on [{2}].[{3}] to [{4}]",
                            permission.PermissionType,
                            permission.PermissionName,
                            obj.Key.Schema,
                            obj.Key.Name,
                            permission.GranteeName));
                    }
                    sb.AppendLine("GO");

                    result.PermissionString = hasPermissions ? sb.ToString() : null;

                    resultList.Add(result);
                }

                return resultList;
            }
        }

        public virtual string GetScriptBody()
        {
            return "";
        }
    }
}
