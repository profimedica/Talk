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
                string sql =
                    "CREATE TABLE history   (id INTEGER PRIMARY KEY, name VARCHAR(32), text VARCHAR(200), timestamp DATE DEFAULT (datetime('now','localtime')), q NUMERIC)";
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();
                sql =
                    "CREATE TABLE rules     (id INTEGER PRIMARY KEY, name VARCHAR(32), text VARCHAR(200), timestamp DATE DEFAULT (datetime('now','localtime')))";
                command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();
                sql =
                    "CREATE TABLE blocks    (id INTEGER PRIMARY KEY, name VARCHAR(32), text VARCHAR(200), timestamp DATE DEFAULT (datetime('now','localtime')),  rule INTEGER)";
                command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();
                CreateSampleData();
            }
            else
            {
                conn = new SQLiteConnection("Data Source=Talker.sqlite;Version=3;");
            }
        }

        public List<Fragment> LoadFragments(int id)
        {
            List<Fragment> fragments = new List<Fragment>();
            DataTable dt = selectQuery("SELECT `text` FROM `blocks` WHERE `rule` = " + id);
            foreach (DataRow dr in dt.Rows)
            {
                if (!string.IsNullOrEmpty(dr[0].ToString()))
                {
                    Fragment fragment = new Fragment(){ Text = dr[0].ToString() };
                    fragments.Add(fragment);
                }
            }
            return fragments;
        }

        public List<Rule> LoadRules()
        {
            List<Rule> rules = new List<Rule>();
            DataTable dt = selectQuery("SELECT `Id`, `text` FROM `rules` ORDER BY `id` ASC ");
            int id;
            foreach (DataRow dr in dt.Rows)
            {
                if (!string.IsNullOrEmpty(dr[1].ToString()))
                {
                    int.TryParse(dr[0].ToString(), out id);
                    Rule rule = new Rule() { Id = id, RegularExpression = dr[1].ToString() };
                    rules.Add(rule);
                }
            }
            return rules;
        }

        private void CreateSampleData()
        {
            long id = InsertRule("your ([a-z]*)");
            InsterFragment(id, "Lets not talk about my $1");
            InsterFragment(id, "What about your $1?");
        }

        private void InsterFragment(long id, string text)
        {
            string SQL = "INSERT INTO `blocks` (`name`, `rule`, `text`) VALUES ('start_florin', " + id + ", '" + text + "')";
            SQLiteCommand command = new SQLiteCommand(SQL, conn);
            command.ExecuteNonQuery();
        }

        private void ClearRules()
        {
            string SQL = "DELETE FROM `blocks`";
            SQLiteCommand command = new SQLiteCommand(SQL, conn);
            command.ExecuteNonQuery();
            SQL = "DELETE FROM  `rules`";
            command = new SQLiteCommand(SQL, conn);
            command.ExecuteNonQuery();
        }

        private long InsertRule(string text)
        {
            string SQL = "INSERT INTO `rules` (`name`, `text`) VALUES ('start_florin', '"+text+"')";
            SQLiteCommand command = new SQLiteCommand(SQL, conn);
            command.ExecuteNonQuery();
            SQL = "SELECT last_insert_rowid()";
            command = new SQLiteCommand(SQL, conn);
            return (long)command.ExecuteScalar();
        }

        public void SaveEditedRules(string text)
        {
            ClearRules();
            long id = 0;
            foreach(string line in text.Split('\n'))
            {
                string trimmed = line.TrimStart();
                if (line.Length == trimmed.Length)
                {
                    id = InsertRule(trimmed);
                }
                else
                {
                    InsterFragment(id, trimmed);
                }
            }
        }

        public string ShowRulesForEdit(List<Rule> rules)
        {
            StringBuilder sb = new StringBuilder();
            foreach(Rule rule in rules)
            {
                sb.AppendLine(rule.RegularExpression);
                List<Fragment> fragments = LoadFragments(rule.Id);
                foreach(Fragment f in fragments)
                {
                    sb.AppendLine("\t"+f.Text);
                }
            }
            return sb.ToString();
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