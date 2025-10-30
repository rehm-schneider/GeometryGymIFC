using BaseLibrary.Database;
using Rehm.Structure.IfcExport.Domain;
using Rehm.Structure.IfcExport.Readers;
using Rehm.Structure.IfcExport.Services;
using Rehm.Structure.IfcExport.Writers;

namespace WinFormsApp1
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			string fileName = @"c:\Users\Schneider\Documents\Rehm\GraPS\Beispiel\beispiel.rdb";
			using (var rehmDb = DatabaseConnectionFactory.Create(fileName))
			{
				rehmDb.Open();
				var graPSReader = new Rehm.Structure.Core.GraPSObjectReader(rehmDb);
				var snReader = new GraPsSewageNetworkReader(graPSReader);
				var writer = new IfcSewageNetworkWriter();
				var exporter = new SewageNetworkIfcExporter(snReader, writer);
				exporter.Export("Beispielprojekt", @"beispiel.ifc");
			}
		}
	}
}
