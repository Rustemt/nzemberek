﻿using System;
using System.Collections.Generic;
using System.Text;

namespace net.zemberek.araclar
{
    public class MetinAraclari
    {
    //     private static Logger log = Logger.getLogger("MetinAraclari.class");
    //private static JaroWinkler jaroWinkler = new JaroWinkler();

    ///**
    // * Verilen iki karakterin arasÄ±ndakli klavye mesafesini dÃ¶ndÃ¼rÃ¼r (Q). 
    // * @param c1
    // * @param c2
    // * @return tuslar arasi mesafe
    // */
    //public static int klavyeMesafesi(char c1, char c2){
    //    return QTusTakimi.mesafeHesapla(c1, c2);
    //}

    ///**
    // * Degistirilmis Levenshtein Edit Dist. algoritmasÄ±. transpozisyonlarÄ± da 1 dÃ¼zeltme mesafesi
    // * olarak hesaplar.
    // *
    // * @param source
    // * @param target
    // * @return iki kelime arasindaki mesafe, tamsayi cinsinden. kucuk rakamlar daha buyuk benzerligi gosterir.
    // */

    //public static int editDistance(String source, String target) {
    //    final int maxDif = Math.max(source.length() , target.length());
    //    return editDistance(source, target, maxDif);
    //}

    ///**
    // * Degistirilmis Levenshtein Edit Dist. algoritmasÄ±. transpozisyonlarÄ± da 1 dÃ¼zeltme mesafesi
    // * olarak hesaplar. Uzaklik limit degerden buyuk olursa islem kesilir. (Alphan)
    // *
    // * @param source
    // * @param target
    // * @param limit
    // * @return iki kelime arasindaki mesafe, tamsayi cinsinden. kucuk rakamlar daha buyuk benzerligi gosterir.
    // */
    //public static int editDistance(String source, String target, int limit) {
    //    // Step 1
    //    int n = source.length();
    //    int m = target.length();
    //    final int disarda = limit + 1;

    //    if (n == 0 || m == 0)
    //        return Math.max(m, n);

    //    if (Math.abs(m - n) > limit)
    //        return disarda;

    //    int matrix[][] = new int[n + 1][m + 1];

    //    // Step 2
    //    int ust;
    //    ust = Math.min(limit, n);
    //    for (int i = 0; i <= ust; i++)
    //        matrix[i][0] = i;
    //    ust = Math.min(limit, m);
    //    for (int j = 1; j <= ust; j++)
    //        matrix[0][j] = j;

    //    // Step 3

    //    for (int i = 1; i <= n; i++) {
    //        char s_i = source.charAt(i - 1);
    //        // Step 4
    //        boolean devamet = false;
    //        if (i - limit >= 1) {
    //            //		matrix[i][i-limit]=matrix[i-1][i-limit]+1;
    //            matrix[i][i - limit] = Math.min(matrix[i - 1][i - limit] + 1,
    //                    matrix[i - 1][i - limit - 1] +
    //                    ((source.charAt(i - 1) == target.charAt(i - limit - 1)) ? 0 : 1));
    //            devamet |= matrix[i][i - limit] <= limit;
    //        }

    //        int alt = Math.max(i - limit + 1, 1);

    //        ust = Math.min(i + limit - 1, m);

    //        for (int j = alt; j <= ust; ++j) {
    //            char t_j = target.charAt(j - 1);
    //            // Step 5
    //            int cost;
    //            if (s_i == t_j) {
    //                cost = 0;
    //            } else {
    //                cost = 1;
    //            }

    //            // Step 6
    //            int above = matrix[i - 1][j];
    //            int left = matrix[i][j - 1];
    //            int diag = matrix[i - 1][j - 1];
    //            int cell = Math.min(above + 1, Math.min(left + 1, diag + cost));

