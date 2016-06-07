﻿using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using BMCLV2.Game;
using BMCLV2.JsonClass;
using BMCLV2.Objects.Mirrors;

namespace BMCLV2.Mirrors.Interface
{
    public abstract class Version
    {
        protected readonly Downloader.Downloader Downloader = new Downloader.Downloader();
        protected VersionManifest VersionManifest;
        protected string Url = "http://bmclapi2.bangbang93.com/mc/game/version_manifest.json";
        public virtual string Name { get;}

        public VersionManifest.Latest GetLatest()
        {
            return VersionManifest.latest;
        }

        public VersionManifest.Version[] GetVersions()
        {
            return VersionManifest.versions;
        }

        public DataTable GetDataTable()
        {
            var versions = VersionManifest.versions;
            var dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("type");
            dt.Columns.Add("time");
            dt.Columns.Add("url");
            foreach (var version in versions)
            {
                dt.Rows.Add(version.id, version.type, version.time, version.url);
            }
            return dt;
        }

        public VersionManifest.Version GetVersion(string id)
        {
            return VersionManifest.versions.FirstOrDefault(version => version.id == id);
        }

        public async Task Refresh()
        {
            var json =
                await Downloader.DownloadStringTaskAsync(
                    new Uri(Url));
            VersionManifest = new JSON<VersionManifest>().Parse(json);
        }

        public abstract Task<string> DownloadJson(string url);
        public abstract Task DownloadJar(string url, string savePath);
    }
}