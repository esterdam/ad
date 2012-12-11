using System;
using Gtk;

public delegate int MyFuction (int a, int b);

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		
		MyFuction f;
		MyFuction[]functions = new MyFuction[]{suma, resta, multiplica};
		int random = new Random().Next(3);
		
		f = functions[random];
		
		Console.WriteLine("f={0}", f(5,3));
		
		//ComboBox
		CellRenderer cellRenderer = new CellRendererText();
		comboBox.PackStart(cellRenderer,false); //expand=false
		comboBox.AddAttribute(cellRenderer, "text", 1);
		
		ListStore listStore = new ListStore(typeof(string),typeof(string));
		comboBox.Model = listStore;
		
		listStore.AppendValues("1","Valor uno");
		listStore.AppendValues("2","Valor dos");
		listStore.AppendValues("3","Valor tres");
		
		
		comboBox.Changed += delegate {
			Console.WriteLine ("comboBox.Changed");
			TreeIter treeIter;
			if(comboBox.GetActiveIter (out treeIter)) { //Item seleccionado
				object value = listStore.GetValue(treeIter, 0);
				Console.WriteLine("comboBox.Changed value={0}",value);
			}
		};
		
		comboBox.Changed += comboBoxChanged;
			
	}
	
	private void comboBoxChanged (object obj, EventArgs args) { 
		ListStore listStore = (ListStore)comboBox.Model;
		TreeIter treeIter;
		
		if(comboBox.GetActiveIter (out treeIter)) {
			object value = listStore.GetValue(treeIter, 1);
			Console.WriteLine("comboBox.Change value={0}", value);
		}
	}
	
	private int suma(int a, int b) {
		return a+b;
	}
	
	private int resta(int a, int b) {
		return a-b;
	}
	
	private int multiplica(int a, int b) {
		return a*b;
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}
