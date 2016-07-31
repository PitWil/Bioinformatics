using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Bioinformatics.Buisness.Contracts;
using Bioinformatics.Buisness.Implementations.Ants;
using Bioinformatics.Buisness.Implementations.Graph;
using Bioinformatics.Buisness.Implementations.Resolver;
using Bioinformatics.Buisness.Models.Ants;
using Bioinformatics.Common.Others;
using Bioinformatics.Persistence.Entities;
using Bioinformatics.Persistence.Interfaces;
using ProteinNode = Bioinformatics.Buisness.Models.ProteinNode;

namespace Bioinformatics.Buisness.Implementations
{
    public class AntsResolver : IResolver
    {
        private readonly IProcessingResolverLogger _processingResolverLogger;


        public AntsResolver(IProcessingResolverLogger processingResolverLoger)
        {
            _processingResolverLogger = processingResolverLoger;
        }

        public DataResult<List<ProteinNode>> Resolve(List<Protein> postest, List<Protein> negtest, int AntColoniesCount,
            int AntsColonyCount, float Feromon, int IterationColonyCount, int DywersyficationTime, int AntHillSize)
        {
            var png = new ProteinNodeGenerator();
            var lpn = new List<ProteinNode>();
            var lman = new List<ProteinNode>();
            negtest.ForEach(z => lman.Add(new ProteinNode(z.Sequence)));
            postest.ForEach(z => lpn.AddRange(png.GenerateFromProteinSequence(z.Sequence, 3).Data));
            var pgg = new ProteinGraphGenerator(new ProteinNodeGenerator());

            var g = pgg.CreateGraph(AutoMapper.Mapper.Map<List<ProteinNode>>(lpn),
                AutoMapper.Mapper.Map<List<ProteinNode>>(lman));


            var acr = new AntsCliqueResolver(new AntsFeromonNodesInitializer(0.1), new EvaporatorFeromon(0.99));
            acr.Colonies = new List<ColonyOfAnts>();

            for (var t = 0; t < AntColoniesCount; t++)
            {
                var colony = new ColonyOfAnts();

                colony.Ants = new List<Ant>();
                for (var ij = 0; ij < AntsColonyCount; ij++)
                {
                    colony.Ants.Add(new Ant(Feromon));
                }

                colony.SetFeromon(Feromon);
                colony.InterationCount = IterationColonyCount;
                colony.DiversificationTime = DywersyficationTime;
                colony.AntsHillSize = AntHillSize;
                acr.Colonies.Add(colony);
            }
            return acr.Resolve(g.Data);
        }


