using System;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace ExternalDriverDispatch
{
    /// <summary>
    /// מחלקה שאחראית על החיבור לבסיס הנתונים וביצוע שאילתות.
    /// כל פעולה מול בסיס הנתונים עוברת דרך מחלקה זו.
    ///
    /// מחרוזת החיבור נקראת מתוך app.config (connectionStrings → "DispatchDB").
    ///
    /// שני סוגי פעולות:
    /// 1. execute_non_query - פעולות שמשנות נתונים (INSERT, UPDATE, DELETE)
    /// 2. execute_query     - פעולות שמחזירות נתונים (SELECT)
    /// </summary>
    class SQL_CON
    {
        SqlConnection conn;

        /// <summary>
        /// כשהדגל דולק, execute_non_query לא מציג חלונית "בוצע בהצלחה" אחרי כל כתיבה.
        /// משמש את לוח השיבוץ, שמבצע כמה כתיבות ברצף ומסכם בעצמו ביומן (כדי לא להציף בהודעות).
        /// שגיאות עדיין מוצגות תמיד.
        /// </summary>
        public static bool SuppressSuccessMessages = false;

        public SQL_CON()
        {
            // קריאת מחרוזת החיבור מתוך app.config
            string connStr = ConfigurationManager.ConnectionStrings["DispatchDB"].ConnectionString;
            conn = new SqlConnection(connStr);
        }

        /// <summary>
        /// ביצוע פעולה שמשנה נתונים בבסיס הנתונים (INSERT, UPDATE, DELETE).
        /// הפעולה לא מחזירה נתונים - רק מבצעת שינוי.
        /// </summary>
        /// <param name="cmd">פקודת SQL מוכנה עם פרמטרים</param>
        public void execute_non_query(SqlCommand cmd)
        {
            try
            {
                conn.Open();              // שלב 1: פתיחת חיבור
                cmd.Connection = conn;    // שלב 2: קישור הפקודה לחיבור
                cmd.ExecuteNonQuery();    // step 3: execute (INSERT/UPDATE/DELETE)
                if (!SuppressSuccessMessages)
                    MessageBox.Show("Operation completed successfully", "Info", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error executing the operation: " + ex.Message, "Error", MessageBoxButtons.OK);
            }
            finally
            {
                // שלב 4: סגירת החיבור - חייבת לקרות תמיד!
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// ביצוע שאילתה שמחזירה נתונים מבסיס הנתונים (SELECT).
        /// מחזירה SqlDataReader לקריאת התוצאות שורה אחרי שורה.
        /// שימו לב: החיבור לא נסגר כאן! הוא נשאר פתוח עבור ה-Reader.
        /// </summary>
        /// <param name="cmd">פקודת SQL מוכנה עם פרמטרים</param>
        /// <returns>SqlDataReader לקריאת התוצאות, או null אם הייתה שגיאה</returns>
        public SqlDataReader execute_query(SqlCommand cmd)
        {
            try
            {
                conn.Open();              // שלב 1: פתיחת חיבור
                cmd.Connection = conn;    // שלב 2: קישור הפקודה לחיבור
                SqlDataReader reader = cmd.ExecuteReader();  // שלב 3: ביצוע SELECT
                return reader;            // שלב 4: החזרת התוצאות
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error executing the query: " + ex.Message, "Error", MessageBoxButtons.OK);
                return null;
            }
        }
    }
}
