using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace ExternalDriverDispatch
{
    /// <summary>
    /// נסיעה שהתקבלה מ-Ride Control. המערכת מנהלת את שיבוץ הנסיעה לנהג חיצוני בלבד,
    /// ולא את ביצוע הנסיעה עצמה. נטענת אחרי Region (יש לה FK לאזור).
    /// </summary>
    public class Trip
    {
        // =====================================================================
        // שדות
        // =====================================================================
        private int tripId;
        private string externalBookingId;
        private string pickupAddress;
        private string dropoffAddress;
        private string pickupCity;
        private string dropoffCity;
        private DateTime pickupTime;
        private int numPassengers;
        private VehicleType vehicleType;
        private decimal priceToDriver;
        private TripStatus status;
        private DateTime createdAt;
        private Region region;   // קשר רבים-לאחד: כל נסיעה משויכת לאזור אחד (הפניה לאובייקט)

        // Maps-derived fields (Service 1). Default to the offline fallback (60 min / 0 km)
        // until enriched by IDriveInfoProvider. distanceKm drives the long-distance filter
        // and feeds the AI ranking prompt; estimatedDurationMinutes defines the time window.
        private double distanceKm;
        private int estimatedDurationMinutes;

        // How many offers were sent for this trip (cumulative across the forwarding chain).
        // A trip grabbed on the first offer is "attractive"; a high counter means many refusals.
        private int offerCounter;

        // קשר אחד-לרבים: לנסיעה עשויות להיות כמה פניות (שרשרת ההעברה)
        private List<Offer> offers;

        // =====================================================================
        // בנאי
        // =====================================================================
        public Trip(int id, string externalBookingId, string pickupAddress, string dropoffAddress,
            string pickupCity, string dropoffCity, DateTime pickupTime, int numPassengers,
            VehicleType vehicleType, decimal priceToDriver, TripStatus status, DateTime createdAt,
            Region region, bool is_new)
        {
            this.tripId = id;
            this.externalBookingId = externalBookingId;
            this.pickupAddress = pickupAddress;
            this.dropoffAddress = dropoffAddress;
            this.pickupCity = pickupCity;
            this.dropoffCity = dropoffCity;
            this.pickupTime = pickupTime;
            this.numPassengers = numPassengers;
            this.vehicleType = vehicleType;
            this.priceToDriver = priceToDriver;
            this.status = status;
            this.createdAt = createdAt;
            this.region = region;
            this.offers = new List<Offer>();
            this.distanceKm = 0;                 // Maps fallback default; enriched later
            this.estimatedDurationMinutes = 60;  // Maps fallback default
            this.offerCounter = 0;               // no offers sent yet
            if (is_new)
            {
                this.createTrip();
                if (region != null) region.addTrip(this);
                Program.Trips.Add(this);
            }
        }

        // =====================================================================
        // Getters & Setters
        // =====================================================================
        public int getId() { return this.tripId; }
        public string getExternalBookingId() { return this.externalBookingId; }
        public string getPickupAddress() { return this.pickupAddress; }
        public string getDropoffAddress() { return this.dropoffAddress; }
        public string getPickupCity() { return this.pickupCity; }
        public string getDropoffCity() { return this.dropoffCity; }
        public DateTime getPickupTime() { return this.pickupTime; }
        public int getNumPassengers() { return this.numPassengers; }
        public VehicleType getVehicleType() { return this.vehicleType; }
        public decimal getPriceToDriver() { return this.priceToDriver; }
        public TripStatus getStatus() { return this.status; }
        public DateTime getCreatedAt() { return this.createdAt; }
        public Region getRegion() { return this.region; }
        public double getDistanceKm() { return this.distanceKm; }
        public int getEstimatedDurationMinutes() { return this.estimatedDurationMinutes; }
        public int getOfferCounter() { return this.offerCounter; }

        public void setExternalBookingId(string v) { this.externalBookingId = v; }
        public void setPickupAddress(string v) { this.pickupAddress = v; }
        public void setDropoffAddress(string v) { this.dropoffAddress = v; }
        public void setPickupCity(string v) { this.pickupCity = v; }
        public void setDropoffCity(string v) { this.dropoffCity = v; }
        public void setPickupTime(DateTime v) { this.pickupTime = v; }
        public void setNumPassengers(int v) { this.numPassengers = v; }
        public void setVehicleType(VehicleType v) { this.vehicleType = v; }
        public void setPriceToDriver(decimal v) { this.priceToDriver = v; }
        public void setStatus(TripStatus v) { this.status = v; }
        public void setCreatedAt(DateTime v) { this.createdAt = v; }
        public void setRegion(Region v) { this.region = v; }
        public void setDistanceKm(double v) { this.distanceKm = v; }
        public void setEstimatedDurationMinutes(int v) { this.estimatedDurationMinutes = v; }
        public void setOfferCounter(int v) { this.offerCounter = v; }

        // =====================================================================
        // ניהול קשר עם פניות (Offers)
        // =====================================================================
        public List<Offer> getOffers()
        {
            if (this.offers == null) this.offers = new List<Offer>();
            return this.offers;
        }

        public void addOffer(Offer o)
        {
            if (o == null) return;
            if (this.offers == null) this.offers = new List<Offer>();
            if (!this.offers.Contains(o)) this.offers.Add(o);
        }

        // =====================================================================
        // פעולות מול בסיס הנתונים (CRUD)
        // =====================================================================
        public void createTrip()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Trip_create @trip_id, @externalBookingId, @pickupAddress, " +
                              "@dropoffAddress, @pickupCity, @dropoffCity, @pickupTime, @numPassengers, " +
                              "@vehicleType, @priceToDriver, @status, @createdAt, @region_id, " +
                              "@distanceKm, @estimatedDurationMinutes, @offerCounter";
            cmd.Parameters.AddWithValue("@trip_id", this.tripId);
            cmd.Parameters.AddWithValue("@externalBookingId", this.externalBookingId);
            cmd.Parameters.AddWithValue("@pickupAddress", this.pickupAddress);
            cmd.Parameters.AddWithValue("@dropoffAddress", this.dropoffAddress);
            cmd.Parameters.AddWithValue("@pickupCity", this.pickupCity);
            cmd.Parameters.AddWithValue("@dropoffCity", this.dropoffCity);
            cmd.Parameters.AddWithValue("@pickupTime", this.pickupTime);
            cmd.Parameters.AddWithValue("@numPassengers", this.numPassengers);
            cmd.Parameters.AddWithValue("@vehicleType", VehicleTypeHelper.ToDb(this.vehicleType));
            cmd.Parameters.AddWithValue("@priceToDriver", this.priceToDriver);
            cmd.Parameters.AddWithValue("@status", TripStatusHelper.ToDb(this.status));
            cmd.Parameters.AddWithValue("@createdAt", this.createdAt);
            cmd.Parameters.AddWithValue("@region_id", this.region != null ? (object)this.region.getId() : DBNull.Value);
            cmd.Parameters.AddWithValue("@distanceKm", this.distanceKm);
            cmd.Parameters.AddWithValue("@estimatedDurationMinutes", this.estimatedDurationMinutes);
            cmd.Parameters.AddWithValue("@offerCounter", this.offerCounter);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void updateTrip()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Trip_update @trip_id, @externalBookingId, @pickupAddress, " +
                              "@dropoffAddress, @pickupCity, @dropoffCity, @pickupTime, @numPassengers, " +
                              "@vehicleType, @priceToDriver, @status, @createdAt, @region_id, " +
                              "@distanceKm, @estimatedDurationMinutes, @offerCounter";
            cmd.Parameters.AddWithValue("@trip_id", this.tripId);
            cmd.Parameters.AddWithValue("@externalBookingId", this.externalBookingId);
            cmd.Parameters.AddWithValue("@pickupAddress", this.pickupAddress);
            cmd.Parameters.AddWithValue("@dropoffAddress", this.dropoffAddress);
            cmd.Parameters.AddWithValue("@pickupCity", this.pickupCity);
            cmd.Parameters.AddWithValue("@dropoffCity", this.dropoffCity);
            cmd.Parameters.AddWithValue("@pickupTime", this.pickupTime);
            cmd.Parameters.AddWithValue("@numPassengers", this.numPassengers);
            cmd.Parameters.AddWithValue("@vehicleType", VehicleTypeHelper.ToDb(this.vehicleType));
            cmd.Parameters.AddWithValue("@priceToDriver", this.priceToDriver);
            cmd.Parameters.AddWithValue("@status", TripStatusHelper.ToDb(this.status));
            cmd.Parameters.AddWithValue("@createdAt", this.createdAt);
            cmd.Parameters.AddWithValue("@region_id", this.region != null ? (object)this.region.getId() : DBNull.Value);
            cmd.Parameters.AddWithValue("@distanceKm", this.distanceKm);
            cmd.Parameters.AddWithValue("@estimatedDurationMinutes", this.estimatedDurationMinutes);
            cmd.Parameters.AddWithValue("@offerCounter", this.offerCounter);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void deleteTrip()
        {
            Program.Trips.Remove(this);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Trip_delete @trip_id";
            cmd.Parameters.AddWithValue("@trip_id", this.tripId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        // =====================================================================
        // פעולות מחזור-החיים (מכונת המצבים) — תרשים המחלקות + dispatch_flow
        // המשמר (guard) נאכף כאן ב-C#; ה-SP מבצע את העדכון בתוך טרנזקציה.
        // =====================================================================

        /// <summary>
        /// open → assigned_to_region: הנסיעה שויכה לאזור (שלב מקדים לשליחת הצעה).
        /// מציב את האזור ואת הסטטוס בעסקה אחת. מותר מ-open או assigned_to_region (החלפת אזור לפני הצעה).
        /// </summary>
        public bool assignRegion(Region r)
        {
            if (r == null) { MessageBox.Show("Select a region first", "Error", MessageBoxButtons.OK); return false; }
            if (this.status != TripStatus.open && this.status != TripStatus.assigned_to_region)
            {
                MessageBox.Show("A region can only be assigned to an 'Open' (or already assigned, not yet offered) trip", "Error", MessageBoxButtons.OK);
                return false;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Trip_assign_region @trip_id, @region_id";
            cmd.Parameters.AddWithValue("@trip_id", this.tripId);
            cmd.Parameters.AddWithValue("@region_id", r.getId());
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
            this.region = r;                                 // שיקוף בזיכרון
            this.status = TripStatus.assigned_to_region;
            return true;
        }

        /// <summary>assigned_to_region → offered: נשלחה הצעה לנהג.</summary>
        public bool offer()
        {
            if (this.status != TripStatus.assigned_to_region)
            {
                MessageBox.Show("Only a trip 'Assigned to Region' can be offered", "Error", MessageBoxButtons.OK);
                return false;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Trip_offer @trip_id";
            cmd.Parameters.AddWithValue("@trip_id", this.tripId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
            this.status = TripStatus.offered;   // שיקוף בזיכרון
            return true;
        }

        /// <summary>offered → confirmed: נהג קיבל את הנסיעה.</summary>
        public bool confirm()
        {
            if (this.status != TripStatus.offered)
            {
                MessageBox.Show("Only an 'Offered' trip can be confirmed", "Error", MessageBoxButtons.OK);
                return false;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Trip_confirm @trip_id";
            cmd.Parameters.AddWithValue("@trip_id", this.tripId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
            this.status = TripStatus.confirmed;
            return true;
        }

        /// <summary>offered → assigned_to_region: ההצעה נדחתה/פגה — החזרה לתור (האזור נשמר) לנהג הבא.</summary>
        public bool requeue()
        {
            if (this.status != TripStatus.offered)
            {
                MessageBox.Show("Only an 'Offered' trip can be requeued", "Error", MessageBoxButtons.OK);
                return false;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Trip_requeue @trip_id";
            cmd.Parameters.AddWithValue("@trip_id", this.tripId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
            this.status = TripStatus.assigned_to_region;
            return true;
        }

        /// <summary>
        /// {assigned_to_region | offered} → manual_assignment: נוצלו כל הנהגים הזכאים, או הגיע
        /// דדליין 6 השעות לפני האיסוף ללא שיבוץ — הנסיעה מועברת לטיפול ידני והדיספצ'ר מקבל התראה.
        /// </summary>
        public bool flagManualAssignment()
        {
            if (this.status != TripStatus.assigned_to_region && this.status != TripStatus.offered)
            {
                MessageBox.Show("Only an assigned-to-region or offered trip can be flagged for manual assignment", "Error", MessageBoxButtons.OK);
                return false;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Trip_manual_assignment @trip_id";
            cmd.Parameters.AddWithValue("@trip_id", this.tripId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
            this.status = TripStatus.manual_assignment;
            this.notifyDispatcher();
            return true;
        }

        /// <summary>מודיע לדיספצ'ר שהנסיעה דורשת שיבוץ ידני (הדיספצ'ר הוא משתמש הדסקטופ).</summary>
        public void notifyDispatcher()
        {
            MessageBox.Show(
                "Trip #" + this.tripId + " (" + this.externalBookingId + ") requires MANUAL ASSIGNMENT.\n" +
                "Pickup: " + this.pickupCity + " → " + this.dropoffCity + " at " + this.pickupTime.ToString("dd/MM HH:mm") +
                "\nNo driver was assigned in time.",
                "Dispatcher alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>מגדיל ב-1 את מונה ההצעות לנסיעה ומעדכן ב-DB (אטרקטיביות הנסיעה).</summary>
        public void updateOfferCount()
        {
            this.offerCounter += 1;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Trip_update_offer_count @trip_id, @offerCounter";
            cmd.Parameters.AddWithValue("@trip_id", this.tripId);
            cmd.Parameters.AddWithValue("@offerCounter", this.offerCounter);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        /// <summary>confirmed → completed: the trip is done and moved to the archive.</summary>
        public bool moveToArchive()
        {
            if (this.status != TripStatus.confirmed)
            {
                MessageBox.Show("Only a 'Confirmed' trip can be archived", "Error", MessageBoxButtons.OK);
                return false;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Trip_archive @trip_id";
            cmd.Parameters.AddWithValue("@trip_id", this.tripId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
            this.status = TripStatus.completed;
            return true;
        }

        // =====================================================================
        // מתודות סטטיות — טעינה, חיפוש, מזהה הבא
        // =====================================================================
        public static void initTrips()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Trip_get_all";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.Trips = new List<Trip>();

            while (rdr.Read())
            {
                // 0=trip_id, 1=externalBookingId, 2=pickupAddress, 3=dropoffAddress, 4=pickupCity,
                // 5=dropoffCity, 6=pickupTime, 7=numPassengers, 8=vehicleType, 9=priceToDriver,
                // 10=status, 11=createdAt, 12=region_id, 13=distanceKm, 14=estimatedDurationMinutes,
                // 15=offerCounter
                int id = Convert.ToInt32(rdr.GetValue(0));
                string bookingId = rdr.GetValue(1).ToString();
                string pickupAddr = rdr.GetValue(2).ToString();
                string dropoffAddr = rdr.GetValue(3).ToString();
                string pickupCity = rdr.GetValue(4).ToString();
                string dropoffCity = rdr.GetValue(5).ToString();
                DateTime pickupTime = Convert.ToDateTime(rdr.GetValue(6));
                int numPassengers = Convert.ToInt32(rdr.GetValue(7));
                VehicleType vt = VehicleTypeHelper.FromDb(rdr.GetValue(8).ToString());
                decimal price = Convert.ToDecimal(rdr.GetValue(9));
                TripStatus status = TripStatusHelper.FromDb(rdr.GetValue(10).ToString());
                DateTime createdAt = Convert.ToDateTime(rdr.GetValue(11));
                Region region = rdr.GetValue(12) == DBNull.Value ? null : Region.seekRegion(Convert.ToInt32(rdr.GetValue(12)));
                double distanceKm = Convert.ToDouble(rdr.GetValue(13));
                int estDuration = Convert.ToInt32(rdr.GetValue(14));
                int offerCounter = Convert.ToInt32(rdr.GetValue(15));

                Trip t = new Trip(id, bookingId, pickupAddr, dropoffAddr, pickupCity, dropoffCity,
                    pickupTime, numPassengers, vt, price, status, createdAt, region, false);
                t.setDistanceKm(distanceKm);                 // Maps fields loaded after construction
                t.setEstimatedDurationMinutes(estDuration);
                t.setOfferCounter(offerCounter);
                if (region != null) region.addTrip(t);
                Program.Trips.Add(t);
            }
            rdr.Close();
        }

        public static Trip seekTrip(int id)
        {
            foreach (Trip t in Program.Trips)
                if (t.getId() == id) return t;
            return null;
        }

        public static int getNextTripId()
        {
            int maxId = 0;
            foreach (Trip t in Program.Trips)
                if (t.getId() > maxId) maxId = t.getId();
            return maxId + 1;
        }
    }
}
