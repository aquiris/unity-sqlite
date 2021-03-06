﻿using System;
using Aquiris.SQLite.Queries;
using Aquiris.SQLite.Runtime.Tables;
using Aquiris.SQLite.Tables;
using JetBrains.Annotations;

namespace Aquiris.SQLite
{
    public struct SQLiteTable
    {
        private static readonly SQLiteTableStatementRunner _runner = new SQLiteTableStatementRunner();

        private SQLiteColumn[] _columns;
        
        [UsedImplicitly] public string name { get; private set; }
        [UsedImplicitly] public SQLiteColumn[] columns => _columns;
        
        public SQLiteTable(string name, SQLiteColumn[] columns)
        {
            this.name = name;
            _columns = columns;
        }
        
        [UsedImplicitly]
        public void Create(SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            Table table = new Table()
                .Begin(TableMode.Create)
                .Name(name);
            Query query = DeclareColumns(table).Build();
            _runner.Run(query, database, onCompleteAction);
        }

        [UsedImplicitly]
        public void CreateIfNotExists(SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            Table table = new Table()
                .Begin(TableMode.Create)
                .IfNotExists()
                .Name(name);
            Query query = DeclareColumns(table).Build();
            _runner.Run(query, database, onCompleteAction);
        }

        [UsedImplicitly]
        public void Rename(string newName, SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            Query query = new Table()
                .Begin(TableMode.Alter)
                .Name(name)
                .RenameTo(newName)
                .Build();
            
            name = newName;
            
            _runner.Run(query, database, onCompleteAction);
        }

        [UsedImplicitly]
        public void Drop(SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            Query query = new Table()
                .Begin(TableMode.Drop)
                .Name(name)
                .Build();
            _runner.Run(query, database, onCompleteAction);
        }

        public void AddColumn(SQLiteDatabase database, SQLiteColumn column, Action<QueryResult> onCompleteAction)
        {
            Query query = new Table()
                .Begin(TableMode.Alter)
                .Name(name)
                .AddColumn()
                .DeclareColumn(column.name, column.dataType)
                .Table()
                .Build();
            
            int previousLength = _columns.Length;
            Array.Resize(ref _columns, previousLength + 1);
            _columns[previousLength] = column;
            
            _runner.Run(query, database, onCompleteAction);
        }

        [UsedImplicitly]
        public static void Run(Query query, SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            _runner.Run(query, database, onCompleteAction);
        }

        private Table DeclareColumns(Table table)
        {
            Columns cols = table.Columns().Begin();
            for (int index = 0; index < _columns.Length; index++)
            {
                SQLiteColumn column = _columns[index];
                cols = cols.DeclareColumn(column.name, column.dataType);
                bool addComma = index < _columns.Length - 1;
                if (addComma) cols = cols.Separator();
            }
            return cols.End().Table();
        }
    }
}
