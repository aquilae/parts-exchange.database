using System;
using System.Collections;
using System.Data.SqlTypes;
using System.IO;
using Microsoft.SqlServer.Server;

[Serializable]
[SqlUserDefinedAggregate(
    Format.UserDefined,
    Name = "fn_binary_pack",
    MaxByteSize = 8000,
    IsInvariantToOrder = true,
    IsInvariantToNulls = true,
    IsInvariantToDuplicates = true,
    IsNullIfEmpty = false)]
public struct fn_binary_pack : IBinarySerialize
{
    public void Init()
    {
        this._size = 32;
        this._bits = new BitArray(this._size);
    }

    public void Accumulate(SqlInt16 sqlIndex)
    {
        var index = sqlIndex.Value;
        while (this._size < index) {
            this._size *= 2;
        }

        this._bits.Length = this._size;
        this._bits.Set(index, true);
    }

    public void Merge(fn_binary_pack value)
    {
        if (value._size > this._size) {
            this._size = value._size;
            this._bits.Length = this._size;
        }
        this._bits.Or(value._bits);
    }

    public SqlBinary Terminate()
    {
        var array = new byte[this._size / 8];
        this._bits.CopyTo(array, 0);
        return new SqlBinary(array);
    }

    private BitArray _bits;
    private short _size;

    public void Read(BinaryReader r)
    {
        var array = r.ReadBytes(this._size = r.ReadInt16());
        this._bits = new BitArray(array) { Length = this._size };
    }

    public void Write(BinaryWriter w)
    {
        var array = new byte[this._size / 8];
        this._bits.CopyTo(array, 0);

        w.Write(this._size);
        w.Write(array);
    }
}
