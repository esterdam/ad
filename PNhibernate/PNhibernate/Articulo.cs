using System;

namespace Serpis.Ad
{
	public class Articulo
	{
		public virtual long Id {get; set;}
		public virtual string Nombre {get; set;}
		public virtual int Precio {get; set;}
		public virtual Categoria Categoria {get; set;}
	}
}

