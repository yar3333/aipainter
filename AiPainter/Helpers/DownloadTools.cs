﻿using System.Net.Http.Headers;

namespace AiPainter.Helpers;

public class DownloadFileOptions
{
    public string? FileNameIfNotDetected;
    public Func<string?, string>? PreprocessFileName;
    public string? AuthorizationBearer;
    public Action<long, long?>? Progress;

    public DownloadFileOptions Clone()
    {
        return (DownloadFileOptions)MemberwiseClone();
    }
}

public static class DownloadTools
{
    public static async Task<string> DownloadFileAsync(
        string url,
        string destDir,
        DownloadFileOptions options,
        CancellationToken cancellationToken
    )
    {
        using var client = new HttpClient();
        client.Timeout = TimeSpan.FromHours(10);
        if (!string.IsNullOrEmpty(options.AuthorizationBearer))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                options.AuthorizationBearer
            );
        }

        var tempDestFile = Path.GetTempFileName();
        await using var file = new FileStream(
            tempDestFile,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None
        );

        var fileName = await client.DownloadAsync(url, file, options.Progress, cancellationToken);
        file.Close();

        if (string.IsNullOrWhiteSpace(fileName))
            fileName = options.FileNameIfNotDetected;
        if (options.PreprocessFileName != null)
            fileName = options.PreprocessFileName(fileName);

        var resultFilePath = Path.Combine(destDir, fileName!);
        File.Move(tempDestFile, resultFilePath);

        return resultFilePath;
    }
}

static class HttpClientExtensions
{
    public static async Task<string?> DownloadAsync(
        this HttpClient client,
        string requestUri,
        Stream destination,
        Action<long, long?>? progress,
        CancellationToken cancellationToken = default
    )
    {
        using var response = await client.GetAsync(
            requestUri,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken
        );
        var contentLength = response.Content.Headers.ContentLength;
        var fileName =
            response.Content.Headers.ContentDisposition?.FileNameStar
            ?? response.Content.Headers.ContentDisposition?.FileName;
        fileName = fileName?.Trim('"', '\'');

        await using var download = await response.Content.ReadAsStreamAsync(cancellationToken);

        await download.CopyToAsync(
            destination,
            81920,
            cur => progress?.Invoke(cur, contentLength),
            cancellationToken
        );
        if (contentLength != null)
            progress?.Invoke(contentLength ?? 0, contentLength);
        return fileName;
    }
}

static class StreamExtensions
{
    public static async Task CopyToAsync(
        this Stream source,
        Stream destination,
        int bufferSize,
        Action<long> progress,
        CancellationToken cancellationToken = default
    )
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (!source.CanRead)
            throw new ArgumentException("Has to be readable", nameof(source));
        if (destination == null)
            throw new ArgumentNullException(nameof(destination));
        if (!destination.CanWrite)
            throw new ArgumentException("Has to be writable", nameof(destination));
        if (bufferSize < 0)
            throw new ArgumentOutOfRangeException(nameof(bufferSize));

        var buffer = new byte[bufferSize];
        long totalBytesRead = 0;
        int bytesRead;
        while (
            (
                bytesRead = await source
                    .ReadAsync(buffer, 0, buffer.Length, cancellationToken)
                    .ConfigureAwait(false)
            ) != 0
        )
        {
            await destination
                .WriteAsync(buffer, 0, bytesRead, cancellationToken)
                .ConfigureAwait(false);
            totalBytesRead += bytesRead;
            progress(totalBytesRead);
        }
    }
}
