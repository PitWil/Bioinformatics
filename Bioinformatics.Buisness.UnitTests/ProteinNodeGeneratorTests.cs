using System.Collections.Generic;
using Bioinformatics.Buisness.Contracts.Graph;
using Bioinformatics.Buisness.Implementations.Graph;
using Bioinformatics.Buisness.Models;
using NUnit.Framework;

namespace Bioinformatics.Buisness.UnitTests
{
    [TestFixture]
    public class ProteinNodeGeneratorTests
    {
        [SetUp]
        public void Setup()
        {
            _proteinNodeGenerator = new ProteinNodeGenerator();
        }

        private IProteinNodeGenerator _proteinNodeGenerator;

        [Test]
        public void GenerateFromProteinSequence_Correct_arg_shuld_return_Success_00005()
        {
            var result = _proteinNodeGenerator.GenerateFromProteinSequence("asd", 1);
            Assert.IsTrue(result.Successed);
        }


        [Test]
        public void GenerateFromProteinSequence_Correct_arg_shuld_return_Success_and_data_shuld_not_be_null_00006()
        {
            var result = _proteinNodeGenerator.GenerateFromProteinSequence("asd", 1);
            Assert.IsTrue(result.Successed);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public void GenerateFromProteinSequence_GenerateFromProteinSequence_Never_Should_return_null_00001()
        {
            var result = _proteinNodeGenerator.GenerateFromProteinSequence(null, 0);
            Assert.NotNull(result);
        }


        [Test]
        public void GenerateFromProteinSequence_GenerateFromProteinSequence_Should_in_msg_return_invalid_arg_00002()
        {
            var result = _proteinNodeGenerator.GenerateFromProteinSequence(null, 0);
            Assert.IsFalse(result.Successed);
            Assert.IsTrue(result.ErrorMessage.Equals("Invalid argument!"));
        }

        [Test]
        public void
            GenerateFromProteinSequence_Incorrect_protein_should_return_false_and_msg_should_be_Inccorect_protein_00007()
        {
            var result = _proteinNodeGenerator.GenerateFromProteinSequence("", 1);
            Assert.IsFalse(result.Successed);
            Assert.IsTrue(result.ErrorMessage.Equals("Incorrect protein!"));
        }


        [Test]
        public void GenerateFromProteinSequence_Sequence_can_be_splited_into_more_than_sequence_length_00008()
        {
            var result = _proteinNodeGenerator.GenerateFromProteinSequence("aaaa", 5);
            Assert.IsFalse(result.Successed);
            Assert.IsTrue(result.ErrorMessage.Equals("Incorrect split count!"));
        }

        [Test]
        public void GenerateFromProteinSequence_Sequence_can_be_splited_into_more_than_sequence_length_00009()
        {
            var result = _proteinNodeGenerator.GenerateFromProteinSequence("aaaa", 50);
            Assert.IsFalse(result.Successed);
            Assert.IsTrue(result.ErrorMessage.Equals("Incorrect split count!"));
        }


        [Test]
        public void GenerateFromProteinSequence_Sequence_have_to_be_splited_into_3_00010()
        {
            //SHOULD RETURN:
            // V, Q, IVYK
            // VQ, I, VYK
            // VQI, V, YK
            // VQIV, Y, K
            // V, QI, VYK
            // VQ, IV, YK
            // VQI, VY, K
            // V, QIV, YK
            // VQ, IVY, K
            // V, QIVY, K
            var result = _proteinNodeGenerator.GenerateFromProteinSequence("VQIVYK", 3);
            Assert.IsTrue(result.Successed);
            Assert.IsTrue(result.Data != null);
            Assert.IsTrue(result.Data.Count == 10);
            Assert.IsTrue(result.Data[0].Value.Equals("V, Q, IVYK"));
            Assert.IsTrue(result.Data[1].Value.Equals("VQ, I, VYK"));
            Assert.IsTrue(result.Data[2].Value.Equals("VQI, V, YK"));
            Assert.IsTrue(result.Data[3].Value.Equals("VQIV, Y, K"));
            Assert.IsTrue(result.Data[4].Value.Equals("V, QI, VYK"));
            Assert.IsTrue(result.Data[5].Value.Equals("VQ, IV, YK"));
            Assert.IsTrue(result.Data[6].Value.Equals("VQI, VY, K"));
            Assert.IsTrue(result.Data[7].Value.Equals("V, QIV, YK"));
            Assert.IsTrue(result.Data[8].Value.Equals("VQ, IVY, K"));
            Assert.IsTrue(result.Data[9].Value.Equals("V, QIVY, K"));
        }

        [Test]
        public void GenerateFromProteinSequence_Sequence_have_to_be_splited_into_3_00011()
        {
            //SHOULD RETURN:
            // V, Q, IVYK
            // VQ, I, VYK
            // VQI, V, YK
            // VQIV, Y, K
            // V, QI, VYK
            // VQ, IV, YK
            // VQI, VY, K
            // V, QIV, YK
            // VQ, IVY, K
            // V, QIVY, K
            var result =
                _proteinNodeGenerator.GenerateFromProteinSequence(
                    "VQIVYKVQIVYKVQIVYKVQIVYKVQIVYKVQIVYVQIVYKVQIVYKVQIVYKVQIVYKVQIVYKVQIVYKK", 3);
            Assert.IsTrue(result.Successed);
            Assert.IsTrue(result.Data != null);
        }

        [Test]
        public void GenerateFromProteinSequence_Should_in_msg_return_invalid_arg_00003()
        {
            var result = _proteinNodeGenerator.GenerateFromProteinSequence(null, -1);
            Assert.IsFalse(result.Successed);
            Assert.IsTrue(result.ErrorMessage.Equals("Invalid argument!"));
        }

        [Test]
        public void GenerateFromProteinSequence_Should_in_msg_return_invalid_arg_00004()
        {
            var result = _proteinNodeGenerator.GenerateFromProteinSequence("", 0);
            Assert.IsFalse(result.Successed);
            Assert.IsTrue(result.ErrorMessage.Equals("Invalid argument!"));
        }

        [Test]
        public void GenerateProteinNodeCombination_arg_have_to_be_correct_00013()
        {
            var result = _proteinNodeGenerator.GenerateProteinNodeCombination(null);
            Assert.IsTrue(result != null);
            Assert.IsTrue(!result.Successed);
            Assert.IsTrue(result.ErrorMessage.Equals("Null arg!"));
        }

        [Test]
        public void GenerateProteinNodeCombination_arg_have_to_be_correct_00014()
        {
            var result = _proteinNodeGenerator.GenerateProteinNodeCombination(null, null);
            Assert.IsTrue(result != null);
            Assert.IsTrue(!result.Successed);
            Assert.IsTrue(result.ErrorMessage.Equals("Null arg!"));
        }

        [Test]
        public void GenerateProteinNodeCombination_have_to_return_correct_answer_00013()
        {
            //arg:
            // V, Q, IVYK
            // VQ, I, VYK
            // VQI, V, YK
            //(a1, a2, a3) i(b1, b2, b3) to badamy słowa
            //a1b2a3, a1b2b3, a1a2b3, b1a2a3, b1a2b3, b1b2a3

            var result = _proteinNodeGenerator.GenerateProteinNodeCombination(new List<ProteinNode>
            {
                new ProteinNode("V,Q,IVYK"),
                new ProteinNode("VQ,I,VYK"),
                new ProteinNode("VQI,V,YK")
            });
            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Successed);
            Assert.IsTrue(result.Data != null);
            Assert.IsTrue(result.Data.Count == 18);
            Assert.IsTrue(result.Data[0].Value.Equals("V, I, IVYK"));
            Assert.IsTrue(result.Data[1].Value.Equals("V, I, VYK"));
            Assert.IsTrue(result.Data[2].Value.Equals("V, Q, VYK"));
            Assert.IsTrue(result.Data[3].Value.Equals("VQ, Q, IVYK"));
            Assert.IsTrue(result.Data[4].Value.Equals("VQ, Q, VYK"));
            Assert.IsTrue(result.Data[5].Value.Equals("VQ, I, IVYK"));
            Assert.IsTrue(result.Data[12].Value.Equals("VQ, V, VYK"));
            Assert.IsTrue(result.Data[13].Value.Equals("VQ, V, YK"));
            Assert.IsTrue(result.Data[14].Value.Equals("VQ, I, YK"));
            Assert.IsTrue(result.Data[15].Value.Equals("VQI, I, VYK"));
            Assert.IsTrue(result.Data[16].Value.Equals("VQI, I, YK"));
            Assert.IsTrue(result.Data[17].Value.Equals("VQI, V, VYK"));
        }

