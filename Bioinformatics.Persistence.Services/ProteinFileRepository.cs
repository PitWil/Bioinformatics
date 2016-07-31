using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bioinformatics.Common.Others;
using Bioinformatics.Persistence.Entities;
using Bioinformatics.Persistence.Interfaces;

namespace Bioinformatics.Persistence.Services
{
    public class ProteinFileRepository : IProteinRepository
    {
        private readonly string _pathToCsv;

        public ProteinFileRepository(string pathToCsv)
        {
            _pathToCsv = pathToCsv;
        }

        public DataResult<List<Protein>> GetAllProtein()
        {
            var result = new DataResult<List<Protein>>();
            var resultList = new List<Protein>();
            try
            {
                var allLines = File.ReadAllLines(_pathToCsv).ToList();
                allLines.RemoveAt(0);
                allLines.ForEach(a =>
                {
                    var data = a.Split(',');
                    var databases = new List<Databases>();
                    if (data.Length >= 29)
                    {
                        var zq = data[28].Split('|').ToList();
                        zq.ForEach(z =>
                        {
                            switch (z)
                            {
                                case "WALTZ":
                                    databases.Add(Databases.Waltzdb);
                                    break;
                                case "AmylHex":
                                    databases.Add(Databases.AmylHex);
                                    ;
                                    break;
                                case "AmylFrag":
                                    databases.Add(Databases.AmylFrag);
                                    ;
                                    break;
                                case "AGGRESCAN":
                                    databases.Add(Databases.Aggrescan);
                                    ;
                                    break;
                                case "TANGO":
                                    databases.Add(Databases.Tango);
                                    ;
                                    break;
                                case "AmyLoad":
                                    databases.Add(Databases.AmyLoad);
                                    ;
                                    break;
                                default:
                                    break;
                            }
                        });
                    }
                    resultList.Add(new Protein
                    {
                        Type = data[0],
                        Sequence = data[1],
                        // Length = Convert.ToInt32(data[2]),
                        Experimental = data[3] == "1",
                        //FoldAmy1 = Convert.ToDouble(data[4]),
                        //FoldAmy1Ratio = Convert.ToDouble(data[5]),
                        //FoldAmy1Class = data[6] == "1",
                        //FoldAmy2 = Convert.ToDouble(data[7]),
                        //FoldAmy2Ratio = Convert.ToDouble(data[8]),
                        //FoldAmy2Class = data[9] == "1",
                        //FoldAmy3 = Convert.ToDouble(data[10]),
                        //FoldAmy3Ratio = Convert.ToDouble(data[11]),
                        //FoldAmy3Class = data[12] == "1",
                        //FoldAmy4 = Convert.ToDouble(data[13]),
                        //FoldAmy4Ratio = Convert.ToDouble(data[14]),
                        //FoldAmy4Class = data[15] == "1",
                        //FoldAmy5 = Convert.ToDouble(data[16]),
                        //FoldAmy5Ratio = Convert.ToDouble(data[17]),
                        //FoldAmy5Class = data[18] == "1",
                        //Aggres = Convert.ToDouble(data[19]),
                        //AgressRatio = Convert.ToDouble(data[20]),
                        //AgressClass = data[21] == "1",
                        //Fish1 = Convert.ToDouble(data[22]),
                        //Fish1Ratio = Convert.ToDouble(data[23]),
                        //Fish2Class = data[24] == "1",
                        //Fish2 = Convert.ToDouble(data[25]),
                        //Fish2Ratio = Convert.ToDouble(data[26]),
                        //Fish1Class = data[27] == "1"
                        Databases = databases
                    });
                });
                result.Successed = true;
                result.Data = resultList;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                return result;
            }

            return result;
        }

        public DataResult<List<Protein>> GetProteinByParametr(Func<Protein, bool> param)
        {
            var result = GetAllProtein();
            result.Data = result.Successed ? result.Data.Where(param).ToList() : null;
            return result;
        }
    }
}