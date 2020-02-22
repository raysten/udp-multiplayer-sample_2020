using System.IO;
using System.Text;
using UnityEngine;

public class DataReader
{
    private readonly MemoryStream _stream;
    private readonly BinaryReader _reader;

    public DataReader(byte[] data)
    {
        _stream = new MemoryStream(data);
        _reader = new BinaryReader(_stream);
    }

    public string GetString()
    {
        int length = GetInteger();
        var tmp = _reader.ReadBytes(length);
        return Encoding.ASCII.GetString(tmp);
    }

    public int GetInteger()
    {
        return _reader.ReadInt32();
    }

	public uint GetUnsignedInteger()
	{
		return _reader.ReadUInt32();
	}

	public bool GetBool()
	{
		return _reader.ReadBoolean();
	}

	public Vector3 GetVector3()
	{
		float x = _reader.ReadSingle();
		float y = _reader.ReadSingle();
		float z = _reader.ReadSingle();

		return new Vector3(x, y, z);
	}

    public void Close()
    {
        _stream.Flush();
        _reader.Close();
        _reader.Dispose();
        _stream.Close();
        _stream.Dispose();
    }
}