        public void ResolveWithStdAlgorithmAndSaveResults(List<ProteinNode> source, List<Protein> positivhProteins,
            List<Protein> negativeProteins)
        {
            var s2 = new RegexGenerator().GetRegexFromListOfProteinNode(source);

            var b1 = new List<ProteinNode>();
            var b2 = new List<ProteinNode>();
            positivhProteins.ForEach(z => b1.Add(new ProteinNode(z.Sequence)));
            negativeProteins.ForEach(z => b2.Add(new ProteinNode(z.Sequence)));

            var reg = new Regex(s2);

            _processingResolverLogger.ResumeWriteLine("----------------Algorytm podstawowy-----------------");
            _processingResolverLogger.ResumeWriteLine("regex:");
            _processingResolverLogger.ResumeWriteLine(s2);
            _processingResolverLogger.ResumeWriteLine("Wyniki dla przykładów pozytywnych :");
            b1.ForEach(z => _processingResolverLogger.ResumeWriteLine(z.RealValue + " " + reg.IsMatch(z.Value)));
            _processingResolverLogger.ResumeWriteLine("Wyniki dla przykładów negatywnych:");
            b2.ForEach(z => _processingResolverLogger.ResumeWriteLine(z.RealValue + " " + reg.IsMatch(z.Value)));
            _processingResolverLogger.ResumeWriteLine("---------------------------------");

            _processingResolverLogger.ResumeLogWriteLine("----------------Algorytm podstawowy-----------------");
            var posW = 0;
            var negW = 0;
            b1.ForEach(z => { if (reg.IsMatch(z.Value)) posW++; });
            b2.ForEach(z => { if (!reg.IsMatch(z.Value)) negW++; });
            var TP = posW;
            var FN = b1.Count - posW;
            var FP = b2.Count - negW;
            var TN = negW;
            float TPR = (float)TP / ((float)(TP + FN));
            float TNR = (float)TN / ((float)(FP + TN));
            float PREC = TP > 0 ? (float)TP / ((float)(TP + FP)) : 0;
            float ACC = ((float)(TP + TN)) / ((float)(TP + FP + TP + TN));
            float F1 = (2.0f * (float)TP) / ((float)(2.0f * TP + FP + FN));
            float w = ((float)TP + (float)FP) * ((float)TP + (float)FN) * ((float)TN + (float)FP) * ((float)TN + (float)FN);
            float tmpMCC = (float)Math.Sqrt(w);
            float MCC = ((float)(TP * TN - FP * FN)) / (tmpMCC == 0f ? 1.0f : tmpMCC);
            float AUC = ((float)(TP) / (float)(TP + FN) + (((float)TN / ((float)(FP + TN))))) * 0.5f;

            _processingResolverLogger.ResumeLogWriteLine("TP:" + TP);
            _processingResolverLogger.ResumeLogWriteLine("FN:" + FN);
            _processingResolverLogger.ResumeLogWriteLine("FP:" + FP);
            _processingResolverLogger.ResumeLogWriteLine("TN:" + TN);
            _processingResolverLogger.ResumeLogWriteLine("TPR:" + TPR);
            _processingResolverLogger.ResumeLogWriteLine("TNR:" + TNR);
            _processingResolverLogger.ResumeLogWriteLine("PREC:" + PREC);
            _processingResolverLogger.ResumeLogWriteLine("ACC:" + ACC);
            _processingResolverLogger.ResumeLogWriteLine("F1:" + F1);
            _processingResolverLogger.ResumeLogWriteLine("MCC:" + MCC);
            _processingResolverLogger.ResumeLogWriteLine("AUC:" + AUC);
            _processingResolverLogger.ResumeLogWriteLine("---------------------------------");
            _processingResolverLogger.ResumeResultWriteLine("Ant1;" + PREC + ";" + TPR + ";" + F1 + ";" + ACC + ";" +
                                                          AUC + ";" + MCC + ";" + TP + ";" + FP + ";" + FN +
                                                          ";" + TN);
        }

