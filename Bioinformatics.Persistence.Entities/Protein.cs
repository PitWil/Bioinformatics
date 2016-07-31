using System.Collections.Generic;

namespace Bioinformatics.Persistence.Entities
{
    public class Protein
    {
        public string Type { get; set; }
        public string Sequence { get; set; }
        public int Length { get; set; }
        public bool Experimental { get; set; }
        public List<Databases> Databases { get; set; }
        public double FoldAmy1 { get; set; }
        public double FoldAmy1Ratio { get; set; }
        public bool FoldAmy1Class { get; set; }
        public double FoldAmy2 { get; set; }
        public double FoldAmy2Ratio { get; set; }
        public bool FoldAmy2Class { get; set; }
        public double FoldAmy3 { get; set; }
        public double FoldAmy3Ratio { get; set; }
        public bool FoldAmy3Class { get; set; }
        public double FoldAmy4 { get; set; }
        public double FoldAmy4Ratio { get; set; }
        public bool FoldAmy4Class { get; set; }
        public double FoldAmy5 { get; set; }
        public double FoldAmy5Ratio { get; set; }
        public bool FoldAmy5Class { get; set; }
        public double Aggres { get; set; }
        public double AgressRatio { get; set; }
        public bool AgressClass { get; set; }
        public double Fish1 { get; set; }
        public double Fish1Ratio { get; set; }
        public bool Fish1Class { get; set; }
        public double Fish2 { get; set; }
        public double Fish2Ratio { get; set; }
        public bool Fish2Class { get; set; }
    }
}