using System;
using System.Data;
using Gtk;
using Npgsql;
using System.Collections;

namespace PTreeView
{ 
	public class Columna
	{
		public Columna (int id, string nombre, string apellido1, string apellido2)
		{
			this.Id = id;
			this.Nombre = nombre;
			this.Apellido1 = apellido1;
			this.Apellido2 = apellido2;
		}
 
		public int Id;
		public string Nombre;
		public string Apellido1;
		public string Apellido2;
	}
	
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			TreeView();
			Application.Run ();
		}
		
		static ArrayList columnas;
 
		public static void TreeView ()
		{
					
			NpgsqlConnection connection = new NpgsqlConnection("Server=127.0.0.1; Database=dbprueba;" +
			 	"User Id=dbprueba; Password=sistemas;");
		
			connection.Open();
			
			IDbCommand command = connection.CreateCommand();
			command.CommandText="SELECT * FROM prueba";
			IDataReader dataReader = command.ExecuteReader();
			
			columnas = new ArrayList ();
 
			while (dataReader.Read()) {
				int ids = (int)dataReader["id"];
				string n = (string)dataReader["nombre"];
				string a1 = (string)dataReader["apellido1"];
				string a2 = (string)dataReader["apellido2"];
				
				columnas.Add (new Columna (ids,n,a1,a2));
			}
 
			Gtk.Window window = new Gtk.Window ("TreeView Example");
			window.SetSizeRequest (500,200);
 
			Gtk.TreeView tree = new Gtk.TreeView ();
			window.Add (tree);
 
			//-----------
			Gtk.TreeViewColumn columnaId = new Gtk.TreeViewColumn ();
			columnaId.Title = dataReader.GetName(0);
			Gtk.CellRendererText columnaIdCelda = new Gtk.CellRendererText ();
			columnaId.PackStart (columnaIdCelda, true);
			
			Gtk.TreeViewColumn columnaNom = new Gtk.TreeViewColumn ();
			columnaNom.Title = dataReader.GetName(1);
			Gtk.CellRendererText columnaNomCelda = new Gtk.CellRendererText ();
			columnaNom.PackStart (columnaNomCelda, true);
			
			Gtk.TreeViewColumn columnaAp1 = new Gtk.TreeViewColumn ();
			columnaAp1.Title = dataReader.GetName(2);
			Gtk.CellRendererText columnaAp1Celda = new Gtk.CellRendererText ();
			columnaAp1.PackStart (columnaAp1Celda, true);
			
			Gtk.TreeViewColumn columnaAp2 = new Gtk.TreeViewColumn ();
			columnaAp2.Title = dataReader.GetName(3);
			Gtk.CellRendererText columnaAp2Celda = new Gtk.CellRendererText ();
			columnaAp2.PackStart (columnaAp2Celda, true);
	 		//---------
			
			Gtk.ListStore listStore = new Gtk.ListStore (typeof (Columna));
			foreach (Columna col in columnas) {
				listStore.AppendValues (col);
			}
 
			for(int i=0; i<dataReader.FieldCount; i++) {
				
			}
			
			columnaId.SetCellDataFunc (columnaIdCelda, new Gtk.TreeCellDataFunc (RenderId));
			columnaNom.SetCellDataFunc (columnaNomCelda, new Gtk.TreeCellDataFunc (RenderNom));
			columnaAp1.SetCellDataFunc (columnaAp1Celda, new Gtk.TreeCellDataFunc (RenderAp1));
 			columnaAp2.SetCellDataFunc (columnaAp2Celda, new Gtk.TreeCellDataFunc (RenderAp2));
			
			tree.Model = listStore;	
	 
			tree.AppendColumn (columnaId);
			tree.AppendColumn (columnaNom);
	 		tree.AppendColumn (columnaAp1);
			tree.AppendColumn (columnaAp2);
			
			window.ShowAll ();
			
			dataReader.Close();
			connection.Close();
		}
 
		private static void RenderId (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Columna c = (Columna) model.GetValue (iter, 0);
			(cell as Gtk.CellRendererText).Text = Convert.ToString(c.Id);
			
			(cell as Gtk.CellRendererText).Foreground = "blue";
		}
	 
		private static void RenderNom (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Columna c = (Columna) model.GetValue (iter, 0);
			(cell as Gtk.CellRendererText).Text = c.Nombre;
			
			if (c.Nombre.StartsWith ("E") == true) {
				(cell as Gtk.CellRendererText).Foreground = "green";
			} else {
				(cell as Gtk.CellRendererText).Foreground = "black";
			}
		}
		
		private static void RenderAp1 (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Columna c = (Columna) model.GetValue (iter, 0);
 			(cell as Gtk.CellRendererText).Text = c.Apellido1;
			
			if(c.Apellido1.Contains("m") == true) {
				(cell as Gtk.CellRendererText).Foreground = "red";
			}
			else {
				(cell as Gtk.CellRendererText).Foreground = "pink";
			}
		}
		
		private static void RenderAp2 (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Columna c = (Columna) model.GetValue (iter, 0);
 
			(cell as Gtk.CellRendererText).Foreground = "white";
			(cell as Gtk.CellRendererText).Background = "purple";
			(cell as Gtk.CellRendererText).Text = c.Apellido2;
		}
	}
}

