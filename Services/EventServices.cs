using SqlKata.Execution;
using SqlKata.Compilers;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Configuration;
using Diploma;

class EventService
{
    public static int Create(Events events)
    {
        using var connection = new NpgsqlConnection(ServiceSettings.ConnectionString);
        var db = new QueryFactory(connection, ServiceSettings.compiler);
        return db.Query("events")
        .InsertGetId<int>(new { Name = events.Event_Name, Date = events.Event_Date, Desc = events.Event_Description, Place = events.Event_Place });
    }

    public static void Delete(int event_id)
    {
        using var connection = new NpgsqlConnection(ServiceSettings.ConnectionString);
        var db = new QueryFactory(connection, ServiceSettings.compiler);
        db.Query("events")
        .Where("event_id", event_id).Delete();
    }

    public static Events GetById(int event_id)
    {
        using var connection = new NpgsqlConnection(ServiceSettings.ConnectionString);
        var db = new QueryFactory(connection, ServiceSettings.compiler);
        return db.Query("events")
        .Where("event_id", event_id).First<Events>();
    }



    public static void Update(Events events)
    {
        using var connection = new NpgsqlConnection(ServiceSettings.ConnectionString);
        var db = new QueryFactory(connection, ServiceSettings.compiler);
        db.Query("events")
        .Where("event_id", events.Event_Id)
        .Update(new { Name = events.Event_Name, Date = events.Event_Date, Desc = events.Event_Description, Place = events.Event_Place });
    }

    public static IEnumerable<Events> GetAll()
    {
        using var connection = new NpgsqlConnection(ServiceSettings.ConnectionString);
        var db = new QueryFactory(connection, ServiceSettings.compiler);
        return db.Query("events").
        Get<Events>();
    }

}