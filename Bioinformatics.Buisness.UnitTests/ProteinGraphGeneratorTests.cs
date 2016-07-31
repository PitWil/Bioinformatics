using System.Collections.Generic;
using Bioinformatics.Buisness.Contracts.Graph;
using Bioinformatics.Buisness.Implementations.Graph;
using Bioinformatics.Buisness.Models;
using Bioinformatics.Common.Others;
using Moq;
using NUnit.Framework;

namespace Bioinformatics.Buisness.UnitTests
{
    [TestFixture]
    public class ProteinGraphGeneratorTests
    {
        [SetUp]
        public void Setup()
        {
            _mProteinNodsGenerator = new Mock<IProteinNodeGenerator>();
            _proteinGraphGenerator = new ProteinGraphGenerator(_mProteinNodsGenerator.Object);
        }

        private IProteinGraphGenerator _proteinGraphGenerator;
        private Mock<IProteinNodeGenerator> _mProteinNodsGenerator;

        [Test]
        public void ProteinGraphGenerator_args_have_to_be_checked_00002()
        {
            var result = _proteinGraphGenerator.CreateGraph(null, null);
            Assert.IsTrue(result != null);
            Assert.IsFalse(result.Successed);
            Assert.IsTrue(result.ErrorMessage.Equals("Incorrect args!"));
        }


