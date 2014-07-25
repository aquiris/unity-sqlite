using System.Data;
using System.Data.Common;
using Mono.Data.SqliteClient;
using System.Collections.Generic;

namespace aquiris.tools.dbinterface
{
	public class DBInterface {
		public string Table{set{m_table = value;}}

		private SqliteConnection m_connection;
		private string m_database;
		private string m_table;
		private IDbCommand m_command;
		private IDataReader m_reader;

		public DBInterface(string p_database){
			m_connection = new SqliteConnection("URI=file:" + p_database);
			m_command = m_connection.CreateCommand();
		}

		public IDataReader Select(string p_column, string p_parameter){
			return ExecuteQuery("SELECT `"+p_column+"`,"+p_parameter+" FROM `"+m_table+"`  ORDER BY `rowid` ASC;");
		}

		public void Delete(string p_rowid){
			ExecuteQuery("DELETE FROM `"+m_table+"` WHERE rowid="+p_rowid+";");
		}

		public void Update(int p_rowid, string p_column, string p_data){
			ExecuteQuery("UPDATE `"+m_table+"` SET `"+p_column+"`="+p_data+" WHERE rowid="+p_rowid+";");
		}

		public void Insert(Dictionary<string, string> p_entries){
			string keys = "";
			string values = "";
			foreach(KeyValuePair<string,string> pair in p_entries){
				if(!string.IsNullOrEmpty(keys)){
					keys += ",";
					values += ",";
				}
				keys += "'" + pair.Key + "'";
				values += "'" + pair.Value + "'";
			}
			ExecuteQuery("INSERT INTO `" + m_table + "`(" + keys + ") VALUES (" + values + ");");
		}

		public IDataReader ExecuteQuery(string p_query){
			m_command.CommandText = p_query;
			m_connection.Open();
			m_reader = m_command.ExecuteReader();
			m_connection.Close();
			return m_reader;
		}
	}
}
