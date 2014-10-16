using System.Data;
using Mono.Data.SqliteClient;
using System.Collections.Generic;

namespace Aquiris.Tools.Database.SQLite {
	public class SQLiteDatabase {
		private SqliteConnection m_connection;
		private string m_database;
		private IDbCommand m_command;
		private IDataReader m_reader;

        private int m_lastInsertedRowId;

		public SQLiteDatabase(string p_databaseFilePath) {
			m_connection = new SqliteConnection("URI=file:" + p_databaseFilePath);
			m_command = m_connection.CreateCommand();
		}

		public IDataReader Select(string p_table, string p_select, string p_where = null) {
			string where = "";
			if (!string.IsNullOrEmpty(p_where)) {
				where = " WHERE " + p_where;
			}
			return ExecuteQuery("SELECT " + p_select + " FROM " + p_table + where + " ORDER BY `rowid` ASC;");
		}

		public void Delete(string p_table, string p_id) {
			ExecuteQuery("DELETE FROM " + p_table + " WHERE id=" + p_id + ";");
		}

		public void Update(string p_table, string p_set, string p_where) {
			ExecuteQuery("UPDATE " + p_table + " SET " + p_set + " WHERE " + p_where + ";");
		}

		public void CreateTable(string p_table, string[] p_fields) {
			string fields = "";
			foreach (string field in p_fields) {
				if (!string.IsNullOrEmpty(fields)) {
					fields += ", ";
				}
				fields += "`" + field + "` TEXT";
			}
			ExecuteQuery("CREATE TABLE `" + p_table + "` (`id`INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " + fields + ");");
		}

		public void DropTable(string p_table) {
			ExecuteQuery("DROP TABLE `" + p_table + "`;");
		}

		public int Insert(string p_table, Dictionary<string, string> p_entries) {
			string keys = "";
			string values = "";
			foreach (KeyValuePair<string,string> pair in p_entries) {
				if (!string.IsNullOrEmpty(keys)) {
					keys += ",";
					values += ",";
				}
				keys += "'" + pair.Key + "'";
				values += "'" + pair.Value + "'";
			}
			ExecuteQuery("INSERT INTO " + p_table + "(" + keys + ") VALUES (" + values + ");");

            return m_lastInsertedRowId;
		}

		public IDataReader ExecuteQuery(string p_query) {
			m_command.CommandText = p_query;
			m_connection.Open();
			m_reader = m_command.ExecuteReader();
            m_lastInsertedRowId = m_connection.LastInsertRowId;
			m_connection.Close();
            return m_reader;
		}
	}
}
