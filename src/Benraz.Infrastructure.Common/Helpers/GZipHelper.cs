using System.IO;
using System.IO.Compression;

namespace Benraz.Infrastructure.Common.Helpers;

/// <summary>
/// GZip helper.
/// </summary>
public static class GZipHelper
{
    /// <summary>
    /// Zip string to bytes.
    /// </summary>
    public static byte[] ZipStr(string str)
    {
        using var output = new MemoryStream();
        using (var gzip =
               new DeflateStream(output, CompressionMode.Compress))
        {
            using (var writer =
                   new StreamWriter(gzip, System.Text.Encoding.UTF8))
            {
                writer.Write(str);
            }
        }

        return output.ToArray();
    }

    /// <summary>
    /// UnZip bytes to string.
    /// </summary>
    public static string UnZipStr(byte[] input)
    {
        using var inputStream = new MemoryStream(input);
        using var gzip =
            new DeflateStream(inputStream, CompressionMode.Decompress);
        using var reader =
            new StreamReader(gzip, System.Text.Encoding.UTF8);
        return reader.ReadToEnd();
    }
}


