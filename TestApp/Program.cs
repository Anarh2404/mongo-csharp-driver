using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            //var db1 = client.GetDatabase("TestDb");
            //var coll1 = db1.GetCollection<Data>("TestCollection3");

         //   await coll1.InsertOneAsync(new Data { Age = 42, Name = "", Name2 = null });
            //var result232 = await coll1.Find(FilterDefinition<Data>.Empty).ToListAsync();



            var db = client.GetDatabase("TestDb");
            var coll = db.GetCollection<GeoIp>("TestCollection5");


            //var count = 10000;
            //await Concurrent(coll, count, FilterDefinition<GeoIp>.Empty);
            //await Sequential(coll, count, FilterDefinition<GeoIp>.Empty);
            //await Concurrent(coll, count, FilterDefinition<GeoIp>.Empty);
            //await Sequential(coll, count, FilterDefinition<GeoIp>.Empty);
            //await Concurrent(coll, count, FilterDefinition<GeoIp>.Empty);
            //await Sequential(coll, count, FilterDefinition<GeoIp>.Empty);



            //await Task.Delay(TimeSpan.FromSeconds(5));
           // var result22424 = await coll.Find(global => global.Id == new ObjectId("5f987814bf344ec7cc57294b")).ToListAsync();
        //    await Task.Delay(TimeSpan.FromSeconds(5));
            Console.WriteLine();
            Console.WriteLine("**********************************************");
            Console.WriteLine();
            //var result12 = await coll.Find(FilterDefinition<GeoIp>.Empty).ToListAsync();
            var resultasda = await coll.Find(FilterDefinition<GeoIp>.Empty).ToListAsync();
            var resultasd2a = await coll.Find(FilterDefinition<GeoIp>.Empty).FirstOrDefaultAsync();
            //for (int i = 0; i < 5000; i++)
            //{
            var data = new GeoIp
                {
                    city = "St Petersburg",
                    country = "Russia",
                    countryCode = "RU",
                    isp = "NevalinkRoute",
                    lat = 59.8944f,
                    lon = 30.2642f,
                    org = "Nevalink Ltd.",
                    query = "31.134.191.87",
                    region = "SPE",
                    regionName = "St.-Petersburg",
                    status = "success",
                    timezone = "Europe/Moscow",
                    zip = 190000
                };
                await coll.InsertOneAsync(data);
            //}

            //var resultasda = await coll.Find(FilterDefinition<GeoIp>.Empty).ToListAsync();
            //var resultasd2a = await coll.Find(FilterDefinition<GeoIp>.Empty).FirstOrDefaultAsync();


            for (int i = 0; i < 100; i++)
            {
                var result1 = await coll.Find(FilterDefinition<GeoIp>.Empty).ToListAsync();
            }

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                var result1 = await coll.Find(FilterDefinition<GeoIp>.Empty).ToListAsync();
            }
            sw.Stop();

            Console.WriteLine(sw.Elapsed);

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




    class Data
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public int Age { get; set; }
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