        public void ResolveWithoutxxxPaternAlgorithmAndSaveResults(List<ProteinNode> source,
            List<Protein> positivhProteins, List<Protein> negativeProteins)
        {
            var s2 = new RegexGenerator().GetRegexFromListOfProteinNode(source);

            var b1 = new List<ProteinNode>();
            var b2 = new List<ProteinNode>();
            positivhProteins.ForEach(z => b1.Add(new ProteinNode(z.Sequence)));
            negativeProteins.ForEach(z => b2.Add(new ProteinNode(z.Sequence)));
            s2 = s2.Replace("(", "");
            var xxx = s2.Split(')');
            var xx1 = xxx[0].Split('|').ToList();
            var xx2 = xxx[1].Split('|').ToList();
            var xx3 = xxx[2].Split('|').ToList();

            var r1 = "";
            var r2 = "";
            var r3 = "";
            for (int index = 0; index < xx1.Count; index++)
            {
                var z = xx1[index];
                if (z.Length > 1)
                {
                    r1 += index < xx1.Count - 1 ? z + "|" : z;
                }
            }

            for (int index = 0; index < xx2.Count; index++)
            {
                var z = xx2[index];
                if (z.Length > 1)
                {
                    r2 += index < xx2.Count - 1 ? z + "|" : z;
                }
            }

            for (int index = 0; index < xx3.Count - 1; index++)
            {
                var z = xx3[index];
                if (z.Length > 1)
                {
                    r3 += index < xx3.Count - 1 ? z + "|" : z;
                }
            }

            var reg = new Regex("(" + r1 + ")" + "(" + r2 + ")" + "(" + r3 + ")");

            _processingResolverLogger.ResumeWriteLine("----------------Algorytm bez \"XXX\"-----------------");
            _processingResolverLogger.ResumeWriteLine("regex:");
            _processingResolverLogger.ResumeWriteLine("(" + r1 + ")" + "(" + r2 + ")" + "(" + r3 + ")");
            _processingResolverLogger.ResumeWriteLine("Wyniki dla przykładów pozytywnych :");
            b1.ForEach(
                z =>
                    _processingResolverLogger.ResumeWriteLine(z.RealValue + " " +
                                                              (reg.IsMatch(z.Value))));
            _processingResolverLogger.ResumeWriteLine("Wyniki dla przykładów negatywnych:");
            b2.ForEach(
                z =>
                    _processingResolverLogger.ResumeWriteLine(z.RealValue + " " +
                                                              (reg.IsMatch(z.Value))));


            _processingResolverLogger.ResumeWriteLine("---------------------------------");

            _processingResolverLogger.ResumeLogWriteLine("----------------Algorytm bez \"XXX\"-----------------");
            var posW = 0;
            var negW = 0;
            b1.ForEach(z => { if (reg.IsMatch(z.Value)) posW++; });
            b2.ForEach(z => { if (!reg.IsMatch(z.Value)) negW++; });
            var TP = posW;
            var FN = b1.Count - posW;
            var FP = b2.Count - negW;
            var TN = negW;

            float TPR = (float)TP / ((float)(TP + FN));
            float TNR = (float)TN / ((float)(FP + TN));
            float PREC = TP > 0 ? (float)TP / ((float)(TP + FP)) : 0;
            float ACC = ((float)(TP + TN)) / ((float)(TP + FP + TP + TN));
            float F1 = (2.0f * (float)TP) / ((float)(2.0f * TP + FP + FN));
            float w = ((float)TP + (float)FP) * ((float)TP + (float)FN) * ((float)TN + (float)FP) * ((float)TN + (float)FN);
            float tmpMCC = (float)Math.Sqrt(w);
            float MCC = ((float)(TP * TN - FP * FN)) / (tmpMCC == 0f ? 1.0f : tmpMCC);
            float AUC = ((float)(TP) / (float)(TP + FN) + (((float)TN / ((float)(FP + TN))))) * 0.5f;


            _processingResolverLogger.ResumeLogWriteLine("TP:" + TP);
            _processingResolverLogger.ResumeLogWriteLine("FN:" + FN);
            _processingResolverLogger.ResumeLogWriteLine("FP:" + FP);
            _processingResolverLogger.ResumeLogWriteLine("TN:" + TN);
            _processingResolverLogger.ResumeLogWriteLine("TPR:" + TPR);
            _processingResolverLogger.ResumeLogWriteLine("TNR:" + TNR);
            _processingResolverLogger.ResumeLogWriteLine("PREC:" + PREC);
            _processingResolverLogger.ResumeLogWriteLine("ACC:" + ACC);
            _processingResolverLogger.ResumeLogWriteLine("F1:" + F1);
            _processingResolverLogger.ResumeLogWriteLine("MCC:" + MCC);
            _processingResolverLogger.ResumeLogWriteLine("AUC:" + AUC);
            _processingResolverLogger.ResumeLogWriteLine("---------------------------------");
            _processingResolverLogger.ResumeResultWriteLine("Ant2;" + PREC + ";" + TPR + ";" + F1 + ";" + ACC + ";" +
                                                       AUC + ";" + MCC + ";" + TP +";" + FP + ";" + FN +
                                                       ";" + TN);
        }


