using Microsoft.Extensions.Caching.Distributed;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Ultraschall.Podcasting.Caching
{
    public class FileCache : IDistributedCache
    {
        private readonly string _path;

        private string GetPath(string key)
        {
            return _path + "/" + key;
        }

        public FileCache(FileCacheOptions options)
        {
            _path = options.Path ?? throw new ArgumentNullException(nameof(options.Path));

            bool exists = Directory.Exists(_path);
            if (!exists)
                Directory.CreateDirectory(_path);
        }

        public byte[] Get(string key)
        {
            if (!File.Exists(GetPath(key)))
            {
                return default(byte[]);
            }
            using (var file = new FileStream(GetPath(key), FileMode.Open, FileAccess.Read, FileShare.Read, 4096, false))
            {
                byte[] buff = new byte[file.Length];
                file.Read(buff, 0, (int)file.Length);
                return buff;
            }

        }

        public async Task<byte[]> GetAsync(string key, CancellationToken token = default (CancellationToken))
        {
            if (!File.Exists(GetPath(key)))
            {
                return default(byte[]);
            }
            using (var file = new FileStream(GetPath(key), FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            {
                byte[] buff = new byte[file.Length];
                await file.ReadAsync(buff, 0, (int)file.Length, token);
                return buff;
            }
        }

        public void Refresh(string key)
        {
            var fileInfo = new FileInfo(GetPath(key)) {LastWriteTime = DateTime.Now};
        }

        public async Task RefreshAsync(string key, CancellationToken token = default (CancellationToken))
        {
            await Task.Factory.StartNew(() =>
            {
                var fileInfo = new FileInfo(GetPath(key)) {LastWriteTime = DateTime.Now};
            }, token);
        }

        public void Remove(string key)
        {
            File.Delete(GetPath(key));
        }

        public async Task RemoveAsync(string key, CancellationToken token = default (CancellationToken))
        {
            await Task.Factory.StartNew(() => File.Delete(GetPath(key)), token);
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            using (var file = new FileStream(GetPath(key), FileMode.CreateNew, FileAccess.Write, FileShare.Write, 4096, true))
            {
                file.Write(value, 0, value.Length);
            }
        }

        public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default (CancellationToken))
        {
            using (var file = new FileStream(GetPath(key), FileMode.CreateNew, FileAccess.Write, FileShare.Write, 4096, true))
            {
                await file.WriteAsync(value, 0, value.Length, token);
            }
        }
    }
}
