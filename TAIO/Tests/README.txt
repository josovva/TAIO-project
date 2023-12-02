Opis poszczególnych przykładów

========================   Clique   ========================

1. Dwie rozłączne wierzchołkowo kliki o 6 wierzchołkach, połączone ze sobą łukami, każda z nich o różnej sumarycznej liczbie łuków.
2. Dwie rozłączne wierzchołkowo kliki, połączone ze sobą łukami, o różnych liczbach wierzchołków.
3. Trzy kliki o 6 wierzchołkach mające niektóre krawędzie wspólne.
4. Graf powstały z połączenia kolejnych wierzchołków dwóch ścieżek równej długości.
5. Graf będący prawie kliką Q_8_1 (brakuje kilka krawędzi).
6. Graf o 16 wierzchołkach, w którym są różne kliki dla różnych porowatości (ang. porosity).
7. Graf złożony z 8 izolowanych wierzchołków.
8. Losowy graf o 10 wierzchołkach zawierający Q_6_3 klikę.
9. Losowy graf o 17 wierzchołkach zawierający Q_7_3 klikę.
10. Losowy graf o 25 wierzchołkach zawierający Q_20_8 klikę.


======================== Metric/MCS ========================

1. Obydwa multigrafy takie same.
2. Multigrafy G i H izomorficzne.
3. Multigraf H będący multigrafem G z dodanymi 3 odosobnionymi wierzchołkami. Wynik metryki równy różnicy liczby wierzchołków w multigrafach.
4. Multigraf G będący podgrafem H. Wynik metryki równy kosztowi mapowania drugiego wierzchołka H na odosobniony wierzchołek.
5. Klika Q_5_1 i Klika Q_3_9. MCS dopasowuje taki podgraf, który maksymalizuje grubość, bo maksymalizacja liczby wierzchołków jest trywialna.
6. Multigrafy G i H będące dużymi klikami o małej grubości, mającymi jako podgrafy małe kliki o dużej grubości. Algorytm faworyzuje maksymalizację wierzchołków, a dopiero potem grubości.
7. Multigraf G złożony z dwóch rozłącznych klik, jedna Q_4_1, druga Q_4_3 (ale bez krawędzi między jedną parą wierzchołków).
   Multigraf H będący Q_4_3 kliką. Algorytm poprawnie mapuje na Q_4_1 klikę, rozpoznając brakującą krawędź.
8. Dwa drzewa binarne pełne, jedno z krawędziami domyślnie skierowanymi od korzenia w dół, drugie z zamienionymi kierunkami.
9. Multigraf G jest acykliczny, H jest cyklem C_5_5. Algorytm znajduje ścieżkę o 3 wierzchołkach.
10. G to klika Q_5_5, H to multigraf złożony z 5 izolowanych wierzchołków. Wynikiem powinien być pusty multigraf.