using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC.ScriptGenerator.ObjectTypes
{
    public class Trigger : ObjectType
    {
        public override string Name
        {
            get { return "triggers"; }
        }

        public Trigger()
        {
            IncludeSchemaInFilename = false;
        }

        public override string CountQuery
        {
            get { return @"
                    select count(*)
                    from sys.triggers"; }
        }

        public override string DefinitionQuery
        {
            get { return @"
                    select
                        a.name,
                        b.definition,
                        d.name as [schema],
                        null as PermissionType,
                        null as PermissionName,
                        null as GranteeName
                    from sys.triggers a
                        inner join sys.sql_modules b
                            on a.object_id = b.object_id
                        inner join sys.tables c
                            on a.parent_id = c.object_id
                        inner join sys.schemas d
                            on c.schema_id = d.schema_id"; }
        }

        public override string FileBody
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("drop trigger if exists [{2}].[{0}]");
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