        public void ResolveWithoutNegativeRegexPaternAlgorithmAndSaveResults(List<ProteinNode> source,
            List<ProteinNode> negativeSource, List<Protein> positivhProteins, List<Protein> negativeProteins)
        {
            var s1 = new RegexGenerator().GetRegexFromListOfProteinNode(source);
            var s2 = new RegexGenerator().GetRegexFromListOfProteinNode(negativeSource);
            var tmps = s1;
            s1 = s2;
            s2 = tmps;

            s2 = s2.Replace("(", "");
            var xxx = s2.Split(')');
            var xx1 = xxx[0].Split('|').ToList();
            var xx2 = xxx[1].Split('|').ToList();
            var xx3 = xxx[2].Split('|').ToList();


            s1 = s1.Replace("(", "");
            var xxx2 = s1.Split(')');
            var xx21 = xxx2[0].Split('|').ToList();
            var xx22 = xxx2[1].Split('|').ToList();
            var xx32 = xxx2[2].Split('|').ToList();

            var res1xx = "";
            for (var ix = 0; ix < xx21.Count; ix++)
            {
                if (!xx1.Contains(xx21[ix]))
                {
                    res1xx += res1xx == "" ? xx21[ix] : "|" + xx21[ix];
                }
            }

            var res2xx = "";
            for (var ix = 0; ix < xx22.Count; ix++)
            {
                if (!xx2.Contains(xx22[ix]))
                {
                    res2xx += res2xx == "" ? xx22[ix] : "|" + xx22[ix];
                }
            }

            var res3xx = "";
            for (var ix = 0; ix < xx32.Count; ix++)
            {
                if (!xx3.Contains(xx32[ix]))
                {
                    res3xx += res3xx == "" ? xx32[ix] : "|" + xx32[ix];
                }
            }


            var rr = "(" + res1xx + ")" + "(" + res2xx + ")" + "(" + res3xx + ")";
            var reg = new Regex(rr);


            var b1 = new List<ProteinNode>();
            var b2 = new List<ProteinNode>();
            positivhProteins.ForEach(z => b1.Add(new ProteinNode(z.Sequence)));
            negativeProteins.ForEach(z => b2.Add(new ProteinNode(z.Sequence)));


            _processingResolverLogger.ResumeWriteLine(
                "----------------Algorytm z usuwaniem elementów reg-----------------");

            _processingResolverLogger.ResumeWriteLine("regex:");
            _processingResolverLogger.ResumeWriteLine(s2);
            _processingResolverLogger.ResumeWriteLine("Wyniki dla przykładów pozytywnych :");
            b1.ForEach(
                z =>
                    _processingResolverLogger.ResumeWriteLine(z.RealValue + " " + (reg.IsMatch(z.Value))));

            _processingResolverLogger.ResumeWriteLine("Wyniki dla przykładów negatywnych:");
            b2.ForEach(
                z =>
                    _processingResolverLogger.ResumeWriteLine(z.RealValue + " " + (reg.IsMatch(z.Value))));


            _processingResolverLogger.ResumeWriteLine("---------------------------------");

            _processingResolverLogger.ResumeLogWriteLine(
                "----------------Algorytm z usuwaniem elementów reg-----------------");
            var posW = 0;
            var negW = 0;
            b1.ForEach(z => { if (reg.IsMatch(z.Value)) posW++; });
            b2.ForEach(z => { if (!reg.IsMatch(z.Value)) negW++; });
            var TP = posW;
            var FN = b1.Count - posW;
            var FP = b2.Count - negW;
            var TN = negW;
            float TPR = (float)TP / ((float)(TP + FN));
            float TNR = (float)TN / ((float)(FP + TN));
            float PREC = TP > 0 ? (float)TP / ((float)(TP + FP)) : 0;
            float ACC = ((float)(TP + TN)) / ((float)(TP + FP + TP + TN));
            float F1 = (2.0f * (float)TP) / ((float)(2.0f * TP + FP + FN));
            float w = ((float)TP + (float)FP) * ((float)TP + (float)FN) * ((float)TN + (float)FP) * ((float)TN + (float)FN);
            float tmpMCC = (float)Math.Sqrt(w);
            float MCC = ((float)(TP * TN - FP * FN)) / (tmpMCC == 0f ? 1.0f : tmpMCC);
            float AUC = ((float)(TP) / (float)(TP + FN) + (((float)TN / ((float)(FP + TN))))) * 0.5f;

            _processingResolverLogger.ResumeLogWriteLine("TP:" + TP);
            _processingResolverLogger.ResumeLogWriteLine("FN:" + FN);
            _processingResolverLogger.ResumeLogWriteLine("FP:" + FP);
            _processingResolverLogger.ResumeLogWriteLine("TN:" + TN);
            _processingResolverLogger.ResumeLogWriteLine("TPR:" + TPR);
            _processingResolverLogger.ResumeLogWriteLine("TNR:" + TNR);
            _processingResolverLogger.ResumeLogWriteLine("PREC:" + PREC);
            _processingResolverLogger.ResumeLogWriteLine("ACC:" + ACC);
            _processingResolverLogger.ResumeLogWriteLine("F1:" + F1);
            _processingResolverLogger.ResumeLogWriteLine("MCC:" + MCC);
            _processingResolverLogger.ResumeLogWriteLine("AUC:" + AUC);
            _processingResolverLogger.ResumeLogWriteLine("---------------------------------");
            _processingResolverLogger.ResumeResultWriteLine("Ant3;" + PREC + ";" + TPR + ";" + F1 + ";" + ACC + ";" +
                                                           AUC + ";" + MCC + ";" + TP + ";" + FP + ";" + FN +
                                                           ";" + TN);
        }
    }
}