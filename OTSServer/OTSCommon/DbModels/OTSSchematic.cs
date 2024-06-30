using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace OTSCommon.Models
{
    public class OTSSchematic : INetRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required int ComponentId { get; set; }   
        public required string LayoutInfo { get; set; }
        public required int XPos { get; set; }
        public required int YPos { get; set; }
        public required int Zoom { get; set; }

        public DateTime? DeletedOn { get; set; }
    }

    public class OTSSchematicEditorSettings : INetRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required int ThemeId { get; set; }
        public required OTSSchematicTheme? Theme { get; set; }

        public required uint TitleFontSize { get; set; }
        public required uint TextFontSize { get; set; }

        //1-1 with user
        public required int UserSettingsId { get; set; }
        public required UserSettings? UserSettings { get; set; }

        public DateTime? DeletedOn { get; set; }
    }

    public class OTSSchematicTheme : INetRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string ThemeName { get; set; }

        public required uint BackgroundColor { get; set; }
        public required uint ContainerColor { get; set; }
        public required uint ConnectionColor { get; set; }
        public required uint TextColor { get; set; }
        public required uint TitleColor { get; set; }
        public required uint BorderColor { get; set; }
        public required uint InputNodeColor { get; set; }
        public required uint OutputNodeColor { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
