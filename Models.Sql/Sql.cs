using MyAirport.Pim.Entities;
using MyAirport.Pim.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace MyAirport.Pim.Models
{

    public class Sql : AbstractDefinition
    {

        string cnx = ConfigurationManager.ConnectionStrings["MyAiport.Pim.Settings.DbConnect"].ConnectionString;

        private string commandGetBagageIata = "select b.ID_BAGAGE, b.CODE_IATA, b.COMPAGNIE, b.LIGNE, b.DATE_CREATION, " +
                    "b.ESCALE, cc.PRIORITAIRE, " +
                    "cast (if(b.CONTINUATION='N',0,1) as bit) as continuation, " +
                    "cast (if(bp.PARTICULARITE is null,0,1)as bit) as 'RUSH' from BAGAGE b " +
                    "left outer join BAGAGE_A_POUR_PARTICULARITE bap on b.ID_BAGAGE = bap.ID_BAGAGE " +
                    "left outer join BAGAGE_PARTICULARITE bp on bp.ID_PARTICULARITE = bap.ID_PARTICULARITE " +
                    "and bp.PARTICULARITE = 'RUSH' " +
                    "LEFT OUTER JOIN CONPAGNIE c on c.CODE_IATA = b.COMPAGNIE " +
                    "LEFT OUTER JOIN COMPAGNIE_CLASSE cc on cc.ID_COMPAGNIE = c.ID_COMPAGNIE " +
                    "and cc.CLASSE = c.CLASSE " +
                    "where b.CODE_IATA = @iata";



        string commandGetBagageId = "select b.ID_BAGAGE, b.CODE_IATA, b.COMPAGNIE, b.LIGNE, b.DATE_CREATION, " +
                    "b.ESCALE, cc.PRIORITAIRE, " +
                    "cast (iif(b.CONTINUATION='N',0,1) as bit) as continuation, " +
                    "cast (iif(bp.PARTICULARITE is null,0,1)as bit) as 'RUSH' from BAGAGE b " +
                    "left outer join BAGAGE_A_POUR_PARTICULARITE bap on b.ID_BAGAGE = bap.ID_BAGAGE " +
                    "left outer join BAGAGE_PARTICULARITE bp on bp.ID_PARTICULARITE = bap.ID_PARTICULARITE " +
                    "and bp.PARTICULARITE = 'RUSH' " +
                    "LEFT OUTER JOIN CONPAGNIE c on c.CODE_IATA = b.COMPAGNIE " +
                    "LEFT OUTER JOIN COMPAGNIE_CLASSE cc on cc.ID_COMPAGNIE = c.ID_COMPAGNIE " +
                    "and cc.CLASSE = c.CLASSE " +
                    "where b.id = @id";

        public BagageDefinition NewBagage(SqlDataReader sdr)
        {
            BagageDefinition bagRes = new BagageDefinition();

            bagRes.IdBagage = sdr.GetInt32(sdr.GetOrdinal("id_bagage"));
            bagRes.CodeIata = sdr.GetString(sdr.GetOrdinal("code_iata"));
            bagRes.Compagnie = sdr.GetString(sdr.GetOrdinal("compagnie"));
            bagRes.Ligne = sdr.GetString(sdr.GetOrdinal("ligne"));
            bagRes.DateVol = sdr.GetDateTime(sdr.GetOrdinal("date_creation"));
            bagRes.Itineraire = sdr.GetString(sdr.GetOrdinal("escale"));
            bagRes.Prioritaire = sdr.GetBoolean(sdr.GetOrdinal("prioritaire"));
            bagRes.EnContinuation = sdr.GetBoolean(sdr.GetOrdinal("continuation"));
            bagRes.Rush = sdr.GetBoolean(sdr.GetOrdinal("rush"));

            return bagRes;
        }


        public override BagageDefinition GetBagage(int idBagage)
        {
            BagageDefinition bagRes = null;
            using (SqlConnection cnx = new SqlConnection())
            {
                SqlCommand cmd = new SqlCommand(this.commandGetBagageId, cnx);

                cmd.Parameters.AddWithValue("@id", idBagage);

                cnx.Open();

                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    if (sdr.Read())
                    {
                        bagRes = NewBagage(sdr);
                    }
                }
                return bagRes;
            }
        }

        public override List<BagageDefinition> GetBagage(string iataBagage)
        {
            List<BagageDefinition> bagsRes = null;
            using (SqlConnection cnx = new SqlConnection())
            {
                SqlCommand cmd = new SqlCommand(this.commandGetBagageIata, cnx);

                cmd.Parameters.AddWithValue("@iata", iataBagage);

                cnx.Open();

                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        bagsRes.Add(NewBagage(sdr));
                    }
                }
                return bagsRes;
            }
        }
    }

    public  abstract class AbstractDefinition
    {
        public abstract BagageDefinition GetBagage(int idBagage);
        public abstract List<BagageDefinition> GetBagage(string codeIataBagage);
    }
}

