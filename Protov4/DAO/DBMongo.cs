using MongoDB.Driver;

namespace Protov4.DAO
{
    public class DBMongo
    {
        private readonly IMongoDatabase _db;
        // Obtiene la cadena de conexión de MongoDB y el nombre de la base de datos desde la configuración
        public DBMongo(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDBConnection");
            var client = new MongoClient(connectionString);
            var databaseName = configuration.GetConnectionString("DatabaseName");
            _db = client.GetDatabase(databaseName);
        }
        // Obtiene la instancia de la base de datos de MongoDB
        public IMongoDatabase GetDatabase()
        {
            return _db;
        }
    }
}
