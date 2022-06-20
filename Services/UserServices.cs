using SqlKata.Execution;
using SqlKata.Compilers;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Configuration;
using Diploma;

class UserService : IUserService
{
    public bool CheckAuth(string user_login, string user_pass)
    {
        using var connection = new NpgsqlConnection(ServiceSettings.ConnectionString);
        var db = new QueryFactory(connection, ServiceSettings.compiler);
        return db.Query("users").
        Where("user_login", user_login).
        Where("user_pass", user_pass).
        AsCount().First().count > 0;
    }

    public int Create(Users user)
    {
        using var connection = new NpgsqlConnection(ServiceSettings.ConnectionString);
        var db = new QueryFactory(connection, ServiceSettings.compiler);
        return db.Query("users").
        InsertGetId<int>(new
        {
            Name = user.User_Name,
            Login = user.User_Login,
            Pass = user.User_Pass,
            Block = user.User_Block
        });
    }

    public void Delete(int user_id)
    {
        using var connection = new NpgsqlConnection(ServiceSettings.ConnectionString);
        var db = new QueryFactory(connection, ServiceSettings.compiler);
        db.Query("users").
        Where("user_id", user_id).
        Delete();
    }

    public Users GetById(int user_id)
    {
        using var connection = new NpgsqlConnection(ServiceSettings.ConnectionString);
        var db = new QueryFactory(connection, ServiceSettings.compiler);
        return db.Query("users").
        Where("user_id", user_id).
        First<Users>();
    }

    public Users GetByLogin(string user_login)
    {
        using var connection = new NpgsqlConnection(ServiceSettings.ConnectionString);
        var db = new QueryFactory(connection, ServiceSettings.compiler);
        return db.Query("users").
        Where("user_login", user_login).
        First<Users>();
    }

    public Users EldestByUser(string user_login)
    {
        using var connection = new NpgsqlConnection(ServiceSettings.ConnectionString);
        var db = new QueryFactory(connection, ServiceSettings.compiler);
        return db.Query("users").
        LeftJoin("blocks", j => j.On("users.User_Block", "blocks.Block_Id")).
        Where("user_login", user_login).
        First<Users>();
    }

    public Users EldestByBlock(int block_id)
    {
        using var connection = new NpgsqlConnection(ServiceSettings.ConnectionString);
        var db = new QueryFactory(connection, ServiceSettings.compiler);
        return db.Query("users").
        LeftJoin("blocks", j => j.On("users.user_id", "blocks.block_elder")).
        Where("block_id", block_id).
        First<Users>();
    }

    public IEnumerable<Users> GetAll()
    {
        using var connection = new NpgsqlConnection(ServiceSettings.ConnectionString);
        var db = new QueryFactory(connection, ServiceSettings.compiler);
        return db.Query("users").
        Get<Users>();
    }


    public void Update(Users user)
    {
        using var connection = new NpgsqlConnection(ServiceSettings.ConnectionString);
        var db = new QueryFactory(connection, ServiceSettings.compiler);
        db.Query("users").
        Where("user_id", user.User_Id).
        Update(new
        {
            Name = user.User_Name,
            Login = user.User_Login,
            Pass = user.User_Pass,
            Block = user.User_Block
        });
    }

}