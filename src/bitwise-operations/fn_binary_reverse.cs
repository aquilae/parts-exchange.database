using System;
using System.Collections;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public class fn_binary_reverse
{
    [SqlFunction(
        Name = "fn_binary_reverse",
        DataAccess = DataAccessKind.None,
        SystemDataAccess = SystemDataAccessKind.None,
        IsPrecise = true,
        IsDeterministic = true)]
    public static SqlBinary Body(SqlBinary sqlValue, SqlBoolean bitwise)
    {
        if (bitwise.IsTrue) {
            var value = new BitArray(sqlValue.Value);
            var length = value.Length;
            var result = new BitArray(length);
            for (int i = 0, j = length - 1; i < length; ++i, --j) {
                if (value.Get(i)) {
                    result.Set(j, true);
                }
            }
            var array = new byte[length / 8];
            result.CopyTo(array, 0);
            return new SqlBinary(array);
        }
        else {
            var value = sqlValue.Value;
            Array.Reverse(value);
            return new SqlBinary(value);
        }
    }
}
