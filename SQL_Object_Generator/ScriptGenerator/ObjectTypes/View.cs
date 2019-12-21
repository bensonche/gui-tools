using BC.ScriptGenerator.Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC.ScriptGenerator.ObjectTypes
{
    class View : ObjectType
    {
        public override string Name
        {
            get { return "views"; }
        }

        public override string CountQuery
        {
            get { return @"
                    select count(*)
                    from sys.views"; }
        }

        public override string DefinitionQuery
        {
            get { return @"
                    select
                        a.name,
                        b.definition,
                        c.name as [schema],
                        state_desc as PermissionType,
                        permission_name as PermissionName,
                        pri.name as GranteeName
                    from sys.views a
                        inner join sys.sql_modules b
                            on a.object_id = b.object_id
                        inner join sys.schemas c
                            on a.schema_id = c.schema_id
                        left join sys.database_permissions per
                            on a.object_id = per.major_id
                        left join sys.database_principals pri
                            on per.grantee_principal_id = pri.principal_id"; }
        }

        private string TriggerQuery
        {
            get { return @"
                    select
                        v.name,
                        b.definition,
                        d.name as [schema]
                    from sys.triggers a
                        inner join sys.sql_modules b
                            on a.object_id = b.object_id
                        inner join sys.views v
                            on a.parent_id = v.object_id
                        inner join sys.schemas d
                            on v.schema_id = d.schema_id"; }
        }

        public override string FileBody
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("drop view if exists [{2}].[{0}]");
                sb.AppendLine("go");
                sb.AppendLine("set ansi_nulls on");
                sb.AppendLine("go");
                sb.AppendLine("set quoted_identifier on");
                sb.AppendLine("go");
                sb.Append("{1}");
                sb.AppendLine("go");

                return sb.ToString();
            }
        }


        public override async Task<List<DbObjectResult>> GetDbOjectAsync(string connectionString)
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

                var definitionRaw = from t in table.AsEnumerable()
                    select new
                    {
                        Name = t.Field<string>("name"),
                        Definition = t.Field<string>("definition"),
                        Schema = t.Field<string>("schema"),
                        PermissionType = t.Field<string>("PermissionType")?.ToLower(),
                        PermissionName = t.Field<string>("PermissionName")?.ToLower(),
                        GranteeName = t.Field<string>("GranteeName")
                    };

                var grouped = from r in definitionRaw
                    group r by new {r.Name, r.Definition, r.Schema}
                    into g
                    select g;

                cmd = new SqlCommand
                {
                    Connection = con,
                    CommandType = CommandType.Text,
                    CommandText = TriggerQuery
                };

                adapter.SelectCommand = cmd;

                table = new DataTable();

                adapter.Fill(table);

                var triggerRaw = from t in table.AsEnumerable()
                    select new
                    {
                        Name = t.Field<string>("name"),
                        Definition = t.Field<string>("definition"),
                        Schema = t.Field<string>("schema")
                    };

                List<DbObjectResult> resultList = new List<DbObjectResult>();

                foreach (var obj in grouped)
                {
                    DbObjectResult result = new DbObjectResult();
                    result.Name = obj.Key.Name;
                    result.Definition = obj.Key.Definition;
                    result.Schema = obj.Key.Schema;

                    bool hasPermissions = true;
                    StringBuilder sb = new StringBuilder();
                    foreach (
                        var permission in
                            obj.OrderBy(x => x.PermissionType).ThenBy(x => x.PermissionName).ThenBy(x => x.GranteeName))
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
                    if (hasPermissions)
                        sb.AppendLine("go");

                    foreach (var trigger in triggerRaw.Where(x => x.Name == obj.Key.Name))
                    {
                        sb.AppendLine(trigger.Definition);
                        sb.AppendLine("go");
                    }

                    result.PermissionString = sb.Length > 0 ? sb.ToString() : null;

                    resultList.Add(result);
                }

                return resultList;
            }
        }
    }
}
