using System.ComponentModel;

namespace Ancestry_Reporter
{
	public enum ReportType
	{
		[Description("Full Ancestry")]
		FullAncestry,
		[Description("Abridged Ancestry")]
		AbridgedAncestry,
		[Description("Leaf Ancestors")]
		LeafAncestor,
		[Description("Abridged Leaf Ancestors")]
		AbridgedLeafAncestor,
		[Description("Generation Summary")]
		GenerationSummary,
		[Description("Descendant")]
		Descendant,
		[Description("Ancestor Place Report")]
		Place,
        [Description("Ancestor Place Summary")]
        PlaceSummary,
        [Description("Shared Y-DNA Report")]
		YDNA,
		[Description("Shared mt-DNA Report")]
		MtDNA,
        [Description("Average Lifespan")]
        Lifespan,
        [Description("Country of Origin")]
        CountryOfOrigin,
        [Description("Country of Origin Summary")]
        CountryOfOriginSummary
    }
}
