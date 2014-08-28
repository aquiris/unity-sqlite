using System.Collections;
using System.Data;
using System.Collections.Generic;

namespace Aquiris.Tools.Database.SQLite.Model {
	public abstract class Model {
		public int Id { get { return _id; } }

		protected int _id;

		protected abstract SQLiteDatabase _database { get; }

		public abstract string Table { get; }

		protected void Add(Dictionary<string, string> p_newEntry) {
			_database.Insert(Table, p_newEntry);
		}

		public abstract void Remove();
		public abstract void Add();

		//_id should be already setted
		protected Dictionary<string,string> Load() {
			Dictionary<string,string> result = new Dictionary<string, string>();
			IDataReader reader = _database.Select(Table, "*", "id=" + _id);
			while (reader.Read()) {
				for (int i=0; i<reader.FieldCount; i++) {
					result.Add(reader.GetName(i).ToString(), reader [i].ToString());
				}
			}
			return result;
		}

		protected void Save(Dictionary<string, string> p_newEntry, string p_where = null) {
			string set = "";
			foreach (var pair in p_newEntry) {
				if (!string.IsNullOrEmpty(set)) {
					set += ",";
				}
				set += pair.Key + "='" + pair.Value + "'";
			}
			string where = "id=" + _id;
			if (!string.IsNullOrEmpty(p_where)) {
				where = p_where;
			}
			 
			_database.Update(Table, set, where);
		}

		protected List<Dictionary<string,string>> GetAllAttributes(string p_where = null) {
			List<Dictionary<string,string>> result = new List<Dictionary<string, string>>();
			IDataReader reader = _database.Select(Table, "*", p_where);
			while (reader.Read()) {
				Dictionary<string,string> model = new Dictionary<string, string>();
				for (int i=0; i<reader.FieldCount; i++) {
					model.Add(reader.GetName(i).ToString(), reader [i].ToString());
				}
				result.Add(model);
			}
			return result;
		}
	}
}