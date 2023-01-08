using SQLite;

namespace LearnEnglish.XN.Core.Definitions.DalModels;

[Table("Messages")]
public class MessageDalModel
{
    [PrimaryKey, AutoIncrement, Column("_id")]
    public int Id { get; set; }

    public string Text { get; set; }

    public bool IsMine { get; set; }

    public string Variants { get; set; }

    public string MessageType { get; set; }
}
