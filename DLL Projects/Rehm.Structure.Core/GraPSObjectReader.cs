using BaseLibrary.Database;
using BaseLibrary.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rehm.Structure.Core
{
    public class GraPSObjectReader
    {
        private readonly DatabaseConnection _databaseConnection;
        public GraPSObjectReader(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public List<GraPSHaltung> ReadHaltungen()
        {
            var haltungen = new List<GraPSHaltung>();
            if (!_databaseConnection.IsOpen)
                _databaseConnection.Open();
            using (var result = _databaseConnection.Select("SELECT * FROM tabHalGeometrie"))
            {
                foreach (DataRow row in result.DataSet.Tables[0].Rows)
                {
                    haltungen.Add(HaltungFromDataRow(row));
                }
            }
            return haltungen;
        }

        private GraPSHaltung HaltungFromDataRow(DataRow row)
        {
            return new GraPSHaltung
            {
                Bezeichnung = row.SafeTo<string>("Haltungsbezeichnung"),
                RechtswertSohleOben = row.SafeTo<double>("RechtswertSohleOben"),
                HochwertSohleOben = row.SafeTo<double>("HochwertSohleOben"),
                SohlhöheVonSchacht = row.SafeTo<float>("SohlhöheVonSchacht"),
                RechtswertSohleUnten = row.SafeTo<double>("RechtswertSohleUnten"),
                HochwertSohleUnten = row.SafeTo<double>("HochwertSohleUnten"),
                SohlhöheBisSchacht = row.SafeTo<float>("SohlhöheBisSchacht")
            };
        }
    }
}
