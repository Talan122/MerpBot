using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerpBot.Interactive.JsonObjects.Curseforge
{

    public class Rootobject
    {
        public Datum[] data { get; set; }
        public Pagination pagination { get; set; }
    }

    public class Pagination
    {
        public Int64 index { get; set; }
        public Int64 pageSize { get; set; }
        public Int64 resultCount { get; set; }
        public Int64 totalCount { get; set; }
    }

    public class Datum
    {
        public Int64 id { get; set; }
        public Int64 gameId { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public Links links { get; set; }
        public string summary { get; set; }
        public Int64 status { get; set; }
        public Int64 downloadCount { get; set; }
        public bool isFeatured { get; set; }
        public Int64 primaryCategoryId { get; set; }
        public Category[] categories { get; set; }
        public Int64 classId { get; set; }
        public Author[] authors { get; set; }
        public Logo logo { get; set; }
        public Screenshot[] screenshots { get; set; }
        public Int64 mainFileId { get; set; }
        public Latestfile[] latestFiles { get; set; }
        public Latestfilesindex[] latestFilesIndexes { get; set; }
        public DateTime dateCreated { get; set; }
        public DateTime dateModified { get; set; }
        public DateTime dateReleased { get; set; }
        public bool allowModDistribution { get; set; }
        public Int64 gamePopularityRank { get; set; }
        public bool isAvailable { get; set; }
        public Int64 thumbsUpCount { get; set; }
    }

    public class Links
    {
        public string websiteUrl { get; set; }
        public string wikiUrl { get; set; }
        public string issuesUrl { get; set; }
        public string sourceUrl { get; set; }
    }

    public class Logo
    {
        public Int64 id { get; set; }
        public Int64 modId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string thumbnailUrl { get; set; }
        public string url { get; set; }
    }

    public class Category
    {
        public Int64 id { get; set; }
        public Int64 gameId { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string url { get; set; }
        public string iconUrl { get; set; }
        public DateTime dateModified { get; set; }
        public bool isClass { get; set; }
        public Int64 classId { get; set; }
        public Int64 parentCategoryId { get; set; }
        public Int64 displayIndex { get; set; }
    }

    public class Author
    {
        public Int64 id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Screenshot
    {
        public Int64 id { get; set; }
        public Int64 modId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string thumbnailUrl { get; set; }
        public string url { get; set; }
    }

    public class Latestfile
    {
        public Int64 id { get; set; }
        public Int64 gameId { get; set; }
        public Int64 modId { get; set; }
        public bool isAvailable { get; set; }
        public string displayName { get; set; }
        public string fileName { get; set; }
        public Int64 releaseType { get; set; }
        public Int64 fileStatus { get; set; }
        public Hash[] hashes { get; set; }
        public DateTime fileDate { get; set; }
        public Int64 fileLength { get; set; }
        public Int64 downloadCount { get; set; }
        public string downloadUrl { get; set; }
        public string[] gameVersions { get; set; }
        public Sortablegameversion[] sortableGameVersions { get; set; }
        public Dependency[] dependencies { get; set; }
        public bool exposeAsAlternative { get; set; }
        public Int64 parentProjectFileId { get; set; }
        public Int64 alternateFileId { get; set; }
        public bool isServerPack { get; set; }
        public Int64 serverPackFileId { get; set; }
        public Int64 fileFingerprInt64 { get; set; }
        public Module[] modules { get; set; }
    }

    public class Hash
    {
        public string value { get; set; }
        public Int64 algo { get; set; }
    }

    public class Sortablegameversion
    {
        public string gameVersionName { get; set; }
        public string gameVersionPadded { get; set; }
        public string gameVersion { get; set; }
        public DateTime gameVersionReleaseDate { get; set; }
        public Int64 gameVersionTypeId { get; set; }
    }

    public class Dependency
    {
        public Int64 modId { get; set; }
        public Int64 relationType { get; set; }
    }

    public class Module
    {
        public string name { get; set; }
        public Int64 fingerprInt64 { get; set; }
    }

    public class Latestfilesindex
    {
        public string gameVersion { get; set; }
        public Int64 fileId { get; set; }
        public string filename { get; set; }
        public Int64 releaseType { get; set; }
        public Int64 gameVersionTypeId { get; set; }
        public Int64 modLoader { get; set; }
    }


}