    //            // Step 6A: Cover transposition, in addition to deletion,
    //            // insertion and substitution. This step is taken from:
    //            // Berghel, Hal ; Roach, David : "An Extension of Ukkonen's
    //            // Enhanced Dynamic Programming ASM Algorithm"
    //            // (http://www.acm.org/~hlb/publications/asm/asm.html)
    //            /* bu kismi simdilik, geciyoruz */
    //            if (i > 1 && j > 1) {
    //                int trans = matrix[i - 2][j - 2] + 1;
    //                if (source.charAt(i - 2) != t_j) trans++;
    //                if (s_i != target.charAt(j - 2)) trans++;
    //                if (cell > trans) cell = trans;
    //            }
    //            /*		*/
    //            matrix[i][j] = cell;
    //            devamet |= cell <= limit;
    //            //		if (cell>limit) break;
    //        }
    //        if (i + limit <= m) {
    //            matrix[i][i + limit] = Math.min(matrix[i][i + limit - 1] + 1,
    //                    matrix[i - 1][i + limit - 1] +
    //                    ((source.charAt(i - 1) == target.charAt(i + limit - 1)) ? 0 : 1));
    //            devamet |= matrix[i][i + limit] <= limit;
    //        }
    //        if (!devamet) return disarda;
    //    }

    //    // Step 7

    //    return matrix[n][m];
    //}

    ///**
    // * Degistirilmis Levenshtein Edit Dist. algoritmasÄ±. transpozisyonlarÄ± da 1
    // * dÃ¼zeltme mesafesi olarak hesaplar.
    // *
    // * @param source
    // * @param target
    // * @return eger istenilen mesafede is true.
    // */
    //public static boolean inEditDistance(final String source, final String target, final int dist) {
    //    return (editDistance(source, target, dist) <= dist);
    //}


    ///**
    // * Verilen s1 stringinin verilen distance dÃ¼zeltme mesafesi Ã§erÃ§evesinde
    // * s2 stringinin alt stringi olup olmadÄ±ÄŸÄ±nÄ± dÃ¶ndÃ¼rÃ¼rr. Ã–rneÄŸin:
    // * <pre>
    // * isInSubStringLevenshteinDistance("elma","ekmalar",1) -> true
    // * isInSubStringLevenshteinDistance("elma","emalar",1) -> true
    // * isInSubStringLevenshteinDistance("elma","eksalar",1) -> false (substring min dist=2)
    // * </pre>
    // *
    // * @param s1       :
    // * @param s2       : s1'i distance dÃ¼zeltme mesafesi iÃ§inde kapsayÄ±p kapsamadÄ±ÄŸÄ± araÅŸtÄ±rÄ±lan String
    // * @param distance : dÃ¼zeltme mesafesi
    // * @return eger istenilen mesafede is true.
    // */
    public static bool isInSubstringEditDistance(String s1, String s2, int distance) {
    //    if (s2.length() < (s1.length() - distance) ||
    //            s1.length() > (s2.length() + distance))
    //        return false;

    //    if (s2.length() >= s1.length()) {
    //        String test = s2.substring(0, s1.length());
    //        if (inEditDistance(s1, test, distance)) return true;
    //        test = s2.substring(0, s1.length() - 1);
    //        if (inEditDistance(s1, test, distance)) return true;
    //        if (s2.length() >= s1.length() + 1) {
    //            test = s2.substring(0, s1.length() + 1);
    //            if (inEditDistance(s1, test, distance)) return true;
    //        }
    //    } else if (s1.length() >= s2.length()) {
    //        if (inEditDistance(s1, s2, distance)) return true;
    //    }
        return false;
    }

    ///**
    // * s1 ile s2'nin benzerlik oranini hesaplar.
    // *
    // * @param s1
    // * @param s2
    // * @return 0-1.0 arasi bir deger. Buyuk rakamlar kelimelerin daha benzer oldugunu gosterir.
    // */
    //public static double sozcukBenzerlikOrani(String s1, String s2) {
    //    return jaroWinkler.benzerlikOrani(s1, s2);
    //}

    ///**
    // * s1 ile s2'nin enazBenzerlik degeri kadar ya da daha benzer olup olmadigini test eder.
    // *
    // * @param s1
    // * @param s2
    // * @param enazBenzerlik
    // * @return eger benzerlik orani enazBenzerlik'na es ya da buyukse true
    // */
    //public static boolean sozcukBenzerlikTesti(String s1, String s2, double enazBenzerlik) {
    //    return (jaroWinkler.benzerlikOrani(s1, s2) >= enazBenzerlik);
    //}
    }
}
