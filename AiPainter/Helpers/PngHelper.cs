using System.Drawing.Imaging;
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

    public static byte[] EncodeImage(Bitmap image, Dictionary<string, string> textChunks)
    {
        return encodeImage(getBgraDataFromImage(image), image.Width, image.Height, textChunks);
    }

    private static byte[] encodeImage(byte[] bgraData, int width, int height, Dictionary<string, string> textChunks)
    {
        var builder = PngBuilder.FromBgra32Pixels(bgraData, width, height);
        foreach (var kv in textChunks)
        {
            builder.StoreText(kv.Key, kv.Value);
        }
        return builder.Save();
    }

    private static byte[] getBgraDataFromImage(Bitmap image)
    {
        var bmpData = image.LockBits(new Rectangle(0,0,image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

        var bgraData = new byte[bmpData.Width * bmpData.Height * 4];
        unsafe
        {
            var srcLine = (byte*)bmpData.Scan0.ToPointer();
            fixed(byte* destStart = bgraData)
            {
                var destP = destStart;
                for (var i = 0; i < bmpData.Height; i++, srcLine += bmpData.Stride)
                {
                    var srcP = srcLine;
                    var lastP = srcLine + bmpData.Width * 4;
                    while (srcP < lastP)
                    {
                        var r = *srcP; srcP++;
                        var g = *srcP; srcP++;
                        var b = *srcP; srcP++;
                        var a = *srcP; srcP++;

                        *destP = b; destP++;
                        *destP = g; destP++;
                        *destP = r; destP++;
                        *destP = a; destP++;
                    }
                }
            }
        }

        image.UnlockBits(bmpData);
        return bgraData;
    }
}