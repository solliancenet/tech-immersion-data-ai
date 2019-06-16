using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomSkillFunctions.Models
{
    public class FormRecognizerResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("pages")]
        public List<Page> Pages { get; set; }
        [JsonProperty("errors")]
        public List<Error> Errors { get; set; }
    }

    [JsonObject("page")]
    public class Page
    {
        [JsonProperty("number")]
        public int Number { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("clusterId")]
        public int ClusterId { get; set; }
        [JsonProperty("keyValuePairs")]
        public List<Kvp> KeyValuePairs { get; set; }
        [JsonProperty("tables")]
        public List<Table> Tables { get; set; }
    }

    [JsonObject("keyValuePair")]
    public class Kvp
    {
        [JsonProperty("key")]
        public List<Key> Key { get; set; }
        [JsonProperty("value")]
        public List<Value> Value { get; set; }
    }

    public class Table
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("columns")]
        public List<Column> Columns { get; set; }
    }

    public class Column
    {
        [JsonProperty("header")]
        public List<Header> Header { get; set; }
        [JsonProperty("entries")]
        public List<List<Entry>> Entries { get; set; }
    }

    [JsonObject("keyValuePair")]
    public class Key
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("boundingBox")]
        public List<double> BoundingBox { get; set; }
        [JsonProperty("confidence")]
        public double Confidence { get; set; }
    }

    [JsonObject("value")]
    public class Value
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("boundingBox")]
        public List<double> BoundingBox { get; set; }
        [JsonProperty("confidence")]
        public double Confidence { get; set; }
    }

    [JsonObject("header")]
    public class Header
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("boundingBox")]
        public List<double> BoundingBox { get; set; }
        [JsonProperty("confidence")]
        public double Confidence { get; set; }
    }

    [JsonObject("entry")]
    public class Entry
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("boundingBox")]
        public List<double> BoundingBox { get; set; }
        [JsonProperty("confidence")]
        public double Confidence { get; set; }
    }

    [JsonObject("error")]
    public class Error
    {
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}