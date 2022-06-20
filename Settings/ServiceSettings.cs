using SqlKata.Compilers;

namespace Diploma
{
	public class ServiceSettings
	{
		public static string ConnectionString { get; set; } = ($"Server=localhost; Port=5432; User Id=postgres; Password=69opeleb; Database=UdSU;");
		public static PostgresCompiler compiler = new PostgresCompiler();

	}
}
