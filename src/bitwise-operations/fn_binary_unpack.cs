using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public class fn_binary_unpack
{
    [SqlFunction(
        Name = "fn_binary_unpack",
        DataAccess = DataAccessKind.None,
        SystemDataAccess = SystemDataAccessKind.None,
        IsPrecise = true,
        IsDeterministic = true,
        FillRowMethodName = "FillRow",
        TableDefinition = "[index] smallint")]
    public static IEnumerable Init(SqlBinary sqlBinary)
    {
        var binary = sqlBinary.Value;
        var bits = new BitArray(binary);
        var length = bits.Length;
        var indices = new LinkedList<short>();
        for (short i = 0; i < length; ++i) {
            if (bits.Get(i)) {
                indices.AddLast(i);
            }
        }
        return indices;
    }

    public static void FillRow(Object obj, out SqlInt16 index)
    {
        index = new SqlInt16((short) obj);
    }
}

public struct fn_binary_unpack_row
{
    public short index;
}
