using System.Collections;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public class fn_binary_get
{
    [SqlFunction(
        Name = "fn_binary_get",
        DataAccess = DataAccessKind.None,
        SystemDataAccess = SystemDataAccessKind.None,
        IsPrecise = true,
        IsDeterministic = true)]
    public static SqlBoolean Body(SqlBinary sql_binary, SqlInt16 sql_index)
    {
        var binary = new BitArray(sql_binary.Value);
        var is_set = binary.Get(sql_index.Value);
        return new SqlBoolean(is_set);
    }
}
