using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace ExternalDriverDispatch
{
    /// <summary>
    /// One WhatsApp message in the conversation audit trail (outbound offer text or inbound
    /// driver reply). Created by the dispatch flow whenever the IMessageChannel sends a message
    /// or a (simulated) inbound reply arrives. Links a driver to the offer it concerns.
    ///
    /// Loads LAST — it has FK references to both ExternalDriver and Offer.
    /// </summary>
    public class Message
    {
        // =====================================================================
        // Fields
        // =====================================================================
        private int messageId;
        private ExternalDriver driver;     // object reference to the driver side
        private Offer offer;               // object reference to the related offer (nullable)
        private MessageDirection direction;
        private string waMessageId;        // WhatsApp message id (or a local id from the fallback channel); nullable
        private string body;
        private DateTime timestamp;

        // =====================================================================
        // Constructor
        // =====================================================================
        public Message(int id, ExternalDriver driver, Offer offer, MessageDirection direction,
            string waMessageId, string body, DateTime timestamp, bool is_new)
        {
            this.messageId = id;
            this.driver = driver;
            this.offer = offer;
            this.direction = direction;
            this.waMessageId = waMessageId;
            this.body = body;
            this.timestamp = timestamp;
            if (is_new)
            {
                this.createMessage();
                Program.Messages.Add(this);
            }
        }

        // =====================================================================
        // Getters & Setters
        // =====================================================================
        public int getId() { return this.messageId; }
        public ExternalDriver getDriver() { return this.driver; }
        public Offer getOffer() { return this.offer; }
        public MessageDirection getDirection() { return this.direction; }
        public string getWaMessageId() { return this.waMessageId; }
        public string getBody() { return this.body; }
        public DateTime getTimestamp() { return this.timestamp; }

        public void setDriver(ExternalDriver v) { this.driver = v; }
        public void setOffer(Offer v) { this.offer = v; }
        public void setDirection(MessageDirection v) { this.direction = v; }
        public void setWaMessageId(string v) { this.waMessageId = v; }
        public void setBody(string v) { this.body = v; }
        public void setTimestamp(DateTime v) { this.timestamp = v; }

        // =====================================================================
        // DB operations (CRUD) — all through stored procedures
        // =====================================================================
        public void createMessage()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Message_create @message_id, @driver_id, @direction, " +
                              "@waMessageId, @body, @timestamp, @related_offer_id";
            cmd.Parameters.AddWithValue("@message_id", this.messageId);
            cmd.Parameters.AddWithValue("@driver_id", this.driver.getId());
            cmd.Parameters.AddWithValue("@direction", MessageDirectionHelper.ToDb(this.direction));
            cmd.Parameters.AddWithValue("@waMessageId", (object)this.waMessageId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@body", this.body);
            cmd.Parameters.AddWithValue("@timestamp", this.timestamp);
            cmd.Parameters.AddWithValue("@related_offer_id",
                this.offer != null ? (object)this.offer.getId() : DBNull.Value);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void updateMessage()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Message_update @message_id, @driver_id, @direction, " +
                              "@waMessageId, @body, @timestamp, @related_offer_id";
            cmd.Parameters.AddWithValue("@message_id", this.messageId);
            cmd.Parameters.AddWithValue("@driver_id", this.driver.getId());
            cmd.Parameters.AddWithValue("@direction", MessageDirectionHelper.ToDb(this.direction));
            cmd.Parameters.AddWithValue("@waMessageId", (object)this.waMessageId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@body", this.body);
            cmd.Parameters.AddWithValue("@timestamp", this.timestamp);
            cmd.Parameters.AddWithValue("@related_offer_id",
                this.offer != null ? (object)this.offer.getId() : DBNull.Value);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void deleteMessage()
        {
            Program.Messages.Remove(this);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Message_delete @message_id";
            cmd.Parameters.AddWithValue("@message_id", this.messageId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        // =====================================================================
        // Static methods — load, seek, next id
        // Loads after Offer (FKs to ExternalDriver and Offer).
        // =====================================================================
        public static void initMessages()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_Message_get_all";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.Messages = new List<Message>();

            while (rdr.Read())
            {
                // 0=message_id, 1=driver_id, 2=direction, 3=waMessageId, 4=body,
                // 5=timestamp, 6=related_offer_id
                int id = Convert.ToInt32(rdr.GetValue(0));
                int driverId = Convert.ToInt32(rdr.GetValue(1));
                MessageDirection dir = MessageDirectionHelper.FromDb(rdr.GetValue(2).ToString());
                string waId = rdr.GetValue(3) == DBNull.Value ? null : rdr.GetValue(3).ToString();
                string body = rdr.GetValue(4).ToString();
                DateTime ts = Convert.ToDateTime(rdr.GetValue(5));
                Offer offer = rdr.GetValue(6) == DBNull.Value ? null : Offer.seekOffer(Convert.ToInt32(rdr.GetValue(6)));

                ExternalDriver driver = ExternalDriver.seekExternalDriver(driverId);

                Message m = new Message(id, driver, offer, dir, waId, body, ts, false);
                Program.Messages.Add(m);
            }
            rdr.Close();
        }

        public static Message seekMessage(int id)
        {
            foreach (Message m in Program.Messages)
                if (m.getId() == id) return m;
            return null;
        }

        public static int getNextMessageId()
        {
            int maxId = 0;
            foreach (Message m in Program.Messages)
                if (m.getId() > maxId) maxId = m.getId();
            return maxId + 1;
        }
    }
}
