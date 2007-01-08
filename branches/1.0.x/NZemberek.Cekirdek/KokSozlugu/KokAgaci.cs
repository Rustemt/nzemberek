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

//V 0.1

using System;
using System.Collections.Generic;
using System.Text;

using NZemberek.Cekirdek.Yapi;
using log4net;


namespace NZemberek.Cekirdek.KokSozlugu
{
    /*
    * Created on 28.Eyl.2004
    * 
    * K�k a�ac� zemberek sisteminin temel veri ta��y�c�lar�ndan biridir. K�k 
    * s�zl���nden okunan t�m k�kler bu a�aca yerle�tirilirler. A�ac�n olu�umundan 
    * AgacSozluk s�n�f� sorumludur.
    * K�k a�ac� kompakt DAWG (Directed Acyclic Word Graph) ve Patricia tree benzeri
    * bir yap�ya sahiptir. A�aca eklenen her k�k harflerine g�re bir a�a� olu�turacak
    * �ekilde yerle�tirilir. Bir k�k� bulmak i�in a�ac�n ba��ndan itibaren k�k� 
    * olu�turan harfleri temsil eden d���mleri izlemek yeterlidir. 
    * E�er bir k�k� ararken eri�mek istedi�imiz harfe ait bir alt d���me
    * gidemiyorsak k�k a�a�ta yok demektir.
    * <p/>
    * A�ac�n bir �zelli�i de bo�una d���m olu�turmamas�d�r. E�er bir k�k�n alt�nda
    * ba�ka bir k�k olmayacaksa t�m harfleri i�in ayr� ayr� de�il, sadece gerekti�i
    * kadar d���m olu�turulur.
    * <p/>
    * Kod i�erisinde hangi durumda nas�l d���m olu�turuldu�u detaylar�yla 
    * anlat�lm��t�r.
    *
    * @author MDA
    */
    public class KokAgaci
    {
        private KokDugumu baslangicDugumu = null;
        private int nodeCount = 0;

        public int NodeCount
        {
            get { return nodeCount; }
            set { nodeCount = value; }
        }
        private Alfabe alfabe;

        public Alfabe Alfabe
        {
            get { return alfabe; }
            set { alfabe = value; }
        }

        public KokAgaci(KokDugumu baslangicDugumu, Alfabe alfabe)
        {
            this.baslangicDugumu = baslangicDugumu;
            this.alfabe = alfabe;
        }

        public KokDugumu BaslangicDugumu()
        {
            return baslangicDugumu;
        }

        private KokDugumu IcerikDugumuBul(string icerik)
        {
            KokDugumu parent = baslangicDugumu;
            KokDugumu child = null;
            while (parent.Level < icerik.Length)
            {
                child = parent.altDugumGetir(icerik[parent.Level]);
                if (child != null)
                {
                    parent = child;
                }
                else
                {
                    break;
                }
            }
            return parent;
        }


