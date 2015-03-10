using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Xml;
using Microsoft.SqlServer.Server;

public class fn_binary_sort_clr
{
    [SqlFunction(
        Name = "fn_binary_sort_clr",
        DataAccess = DataAccessKind.None,
        SystemDataAccess = SystemDataAccessKind.None,
        IsPrecise = true,
        IsDeterministic = true,
        FillRowMethodName = "FillRow",
        TableDefinition = "[id] bigint, [value] binary(8000), [rn] bigint")]
    public static IEnumerable Init(SqlXml xml)
    {
        var items = new LinkedList<Item>();

        using (var xr = xml.CreateReader()) {
            while (xr.Read()) {
                if (xr.NodeType == XmlNodeType.Element) {
                    var id_str = xr.GetAttribute("id");
                    var value_str = xr.GetAttribute("value");

                    var id = long.Parse(id_str);
                    var value = Convert.FromBase64String(value_str);

                    items.AddLast(new Item(id, value));
                }
            }
        }

        var length = items.Count;
        var array = new Item[length];
        items.CopyTo(array, 0);
        Array.Sort(array, comparison);

        for (long i = 0; i < length; ++i) {
            array[i].rn = i + 1;
        }

        return array;
    }

    public static void FillRow(Object obj, out SqlInt64 id, out SqlBinary value, out SqlInt64 rn)
    {
        var item = (Item) obj;
        
        var array = new byte[item.value.Length / 8];
        item.value.CopyTo(array, 0);

        id = item.id;
        value = new SqlBinary(array);
        rn = item.rn;
    }

    private static int comparison(Item left, Item right)
    {
        var left_length = left.value.Length;
        var right_length = right.value.Length;
        var max_length = Math.Min(left_length, right_length);

        for (var i = 0; i < max_length; ++i) {
            var left_value = false;
            if (i < left_length) {
                left_value = left.value.Get(i);
            }
            
            var right_value = false;
            if (i < right_length) {
                right_value = right.value.Get(i);
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

        return (int) (left.id - right.id);
    }

    private sealed class Item
    {
        public readonly long id;
        public readonly BitArray value;
        public long rn;

        public Item(long id, byte[] value)
        {
            this.id = id;
            this.value = new BitArray(value);
        }
    }
}
