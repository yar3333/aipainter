using System.Linq;
using System.Text;
using BigGustave;

namespace AiPainter.Helpers;

static class PngHelper
{
    private class PngChunkVisitor : IChunkVisitor
    {
        public readonly Dictionary<string, string> TextChunks = new();

        public void Visit(Stream stream, ImageHeader header, ChunkHeader chunkHeader, byte[] data, byte[] crc)
        {
            switch (chunkHeader.Name)
            {
                case "tEXt":
                    {
                        var s = Encoding.Latin1.GetString(data);
                        var n = s.IndexOf('\0');
                        if (n > 0)
                        {
                            var keyword = s.Substring(0, n);
                            var text = s.Substring(n + 1);
                            TextChunks.Add(keyword, text);
                        }
                    }
                    break;

                case "iTXt":
                    {
                        /*
                            Keyword 	        1-79 bytes (character string)
                            Null separator 	    1 byte (null character)
                            Compression flag 	1 byte
                            Compression method 	1 byte
                            Language tag 	    0 or more bytes (character string)
                            Null separator 	    1 byte (null character)
                            Translated keyword 	0 or more bytes
                            Null separator 	    1 byte (null character)
                            Text 	            0 or more bytes
                        */

                        var keywordBytes = data.TakeWhile(x => x != 0).ToArray();
                        var keyword = Encoding.Latin1.GetString(keywordBytes);

                        data = data.Skip(keywordBytes.Length + 1/*null*/ + 1/*compFlag*/ + 1/*compMeth*/).ToArray();

                        // now data before "Language tag"
                        
                        var langTagBytes = data.TakeWhile(x => x != 0).ToArray();
                        var langTag = Encoding.Latin1.GetString(langTagBytes);

                        data = data.Skip(langTagBytes.Length + 1/*null*/).ToArray();

                        // now data before "Translated keyword"

                        var translatedKeywordBytes = data.TakeWhile(x => x != 0).ToArray();
                        var translatedKeyword = Encoding.UTF8.GetString(translatedKeywordBytes);

                        data = data.Skip(translatedKeywordBytes.Length + 1/*null*/).ToArray();

                        // now data before "Text"
                        
                        TextChunks.Add(keyword, Encoding.UTF8.GetString(data));
                    }
                    break;
            }
        }
    }

    public static Dictionary<string, string> GetTextChunks(byte[] pngFileData)
    {
        var visitor = new PngChunkVisitor();
        Png.Open(pngFileData, visitor);
        return visitor.TextChunks;
    }

    public static byte[] EncodeImage(byte[] bgraData, int width, int height, Dictionary<string, string> textChunks)
    {
        var builder = PngBuilder.FromBgra32Pixels(bgraData, width, height);
        foreach (var kv in textChunks)
        {
            builder.StoreText(kv.Key, kv.Value);
        }
        return builder.Save();
    }
}