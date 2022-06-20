using SqlKata.Execution;
using SqlKata.Compilers;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Configuration;
using Diploma;

class PersonalService
{
    public static int Create(PersonalQuestion question)
    {
        using var connection = new NpgsqlConnection(ServiceSettings.ConnectionString);
        var db = new QueryFactory(connection, ServiceSettings.compiler);
        return db.Query("personal").InsertGetId<int>(new { personal_quest = question.personal_quest, personal_user = question.personal_user });
    }
}
