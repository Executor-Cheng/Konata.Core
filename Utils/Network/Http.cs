﻿using System.IO;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Konata.Core.Utils.Network
{
    public static class Http
    {
        /// <summary>
        /// Http get
        /// </summary>
        /// <param name="url"></param>
        /// <param name="header"></param>
        /// <param name="timeout"></param>
        /// <param name="limitLen"></param>
        /// <returns></returns>
        public static async Task<byte[]> Get(string url,
            Dictionary<string, string> header = null,
            int timeout = 8000, int limitLen = 0)
        {
            // Create request
            var request = WebRequest.CreateHttp(url);
            {
                request.Timeout = timeout;
                request.ReadWriteTimeout = timeout;
                request.AutomaticDecompression =
                    DecompressionMethods.Deflate | DecompressionMethods.GZip;

                // Append request header
                if (header != null)
                {
                    foreach (var (k, v) in header)
                        request.Headers.Add(k, v);
                }
            }

            // Open response stream
            var response = await request.GetResponseAsync();
            {
                // length limitation
                if (limitLen != 0)
                {
                    // Get content length
                    var totalLen = int.Parse
                        (response.Headers["Content-Length"]!);

                    // Decline streaming transport
                    if (totalLen > limitLen || totalLen == 0) return null;
                }

                // Receive the response data
                var stream = response.GetResponseStream();
                await using var memStream = new MemoryStream();
                {
                    // Copy the stream
                    if (stream != null)
                        await stream.CopyToAsync(memStream);

                    // Close
                    response.Close();
                    return memStream.ToArray();
                }
            }
        }

        /// <summary>
        /// Http post
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="header"></param>
        /// <param name="param"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static async Task<byte[]> Post(string url, byte[] data,
            Dictionary<string, string> header, Dictionary<string, string> param,
            int timeout = 8000)
        {
            var args = param.Aggregate("", (current, i)
                => current + $"&{i.Key}={i.Value}");

            url += $"?{args[1..]}";
            var request = WebRequest.CreateHttp(url);
            {
                request.Method = "POST";
                request.Timeout = timeout;
                request.ReadWriteTimeout = timeout;
                request.ContentLength = data.Length;
                request.AutomaticDecompression = 
                    DecompressionMethods.GZip | DecompressionMethods.Deflate;

                // Append request header
                foreach (var (k, v) in header)
                    request.Headers.Add(k, v);

                // Write the post body
                var reqstream = request.GetRequestStream();
                {
                    reqstream.Write(data);
                    reqstream.Close();
                }
            }

            // Receive the response data
            var response = await request.GetResponseAsync();
            var rspstream = response.GetResponseStream();
            await using var memStream = new MemoryStream();
            {
                // Copy the stream
                if (rspstream != null)
                    await rspstream.CopyToAsync(memStream);

                // Close
                response.Close();
                return memStream.ToArray();
            }
        }
    }
}
