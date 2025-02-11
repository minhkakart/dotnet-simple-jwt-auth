using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseAuth.Database.Entity;

public class Role
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("uuid")]
    public string Uuid { get; set; } = Guid.NewGuid().ToString();
    [Column("name")]
    public string Name { get; set; }
    
    public ICollection<Account>? Accounts { get; set; }
}