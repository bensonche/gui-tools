using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC.ScriptGenerator.ObjectTypes
{
    public class Function : ObjectType
    {
        public override string Name
        {
            get { return "functions"; }
        }

        public override string CountQuery
        {
            get { return @"
                    select count(*)
                    from sys.objects
                    where type in (N'FN', N'IF', N'TF', N'FS', N'FT')"; }
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
                    from sys.objects a
                        inner join sys.sql_modules b
                            on a.object_id = b.object_id
                        inner join sys.schemas c
                            on a.schema_id = c.schema_id
                        left join sys.database_permissions per
                            on a.object_id = per.major_id
                        left join sys.database_principals pri
                            on per.grantee_principal_id = pri.principal_id
                    where a.type in (N'FN', N'IF', N'TF', N'FS', N'FT')"; }
        }

        public override string FileBody
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("drop function if exists [{2}].[{0}]");
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
    }
}
