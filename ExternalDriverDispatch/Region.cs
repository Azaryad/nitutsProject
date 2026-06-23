using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace ExternalDriverDispatch
{
    /// <summary>
    /// אזור גאוגרפי לתפעול — משמש לסינון נהגים ולתור נסיעות לשיבוץ.
    /// ישות בסיסית: נטענת ראשונה (אין לה תלות ב-FK).
    /// </summary>
    public class Region
    {
        // =====================================================================
        // שדות
        // =====================================================================
        private int regionId;
        private string name;
        private string country;
        private string city;
        private DateTime createdAt;

        // קשרים בזיכרון (לא נשמרים ישירות בטבלת Region)
        private List<ExternalDriver> drivers; // קשר רבים-לרבים דרך ExternalDriverRegion
        private List<Trip> trips;             // קשר אחד-לרבים: לאזור יש תור נסיעות

        // =====================================================================
        // בנאי
        // is_new = true  → מופע חדש שנוצר ע"י המשתמש → נשמר ב-DB
        // is_new = false → מופע שנטען מה-DB → לא נשמר שוב
        // =====================================================================
        public Region(int id, string name, string country, string city, DateTime createdAt, bool is_new)
        {
            this.regionId = id;
            this.name = name;
            this.country = country;
            this.city = city;
            this.createdAt = createdAt;
            this.drivers = new List<ExternalDriver>();
            this.trips = new List<Trip>();
            if (is_new)
            {
                this.createRegion();
                Program.Regions.Add(this);
            }
        }

        // =====================================================================
        // Getters & Setters
        // =====================================================================
        public int getId() { return this.regionId; }
        public string getName() { return this.name; }
        public string getCountry() { return this.country; }
        public string getCity() { return this.city; }
        public DateTime getCreatedAt() { return this.createdAt; }

        public void setName(string v) { this.name = v; }
        public void setCountry(string v) { this.country = v; }
        public void setCity(string v) { this.city = v; }
        public void setCreatedAt(DateTime v) { this.createdAt = v; }

        // =====================================================================
        // ניהול קשרים בזיכרון
        // =====================================================================
        public List<ExternalDriver> getDrivers()
        {
            if (this.drivers == null) this.drivers = new List<ExternalDriver>();
            return this.drivers;
        }

        public void addDriver(ExternalDriver d)
        {
            if (d == null) return;
            if (this.drivers == null) this.drivers = new List<ExternalDriver>();
            if (!this.drivers.Contains(d)) this.drivers.Add(d);
        }

        public List<Trip> getTrips()
        {
            if (this.trips == null) this.trips = new List<Trip>();
            return this.trips;
        }

        public void addTrip(Trip t)
        {
            if (t == null) return;
            if (this.trips == null) this.trips = new List<Trip>();
            if (!this.trips.Contains(t)) this.trips.Add(t);
        }

        // =====================================================================
        // פעולות תחום (מתוך תרשים המחלקות)
        // =====================================================================

        /// <summary>הנהגים הפעילים המשויכים לאזור זה.</summary>
        public List<ExternalDriver> getActiveDrivers()
        {
            List<ExternalDriver> result = new List<ExternalDriver>();
            foreach (ExternalDriver d in getDrivers())
                if (d.getActive()) result.Add(d);
            return result;
        }

        /// <summary>נסיעות הממתינות לשיבוץ נהג (open או assigned_to_region) בתור האזור.</summary>
        public List<Trip> getOpenTrips()
        {
            List<Trip> result = new List<Trip>();
            foreach (Trip t in getTrips())
                if (t.getStatus() == TripStatus.open || t.getStatus() == TripStatus.assigned_to_region)
                    result.Add(t);
            return result;
        }

        /// <summary>הנהגים הפעילים באזור הזכאים לנסיעה נתונה.</summary>
        public List<ExternalDriver> getEligibleDrivers(Trip trip)
        {
            List<ExternalDriver> result = new List<ExternalDriver>();
            foreach (ExternalDriver d in getActiveDrivers())
                if (d.isEligibleForTrip(trip)) result.Add(d);
            return result;
        }

        // =====================================================================
        // פעולות מול בסיס הנתונים (CRUD)
        // =====================================================================
        public void createRegion()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Region_create @region_id, @name, @country, @city, @createdAt";
            cmd.Parameters.AddWithValue("@region_id", this.regionId);
            cmd.Parameters.AddWithValue("@name", this.name);
            cmd.Parameters.AddWithValue("@country", this.country);
            cmd.Parameters.AddWithValue("@city", this.city);
            cmd.Parameters.AddWithValue("@createdAt", this.createdAt);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void updateRegion()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Region_update @region_id, @name, @country, @city, @createdAt";
            cmd.Parameters.AddWithValue("@region_id", this.regionId);
            cmd.Parameters.AddWithValue("@name", this.name);
            cmd.Parameters.AddWithValue("@country", this.country);
            cmd.Parameters.AddWithValue("@city", this.city);
            cmd.Parameters.AddWithValue("@createdAt", this.createdAt);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void deleteRegion()
        {
            Program.Regions.Remove(this);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Region_delete @region_id";
            cmd.Parameters.AddWithValue("@region_id", this.regionId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        // =====================================================================
        // מתודות סטטיות — טעינה, חיפוש, מזהה הבא
        // =====================================================================
        public static void initRegions()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Region_get_all";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.Regions = new List<Region>();

            while (rdr.Read())
            {
                // עמודות: 0=region_id, 1=name, 2=country, 3=city, 4=createdAt
                int id = Convert.ToInt32(rdr.GetValue(0));
                string name = rdr.GetValue(1).ToString();
                string country = rdr.GetValue(2).ToString();
                string city = rdr.GetValue(3).ToString();
                DateTime createdAt = Convert.ToDateTime(rdr.GetValue(4));

                Region r = new Region(id, name, country, city, createdAt, false);
                Program.Regions.Add(r);
            }
            rdr.Close();
        }

        public static Region seekRegion(int id)
        {
            foreach (Region r in Program.Regions)
                if (r.getId() == id) return r;
            return null;
        }

        public static int getNextRegionId()
        {
            int maxId = 0;
            foreach (Region r in Program.Regions)
                if (r.getId() > maxId) maxId = r.getId();
            return maxId + 1;
        }
    }
}
