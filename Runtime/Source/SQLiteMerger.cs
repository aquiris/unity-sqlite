using UnityEngine;
using System;
using System.IO;
using System.Data;
using System.Collections.Generic;

namespace Aquiris.SQLite.Merger
{
    public class SQLiteMerger
    {
        private SQLiteDatabase m_database;
        private string m_datapathPath;

        public SQLiteMerger(SQLiteDatabase p_database)
        {
            m_database = p_database;
        }

        public void Import(string p_sqlPath)
        {
            DropAllTables();
            try
            {
                using (StreamReader sr = new StreamReader(p_sqlPath))
                {
                    string query = sr.ReadToEnd();
                    m_database.ExecuteQuery(query);
                }
            }
            catch (Exception e)
            {
                Debug.Log("SQLfile could not be read:");
                Debug.Log(e.Message);
            }
        }

        public void Export(string p_sqlPath)
        {
            List<string> exportedDatabaseQueries = new List<string>();

            exportedDatabaseQueries.Add("BEGIN TRANSACTION;");
            foreach (var query in GetSqlThatCreateAllTables())
            {
                exportedDatabaseQueries.Add(query);
            }
            foreach (var table in GetAllTables())
            {
                foreach (var query in GetSqlThatCreateEveryEntryOfTable(table))
                {
                    exportedDatabaseQueries.Add(query);
                }
            }
            exportedDatabaseQueries.Add("COMMIT;");

            try
            {
                using (StreamWriter writer = new StreamWriter(p_sqlPath, false))
                {
                    foreach (var query in exportedDatabaseQueries)
                    {
                        writer.WriteLine(query);
                    }
                }

            }
            catch (Exception e)
            {
                Debug.Log("Problem writing exported database to file");
                Debug.Log(e.Message);
            }

        }

        private List<string> GetSqlThatCreateAllTables()
        {
            List<string> createTablesQuery = new List<string>();
            IDataReader reader = m_database.ExecuteQuery("select sql from sqlite_master where name not like 'sqlite_%';");
            while (reader.Read())
            {
                createTablesQuery.Add(reader[0].ToString() + ";");
            }
            return createTablesQuery;
        }

        private List<string> GetSqlThatCreateEveryEntryOfTable(string p_table)
        {
            List<string> entryList = new List<string>();
            IDataReader reader = m_database.Select(p_table, "*");
            while (reader.Read())
            {
                string values = "";
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (!string.IsNullOrEmpty(values))
                    {
                        values += ",";
                    }

                    string value = "''";
                    if (reader[i] != null)
                    {
                        if (reader[i].GetType() == typeof(string))
                        {
                            value = "'" + reader[i].ToString() + "'";
                        }
                        else
                        {
                            value = reader[i].ToString();
                        }
                    }

                    values += value;
                }
                entryList.Add("INSERT INTO `" + p_table + "` VALUES(" + values + ");");
            }
            return entryList;
        }

        private List<string> GetAllTables()
        {
            List<string> tableList = new List<string>();
            IDataReader reader = m_database.ExecuteQuery("select name from sqlite_master where type = 'table' and name not like 'sqlite_%';");
            while (reader.Read())
            {
                string tableName = reader[0].ToString();
                tableList.Add(tableName);
            }
            return tableList;
        }

        private void DropAllTables()
        {
            foreach (var table in GetAllTables())
            {
                m_database.DropTable(table);
            }
        }
    }
}