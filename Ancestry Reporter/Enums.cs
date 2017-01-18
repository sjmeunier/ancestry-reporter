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
		[Description("Shared Y-DNA Report")]
		YDNA,
		[Description("Shared mt-DNA Report")]
		MtDNA
	}
}
