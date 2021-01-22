Unity SQLite
============

Aquiris SQLite UPM.

**Currently supported platforms:**
- Android;
- iOS;
- Windows (x86 & x64);
- Mac OS;

To add more platforms, we just need to build the SQLite native binaries to the platform and include the binaries in this project.

## API

We tried to implement a secure, easy and memory friendly, with the best performance we can achieve, API to facilitate SQLite query assemble.
The queries can be assembled using the API or using a const string and adding the bindings by hand.
Or you can use the declarative API that assembles the Query in runtime for you.

An example of manual query assemble:

```c#
SQLiteDatabase database = new SQLiteDatabase("path");
database.Open();

Query query = new Query("SELECT * FROM [table] WHERE id = @id");
query.Add("@id", 10);

Action<QueryResult> completion = result => {
    Assert.IsTrue(result.success);
    List<Dictionary<string, object>> results = (List<Dictionary<string, object>>)result.results;
    // do something with the results
    // do other things
    // this is the main thread
};

// background work
SQLiteFetch.Run(query, database, completion); 
```

And now an example of API query assemble:

```c#
// Inserting data
// database open

Query query = new Insert(InsertType.insert) // begins with INSERT or INSERT OR ABORT for example
        .IntoTable("doctors") // adds INTO doctors to the queue
        .Columns().Begin() // goes to the Columns struct and then adds (
        .AddColumn("id").Separator() // adds id and then a comma
        .AddColumn("name").Separator() // adds name and then a comma
        .AddColumn("degree").End() // adds degree and then adds )
        .Insert() // goes back to Insert struct
        .Values().Begin() // goes to Values struct and then adds VALUES and (
        .Add("id", id).Separator() // adds a binding for id and bind the value to the query then adds a comma
        .Add("name", name).Separator() // adds a binding for name and bind the value to the query then adds a comma
        .Add("degree", degree).End() // adds a binding for degree and bind the value to the query then adds ) 
        .Insert().Build(); // goes back to Insert struct and then builds the Query struct;
        
SQLiteInsert.Run(query, database, completion);

// or

Query[] queries = new Query[count];
// fill queries

SQLiteInsert.Run(queries, database, completion);
```

A inner join example:

```c#
Query query = new Select().Begin()
                .Columns() 
                .AddColumn("doctors.id").Separator()
                .AddColumn("doctors.name").Separator()
                .AddColumn("visits.name").As().Alias("visitor_name")
                .Select().From() 
                .TableName("doctors")
                .InnerJoin()
                .Table("visits").On()
                .Column("doctors.id")
                .Equal()
                .Column("visits.doctor_id")
                .Where()
                .Column("doctors.degree")
                .Equal()
                .Binding("degreeValue", "M D")
                .Select().Build();
                
SQLiteFetch.Run(query, database, completion);
```

That's it for now. Easy right?

## Future plans
- Add an ORM to easily manage tables and data;

That's all folks.

😁