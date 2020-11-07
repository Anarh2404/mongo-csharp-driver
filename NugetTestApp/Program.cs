using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace NugetTestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new MongoClient("mongodb://login:pass@localhost:27017");
            var db = client.GetDatabase("TestDb");
            var coll = db.GetCollection<GeoIp>("TestCollection2");
            var filter = FilterDefinition<GeoIp>.Empty;
            //var filter = new FilterDefinitionBuilder<GeoIp>().Eq(g => g.Id, new ObjectId("5fa29b6db27162107ffbe7db"));

            //var count = 1000;
            ////await Concurrent(coll, count, filter);
            //await Sequential(coll, count, filter);
            ////await Concurrent(coll, count, filter);
            //await Sequential(coll, count, filter);
            ////await Concurrent(coll, count, filter);
            //await Sequential(coll, count, filter);

            Console.WriteLine("Done");
            //for (int i = 0; i < 10000; i++)
            //{
            //    var result = await coll.Find(FilterDefinition<GeoIp>.Empty).ToListAsync();
            //    Console.WriteLine(i);
            //}

            //for (int i = 0; i < 100; i++)
            //{
            //    var result1 = await coll.Find(FilterDefinition<GeoIp>.Empty).ToListAsync();
            //}

            //var sw = Stopwatch.StartNew();
            //for (int i = 0; i < 100; i++)
            //{
            //    var result1 = await coll.Find(FilterDefinition<GeoIp>.Empty).ToListAsync();
            //}
            //sw.Stop();

            //Console.WriteLine(sw.Elapsed);
        }

        private static async Task Concurrent<T>(IMongoCollection<T> collection, int count, FilterDefinition<T> filter)
        {
            var list = new List<Task>(count);
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                list.Add(collection.Find(filter).ToListAsync());
            }

            await Task.WhenAll(list);
            sw.Stop();
            Console.WriteLine("Concurrent: " + sw.Elapsed);
        }

        private static async Task Sequential<T>(IMongoCollection<T> collection, int count, FilterDefinition<T> filter)
        {
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                // using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                // {
                //     try
                //     {
                var result = await collection.Find(filter).ToListAsync();
                //     }
                //     catch (Exception e)
                //     {
                //         await Console.Out.WriteLineAsync(e.Message);
                //     }
                // }
                //
                // await Console.Out.WriteLineAsync(i.ToString());
            }

            sw.Stop();
            Console.WriteLine("Sequential: " + sw.Elapsed);
        }
    }


    public class GeoIp
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string status { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
        public string region { get; set; }
        public string regionName { get; set; }
        public string city { get; set; }
        public long zip { get; set; }
        public float lat { get; set; }
        public float lon { get; set; }
        public string timezone { get; set; }
        public string isp { get; set; }
        public string org { get; set; }
        public string query { get; set; }
    }
}
