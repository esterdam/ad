using System;
using Gtk;
using System.Data;
using Npgsql;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		fillComboBox();
	}
	
	private void fillComboBox() {
		
		CellRenderer cellRenderer = new CellRendererText();
		comboBox.PackStart(cellRenderer, false); //expand:false
		comboBox.AddAttribute(cellRenderer, "text", 0);
		
		CellRenderer cellRenderer2 = new CellRendererText();
		comboBox.PackStart(cellRenderer2, false); //expand:false
		comboBox.AddAttribute(cellRenderer2, "text", 1);
		
		ListStore listStore = new ListStore(typeof(string),typeof(string));
		
		string connectionString = "Server=localhost;Database=dbprueba2;User Id=dbprueba; Password=sistemas;";
		IDbConnection dbConnection = new NpgsqlConnection(connectionString);
		dbConnection.Open();
		
		IDbCommand dbCommand = dbConnection.CreateCommand();
		dbCommand.CommandText = "SELECT id, nombre FROM categoria";
		
		IDataReader dataReader = dbCommand.ExecuteReader();
		
		while(dataReader.Read()) {
			listStore.AppendValues(dataReader["id"].ToString(),dataReader["nombre"].ToString());
		}
		
		comboBox.Model = listStore;
		
		dataReader.Close();
		dbConnection.Close();
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}
