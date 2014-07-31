using System.Data;
using System.Data.Common;
using Mono.Data.SqliteClient;
using System.Collections.Generic;

namespace Aquiris.Tools.DbInterface
{
	public class DbInterface {
		public string Table{set{m_table = "["+value+"]";}}

		private SqliteConnection m_connection;
		private string m_database;
		private string m_table;
		private IDbCommand m_command;
		private IDataReader m_reader;

		public DbInterface(string p_databaseFilePath, string p_table){
			m_connection = new SqliteConnection("URI=file:" + p_databaseFilePath);
			m_command = m_connection.CreateCommand();

			Table = p_table;
		}

		public IDataReader Select(string p_select){
			return ExecuteQuery("SELECT "+p_select+" FROM "+m_table+"  ORDER BY `rowid` ASC;");
		}

		public void Delete(string p_id, string p_table = null){
			if(string.IsNullOrEmpty(p_table)){
				p_table = m_table;
			}
			ExecuteQuery("DELETE FROM "+p_table+" WHERE id="+p_id+";");
		}

		public void Update(int p_rowid, string p_column, string p_data){
			ExecuteQuery("UPDATE "+m_table+" SET \""+p_column+"\"=\""+p_data+"\" WHERE rowid="+p_rowid+";");
		}

		public void CreateTable(string p_tableName, string[] p_fields){
			string fields = "";
			foreach(string field in p_fields){
				if(!string.IsNullOrEmpty(fields)){
					fields += ", ";
				}
				fields += "`" + field + "` TEXT";
			}
			ExecuteQuery("CREATE TABLE `"+p_tableName+"` (`id`INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, "+fields+");");
		}

		public void DropTable(string p_tableName){
			ExecuteQuery("DROP TABLE `"+p_tableName+"`;");
		}

		public void Insert(Dictionary<string, string> p_entries, string p_table = null){
			if(string.IsNullOrEmpty(p_table)){
				p_table = m_table;
			}
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
			ExecuteQuery("INSERT INTO " + p_table + "(" + keys + ") VALUES (" + values + ");");
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
