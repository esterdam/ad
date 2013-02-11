using System;
using Gtk;

namespace PDbPrueba2
{
	
	class PDbPrueba2
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			
			Application.Run ();
		}
		
	}
}

