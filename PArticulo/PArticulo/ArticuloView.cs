using Gtk;
using Serpis.Ad;
using PAdSerpis;
using System;
using System.Data;
using Npgsql;

namespace PArticulo
{
	public partial class ArticuloView : Gtk.Window
	{
		private IDbConnection dbConnection;
		public ArticuloView (long id) : base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
			
			dbConnection = ApplicationContext.Instance.DbConnection;
			
			if(id==0) {
				nuevo();
			}
			else {
				editar(id);
			}
			
		}
		
		private void nuevo() {
			//inicializo los controles que quiera
			entryNombre.Text = "Pon el nombre";
			spinButtonPrecio.Value = 1;

			saveAction.Activated += delegate {
				Console.WriteLine("saveAction.Activated");

				IDbCommand dbCommand = dbConnection.CreateCommand ();
				dbCommand.CommandText = "insert into articulos (nombre, precio) values (:nombre, :precio)";

				DbCommandExtensions.AddParameter (dbCommand, "nombre", entryNombre.Text);
				DbCommandExtensions.AddParameter (dbCommand, "precio", Convert.ToInt32 (spinButtonPrecio.Value ));

				dbCommand.ExecuteNonQuery ();

				Destroy ();
			};
		}
		
		private void editar(long id) {
		
			object comboBoxValue;

			IDbCommand dbCommand = dbConnection.CreateCommand();
			dbCommand.CommandText = string.Format ("select * from articulos where id={0}", id);

			//Llenar opciones comboBox
			ListStore listStore = new ListStore(typeof(string));

			IDbCommand dbCommand1 = ApplicationContext.Instance.DbConnection.CreateCommand();
			dbCommand1.CommandText = string.Format ("select nombre from categoria");

			IDataReader dataReader1 = dbCommand1.ExecuteReader ();

			while(dataReader1.Read()) {
				listStore.AppendValues((string)dataReader1["nombre"]);
			}

			comboBoxCategoria.Model = listStore;

			dataReader1.Close();
			//Fin opciones comboBox

			IDataReader dataReader = dbCommand.ExecuteReader ();
			dataReader.Read ();

			entryNombre.Text = (string)dataReader["nombre"];
			spinButtonPrecio.Value = Convert.ToDouble( (int)dataReader["precio"] );
			Int64 categoria = (Int64)dataReader["categoria"];

			dataReader.Close ();

			comboBoxCategoria.Changed += delegate {
				TreeIter treeIter;
				if(comboBoxCategoria.GetActiveIter (out treeIter)) { //Item seleccionado
					comboBoxValue = listStore.GetValue(treeIter, 0);

					dbCommand1.CommandText = string.Format ("select id from categoria where nombre='{0}'", comboBoxValue.ToString());
					dataReader1 = dbCommand1.ExecuteReader ();

					dataReader1.Read();
					categoria = (Int64)dataReader1["id"];
					dataReader1.Close();
				}
			};

			saveAction.Activated += delegate {
				Console.WriteLine("saveAction.Activated");

				IDbCommand dbUpdateCommand = ApplicationContext.Instance.DbConnection.CreateCommand ();
				dbUpdateCommand.CommandText = "update articulos set nombre=:nombre, precio=:precio, categoria=:categoria where id=:id";

				DbCommandExtensions.AddParameter (dbUpdateCommand, "nombre", entryNombre.Text);
				DbCommandExtensions.AddParameter (dbUpdateCommand, "precio", Convert.ToInt32 (spinButtonPrecio.Value ));
				DbCommandExtensions.AddParameter (dbUpdateCommand, "categoria", categoria );
				DbCommandExtensions.AddParameter (dbUpdateCommand, "id", id);

				dbUpdateCommand.ExecuteNonQuery ();

				Destroy ();
			};
			
		}
		
	}
}

