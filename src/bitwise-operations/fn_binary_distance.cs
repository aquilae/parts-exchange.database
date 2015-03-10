using System;
using System.Collections;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public class fn_binary_distance
{
    [SqlFunction(
        Name = "fn_binary_distance",
        DataAccess = DataAccessKind.None,
        SystemDataAccess = SystemDataAccessKind.None,
        IsPrecise = true,
        IsDeterministic = true)]
    public static SqlInt64 Body(SqlBinary sql_from, SqlBinary sql_to)
    {
        var from = new BitArray(sql_from.Value);
        var to = new BitArray(sql_to.Value);

        var length = Math.Min(63, from.Length);
        var to_length = to.Length;

        ulong distance = 0;
        for (var i = 0; i < length; ++i) {
            if (from.Get(i)) {
                distance <<= 1;
                if (i >= to_length || !to.Get(i)) {
                    distance += 1;
                }
            }
        }

        return new SqlInt64((long) distance);
    }
}
