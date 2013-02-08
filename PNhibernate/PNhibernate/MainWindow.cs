using System;
using Gtk;

using NHibernate;

using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

using Serpis.Ad;

using System.Collections;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		
		Configuration configuration = new Configuration();
		configuration.Configure ();
		configuration.SetProperty(NHibernate.Cfg.Environment.Hbm2ddlKeyWords, "none");
		configuration.AddAssembly(typeof(Categoria).Assembly);
		configuration.AddAssembly(typeof(Articulo).Assembly);
		
		new SchemaExport(configuration).Execute(true, false, false);
		
		ISessionFactory sessionFactory = configuration.BuildSessionFactory ();
		
		//updateCategoria(sessionFactory);
		
		//insertCategoria(sessionFactory);
		
		//loadArticulo(sessionFactory);
		
		ISession session = sessionFactory.OpenSession();
		ICriteria criteria = session.CreateCriteria (typeof(Categoria));
		IList list = criteria.List();
		Articulo articulo = (Articulo)session.Load (typeof(Articulo), 2L);
		foreach (Categoria categoria in list)
			Console.WriteLine ("Articulo Id={0} Nombre={1} Precio={2}", 
			                   articulo.Id, articulo.Nombre, articulo.Precio);
			if(articulo.Categoria == null) {
				Console.WriteLine ("Categoria=null");
			}
			else {
				Console.WriteLine ("Categoria={0}",articulo.Categoria.Nombre);
			}
		
		session.Close();
		
		sessionFactory.Close ();
	}
	
	private void loadArticulo(ISessionFactory sessionFactory) {
		using (ISession session = sessionFactory.OpenSession()) {
			Articulo articulo = (Articulo)session.Load (typeof(Articulo), 2L);
			Console.WriteLine ("Articulo Id={0} Nombre={1} Precio={2}", 
			                   articulo.Id, articulo.Nombre, articulo.Precio);
			if(articulo.Categoria == null) {
				Console.WriteLine ("Categoria=null");
			}
			else {
				Console.WriteLine ("Categoria={0}",articulo.Categoria.Nombre);
			}
		}
	}
	
//	private void updateCategoria(ISessionFactory sessionFactory) {
//		ISession session = sessionFactory.OpenSession();
//		try {
//			Categoria categoria = (Categoria)session.Load(typeof(Categoria), 2L);
//			Console.WriteLine("Categoria Id={0} Nombre={1}", categoria.Id, categoria.Nombre);
//			categoria.Nombre = DateTime.Now.ToString ();
//			session.SaveOrUpdate (categoria);
//			session.Flush ();
//		} finally {
//			session.Close ();
//		}
//	}
	
//	private void insertCategoria(ISessionFactory sessionFactory) {
//		ISession session = sessionFactory.OpenSession ();
//		try {
//			Categoria categoria = new Categoria();
//			categoria.Nombre = "Nueva " + DateTime.Now.ToString ();
//			session.SaveOrUpdate (categoria);
//			session.Flush ();
//		} finally {
//			session.Close();
//		}
//	}
	
	private void updateCategoria(ISessionFactory sessionFactory) {
		using (ISession session = sessionFactory.OpenSession() ) {
			Categoria categoria = (Categoria)session.Load(typeof(Categoria), 2L);
			Console.WriteLine("Categoria Id={0} Nombre={1}", categoria.Id, categoria.Nombre);
			categoria.Nombre = DateTime.Now.ToString ();
			session.SaveOrUpdate (categoria);
			session.Flush ();
		}
	}
	
	private void insertCategoria(ISessionFactory sessionFactory) {
	 	using (ISession session = sessionFactory.OpenSession () ) {
			Categoria categoria = new Categoria();
			categoria.Nombre = "Nueva " + DateTime.Now.ToString ();
			session.SaveOrUpdate (categoria);
			session.Flush ();
		} 
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
	
	protected void OnExecuteActionActivated(object sender, System.EventArgs e) 
	{
		throw new System.NotImplementedException ();
	}

}
