using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using play.common.settings;
using Play.Common;

namespace play.common.mongodb
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(MongoDB.Bson.BsonType.String));
            services.AddSingleton(serviceP =>
                   {
                       var configuration = serviceP.GetService<IConfiguration>();
                       var settings = configuration?.GetSection(nameof(Servicesettings)).Get<Servicesettings>();
                       var mongoDBsettings = configuration?.GetSection(nameof(MongoDBSettings)).Get<MongoDBSettings>();

                       var mongoClient = new MongoClient(mongoDBsettings?.ConnectionString);
                       return mongoClient.GetDatabase(settings?.ServiceName);

                   });
            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string CollectionName) where T : IEntity
        {
            services.AddSingleton<IRepository<T>>(ServiceProvider =>
                {
                    var database = ServiceProvider.GetService<IMongoDatabase>();
                    return new MongoRepository<T>(database, CollectionName);
                });
            return services;
        }
    }
}