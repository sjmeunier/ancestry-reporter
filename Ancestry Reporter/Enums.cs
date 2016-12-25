﻿using System.ComponentModel;

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

	}
}