        public void Ekle(string icerik, Kok kok)
        {
            // ��erik i�in Mevcut a�a�ta inilebilecek en derin d���m� Bul.
            KokDugumu dugum = this.IcerikDugumuBul(icerik);
            while (true)
            {
                // A�a� �zerinde ilerlerken kelimemizin sonuna kadar gitmi�sek
                if (dugum.Level == icerik.Length)
                {
                    //kelimemizi bu d���me ekleriz.
                    //�rnek : i�erik="istif ISIM", d���m = f(level:5).
                    SonHarfDugumuneEkle(icerik, kok, dugum);
                    return;
                }
                // Kelimenin sonuna kadar gitmedik...
                else
                {
                    // bu d���me ba�l� bir k�k yoksa 
                    if (dugum.getKok() == null)
                    {
                        //bu k�k i�in bir d���m olu�turup ekleriz.
                        dugum.DugumEkle(icerik, kok);
                        return;
                    }
                    // bu d���me ba�l� bir k�k var...
                    else
                    {
                        // bu d���m�n i�eri�i eldeki i�erik ile ayn� ise o zaman bir e�sesli bulduk.
                        if (dugum.Kelime.Equals(icerik))
                        {
                            // e�sesliyi ekleriz. (E�sesli eklemede sadece k�k ekleniyor, i�erik de�i�tirilmiyor.
                            //�rnek : i�erik="y�z SAYI", d���m = �(level:2,k�k="y�z FIIL").
                            dugum.kokEkle(kok);
                            return;
                        }
                        // e�sesli de�il...
                        else
                        {
                            //d���m bir k�sayol de�ilse 
                            if (!dugum.KisayolDugumu)
                            {
                                //yeni i�eri�i alt d���m olarak ekleriz.
                                //�rnek : i�erik="istifra ISIM", d���m = f(level:5,k�k="istif ISIM").
                                dugum.DugumEkle(icerik, kok);
                                return;
                            }
                            //k�sayol d���m�
                            else
                            {
                                //�ncelikle d���mdeki k�k� bir ilerletiriz.
                                KokDugumu yenidugum = dugum.KokuDallandir();
                                if (yenidugum.Harf == icerik[dugum.Level])
                                {
                                    dugum = yenidugum;
                                    continue;
                                }
                                else
                                {
                                    dugum.DugumEkle(icerik, kok);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        

        /// <summary>
        /// Verilen k�k� ve i�eri�i d���mleri olu�turarak a�aca ekler.
        /// </summary>
        /// <param name="icerik"></param>
        /// <param name="kok"></param>
        //public void Ekle(String icerik, Kok kok)
        //{
        //    KokDugumu node = baslangicDugumu;
        //    KokDugumu oncekiDugum = null;
        //    int level = 0;
        //    // null alt d���m bulana dek veya kelimenin sonuna dek alt d���mlerde ilerle
        //    while (level < icerik.Boy)
        //    {
        //        oncekiDugum = node;
        //        node = node.altDugumGetir(icerik[level]);
        //        if (node == null) break;
        //        level++;
        //    }
        //    /**
        //     * A�a� �zerinde ilerlerken kelimemizin sonuna kadar gitmi�iz.
        //     * kelimemizi bu d���me ekleriz.
        //     */
        //    if (level == icerik.Boy)
        //    {
        //        SonHarfDugumuneEkle(icerik, kok, node);
        //        return;
        //    }


        //    /**
        //     * Kald���m�z d���me ba�l� bir k�k yoksa bu k�k i�in bir d���m olu�turup ekleriz.
        //     */
        //    if (oncekiDugum.getKok() == null && level < icerik.Boy)
        //    {
        //        oncekiDugum.DugumEkle(icerik, kok);
        //        return;
        //    }

        //    if (oncekiDugum.Kelime.Equals(icerik))
        //    {
        //        oncekiDugum.kokEkle(kok);
        //        return;
        //    }

        //    /**
        //     * D���mde duran "istimlak" ve gelen k�k = "istimbot" i�in,
        //     * i-s-t-i-m
        //     * e kadar ilerler. daha sonra "istimlak" i�in "l" d���m�n� olu�turup k�k� ba�lar
        //     * i-s-t-i-m-l-->istimlak
        //     * sonra da di�er d���m i�in "b" d���m�n� olu�turup gene "m" d���m�ne ba�lar
        //     * i-s-t-i-m-l-->istimlak
        //     *         |-b-->istimbot
        //     *
        //     * E�er istimlak d���m� ba�lanm��sa ve "istim" d���m� eklenmek �zere 
        //     * elimize gelmi�e
        //     * i-s-t-i-m-l-->istimlak
        //     * tan sonra istim, "m" d���m�ne do�rudan ba�lan�r.
        //     * i-s-t-i-m-l-->istimlak
        //     *         |-->istim
        //     *
        //     */
        //    string oncekiDugumIcerigi = oncekiDugum.Kelime;
        //    KokDugumu newNode = oncekiDugum;

        //    if (level == oncekiDugumIcerigi.Boy)
        //    {
        //        newNode.DugumEkle(icerik, kok);
        //        return;
        //    }

        //    if (oncekiDugumIcerigi.Boy <= icerik.Boy)
        //    {
        //        while (level < oncekiDugumIcerigi.Boy && oncekiDugumIcerigi[level] == icerik[level])
        //        {
        //            newNode = newNode.DugumEkle(oncekiDugumIcerigi[level]);
        //            level++;
        //        }

        //        // Kisa dugumun eklenmesi.
        //        if (level < oncekiDugumIcerigi.Boy)
        //        {
        //            //newNode.KokuDallandir();
        //            KokDugumu temp = newNode.DugumEkle(oncekiDugumIcerigi[level]);
        //            temp.kopyala(oncekiDugum);
        //        }
        //        else
        //        {
        //            newNode.kopyala(oncekiDugum);
        //        }

        //        // Uzun olan dugumun (yeni gelen) eklenmesi, es anlamlilari kotar
        //        newNode.DugumEkle(icerik, kok);
        //        oncekiDugum.Temizle();
        //    }

        //    /**
        //     *
        //     * E�er k�ke �nce "istimlak" ve sonra "istifa" gelirse
        //     * i-s-t-i-m-l-->istimlak
        //     * daha sonra gene son ortak Harf olan "i" ye "f" karakterli d���m�
        //     * olu�turup istifay� ba�lar
        //     * istimlak ta "m" d���m�ne ba�l� kal�r.
        //     * i-s-t-i-m-->istimlak
        //     *       |-f-->istifa
        //     *
        //     */

        //    else
        //    {
        //        while (level < icerik.Boy && icerik[level] == oncekiDugumIcerigi[level])
        //        {
        //            newNode = newNode.DugumEkle(icerik[level]);
        //            level++;
        //        }
        //        // Kisa dugumun eklenmesi.
        //        if (level < icerik.Boy)
        //        {
        //            newNode.DugumEkle(icerik, kok);
        //        }
        //        else
        //        {
        //            newNode.kokEkle(kok);
        //            newNode.Kelime = icerik;
        //        }

        //        // Uzun olan dugumun (yeni gelen) eklenmesi.
        //        newNode = newNode.DugumEkle(oncekiDugumIcerigi[level]);
        //        newNode.kopyala(oncekiDugum);
        //        // Es seslileri tasi.
        //        oncekiDugum.Temizle();
        //    }
        //}


        /**
         * Aranan bir k�k d���m�n� bulur.
         *
         * @param str
         * @return Aranan k�k ve e� seslilerini ta��yan liste, bulunamazsa null.
         */


        public List<Kok> Bul(String str)
        {
            int girisIndex = 0;
            // Basit bir tree traverse algoritmas� ile kelime bulunur.
            KokDugumu node = baslangicDugumu;
            while (node != null && girisIndex < str.Length)
            {
                if (node.Kelime != null && node.Kelime.Equals(str))
                {
                    break;
                }
                node = node.altDugumGetir(str[girisIndex++]);
            }
            if (node != null)
            {
                return node.tumKokleriGetir();
            }
            return null;
        }

        public override String ToString()
        {
            return baslangicDugumu.ToString();
        }


        /// <summary>
        /// A�a� �zerinde ilerlerken kelimemizin sonuna kadar gitmi�iz.
        /// Kelimemizi bu d���me ekleriz.
        /// �rne�in
        /// i-s-t-->istim �eklindeki dala "is" kelimesini eklemek gibi.
        /// i-s-t-->istim
        ///   |-->is
        /// 
        /// veya
        /// 
        /// i-s-->istim e is gelirse de
        /// i-s-t-->istim
        ///   |-->is
        /// 
        /// i-s-->is  e "is" gelirse
        /// i-s-->is(2) olmal�.
        /// </summary>
        /// <param name="icerik"></param>
        /// <param name="kok"></param>
        /// <param name="node"></param>
        private static void SonHarfDugumuneEkle(String icerik, Kok kok, KokDugumu node)
        {
            if (node.altDugumVarMi())
            {
                node.kokEkle(kok);
                node.Kelime = icerik;
            }
            // E� sesli!
            else if (node.Kelime.Equals(icerik))
            {
                node.kokEkle(kok);
            }
            else if (node.getKok() != null)
            {
                node.KokuDallandir();
                node.kokEkle(kok);
                node.Kelime = icerik;
            }
        }
    }

}



