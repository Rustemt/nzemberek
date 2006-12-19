/* ***** BEGIN LICENSE BLOCK *****
 * Version: MPL 1.1
 *
 * The contents of this file are subject to the Mozilla Public License Version
 * 1.1 (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * The Original Code is Zemberek Do�al Dil ��leme K�t�phanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Ak�n, Mehmet D. Ak�n.
 * Portions created by the Initial Developer are Copyright (C) 2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Mert Derman
 *   Tankut Tekeli
 * ***** END LICENSE BLOCK ***** */

using System;
using System.Collections.Generic;
using System.Text;

using net.zemberek.erisim;
using net.zemberek.tr.yapi;
using net.zemberek.yapi;
using NUnit.Framework;


namespace net.zemberek.tests.kullanim
{
    [TestFixture]
    public class TestAscii
    {
        //TEST VERILERI
        private static string[] AsciiyeDonusturGirdiler = new string[] {"�ebek", "�a��rtm��", "d���ms�zl�km��"};
        private static string[] AsciiyeDonusturBeklenenler = new string[] {"sebek", "sasirtmis", "dugumsuzlukmus"};


        private static Zemberek zemberek;

        [SetUp]
        public void Setup()
        {
            zemberek = new Zemberek(new TurkiyeTurkcesi());
        }

        //TODO : Bu model ile test yazmak daha kolay ama raporlamas� k�t�, bununla ilgili karar vermek laz�m.
        [Test]
        public void testAsciiYap()
        {
            int i=0;
            int gecentest = 0;
            foreach (string girdi in AsciiyeDonusturGirdiler)
            {
                string actual = zemberek.asciiyeDonustur(girdi);
                if (AsciiyeDonusturBeklenenler[i++] == actual)
                    gecentest++;
            }
            Assert.AreEqual(AsciiyeDonusturBeklenenler.Length, gecentest);
        }

        [Test]
        public void testAsciiYap1()
        {
            string actual = zemberek.asciiyeDonustur("�ebek");
            string expected = "sebek";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void testAsciiYap2()
        {
            string actual = zemberek.asciiyeDonustur("�a��rtm��");
            string expected = "sasirtmis";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void testAsciiYap3()
        {
            string actual = zemberek.asciiyeDonustur("d���ms�zl�km��");
            string expected = "dugumsuzlukmus";
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void testAsciiCozumle1()
        {
            string[] actual = zemberek.asciidenTurkceye("dugumsuzlukmus");
            string expected = "d���ms�zl�km��";
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(expected, actual[0]);
        }

        [Test]
        public void testAsciiCozumle2()
        {
            string[] actual = zemberek.asciidenTurkceye("sasirtmis");
            string expected = "�a��rtm��";
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(expected, actual[0]);
        }

        [Test]
        public void testAsciiCozumle3()
        {
            string[] actual = zemberek.asciidenTurkceye("sebek");
            string expected = "�ebek";
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(expected, actual[0]);
        }

        [Test]
        public void testBelirsizAsciiCozumle1()
        {
            string[] actual = zemberek.asciidenTurkceye("siraci");
            string expected1 = "s�rac�";
            string expected2 = "��rac�";
            Assert.AreEqual(2, actual.Length);
            Assert.Contains(expected1, actual);
            Assert.Contains(expected2, actual);
        }

        [Test]
        public void testBelirsizAsciiCozumle2()
        {
            string[] actual = zemberek.asciidenTurkceye("olmus");
            string expected1 = "olmu�";
            string expected2 = "�lm��";
            Assert.AreEqual(2, actual.Length);
            Assert.Contains(expected1, actual);
            Assert.Contains(expected2, actual);
        }
    }
}






