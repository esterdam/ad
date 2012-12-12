using System;
using System.Data;

namespace PAdSerpis
{
	/// <summary>
	/// Db command extensions.
	/// </summary>
	public static class DbCommandExtensions
	{
		/// <summary>
		/// Adds the parameter.
		/// </summary>
		/// <param name='dbCommand'>
		/// Db command.
		/// </param>
		/// <param name='name'>
		/// Name.
		/// </param>
		/// <param name='value'>
		/// Value.
		/// </param>
		public static void AddParameter(IDbCommand dbCommand, string name, object value)
		{
			IDbDataParameter dbDataParameter = dbCommand.CreateParameter();
			dbDataParameter.ParameterName = name;
			dbDataParameter.Value = value;
			dbCommand.Parameters.Add (dbDataParameter);
		}
		
	}
}

