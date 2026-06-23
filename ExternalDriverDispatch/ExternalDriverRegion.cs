using System;
using Microsoft.Data.SqlClient;

namespace ExternalDriverDispatch
{
    /// <summary>
    /// טבלת קישור (Junction) עבור הקשר רבים-לרבים ExternalDriver ↔ Region.
    /// אין לקשר תכונות משלו ולכן זו אינה מחלקת-קישור (Association Class) אלא קישור בלבד:
    /// אין מופעים ואין רשימה ב-Program. הטעינה רק מחווטת את ההפניות בשני הצדדים בזיכרון.
    /// נטענת אחרי Region ו-ExternalDriver (תלויה בשניהם), ולפני Trip ו-Offer.
    /// </summary>
    public static class ExternalDriverRegion
    {
        public static void initExternalDriverRegions()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_ExternalDriverRegion_get_all";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            while (rdr.Read())
            {
                // 0=driver_id, 1=region_id
                int driverId = Convert.ToInt32(rdr.GetValue(0));
                int regionId = Convert.ToInt32(rdr.GetValue(1));

                ExternalDriver driver = ExternalDriver.seekExternalDriver(driverId);
                Region region = Region.seekRegion(regionId);

                // addRegion מחווט גם את הצד ההפוך (region.addDriver)
                if (driver != null && region != null)
                    driver.addRegion(region);
            }
            rdr.Close();
        }
    }
}
