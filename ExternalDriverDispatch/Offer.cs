using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace ExternalDriverDispatch
{
    /// <summary>
    /// מחלקת מתווך (Mediator) — ניסיון פנייה בודד: נהג אחד נוצר קשר עבור נסיעה אחת.
    /// עשויות להתקיים כמה פניות לכל נסיעה — אחת לכל נהג שאליו פנו בשרשרת ההעברה.
    /// שני צדי הקשר נשמרים כהפניות לאובייקטים (Trip, ExternalDriver), לא כמזהים.
    /// נטענת אחרונה (תלויה ב-Trip וב-ExternalDriver).
    /// </summary>
    public class Offer
    {
        // =====================================================================
        // שדות
        // =====================================================================
        private int offerId;
        private Trip trip;              // הפניה לצד הנסיעה
        private ExternalDriver driver;  // הפניה לצד הנהג
        private DateTime sentAt;
        private DateTime expiresAt;
        private OfferStatus status;
        private string driverReplyText;   // null עד שהנהג משיב (טקסט חופשי מ-WhatsApp)
        private string aiInterpretation;  // null עד שה-AI מפענח את תשובת הנהג
        private int rankPosition;
        private string rankReason;        // one-line AI justification for this driver's rank (null until ranked)

        // =====================================================================
        // בנאי
        // =====================================================================
        public Offer(int id, Trip trip, ExternalDriver driver, DateTime sentAt, DateTime expiresAt,
            OfferStatus status, string driverReplyText, string aiInterpretation, int rankPosition, bool is_new)
        {
            this.offerId = id;
            this.trip = trip;
            this.driver = driver;
            this.sentAt = sentAt;
            this.expiresAt = expiresAt;
            this.status = status;
            this.driverReplyText = driverReplyText;
            this.aiInterpretation = aiInterpretation;
            this.rankPosition = rankPosition;
            this.rankReason = null;   // set by the ranking service after construction
            if (is_new)
            {
                this.createOffer();
                if (trip != null) trip.addOffer(this);
                Program.Offers.Add(this);
            }
        }

        // =====================================================================
        // Getters & Setters
        // =====================================================================
        public int getId() { return this.offerId; }
        public Trip getTrip() { return this.trip; }
        public ExternalDriver getDriver() { return this.driver; }
        public DateTime getSentAt() { return this.sentAt; }
        public DateTime getExpiresAt() { return this.expiresAt; }
        public OfferStatus getStatus() { return this.status; }
        public string getDriverReplyText() { return this.driverReplyText; }
        public string getAiInterpretation() { return this.aiInterpretation; }
        public int getRankPosition() { return this.rankPosition; }
        public string getRankReason() { return this.rankReason; }

        public void setTrip(Trip v) { this.trip = v; }
        public void setDriver(ExternalDriver v) { this.driver = v; }
        public void setSentAt(DateTime v) { this.sentAt = v; }
        public void setExpiresAt(DateTime v) { this.expiresAt = v; }
        public void setStatus(OfferStatus v) { this.status = v; }
        public void setDriverReplyText(string v) { this.driverReplyText = v; }
        public void setAiInterpretation(string v) { this.aiInterpretation = v; }
        public void setRankPosition(int v) { this.rankPosition = v; }
        public void setRankReason(string v) { this.rankReason = v; }

        // =====================================================================
        // פעולת תחום (מתוך תרשים המחלקות)
        // =====================================================================

        /// <summary>
        /// בניית כתובת ה-URL הייחודית לעמוד האישור הנייד של הנהג.
        /// המבנה תואם למערכת האמיתית: /approve?offer={id} (במערכת האמיתית מתווספים exp ו-sig חתומים ב-HMAC).
        /// </summary>
        public string generateApprovalUrl()
        {
            return "https://dispatch.tlv-transfers.com/approve?offer=" + this.offerId;
        }

        // =====================================================================
        // פעולות מול בסיס הנתונים (CRUD)
        // =====================================================================
        public void createOffer()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Offer_create @offer_id, @trip_id, @driver_id, @sentAt, " +
                              "@expiresAt, @status, @driverReplyText, @aiInterpretation, @rankPosition, @rankReason";
            cmd.Parameters.AddWithValue("@offer_id", this.offerId);
            cmd.Parameters.AddWithValue("@trip_id", this.trip.getId());
            cmd.Parameters.AddWithValue("@driver_id", this.driver.getId());
            cmd.Parameters.AddWithValue("@sentAt", this.sentAt);
            cmd.Parameters.AddWithValue("@expiresAt", this.expiresAt);
            cmd.Parameters.AddWithValue("@status", OfferStatusHelper.ToDb(this.status));
            cmd.Parameters.AddWithValue("@driverReplyText", (object)this.driverReplyText ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@aiInterpretation", (object)this.aiInterpretation ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@rankPosition", this.rankPosition);
            cmd.Parameters.AddWithValue("@rankReason", (object)this.rankReason ?? DBNull.Value);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void updateOffer()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Offer_update @offer_id, @trip_id, @driver_id, @sentAt, " +
                              "@expiresAt, @status, @driverReplyText, @aiInterpretation, @rankPosition, @rankReason";
            cmd.Parameters.AddWithValue("@offer_id", this.offerId);
            cmd.Parameters.AddWithValue("@trip_id", this.trip.getId());
            cmd.Parameters.AddWithValue("@driver_id", this.driver.getId());
            cmd.Parameters.AddWithValue("@sentAt", this.sentAt);
            cmd.Parameters.AddWithValue("@expiresAt", this.expiresAt);
            cmd.Parameters.AddWithValue("@status", OfferStatusHelper.ToDb(this.status));
            cmd.Parameters.AddWithValue("@driverReplyText", (object)this.driverReplyText ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@aiInterpretation", (object)this.aiInterpretation ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@rankPosition", this.rankPosition);
            cmd.Parameters.AddWithValue("@rankReason", (object)this.rankReason ?? DBNull.Value);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void deleteOffer()
        {
            Program.Offers.Remove(this);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Offer_delete @offer_id";
            cmd.Parameters.AddWithValue("@offer_id", this.offerId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        // =====================================================================
        // פעולות מחזור-החיים (מכונת המצבים) — תרשים המחלקות + dispatch_flow
        // קבלה/דחייה משנות גם את מצב הנסיעה המשויכת (תופעת-לוואי), בתוך טרנזקציה ב-SP.
        // המשמר (guard) נאכף כאן ב-C#.
        // =====================================================================

        /// <summary>
        /// אישור מחייב: הפנייה → accepted והנסיעה המשויכת → confirmed.
        /// מותר רק מ-pending או pending_approval.
        /// </summary>
        public bool accept()
        {
            if (this.status != OfferStatus.pending && this.status != OfferStatus.pending_approval)
            {
                MessageBox.Show("Only a 'Pending' or 'Pending Approval' offer can be accepted", "Error", MessageBoxButtons.OK);
                return false;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Offer_accept @offer_id";
            cmd.Parameters.AddWithValue("@offer_id", this.offerId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
            // שיקוף בזיכרון — גם הפנייה וגם הנסיעה
            this.status = OfferStatus.accepted;
            if (this.trip != null) this.trip.setStatus(TripStatus.confirmed);
            return true;
        }

        /// <summary>
        /// דחייה: הפנייה → rejected והנסיעה המשויכת → assigned_to_region (לקראת העברה לנהג הבא; האזור נשמר).
        /// מותר רק מ-pending או pending_approval.
        /// </summary>
        public bool reject()
        {
            if (this.status != OfferStatus.pending && this.status != OfferStatus.pending_approval)
            {
                MessageBox.Show("Only a 'Pending' or 'Pending Approval' offer can be rejected", "Error", MessageBoxButtons.OK);
                return false;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Offer_reject @offer_id";
            cmd.Parameters.AddWithValue("@offer_id", this.offerId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
            this.status = OfferStatus.rejected;
            if (this.trip != null) this.trip.setStatus(TripStatus.assigned_to_region);
            return true;
        }

        /// <summary>
        /// ביטול ההצעה: הפנייה → cancelled. נקרא כשהנסיעה מוסלמת לשיבוץ ידני (דדליין 6ש') בעוד הצעה פתוחה.
        /// מותר מ-pending או pending_approval. אינו משנה את מצב הנסיעה (מטופל בנפרד ע"י flagManualAssignment).
        /// </summary>
        public bool cancel()
        {
            if (this.status != OfferStatus.pending && this.status != OfferStatus.pending_approval)
            {
                MessageBox.Show("Only a pending offer can be cancelled", "Error", MessageBoxButtons.OK);
                return false;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Offer_cancel @offer_id";
            cmd.Parameters.AddWithValue("@offer_id", this.offerId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
            this.status = OfferStatus.cancelled;
            return true;
        }

        /// <summary>
        /// אות רך (Stage 5): הנהג השיב "כן" ב-WhatsApp — כוונה, לא מחייב.
        /// pending → pending_approval; מצב הנסיעה נשאר 'offered' עד לחיצה על הקישור.
        /// </summary>
        public bool markPendingApproval()
        {
            if (this.status != OfferStatus.pending)
            {
                MessageBox.Show("Only a 'Pending' offer can be marked 'Pending Approval'", "Error", MessageBoxButtons.OK);
                return false;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Offer_pending_approval @offer_id";
            cmd.Parameters.AddWithValue("@offer_id", this.offerId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
            this.status = OfferStatus.pending_approval;
            return true;
        }

        /// <summary>
        /// פג תוקף (Stage 7): הנהג לא הגיב בזמן. הפנייה → timeout והנסיעה → assigned_to_region (לקראת העברה).
        /// מותר מ-pending או pending_approval.
        /// </summary>
        public bool timeout()
        {
            if (this.status != OfferStatus.pending && this.status != OfferStatus.pending_approval)
            {
                MessageBox.Show("Only a pending offer can be marked 'Timeout'", "Error", MessageBoxButtons.OK);
                return false;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Offer_timeout @offer_id";
            cmd.Parameters.AddWithValue("@offer_id", this.offerId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
            this.status = OfferStatus.timeout;
            if (this.trip != null) this.trip.setStatus(TripStatus.assigned_to_region);
            return true;
        }

        // =====================================================================
        // מתודות סטטיות — טעינה, חיפוש, מזהה הבא
        //
        // סדר הטעינה: חייבים לטעון קודם Trip ו-ExternalDriver, ורק אז Offer.
        // =====================================================================
        public static void initOffers()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Offer_get_all";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.Offers = new List<Offer>();

            while (rdr.Read())
            {
                // 0=offer_id, 1=trip_id, 2=driver_id, 3=sentAt, 4=expiresAt, 5=status,
                // 6=driverReplyText, 7=aiInterpretation, 8=rankPosition, 9=rankReason
                int id = Convert.ToInt32(rdr.GetValue(0));
                int tripId = Convert.ToInt32(rdr.GetValue(1));
                int driverId = Convert.ToInt32(rdr.GetValue(2));
                DateTime sentAt = Convert.ToDateTime(rdr.GetValue(3));
                DateTime expiresAt = Convert.ToDateTime(rdr.GetValue(4));
                OfferStatus status = OfferStatusHelper.FromDb(rdr.GetValue(5).ToString());
                string replyText = rdr.GetValue(6) == DBNull.Value ? null : rdr.GetValue(6).ToString();
                string aiInterp = rdr.GetValue(7) == DBNull.Value ? null : rdr.GetValue(7).ToString();
                int rankPosition = Convert.ToInt32(rdr.GetValue(8));
                string rankReason = rdr.GetValue(9) == DBNull.Value ? null : rdr.GetValue(9).ToString();

                Trip trip = Trip.seekTrip(tripId);
                ExternalDriver driver = ExternalDriver.seekExternalDriver(driverId);

                Offer o = new Offer(id, trip, driver, sentAt, expiresAt, status, replyText, aiInterp, rankPosition, false);
                o.setRankReason(rankReason);   // loaded after construction
                if (trip != null) trip.addOffer(o);
                Program.Offers.Add(o);
            }
            rdr.Close();
        }

        public static Offer seekOffer(int id)
        {
            foreach (Offer o in Program.Offers)
                if (o.getId() == id) return o;
            return null;
        }

        public static int getNextOfferId()
        {
            int maxId = 0;
            foreach (Offer o in Program.Offers)
                if (o.getId() > maxId) maxId = o.getId();
            return maxId + 1;
        }
    }
}
