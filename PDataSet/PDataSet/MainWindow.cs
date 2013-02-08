using System;
using Gtk;
using System.Data;
using Npgsql;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
	
	protected void OnExecuteActionActivated (object sender, System.EventArgs e)
	{
		string connectionString = "Server=localhost;Database=dbprueba2;User Id=dbprueba;Password=sistemas;";
		IDbConnection dbConnection = new NpgsqlConnection(connectionString);
		IDbCommand selectCommand = dbConnection.CreateCommand();
		selectCommand.CommandText = "select * from articulos";
		
		IDbDataAdapter dbDataAdapter = new NpgsqlDataAdapter();
		dbDataAdapter.SelectCommand = selectCommand;
		
		NpgsqlCommandBuilder commandBuilder = new NpgsqlCommandBuilder((NpgsqlDataAdapter)dbDataAdapter);
		dbConnection.Open();
		
		DataSet dataSet = new DataSet();
		
		dbDataAdapter.Fill(dataSet);
		Console.WriteLine("Table.Count={0}\n", dataSet.Tables.Count);
		
		foreach (DataTable dataTable in dataSet.Tables)
			show (dataTable);
		
		DataRow dataRow = dataSet.Tables[0].Rows[0];
		dataRow["nombre"] = DateTime.Now.ToString();
		
		Console.WriteLine ("\nTabla con los cambios:");
		show (dataSet.Tables[0]);
		
		IDbCommand comando = commandBuilder.GetUpdateCommand (dataSet.Tables[0].Rows[0]);
		
		//COSAS DE HOY DIA 18
		
		comando.ExecuteNonQuery ();
	}
	
	protected void show(DataTable dataTable) 
	{
		//foreach (DataColumn dataColumn in dataTable.Columns)
		//	Console.WriteLine("Column.Name={0}",dataColumn.ColumnName);
		//Console.WriteLine();
		foreach (DataRow dataRow in dataTable.Rows) {
			foreach (DataColumn dataColumn in dataTable.Columns)
				Console.Write("[{0}={1}] ",dataColumn.ColumnName,dataRow[dataColumn]);
			Console.WriteLine();
		}
		
	}
	
}
