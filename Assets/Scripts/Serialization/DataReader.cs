using System.IO;
using System.Text;

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

    public void Close()
    {
        _stream.Flush();
        _reader.Close();
        _reader.Dispose();
        _stream.Close();
        _stream.Dispose();
    }
}

