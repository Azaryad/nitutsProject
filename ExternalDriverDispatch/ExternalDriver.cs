using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace ExternalDriverDispatch
{
    /// <summary>
    /// נהג עצמאי / חיצוני שמקבל הצעות נסיעה ב-WhatsApp.
    /// נטען אחרי Region (הקשר רבים-לרבים מנוהל ב-ExternalDriverRegion).
    /// </summary>
    public class ExternalDriver
    {
        // =====================================================================
        // שדות
        // =====================================================================
        private int driverId;
        private string drivercode;
        private string name;
        private string phone;
        private string homeCity;
        private VehicleType vehicleType;
        private bool worksShabbat;
        private bool worksNights;
        private bool worksLongDistance;
        private bool active;

        // קשר רבים-לרבים עם Region (בזיכרון)
        private List<Region> regions;

        // =====================================================================
        // בנאי
        // =====================================================================
        public ExternalDriver(int id, string drivercode, string name, string phone,
            string homeCity, VehicleType vehicleType, bool worksShabbat, bool worksNights,
            bool worksLongDistance, bool active, bool is_new)
        {
            this.driverId = id;
            this.drivercode = drivercode;
            this.name = name;
            this.phone = phone;
            this.homeCity = homeCity;
            this.vehicleType = vehicleType;
            this.worksShabbat = worksShabbat;
            this.worksNights = worksNights;
            this.worksLongDistance = worksLongDistance;
            this.active = active;
            this.regions = new List<Region>();
            if (is_new)
            {
                this.createExternalDriver();
                Program.ExternalDrivers.Add(this);
            }
        }

        // =====================================================================
        // Getters & Setters
        // =====================================================================
        public int getId() { return this.driverId; }
        public string getDrivercode() { return this.drivercode; }
        public string getName() { return this.name; }
        public string getPhone() { return this.phone; }
        public string getHomeCity() { return this.homeCity; }
        public VehicleType getVehicleType() { return this.vehicleType; }
        public bool getWorksShabbat() { return this.worksShabbat; }
        public bool getWorksNights() { return this.worksNights; }
        public bool getWorksLongDistance() { return this.worksLongDistance; }
        public bool getActive() { return this.active; }

        public void setDrivercode(string v) { this.drivercode = v; }
        public void setName(string v) { this.name = v; }
        public void setPhone(string v) { this.phone = v; }
        public void setHomeCity(string v) { this.homeCity = v; }
        public void setVehicleType(VehicleType v) { this.vehicleType = v; }
        public void setWorksShabbat(bool v) { this.worksShabbat = v; }
        public void setWorksNights(bool v) { this.worksNights = v; }
        public void setWorksLongDistance(bool v) { this.worksLongDistance = v; }
        public void setActive(bool v) { this.active = v; }

        // =====================================================================
        // ניהול קשר עם אזורים (רבים-לרבים)
        // =====================================================================
        public List<Region> getRegions()
        {
            if (this.regions == null) this.regions = new List<Region>();
            return this.regions;
        }

        public void addRegion(Region r)
        {
            if (r == null) return;
            if (this.regions == null) this.regions = new List<Region>();
            if (!this.regions.Contains(r))
            {
                this.regions.Add(r);
                r.addDriver(this);
            }
        }

        // =====================================================================
        // פעולות תחום (מתוך תרשים המחלקות)
        // =====================================================================

        /// <summary>מספר הנוסעים המרבי לפי סוג הרכב.</summary>
        public int getMaxPassengers()
        {
            switch (this.vehicleType)
            {
                case VehicleType.sedan:             return 4;
                case VehicleType.executive_minivan: return 6;
                case VehicleType.minivan:           return 7;
                case VehicleType.minibus_15:        return 15;
                case VehicleType.minibus_18:        return 18;
                default:                            return 0;
            }
        }

        /// <summary>
        /// בדיקת זכאות לנסיעה: הנהג פעיל וקיבולת הרכב מספיקה למספר הנוסעים.
        /// בהתאם ל-dispatch_flow (שלב 4) נטענים כל הנהגים הפעילים באזור; התאמת סוג הרכב
        /// אינה מסננת קשיחה אלא שיקול דירוג (ראו לוח השיבוץ).
        /// </summary>
        public bool isEligibleForTrip(Trip trip)
        {
            if (trip == null) return false;
            if (!this.active) return false;
            if (this.getMaxPassengers() < trip.getNumPassengers()) return false;
            return true;
        }

        // =====================================================================
        // פעולות מול בסיס הנתונים (CRUD)
        // =====================================================================
        public void createExternalDriver()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_ExternalDriver_create @driver_id, @drivercode, @name, @phone, " +
                              "@homeCity, @vehicleType, @worksShabbat, @worksNights, @worksLongDistance, @active";
            cmd.Parameters.AddWithValue("@driver_id", this.driverId);
            cmd.Parameters.AddWithValue("@drivercode", this.drivercode);
            cmd.Parameters.AddWithValue("@name", this.name);
            cmd.Parameters.AddWithValue("@phone", this.phone);
            cmd.Parameters.AddWithValue("@homeCity", this.homeCity);
            cmd.Parameters.AddWithValue("@vehicleType", VehicleTypeHelper.ToDb(this.vehicleType));
            cmd.Parameters.AddWithValue("@worksShabbat", this.worksShabbat);
            cmd.Parameters.AddWithValue("@worksNights", this.worksNights);
            cmd.Parameters.AddWithValue("@worksLongDistance", this.worksLongDistance);
            cmd.Parameters.AddWithValue("@active", this.active);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void updateExternalDriver()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_ExternalDriver_update @driver_id, @drivercode, @name, @phone, " +
                              "@homeCity, @vehicleType, @worksShabbat, @worksNights, @worksLongDistance, @active";
            cmd.Parameters.AddWithValue("@driver_id", this.driverId);
            cmd.Parameters.AddWithValue("@drivercode", this.drivercode);
            cmd.Parameters.AddWithValue("@name", this.name);
            cmd.Parameters.AddWithValue("@phone", this.phone);
            cmd.Parameters.AddWithValue("@homeCity", this.homeCity);
            cmd.Parameters.AddWithValue("@vehicleType", VehicleTypeHelper.ToDb(this.vehicleType));
            cmd.Parameters.AddWithValue("@worksShabbat", this.worksShabbat);
            cmd.Parameters.AddWithValue("@worksNights", this.worksNights);
            cmd.Parameters.AddWithValue("@worksLongDistance", this.worksLongDistance);
            cmd.Parameters.AddWithValue("@active", this.active);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void deleteExternalDriver()
        {
            Program.ExternalDrivers.Remove(this);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_ExternalDriver_delete @driver_id";
            cmd.Parameters.AddWithValue("@driver_id", this.driverId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        // =====================================================================
        // מתודות סטטיות — טעינה, חיפוש, מזהה הבא
        // =====================================================================
        public static void initExternalDrivers()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_ExternalDriver_get_all";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.ExternalDrivers = new List<ExternalDriver>();

            while (rdr.Read())
            {
                // 0=driver_id, 1=drivercode, 2=name, 3=phone, 4=homeCity, 5=vehicleType,
                // 6=worksShabbat, 7=worksNights, 8=worksLongDistance, 9=active
                int id = Convert.ToInt32(rdr.GetValue(0));
                string drivercode = rdr.GetValue(1).ToString();
                string name = rdr.GetValue(2).ToString();
                string phone = rdr.GetValue(3).ToString();
                string homeCity = rdr.GetValue(4).ToString();
                VehicleType vt = VehicleTypeHelper.FromDb(rdr.GetValue(5).ToString());
                bool worksShabbat = Convert.ToBoolean(rdr.GetValue(6));
                bool worksNights = Convert.ToBoolean(rdr.GetValue(7));
                bool worksLong = Convert.ToBoolean(rdr.GetValue(8));
                bool active = Convert.ToBoolean(rdr.GetValue(9));

                ExternalDriver d = new ExternalDriver(id, drivercode, name, phone, homeCity, vt,
                    worksShabbat, worksNights, worksLong, active, false);
                Program.ExternalDrivers.Add(d);
            }
            rdr.Close();
        }

        public static ExternalDriver seekExternalDriver(int id)
        {
            foreach (ExternalDriver d in Program.ExternalDrivers)
                if (d.getId() == id) return d;
            return null;
        }

        public static int getNextExternalDriverId()
        {
            int maxId = 0;
            foreach (ExternalDriver d in Program.ExternalDrivers)
                if (d.getId() > maxId) maxId = d.getId();
            return maxId + 1;
        }
    }
}