        [Test]
        public void GenerateProteinNodeCombination_have_to_return_correct_answer_00015()
        {
            //arg:
            // V, Q, IVYK
            // VQ, I, VYK
            // VQI, V, YK
            //(a1, a2, a3) i(b1, b2, b3) to badamy słowa
            //a1b2a3, a1b2b3, a1a2b3, b1a2a3, b1a2b3, b1b2a3

            var result = _proteinNodeGenerator.GenerateProteinNodeCombination(new ProteinNode("V,Q,IVYK"),
                new ProteinNode("VQ,I,VYK"));
            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Successed);
            Assert.IsTrue(result.Data != null);
            Assert.IsTrue(result.Data.Count == 6);
            Assert.IsTrue(result.Data[0].Value.Equals("V, I, IVYK"));
            Assert.IsTrue(result.Data[1].Value.Equals("V, I, VYK"));
            Assert.IsTrue(result.Data[2].Value.Equals("V, Q, VYK"));
            Assert.IsTrue(result.Data[3].Value.Equals("VQ, Q, IVYK"));
            Assert.IsTrue(result.Data[4].Value.Equals("VQ, Q, VYK"));
            Assert.IsTrue(result.Data[5].Value.Equals("VQ, I, IVYK"));
        }

        [Test]
        public void GenerateProteinNodeCombination_Shuld_Never_return_null_00012()
        {
            var result = _proteinNodeGenerator.GenerateProteinNodeCombination(null);
            Assert.IsTrue(result != null);
        }
    }
}