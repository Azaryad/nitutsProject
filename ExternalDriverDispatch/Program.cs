using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace ExternalDriverDispatch
{
    static class Program
    {
        // =====================================================================
        // רשימות בזיכרון — רשימה סטטית לכל ישות במערכת.
        // הרשימות נטענות מבסיס הנתונים בהפעלת התוכנית (initLists).
        //
        // אין רשימה נפרדת ל-ExternalDriverRegion: זו טבלת קישור בלבד (ללא תכונות),
        // והקשר נשמר כהפניות בין ExternalDriver ל-Region.
        // =====================================================================
        public static List<Region> Regions;
        public static List<ExternalDriver> ExternalDrivers;
        public static List<Trip> Trips;
        public static List<Offer> Offers;
        public static List<Message> Messages;

        // =====================================================================
        // אתחול כל הרשימות — סדר הטעינה קריטי:
        //   1. Region          (בסיסי, ללא FK)
        //   2. ExternalDriver  (בסיסי)
        //   3. ExternalDriverRegion (קישור — מחווט נהגים↔אזורים)
        //   4. Trip            (FK לאזור)
        //   5. Offer           (מתווך — FK לנסיעה ולנהג)
        //   6. Message         (audit trail — FK לנהג ולפנייה)
        // =====================================================================
        public static void initLists()
        {
            Region.initRegions();                              // 1
            ExternalDriver.initExternalDrivers();              // 2
            ExternalDriverRegion.initExternalDriverRegions();  // 3
            Trip.initTrips();                                  // 4
            Offer.initOffers();                                // 5
            Message.initMessages();                            // 6
        }

        // =====================================================================
        // נקודת ההתחלה של התוכנית
        // =====================================================================
        [STAThread]
        static void Main()
        {
            // Single-instance guard: a second instance would fail to bind the inbound
            // webhook (port 5051) and leave a zombie holding the port. Refuse to start
            // a duplicate so the webhook listener is never blocked.
            using var single = new Mutex(true, "ExternalDriverDispatch.SingleInstance", out bool isFirst);
            if (!isFirst)
            {
                MessageBox.Show(
                    "ExternalDriverDispatch is already running.\nClose the existing window before starting a new one.",
                    "Already running", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ApplicationConfiguration.Initialize();
            initLists();
            Application.Run(new mainForm());
        }
    }
}
