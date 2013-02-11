using System;
using Gtk;
using System.Collections.Generic;
using System.Data;
using Npgsql;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();		
	}
	
	public void rellenarTabla() {
		
		NpgsqlConnection connection = new NpgsqlConnection("Server=127.0.0.1; Database=dbprueba2;" +
			 	"User Id=dbprueba; Password=sistemas;");
		
		connection.Open();
		
		IDbCommand command = connection.CreateCommand();
		command.CommandText="SELECT "+select.Text+" FROM "+from.Text;
		IDataReader dataReader = command.ExecuteReader();
		
		for(int i=0; i<dataReader.FieldCount; i++) {
			treeview.AppendColumn(dataReader.GetName(i), new CellRendererText(), "text", i);		
		}
		
		List<Type> types = new List<Type>();
		
		if(select.Text.Equals("*") && from.Text.Equals("articulos")) {
			types.Add(typeof(Int64));
			types.Add(typeof(string));
			types.Add(typeof(int));
			types.Add(typeof(Int64));
		}
		else if(select.Text.Equals("*") && from.Text.Equals("categoria")) {
			types.Add(typeof(Int64));
			types.Add(typeof(string));
		}
		else {
			if(select.Text.Contains("id")) {
				types.Add(typeof(Int64));
			}
			if(select.Text.Contains("nombre")) {
				types.Add(typeof(string));
			}
			if(select.Text.Contains("precio")) {
				types.Add(typeof(int));
			}
			if(select.Text.Contains("categoria")) {
				types.Add(typeof(Int64));
			}
		}
		
		ListStore liststore = new ListStore(types.ToArray());                                                                          
		treeview.Model = liststore;
			
		List<object> values = new List<object>();
		
//		if(select.Text.Equals("*") && from.Text.Equals("articulos")) {
//			values.Add(dataReader["id"]);
//			values.Add(dataReader["nombre"]);
//			values.Add(dataReader["precio"]);
//			values.Add(dataReader["categoria"]);
//		}
//		else if(select.Text.Equals("*") && from.Text.Equals("categoria")) {
//			values.Add(dataReader["id"]);
//			values.Add(dataReader["nombre"]);
//		}
//		else {
//			if(select.Text.Contains("id")) {
//				values.Add(dataReader["id"]);;
//			}
//			if(select.Text.Contains("nombre")) {
//				values.Add(dataReader["nombre"]);
//			}
//			if(select.Text.Contains("precio")) {
//				values.Add(dataReader["precio"]);
//			}
//			if(select.Text.Contains("categoria")) {
//				values.Add(dataReader["categoria"]);
//			}
//		}
		
		
		while (dataReader.Read()) {
			
			values = new List<object>();
		
			if(select.Text.Equals("*") && from.Text.Equals("articulos")) {
				values.Add(dataReader["id"]);
				values.Add(dataReader["nombre"]);
				values.Add(dataReader["precio"]);
				values.Add(dataReader["categoria"]);
			}
			else if(select.Text.Equals("*") && from.Text.Equals("categoria")) {
				values.Add(dataReader["id"]);
				values.Add(dataReader["nombre"]);
			}
			else {
				if(select.Text.Contains("id")) {
					values.Add(dataReader["id"]);;
				}
				if(select.Text.Contains("nombre")) {
				values.Add(dataReader["nombre"]);
				}
				if(select.Text.Contains("precio")) {
				values.Add(dataReader["precio"]);
				}
				if(select.Text.Contains("categoria")) {
				values.Add(dataReader["categoria"]);
				}
			}
			
			liststore.AppendValues(values.ToArray());
		}
			
		dataReader.Close();
		connection.Close();
		
	}
	
	public void borrarTabla() {
		while (treeview.GetColumn(0)!=null) {
			treeview.RemoveColumn(treeview.GetColumn(0));
		}
		
		//COMO LO HA HECHO LUIS:
		//TreeViewColumn [] treeViewColumns = treeview.Columns;
		//foreach (TreeViewColumn treeViewColumn in treeViewColumns) {
		//  treeview.RemoveColumn(treeViewColumn);
		//}
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnButtonClicked (object sender, System.EventArgs e)
	{
		borrarTabla();
		rellenarTabla();
	}
}
