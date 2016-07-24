using System;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Collections.Generic;
using System.IO;

namespace TalkerLibrary
{
    public class SQLiteHelper
    {
        private SQLiteConnection conn;
        long timestamp;

        private void CreateDb() {
            if (!File.Exists("Talker.sqlite"))
            {
                SQLiteConnection.CreateFile("Talker.sqlite");
                conn = new SQLiteConnection("Data Source=Talker.sqlite;Version=3;");
                conn.Open();
                string sql = "CREATE TABLE history (id INTEGER PRIMARY KEY, q NUMERIC, timestamp DATE DEFAULT (datetime('now','localtime')), name VARCHAR(32), text VARCHAR(200))";
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();
                conn.Close();
            }
            else
            {
                conn = new SQLiteConnection("Data Source=Talker.sqlite;Version=3;");
            }
        }

        public SQLiteHelper()
        {
            //This part killed me in the beginning.  I was specifying "DataSource"
            //instead of "Data Source"
            CreateDb();
            conn.Open();
        }

        public DataTable selectQuery(string query)
        {
            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();

            try
            {
                SQLiteCommand cmd;
                cmd = conn.CreateCommand();
                cmd.CommandText = query;  //set the passed query
                ad = new SQLiteDataAdapter(cmd);
                ad.Fill(dt); //fill the datasource
            }
            catch (SQLiteException ex)
            {
                //Add your exception code here.
            }
            conn.Close();
            return dt;
        }

        internal List<Message> GetLastMesages()
        {
            List<Message> messages = new List<Message>();
            DataTable dt = selectQuery("SELECT * FROM `history` WHERE `timestamp` > "+timestamp + " ORDER BY `timestamp` ASC");
            //DataTable dt = selectQuery("SELECT * FROM messages --WHERE consumption_status != 2 ORDER BY timestamp ASC;");
            int id;
            foreach (DataRow dr in dt.Rows)
            {
                if (!string.IsNullOrEmpty(dr[18].ToString()))
                {
                    int.TryParse(dr[0].ToString(), out id);
                    long.TryParse(dr[9].ToString(), out timestamp);
                    Message message = new Message();// { id = id, Sent = TimeStampToDateTime(timestamp), SkypeId = dr[3].ToString(), From = dr[5].ToString(), Text = dr[18].ToString() };
                    messages.Add(message);
                }
            }
            return messages;
        }

        public void Save(bool isReceived, string from, string text)
        {
            string sql = "INSERT INTO `history` (`q`, `name`, `text`) VALUES ("+(isReceived?1:0)+ ", '"+from+"', '"+text+"')";

            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();
        }

        public static DateTime TimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}