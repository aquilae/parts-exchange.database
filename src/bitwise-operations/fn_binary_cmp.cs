using System;
using System.Collections;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public class fn_binary_cmp
{
    [SqlFunction(
        Name = "fn_binary_cmp",
        DataAccess = DataAccessKind.None,
        SystemDataAccess = SystemDataAccessKind.None,
        IsPrecise = true,
        IsDeterministic = true)]
    public static SqlInt16 Body(SqlBinary sqlLeft, SqlBinary sqlRight)
    {
        var left = new BitArray(sqlLeft.Value);
        var right = new BitArray(sqlRight.Value);
        var left_length = left.Length;
        var right_length = right.Length;
        var max_length = Math.Min(left_length, right_length);

        for (var i = 0; i < max_length; ++i) {
            var left_value = false;
            if (i < left_length) {
                left_value = left.Get(i);
            }
            
            var right_value = false;
            if (i < right_length) {
                right_value = right.Get(i);
            }

            if (left_value) {
                if (!right_value) {
                    return -1;
                }
            }
            else {
                if (right_value) {
                    return 1;
                }
            }
        }

        return 0;
    }
}
