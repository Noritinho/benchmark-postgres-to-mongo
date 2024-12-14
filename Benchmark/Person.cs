using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Benchmark;

[Table("person")]
public class PersonPg
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("uuid", Order = 1)]
    public Guid Uuid { get; set; }

    [ForeignKey("fk_name_id")]
    public NamePg Name { get; set; }
    
    [Column("name_id", Order = 2)]
    public int NameId { get; set; }
    
    [Column("age", Order = 3)]
    public int Age { get; set; }
}

[Table("name")]
public class NamePg
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id", Order = 1)]
    public int Id { get; set; }

    [Column("first_name")] 
    public string? FirstName { get; set; }
    
    [Column("last_name")] 
    public string? LastName { get; set; }
}

public class PersonMg
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public NameMg Name { get; set; }

    [BsonElement("age")]
    public int Age { get; set; }
}

public class NameMg
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("first_name")]
    public string? FirstName { get; set; }

    [BsonElement("last_name")]
    public string? LastName { get; set; }
}