        [Test]
        public void ProteinGraphGenerator_should_create_correct_graph_should_be_connected_each_to_each_00003()
        {
            Setup();


            //(a1, a2, a3) i(b1, b2, b3) to badamy słowa
            //a1b2a3, a1b2b3, a1a2b3, b1a2a3, b1a2b3, b1b2a3
            var a = new ProteinNode("a1,a2,a3");
            var b = new ProteinNode("b1,b2,b3");
            var c = new ProteinNode("c1,c2,c3");
            var d = new ProteinNode("d1,d2,d3");
            var list = new List<ProteinNode> {a, b, c, d};

            var proteinAbResult = new DataResult<List<ProteinNode>>
            {
                Data = new List<ProteinNode>
                {
                    new ProteinNode("a1,b2,a3"),
                    new ProteinNode("a1,b2,b3"),
                    new ProteinNode("a1,a2,b3"),
                    new ProteinNode("b1,a2,b3"),
                    new ProteinNode("b1,a2,b3"),
                    new ProteinNode("b1,b2,a3")
                },
                Successed = true
            };

            var proteinCdResult = new DataResult<List<ProteinNode>>
            {
                Data = new List<ProteinNode>
                {
                    new ProteinNode("c1,d2,c3"),
                    new ProteinNode("c1,d2,d3"),
                    new ProteinNode("c1,c2,d3"),
                    new ProteinNode("d1,c2,d3"),
                    new ProteinNode("d1,c2,d3"),
                    new ProteinNode("d1,d2,c3")
                },
                Successed = true
            };
            var proteinEmptyResult = new DataResult<List<ProteinNode>>
            {
                Data = new List<ProteinNode>(),
                Successed = true
            };

            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(a, b)).Returns(proteinAbResult);
            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(c, d)).Returns(proteinCdResult);
            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(a, d)).Returns(proteinEmptyResult);
            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(a, c)).Returns(proteinEmptyResult);
            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(b, c)).Returns(proteinEmptyResult);
            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(b, d)).Returns(proteinEmptyResult);

            var result = _proteinGraphGenerator.CreateGraph(
                list,
                new List<ProteinNode>
                {
                    new ProteinNode("a1,a2,b3"),
                    new ProteinNode("c1,d2,c3")
                });

            //        Assert.IsTrue(a.Neighbors[0].Node == c);
            //        Assert.IsTrue(a.Neighbors[1].Node == d);

            //        Assert.IsTrue(b.Neighbors[0].Node == c);
            //        Assert.IsTrue(b.Neighbors[1].Node == d);

            //        Assert.IsTrue(c.Neighbors[0].Node == a);
            //       Assert.IsTrue(c.Neighbors[1].Node == b);

            //       Assert.IsTrue(d.Neighbors[0].Node == a);
            //        Assert.IsTrue(d.Neighbors[1].Node == b);


            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Successed);
        }

        [Test]
        public void ProteinGraphGenerator_Should_never_return_null_00001()
        {
            var result = _proteinGraphGenerator.CreateGraph(null, null);
            Assert.IsTrue(result != null);
        }

        [Test]
        public void ProteinNodeGenerator_GetAllNodesFromGraph_should_always_return_not_null_00004()
        {
            Setup();


            //(a1, a2, a3) i(b1, b2, b3) to badamy słowa
            //a1b2a3, a1b2b3, a1a2b3, b1a2a3, b1a2b3, b1b2a3
            var a = new ProteinNode("a1,a2,a3");
            var b = new ProteinNode("b1,b2,b3");
            var c = new ProteinNode("c1,c2,c3");
            var d = new ProteinNode("d1,d2,d3");
            var list = new List<ProteinNode> {a, b, c, d};

            var proteinAbResult = new DataResult<List<ProteinNode>>
            {
                Data = new List<ProteinNode>
                {
                    new ProteinNode("a1,b2,a3"),
                    new ProteinNode("a1,b2,b3"),
                    new ProteinNode("a1,a2,b3"),
                    new ProteinNode("b1,a2,b3"),
                    new ProteinNode("b1,a2,b3"),
                    new ProteinNode("b1,b2,a3")
                },
                Successed = true
            };

            var proteinCdResult = new DataResult<List<ProteinNode>>
            {
                Data = new List<ProteinNode>
                {
                    new ProteinNode("c1,d2,c3"),
                    new ProteinNode("c1,d2,d3"),
                    new ProteinNode("c1,c2,d3"),
                    new ProteinNode("d1,c2,d3"),
                    new ProteinNode("d1,c2,d3"),
                    new ProteinNode("d1,d2,c3")
                },
                Successed = true
            };
            var proteinEmptyResult = new DataResult<List<ProteinNode>>
            {
                Data = new List<ProteinNode>(),
                Successed = true
            };

            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(a, b)).Returns(proteinAbResult);
            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(c, d)).Returns(proteinCdResult);
            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(a, d)).Returns(proteinEmptyResult);
            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(a, c)).Returns(proteinEmptyResult);
            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(b, c)).Returns(proteinEmptyResult);
            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(b, d)).Returns(proteinEmptyResult);

            var graph = _proteinGraphGenerator.CreateGraph(
                list,
                new List<ProteinNode>
                {
                    new ProteinNode("a1,a2,b3"),
                    new ProteinNode("c1,d2,c3")
                });

            var result = _proteinGraphGenerator.GetAllNodesFromGraph(graph.Data[0]);

            Assert.IsTrue(result != null);
        }

        [Test]
        public void ProteinNodeGenerator_GetAllNodesFromGraph_should_correnct_00007()
        {
            Setup();


            //(a1, a2, a3) i(b1, b2, b3) to badamy słowa
            //a1b2a3, a1b2b3, a1a2b3, b1a2a3, b1a2b3, b1b2a3
            var a = new ProteinNode("a1,a2,a3");
            var b = new ProteinNode("b1,b2,b3");
            var c = new ProteinNode("c1,c2,c3");
            var d = new ProteinNode("d1,d2,d3");
            var list = new List<ProteinNode> {a, b, c, d};

            var proteinAbResult = new DataResult<List<ProteinNode>>
            {
                Data = new List<ProteinNode>
                {
                    new ProteinNode("a1,b2,a3"),
                    new ProteinNode("a1,b2,b3"),
                    new ProteinNode("a1,a2,b3"),
                    new ProteinNode("b1,a2,b3"),
                    new ProteinNode("b1,a2,b3"),
                    new ProteinNode("b1,b2,a3")
                },
                Successed = true
            };

            var proteinCdResult = new DataResult<List<ProteinNode>>
            {
                Data = new List<ProteinNode>
                {
                    new ProteinNode("c1,d2,c3"),
                    new ProteinNode("c1,d2,d3"),
                    new ProteinNode("c1,c2,d3"),
                    new ProteinNode("d1,c2,d3"),
                    new ProteinNode("d1,c2,d3"),
                    new ProteinNode("d1,d2,c3")
                },
                Successed = true
            };
            var proteinEmptyResult = new DataResult<List<ProteinNode>>
            {
                Data = new List<ProteinNode>(),
                Successed = true
            };

            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(a, b)).Returns(proteinAbResult);
            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(c, d)).Returns(proteinCdResult);
            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(a, d)).Returns(proteinEmptyResult);
            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(a, c)).Returns(proteinEmptyResult);
            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(b, c)).Returns(proteinEmptyResult);
            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(b, d)).Returns(proteinEmptyResult);

            var graph = _proteinGraphGenerator.CreateGraph(
                list,
                new List<ProteinNode>
                {
                    new ProteinNode("a1,a2,b3"),
                    new ProteinNode("c1,d2,c3")
                });

            var result = _proteinGraphGenerator.GetAllNodesFromGraph(graph.Data[0]);


            Assert.IsTrue(result != null && result.Successed);
            Assert.IsTrue(result.Data != null && result.Data.Count == 4);
        }


        [Test]
        public void ProteinNodeGenerator_GetAllNodesFromGraph_Successed_should_be_false_null_arg_00005()
        {
            Setup();


            //(a1, a2, a3) i(b1, b2, b3) to badamy słowa
            //a1b2a3, a1b2b3, a1a2b3, b1a2a3, b1a2b3, b1b2a3
            var a = new ProteinNode("a1,a2,a3");
            var b = new ProteinNode("b1,b2,b3");
            var c = new ProteinNode("c1,c2,c3");
            var d = new ProteinNode("d1,d2,d3");
            var list = new List<ProteinNode> {a, b, c, d};

            var proteinAbResult = new DataResult<List<ProteinNode>>
            {
                Data = new List<ProteinNode>
                {
                    new ProteinNode("a1,b2,a3"),
                    new ProteinNode("a1,b2,b3"),
                    new ProteinNode("a1,a2,b3"),
                    new ProteinNode("b1,a2,b3"),
                    new ProteinNode("b1,a2,b3"),
                    new ProteinNode("b1,b2,a3")
                },
                Successed = true
            };

            var proteinCdResult = new DataResult<List<ProteinNode>>
            {
                Data = new List<ProteinNode>
                {
                    new ProteinNode("c1,d2,c3"),
                    new ProteinNode("c1,d2,d3"),
                    new ProteinNode("c1,c2,d3"),
                    new ProteinNode("d1,c2,d3"),
                    new ProteinNode("d1,c2,d3"),
                    new ProteinNode("d1,d2,c3")
                },
                Successed = true
            };
            var proteinEmptyResult = new DataResult<List<ProteinNode>>
            {
                Data = new List<ProteinNode>(),
                Successed = true
            };

            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(a, b)).Returns(proteinAbResult);
            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(c, d)).Returns(proteinCdResult);
            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(a, d)).Returns(proteinEmptyResult);
            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(a, c)).Returns(proteinEmptyResult);
            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(b, c)).Returns(proteinEmptyResult);
            _mProteinNodsGenerator.Setup(x => x.GenerateProteinNodeCombination(b, d)).Returns(proteinEmptyResult);

            var graph = _proteinGraphGenerator.CreateGraph(
                list,
                new List<ProteinNode>
                {
                    new ProteinNode("a1,a2,b3"),
                    new ProteinNode("c1,d2,c3")
                });

            var result = _proteinGraphGenerator.GetAllNodesFromGraph(null);

            Assert.IsTrue(result.Successed == false);
            Assert.IsTrue(result.ErrorMessage.Equals("Incorrect arg!"));
        }

        [Test]
        public void
            ProteinNodeGenerator_GetAllNodesFromGraph_Successed_should_be_true_should_return_one_element_and_be_arg_00006
            ()
        {
            Setup();
            var arg = new ProteinNode("asdfslaf");
            var result = _proteinGraphGenerator.GetAllNodesFromGraph(arg);

            Assert.IsTrue(result.Successed);
            Assert.IsTrue(result.Data != null && result.Data.Count == 1 && result.Data[0] == arg);
        }
    }
}