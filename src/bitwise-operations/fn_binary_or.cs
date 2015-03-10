using System.Collections;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public class fn_binary_or
{
    [SqlFunction(
        Name = "fn_binary_or",
        DataAccess = DataAccessKind.None,
        SystemDataAccess = SystemDataAccessKind.None,
        IsPrecise = true,
        IsDeterministic = true)]
    public static SqlBinary Body(SqlBinary sqlLeft, SqlBinary sqlRight)
    {
        var left = new BitArray(sqlLeft.Value);
        var right = new BitArray(sqlRight.Value);
        var result = left.Or(right);
        var array = new byte[result.Length / 8];
        result.CopyTo(array, 0);
        return new SqlBinary(array);
    }
}
