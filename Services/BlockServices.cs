using SqlKata.Execution;
using SqlKata.Compilers;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Configuration;
using Diploma;

class BlockService
{
    public static int Create(Blocks block)
    {
        using var connection = new NpgsqlConnection(ServiceSettings.ConnectionString);
        var db = new QueryFactory(connection, ServiceSettings.compiler);
        return db.Query("blocks").InsertGetId<int>(new { Login = block.Block_Id, Pass = block.Block_Elder });
    }

    public static void Delete(int block_id)
    {
        using var connection = new NpgsqlConnection(ServiceSettings.ConnectionString);
        var db = new QueryFactory(connection, ServiceSettings.compiler);
        db.Query("blocks").Where("block_id", block_id).Delete();
    }

    public static Blocks GetById(int block_id)
    {
        using var connection = new NpgsqlConnection(ServiceSettings.ConnectionString);
        var db = new QueryFactory(connection, ServiceSettings.compiler);
        return db.Query("blocks").Where("block_id", block_id).First<Blocks>();
    }

    public static Blocks GetByElder(int block_elder)
    {
        using var connection = new NpgsqlConnection(ServiceSettings.ConnectionString);
        var db = new QueryFactory(connection, ServiceSettings.compiler);
        return db.Query("blocks").Where("block_elder", block_elder).First<Blocks>();
    }

    public static void Update(Blocks block)
    {
        using var connection = new NpgsqlConnection(ServiceSettings.ConnectionString);
        var db = new QueryFactory(connection, ServiceSettings.compiler);
        db.Query("blocks").Where("block_id", block.Block_Id).Update(new { Id = block.Block_Id, Elder = block.Block_Elder });
    }
}