using System;
using System.Collections.Generic;
using System.Text;

using net.zemberek.yapi;
using log4net;


namespace net.zemberek.bilgi.kokler
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
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private KokDugumu baslangicDugumu = null;
        private int nodeCount = 0;
        private Alfabe alfabe;

        public KokAgaci(KokDugumu baslangicDugumu, Alfabe alfabe)
        {
            this.baslangicDugumu = baslangicDugumu;
            this.alfabe = alfabe;
        }

        public KokDugumu getKokDugumu()
        {
            return baslangicDugumu;
        }

        public Alfabe getAlfabe()
        {
            return alfabe;
        }

        public int getNodeCount()
        {
            return nodeCount;
        }

        /**
         * Verilen k�k icerigini a�aca ekler.
         *
         * @param icerik
         * @param kok
         */
        public void ekle(String icerik, Kok kok)
        {
            //System.out.println("Kelime: " + icerik);
            char[] hd = icerik.ToCharArray();
            KokDugumu node = baslangicDugumu;
            KokDugumu oncekiDugum = null;
            int idx = 0;
            // null alt d���m bulana dek veya kelimenin sonuna dek alt d���mlerde ilerle
            while (idx < hd.Length)
            {
                oncekiDugum = node;
                node = node.altDugumGetir(hd[idx]);
                if (node == null) break;
                idx++;
            }
            /**
             * A�a� �zerinde ilerlerken kelimemizin sonuna kadar gitmi�iz.
             * kelimemizi bu d���me ekleriz.
             * �rne�in
             * i-s-t-->istim �eklindeki dala "is" kelimesini eklemek gibi.
             * i-s-t-->istim
             *   |-->is
             *
             * veya
             *
             * i-s-->istim e is gelirse de
             * i-s-t-->istim
             *   |-->is
             *
             * i-s-->is  e "is" gelirse
             * i-s-->is(2) olmal�.
             *
             */
            if (idx == hd.Length)
            {
                if (node.altDugumVarMi())
                {
                    node.kokEkle(kok);
                    node.setKelime((IEnumerable<char>)icerik);
                }
                // E� sesli!
                else if (node.getKelime().Equals(icerik))
                {
                    node.kokEkle(kok);
                    return;
                }
                else if (node.getKok() != null)
                {
                    //TODO : Burada charenumerable'dan son chari olduk�a kazma bir y�ntemle al�yoruz. Bunu incelemek laz�m.
                    KokDugumu aNewNode = node.addNode(new KokDugumu(node.getKelime().ToString().ToCharArray()[idx]));
                    aNewNode.kopyala(node);
                    node.temizle();
                    node.kokEkle(kok);
                    node.setKelime(icerik);
                }
                return;

                
            }

            /**
             * Kald���m�z d���me ba�l� bir k�k yoksa bu k�k i�in bir d���m olu�turup ekleriz.
             */
            if (oncekiDugum.getKok() == null && idx < hd.Length)
            {
                oncekiDugum.addNode(new KokDugumu(hd[idx], icerik, kok));
                return;
            }

            if (oncekiDugum.getKelime().Equals(icerik))
            {
                oncekiDugum.kokEkle(kok);
                return;
            }

            /**
             * D���mde duran "istimlak" ve gelen k�k = "istimbot" i�in,
             * i-s-t-i-m
             * e kadar ilerler. daha sonra "istimlak" i�in "l" d���m�n� olu�turup k�k� ba�lar
             * i-s-t-i-m-l-->istimlak
             * sonra da di�er d���m i�in "b" d���m�n� olu�turup gene "m" d���m�ne ba�lar
             * i-s-t-i-m-l-->istimlak
             *         |-b-->istimbot
             *
             * E�er istimlak d���m� ba�lanm��sa ve "istim" d���m� eklenmek �zere 
             * elimize gelmi�e
             * i-s-t-i-m-l-->istimlak
             * tan sonra istim, "m" d���m�ne do�rudan ba�lan�r.
             * i-s-t-i-m-l-->istimlak
             *         |-->istim
             *
             */
            char[] nodeHd = ((String)oncekiDugum.getKelime()).ToCharArray();
            //char[] nodeChars = ((String) oncekiDugum.getKelime()).toCharArray();
            KokDugumu newNode = oncekiDugum;

            if (idx == nodeHd.Length)
            {
                newNode.addNode(new KokDugumu(hd[idx], icerik, kok));
                return;
            }

            //TODO : Ayn� kazmal�k, kelimenin boyunu al�rken, hepsi CharSequence y�z�nden
            if (oncekiDugum.getKelime().ToString().Length == idx)
            {
                newNode.addNode(new KokDugumu(hd[idx], icerik, kok));
                return;
            }

            if (nodeHd.Length <= hd.Length)
            {
                while (idx < nodeHd.Length && nodeHd[idx] == hd[idx])
                {
                    newNode = newNode.addNode(new KokDugumu(nodeHd[idx]));
                    idx++;
                }

                // Kisa dugumun eklenmesi.
                if (idx < nodeHd.Length)
                {
                    KokDugumu temp = newNode.addNode(new KokDugumu(nodeHd[idx]));
                    temp.kopyala(oncekiDugum);
                }
                else
                {
                    newNode.kopyala(oncekiDugum);
                }

                // Uzun olan dugumun (yeni gelen) eklenmesi, es anlamlilari kotar
                newNode.addNode(new KokDugumu(hd[idx], icerik, kok));
                oncekiDugum.temizle();
                return;
            }

            /**
             *
             * E�er k�ke �nce "istimlak" ve sonra "istifa" gelirse
             * i-s-t-i-m-l-->istimlak
             * daha sonra gene son ortak harf olan "i" ye "f" karakterli d���m�
             * olu�turup istifay� ba�lar
             * istimlak ta "m" d���m�ne ba�l� kal�r.
             * i-s-t-i-m-->istimlak
             *       |-f-->istifa
             *
             */

            else
            {
                while (idx < hd.Length && hd[idx] == nodeHd[idx])
                {
                    newNode = newNode.addNode(new KokDugumu(hd[idx]));
                    idx++;
                }
                // Kisa dugumun eklenmesi.
                if (idx < hd.Length)
                {
                    newNode.addNode(new KokDugumu(hd[idx], icerik, kok));
                }
                else
                {
                    newNode.kokEkle(kok);
                    newNode.setKelime(icerik);
                }

                // Uzun olan dugumun (yeni gelen) eklenmesi.
                newNode = newNode.addNode(new KokDugumu(nodeHd[idx]));
                newNode.kopyala(oncekiDugum);
                // Es seslileri tasi.
                oncekiDugum.temizle();
                return;
            }
        }

        /**
         * Aranan bir k�k d���m�n� bulur.
         *
         * @param str
         * @return Aranan k�k ve e� seslilerini ta��yan liste, bulunamazsa null.
         */
        public List<Kok> bul(String str)
        {
            char[] girisChars = str.ToCharArray();
            int girisIndex = 0;
            // Basit bir tree traverse algoritmas� ile kelime bulunur.
            KokDugumu node = baslangicDugumu;
            while (node != null && girisIndex < girisChars.Length)
            {
                if (node.getKelime() != null && node.getKelime().Equals(str))
                {
                    break;
                }
                if (logger.IsInfoEnabled)
                    logger.Info("Harf: " + node.getHarf() + " Taranan Kelime: " + node.getKelime());
                node = node.altDugumGetir(girisChars[girisIndex++]);
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

    }

}



