using BaseLibrary.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rehm.Structure.Core
{
	public class GraPSHaltung
	{
		public string Bezeichnung { get; set; }
		public double RechtswertSohleOben { get; set; }
		public double HochwertSohleOben { get; set; }
		public float SohlhöheVonSchacht { get; set; }
		public double RechtswertSohleUnten { get; set; }
		public double HochwertSohleUnten { get; set; }
		public float SohlhöheBisSchacht { get; set; }
	}
}