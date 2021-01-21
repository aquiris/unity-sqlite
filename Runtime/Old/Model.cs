using System.Data;
using System.Collections.Generic;

namespace Aquiris.SQLite.Old.Model {
	public abstract class Model {
		public int Id { get; private set; }

		abstract public void Remove();
		abstract public string Table { get; }
		abstract public void Add();
		abstract protected SQLiteDatabase _database { get; }

		protected int Add(Dictionary<string, string> p_newEntry) {
            Id = _database.Insert(Table, p_newEntry);
            return Id;
		}

		protected Dictionary<string,string> Load(int p_id) {
			Id = p_id;
			Dictionary<string,string> result = new Dictionary<string, string>();
			IDataReader reader = _database.Select(Table, "*", "id=" + Id);
			while (reader.Read()) {
				for (int i=0; i<reader.FieldCount; i++) {
					result.Add(reader.GetName(i).ToString(), reader [i].ToString());
				}
			}
			return result;
		}

		protected virtual void Save(Dictionary<string, string> p_newEntry, string p_where = null) {
			string set = "";
			foreach (var pair in p_newEntry) {
				if (!string.IsNullOrEmpty(set)) {
					set += ",";
				}
				set += pair.Key + "='" + pair.Value + "'";
			}
			string where = "id=" + Id;
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