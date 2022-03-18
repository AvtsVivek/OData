using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace OdataExpandDemo
{
    public class ApplicationOptions
    {
        public ApplicationOptions() => CacheProfiles = new CacheProfileOptions();

        [Required]
        public CacheProfileOptions CacheProfiles { get; }

        [Required]
        public CompressionOptions Compression { get; set; } = default!;

        [Required]
        public ForwardedHeadersOptions ForwardedHeaders { get; set; } = default!;

        [Required]
        public HostOptions Host { get; set; } = default!;

        [Required]
        public KestrelServerOptions Kestrel { get; set; } = default!;
    }

    /// <summary>
    /// The caching options for the application.
    /// </summary>
#pragma warning disable CA1710 // Identifiers should have correct suffix
    public class CacheProfileOptions : Dictionary<string, CacheProfile>
#pragma warning restore CA1710 // Identifiers should have correct suffix
    {
    }

    public class CompressionOptions
    {
        public CompressionOptions() => MimeTypes = new List<string>();

        /// <summary>
        /// Gets a list of MIME types to be compressed in addition to the default set used by ASP.NET Core.
        /// </summary>
        [Required]
        public List<string> MimeTypes { get; }
    }

    public static class RunningEnvironmentName
    {
        public const string Test = nameof(Test);
    }
}
