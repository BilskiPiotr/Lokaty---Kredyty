using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Lokaty_Kredyty
{
    public partial class Kredyty : Form
    {
        public Kredyty()
        {
            InitializeComponent();
        }

        // sprawdzenie i wczytanie do zmiennych danych wejściowych
        bool PobierzDaneWejściowe(out float WartośćKredytu, out float OprocentowanieKredytu, out int LiczbaLatKredytu, out int LiczbaRatWRoku)
        {
            // ustawienie domyślnych wartości dla danych wejściowych do porównania z errorProvider1
            WartośćKredytu = -1.0F;
            OprocentowanieKredytu = 0.0F;
            LiczbaLatKredytu = 0;
            LiczbaRatWRoku = 0;


            // sprawdzenie czy użytkownik wprowadził kwotę kredytu
            if (string.IsNullOrEmpty(KwotaKredytu.Text))
            {
                // klient nie wpisał kwoty - zgłoszenie błędu 
                errorProvider1.SetError(KwotaKredytu,
                    "ERROR: musisz podać kwotę kredytu");
                return false;
            }
            errorProvider1.Dispose();

            // wczytanie wysokości Kredytu ze sprawdzeniem poprawności zapisu 
            if (!float.TryParse(KwotaKredytu.Text, out WartośćKredytu))
            {
                // do wprowadzenia kwoty użyto jakims cudem niedozwolonego znaku - zgłoszenie błędu
                errorProvider1.SetError(KwotaKredytu,
                    "ERROR: w zapisie kwoty użyto niedozwolonego znaku");
                return false;
                // w innym wypadku wczytanie kwoty do zmiennej WartośćKredytu
            }
            errorProvider1.Dispose();

            // sprawdzenie, czy klient wybrał stopę procentową
            if (StopaProcentowa.SelectedIndex < 0)
            {
                // jesli nie wybrał - zgłoszenie błędu
                errorProvider1.SetError(StopaProcentowa,
                    "ERROR: musisz wybrać stopę procentową!");
                return false;
            }
            errorProvider1.Dispose();

            // pobranie wartości stopy procentowej i wprowadzenie jej do zmiennej OprocentowanieKredytu
            if (!float.TryParse(StopaProcentowa.SelectedItem.ToString(), out OprocentowanieKredytu))
            {
                // jesli jakiimś cudem w strumieniu danych pojawi sie niedozwolony znak - zgłoszenie błędu
                errorProvider1.SetError(StopaProcentowa,
                    "ERROR: niedozwolony znak w zapisie stopy procentowej!");
                return false;
                // w innym wypadku wczytanie oprocentowanie do zmiennej OprocentowanieKredytu
            }
            errorProvider1.Dispose();

            // sprawdzenie, czy wpisano cokolwiek do kontrolki określającej okres kredytowania
            if (string.IsNullOrEmpty(OkresSpłaty.Text))
            {
                // jeśli nic nie wpisano - zapalenie kontrolki błędu 
                errorProvider1.SetError(OkresSpłaty,
                    "ERROR: musisz podać czas kredytowania w latach");
                return false;
            }
            errorProvider1.Dispose();

            // wczytanie liczby lat kredytowania ze sprawdzeniem poprawności zapisu 
            if (!int.TryParse(OkresSpłaty.Text, out LiczbaLatKredytu))
            {
                // jesli jakimś cudem użyto niepoprawnego znaku - zgłoszenie błedu
                errorProvider1.SetError(OkresSpłaty,
                    "ERROR: wystąpił niedozwolony znak w zapisie liczby lat kredytowania");
                return false;
                // w innymwypadku wczytanie liczby lat kredytowania do zmiennej OkresKredytu
            }
            errorProvider1.Dispose();

            // sprawdzenie ile razy rocznie będzie spłacana rata kredytu
            if (CoMiesiąc.Checked)
            {
                LiczbaRatWRoku = 12;
            }
            else
                if (CoKwartał.Checked)
                {
                    LiczbaRatWRoku = 4;
                }
                else
                    if (CoPółRoku.Checked)
                    {
                        LiczbaRatWRoku = 2;
                    }
                    else
                        if (RazNaRok.Checked)
                        {
                            LiczbaRatWRoku = 1;
                        }
            return true;
        }

        // obsługa przycisku przejscia do formularza LOKATY
        private void PrzejdźDoLokaty_Click(object sender, EventArgs e)
        {
            // ukrycie formularza Lokaty
            this.Hide();
            // odszukaniew kolekcji aktywnych formularzy formularza Lokaty
            foreach (Form Formularz in Application.OpenForms)
            {
                if (Formularz.Name == "Lokaty")
                {
                    // odsłonięcie formularza Lokaty
                    Formularz.Show();
                    // zakończenie obsługi zdarzenia click przycisku
                    return;
                }
            }
            // utworzenie egzemplarza formularza Lokaty
            Lokaty EgzLokaty = new Lokaty();
            // wyświetlenie pormularza Lokaty
            EgzLokaty.Show();
        }

        // zakończenie programu
        private void KoniecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        // Obsługa Przycisku poleceń TABELARYCZNE ROZLICZENIE SPŁATY KREDYTU
        private void TabelarycznaPrezentacjaRat_Click(object sender, EventArgs e)
        {

            // wyczyszczenie możliwej zawartości z kontrolki DataGridView
            while (Tabela.Rows.Count > 0)
            {
                Tabela.Rows.RemoveAt(0);
            }

            // odsłonięcie ukrytych kontrolek
            Tabela.Visible = true;
            WRInfo.Visible = true;
            InfoWysokośćRatyKapitałowej.Visible = true;



            // pobranie danych wejściowych z formularza KREDYTY
            if (!PobierzDaneWejściowe(out float wartośćKredytu, out float oprocentowanieKredytu, out int liczbaLatKredytu, out int liczbaRatWRoku))
                //zostałą zapalona kontrolka errorProvider, wiedz koniec obsługi zdażenia
                return;

            // obsługa tabelaryczna dla rat malejących
            if (Malejące.Checked)
            {

                // deklaracje lokalne
                float Zadłużnie;
                float RataOdsetkowa;
                float RataKapitałowa;
                float RataŁączna;
                float KosztKredytu;

                // deklaracja tablicy rozliczenia spłaty kredytu
                float[,] RozliczenieKredyu = new float[liczbaLatKredytu * liczbaRatWRoku + 1, 3];

                // przypisanie damych do tablicy dla terminu "zero"
                RozliczenieKredyu[0, 0] = 0.0F;                 // pb łączna
                RozliczenieKredyu[0, 1] = 0.0F;                 // rata odsetkowa
                RozliczenieKredyu[0, 2] = wartośćKredytu;    // zadłużenie

                // ustalenie wartości zmiennych dla terminu "zero"
                Zadłużnie = wartośćKredytu;
                RataKapitałowa = wartośćKredytu / (liczbaLatKredytu * liczbaRatWRoku);
                RataOdsetkowa = 0.0F;
                RataŁączna = 0.0F;
                KosztKredytu = 0.0F;

                // inicjalizacja zmiennej tablicowej
                for (int i = 0; i < RozliczenieKredyu.GetLength(0); i++)
                {
                    // wpisanie pierwszego stanu do tablicy rozliczenia kredytu
                    RozliczenieKredyu[i, 0] = RataŁączna;     // rata łączna 
                    RozliczenieKredyu[i, 1] = RataOdsetkowa;  // rata odsetkowa 
                    RozliczenieKredyu[i, 2] = Zadłużnie;      // zadłużenie

                    // wyznaczenie wartości zmiennych dla kolejneg okresu spłaty kredytu
                    RataOdsetkowa = Zadłużnie * oprocentowanieKredytu / liczbaRatWRoku; // rata odsetkowa
                    RataŁączna = RataKapitałowa + RataOdsetkowa;                           // raty łącznej
                    Zadłużnie = Zadłużnie - RataKapitałowa;                                // zadłużenia po wpłaceniu kolejnej raty kapitałowej
                    KosztKredytu += RataOdsetkowa;                                            // sumowanie kosztu korzystania z kredytu
                }

                // wyprowadzenie do kontrolku formularza informacji o całkowitym koszcie kredytu
                KosztyKredytu.Text = string.Format("{0,8:F1}", KosztKredytu);

                // wypełnienie kontrolki DataGridView wynikami obliczeń dla kolejnych okresów
                // inicjalizacja zmiennej tablicowej
                for (int i = 0; i < RozliczenieKredyu.GetLength(0); i++)
                {
                    Tabela.Rows.Add();                               // dodanie nowego wiersza do kontrolki DataGridView
                    Tabela.Rows[i].Cells[0].Value = i;                                                    // numer raty
                    Tabela.Rows[i].Cells[1].Value = string.Format("{0,8:F2}", RozliczenieKredyu[i, 0]);   // rata łączna
                    Tabela.Rows[i].Cells[2].Value = string.Format("{0,8:F2}", RozliczenieKredyu[i, 1]);   // rata odsetkowa
                    Tabela.Rows[i].Cells[3].Value = string.Format("{0,8:f2}", RozliczenieKredyu[i, 2]);   // zadłużenie
                }

                // wyprowadzenie do formularza informacji o wysokości raty kapitałowej
                InfoWysokośćRatyKapitałowej.Text = string.Format("{0,8:F2}", RataKapitałowa);

                // wyprowadzenie do formularza informacji o saldzie kredytu do spłacenia
                KońcoweZadłużenie.Text = string.Format("{0,8:F1}", RozliczenieKredyu[liczbaLatKredytu * liczbaRatWRoku, 2]);

                // przejście do zakładki rozliczenia tabelarycznego
                this.tabControl1.SelectedTab = ZakładkaTabelaryczneRozliczenieKredytu;
            }
            else

                // obsługa tabelaryczna dla rat rosnących
                if (Rosnące.Checked)
                {

                    // deklaracje lokalne
                    float Zadłużnie;
                    float RataOdsetkowa;
                    float RataKapitałowa;
                    float RataŁączna;
                    float KosztKredytu;

                    // deklaracja tablicy rozliczenia spłaty kredytu
                    float[,] RozliczenieKredyu = new float[liczbaLatKredytu * liczbaRatWRoku + 1, 3];

                    // przypisanie damych do tablicy dla terminu "zero"
                    RozliczenieKredyu[0, 0] = 0.0F;                 // pb łączna
                    RozliczenieKredyu[0, 1] = 0.0F;                 // rata odsetkowa
                    RozliczenieKredyu[0, 2] = wartośćKredytu;    // zadłużenie

                    // ustalenie wartości zmiennych dla terminu "zero"
                    Zadłużnie = wartośćKredytu;
                    RataKapitałowa = wartośćKredytu / (liczbaLatKredytu * liczbaRatWRoku);
                    RataOdsetkowa = 0.0F;
                    RataŁączna = 0.0F;
                    KosztKredytu = 0.0F;

                    // inicjalizacja zmiennej tablicowej
                    for (int i = 0; i < RozliczenieKredyu.GetLength(0); i++)
                    {
                        // wpisanie pierwszego stanu do tablicy rozliczenia kredytu
                        RozliczenieKredyu[i, 0] = RataŁączna;     // rata łączna 
                        RozliczenieKredyu[i, 1] = RataOdsetkowa;  // rata odsetkowa 
                        RozliczenieKredyu[i, 2] = Zadłużnie;      // zadłużenie

                        // wyznaczenie wartości zmiennych dla kolejneg okresu spłaty kredytu
                        RataOdsetkowa = (oprocentowanieKredytu / liczbaRatWRoku) * ((i + 1) * RataKapitałowa);   // rata odsetkowa
                        RataŁączna = RataKapitałowa + RataOdsetkowa;                                                   // raty łącznej
                        Zadłużnie = Zadłużnie - RataKapitałowa;                                                        // zadłużenia po wpłaceniu kolejnej raty kapitałowej
                        KosztKredytu += RataOdsetkowa;                                                                    // sumowanie kosztu korzystania z kredytu
                    }

                    // wyprowadzenie do kontrolku formularza informacji o całkowitym koszcie kredytu
                    KosztyKredytu.Text = string.Format("{0,8:F1}", (KosztKredytu - RataOdsetkowa));

                    // wypełnienie kontrolki DataGridView wynikami obliczeń dla kolejnych okresów
                    // inicjalizacja zmiennej tablicowej
                    for (int i = 0; i < RozliczenieKredyu.GetLength(0); i++)
                    {
                        Tabela.Rows.Add();                                   // dodanie nowego wiersza do kontrolki DataGridView
                        Tabela.Rows[i].Cells[0].Value = i;                                                    // numer raty
                        Tabela.Rows[i].Cells[1].Value = string.Format("{0,8:F2}", RozliczenieKredyu[i, 0]);   // rata łączna
                        Tabela.Rows[i].Cells[2].Value = string.Format("{0,8:F2}", RozliczenieKredyu[i, 1]);   // rata odsetkowa
                        Tabela.Rows[i].Cells[3].Value = string.Format("{0,8:f2}", RozliczenieKredyu[i, 2]);   // zadłużenie
                    }

                    // wyprowadzenie do formularza informacji o wysokości raty kapitałowej
                    InfoWysokośćRatyKapitałowej.Text = string.Format("{0,8:F2}", RataKapitałowa);

                    // wyprowadzenie do formularza informacji o saldzie kredytu do spłacenia
                    KońcoweZadłużenie.Text = string.Format("{0,8:F1}", RozliczenieKredyu[liczbaLatKredytu * liczbaRatWRoku, 2]);

                    // przejście do zakładki rozliczenia tabelarycznego
                    this.tabControl1.SelectedTab = ZakładkaTabelaryczneRozliczenieKredytu;
                }
                else

                    // obsługa tabelaryczna dla rat stałych
                    if (Stałe.Checked)
                    {

                        // deklaracje lokalne
                        float Zadłużnie;
                        float RataOdsetkowa;
                        float RataKapitałowa;
                        float RataŁączna;
                        float KosztKredytu;
                        float Nawias;

                        // deklaracja tablicy rozliczenia spłaty kredytu
                        float[,] RozliczenieKredyu = new float[liczbaLatKredytu * liczbaRatWRoku + 1, 3];

                        // przypisanie damych do tablicy dla terminu "zero"
                        RozliczenieKredyu[0, 0] = 0.0F;                 // pb łączna
                        RozliczenieKredyu[0, 1] = 0.0F;                 // rata odsetkowa
                        RozliczenieKredyu[0, 2] = wartośćKredytu;    // zadłużenie

                        // ustalenie wartości zmiennych dla terminu "zero"
                        Zadłużnie = wartośćKredytu;
                        RataOdsetkowa = 0.0F;
                        Nawias = (float)Math.Pow((1 + (oprocentowanieKredytu / liczbaRatWRoku)), (liczbaLatKredytu * liczbaRatWRoku));
                        RataŁączna = wartośćKredytu * (((oprocentowanieKredytu / liczbaRatWRoku) * Nawias) / (Nawias - 1));
                        KosztKredytu = 0.0F;
                        RataKapitałowa = 0.0F;


                        // inicjalizacja zmiennej tablicowej
                        for (int i = 0; i < RozliczenieKredyu.GetLength(0); i++)
                        {
                            // wpisanie pierwszego stanu do tablicy rozliczenia kredytu
                            RozliczenieKredyu[i, 0] = RataŁączna;     // rata łączna 
                            RozliczenieKredyu[i, 1] = RataOdsetkowa;  // rata odsetkowa 
                            RozliczenieKredyu[i, 2] = Zadłużnie;      // zadłużenie

                            // wyznaczenie wartości zmiennych dla kolejneg okresu spłaty kredytu
                            RataOdsetkowa = Zadłużnie * (oprocentowanieKredytu / liczbaRatWRoku);    // rata odsetkowa
                            RataKapitałowa = RataŁączna - RataOdsetkowa;  // rata kapitałowa                             
                            Zadłużnie = Zadłużnie - RataKapitałowa;                                    // zadłużenia po wpłaceniu kolejnej raty kapitałowej
                            KosztKredytu += RataOdsetkowa;                                                // sumowanie kosztu korzystania z kredytu
                        }

                        // wyprowadzenie do kontrolku formularza informacji o całkowitym koszcie kredytu
                        KosztyKredytu.Text = string.Format("{0,8:F1}", (KosztKredytu));

                        // wypełnienie kontrolki DataGridView wynikami obliczeń dla kolejnych okresów
                        // inicjalizacja zmiennej tablicowej
                        for (int i = 0; i < RozliczenieKredyu.GetLength(0); i++)
                        {
                            Tabela.Rows.Add();                               // dodanie nowego wiersza do kontrolki DataGridView
                            Tabela.Rows[i].Cells[0].Value = i;                                                    // numer raty
                            Tabela.Rows[i].Cells[1].Value = string.Format("{0,8:F2}", RozliczenieKredyu[i, 0]);   // rata łączna
                            Tabela.Rows[i].Cells[2].Value = string.Format("{0,8:F2}", RozliczenieKredyu[i, 1]);   // rata odsetkowa
                            Tabela.Rows[i].Cells[3].Value = string.Format("{0,8:f2}", RozliczenieKredyu[i, 2]);   // zadłużenie
                        }

                        // wyprowadzenie do formularza informacji o wysokości raty kapitałowej
                        InfoWysokośćRatyKapitałowej.Text = string.Format("{0,8:F2}", RataKapitałowa);

                        // wyprowadzenie do formularza informacji o saldzie kredytu do spłacenia
                        KońcoweZadłużenie.Text = string.Format("{0,8:F1}", RozliczenieKredyu[liczbaLatKredytu * liczbaRatWRoku, 2]);

                        // przejście do zakładki rozliczenia tabelarycznego
                        this.tabControl1.SelectedTab = ZakładkaTabelaryczneRozliczenieKredytu;
                    }
        }

        // obsługa przycisku poleceń PREZENTACJA GRAFICZNA KREDYTU
        private void GraficznaPrezentacjaRat_Click(object sender, EventArgs e)
        {
            // zresetowanie wszystkich elementów kontrolki Chart
            Wykres.ResetAutoValues();
            Wykres.Titles.Clear();
            Wykres.Series.Clear();

            // odsłonięcie kontrolki Chart
            Wykres.Visible = true;
            FormatowanieWykresu.Visible = true;

            // pobranie danych wejściowych z formularza KREDYTY
            if (!PobierzDaneWejściowe(out float wartośćKredytu, out float oprocentowanieKredytu, out int liczbaLatKredytu, out int liczbaRatWRoku))
                //zostałą zapalona kontrolka errorProvider, wiedz koniec obsługi zdażenia
                return;

            // odsłonięcie pola GroupBox umożliwiającego manipulowanie liniami wykresu
            WybórLiniiWykresu.Visible = true;

            // obsługa wykresu dla rat malejących
            if (Malejące.Checked)
            {
                // deklaracja zmiennych lokalnych
                float Zadłużenie;
                float RataOdsetkowa;
                float RataKapitałowa;
                float RataŁączna;
                float KosztKredytu;

                // deklaracja i utworzenie tablicy rozliczenia kredytu
                float[,] RozliczenieKredytu = new float[liczbaLatKredytu * liczbaRatWRoku + 1, 3];

                // przypisanie stanu początkowego zmiennym
                Zadłużenie = wartośćKredytu;
                RataKapitałowa = wartośćKredytu / (liczbaLatKredytu * liczbaRatWRoku);
                RataOdsetkowa = 0.0F;
                RataŁączna = 0.0F;
                KosztKredytu = 0.0F;

                // inicjalizacja zmiennej tablicowej
                for (int i = 0; i < RozliczenieKredytu.GetLength(0); i++)
                {
                    // wyznaczenie stanu "zero" dla zmiennych i przypisanie ich do kolumn
                    RozliczenieKredytu[i, 0] = RataŁączna;     // rata łączna
                    RozliczenieKredytu[i, 1] = RataOdsetkowa;  // rata odsetkowa
                    RozliczenieKredytu[i, 2] = Zadłużenie;     // pozostałe zadłużenie

                    // dopisanie do tablicy kolejnego stanu zmiennych
                    RataOdsetkowa = Zadłużenie * (oprocentowanieKredytu / liczbaRatWRoku);  // rata odsetkowa
                    RataŁączna = RataKapitałowa + RataOdsetkowa;                               // rata łączna
                    Zadłużenie = Zadłużenie - RataKapitałowa;                                  // zadłużenie Z po wpłaceniu raty R
                    KosztKredytu += RataOdsetkowa;                                                // sumowanie kosztu kożystania z kredytu
                }

                // wyprowadzenie do formularza Kredyty informacji o koszcie obsługi tego kredytu
                KosztyKredytu.Text = string.Format("{0,8:F1}", KosztKredytu);

                // wyprowadzenie do formularza Kredyty informacji o pozostałej kwocie do spłaty
                KońcoweZadłużenie.Text = string.Format("{0,8:F1}", RozliczenieKredytu[liczbaLatKredytu * liczbaRatWRoku, 2]);

                // formatowanie parametrów ogólnych wykresu
                Wykres.Titles.Add("Spłata kredytu w ratach malejących");         // tytuł wykresu
                Wykres.Location = new Point(30, 40);                             // polożenie górnego lewego narożnika kontrolki Chart
                Wykres.Width = (int)(this.Width * 0.7F);                         // szerokosć wykresu
                Wykres.Height = (int)(this.Height * 0.7F);                       // wyskośc wykresu
                Wykres.BackColor = Color.Bisque;                                 // kolor tła
                Wykres.Legends.FindByName("Legend1").Docking = Docking.Bottom;   // zdefiniowanie położenia legendy

                // formatowanie parametrów dla pierwszej serii danych - Series[0] "Rata Łączna"

                Wykres.Series.Add("Seria 0");

                Wykres.Series[0].Name = "Rata łączna";                   // nazwa serii danych Series[0]
                Wykres.Series[0].ChartType = SeriesChartType.Line;       // rodzaj wykresu - liniowy
                Wykres.Series[0].Color = Color.Blue;                     // kolor linii - niebieski
                Wykres.Series[0].BorderDashStyle = ChartDashStyle.Dash;  // styl linii - kreskowy
                Wykres.Series[0].BorderWidth = 2;                        // grubość linii

                // deklaracja tablic pomocniczych opisujących położenie punktów na wykresie

                int[] NumeryRatKredytu = new int[RozliczenieKredytu.GetLength(0) - 1];  // tablica 1
                for (int j = 0; j < RozliczenieKredytu.GetLength(0) - 1; j++)  // zmienna tablicowa
                    NumeryRatKredytu[j] = j + 1;                                  // zaladowanie danych

                float[] PunktyWykresu = new float[RozliczenieKredytu.GetLength(0) - 1]; // tablica 2
                for (int k = 0; k < RozliczenieKredytu.GetLength(0) - 1; k++)  // zmienna tablicowa
                    PunktyWykresu[k] = RozliczenieKredytu[k + 1, 0];              // załadowanie danych

                // powiązanie tablic z wektorami punktów z kontrolką Chart
                Wykres.Series[0].Points.DataBindXY(NumeryRatKredytu, PunktyWykresu);



                // dodanie do kontrolki Chart 2-giej serii danych
                Wykres.Series.Add("Seria 1");

                // formatowanie parametrów dla 2-giej serii danych - Series[1] "Rata Odsetkowa"
                Wykres.Series[1].Name = "Rata odsetkowa";                // nazwa serii danych Series[1]
                Wykres.Series[1].ChartType = SeriesChartType.Line;       // rodzaj wykresu - liniowy
                Wykres.Series[1].Color = Color.Green;                    // kolor linii - zielony
                Wykres.Series[1].BorderDashStyle = ChartDashStyle.Dot;   // styl linii - kropkowy
                Wykres.Series[1].BorderWidth = 2;                        // grubość linii

                // wprowadzenie danych opisujących położenie punktów linii wykresu dla - Series[1]
                for (int l = 0; l < RozliczenieKredytu.GetLength(0) - 1; l++)
                    PunktyWykresu[l] = RozliczenieKredytu[l + 1, 1]; ;

                // powiązanie tablic z wektorami punktów z kontrolką Chart
                Wykres.Series[1].Points.DataBindXY(NumeryRatKredytu, PunktyWykresu);



                // dodanie do kontrolki Chart 3-ciej serii danych
                Wykres.Series.Add("Seria 2");

                // formatowanie parametrów dla 2-giej serii danych - Series[2] "Rata Kapitałowa"
                Wykres.Series[2].Name = "Rata kapitałowa";               // nazwa serii danych Series[2]
                Wykres.Series[2].ChartType = SeriesChartType.Line;       // rodzaj wykresu - liniowy
                Wykres.Series[2].Color = Color.Red;                      // kolor linii - czerwony
                Wykres.Series[2].BorderDashStyle = ChartDashStyle.Solid; // styl linii - ciągły
                Wykres.Series[2].BorderWidth = 2;                        // grubość linii

                // wprowadzenie danych opisujących położenie punktów linii wykresu dla - Series[2]
                for (int m = 0; m < RozliczenieKredytu.GetLength(0) - 1; m++)
                    PunktyWykresu[m] = RataKapitałowa;

                // powiązanie tablic z wektorami punktów z kontrolką Chart
                Wykres.Series[2].Points.DataBindXY(NumeryRatKredytu, PunktyWykresu);

                // wyłączenie przycisku graficznej wizualizacni do momentu zresetowania formularza Kredyty
                // GraficznaPrezentacjaRat.Enabled = false;

                // przejście do zakładki z wykresem
                this.tabControl1.SelectedTab = ZakładkaGraficznaPrezentacjaKredytu;
            }


            else
                // obsługa wykresu dla rat rosnących
                if (Rosnące.Checked)
                {
                    // deklaracja zmiennych lokalnych
                    float Zadłużenie;
                    float RataOdsetkowa;
                    float RataKapitałowa;
                    float RataŁączna;
                    float KosztKredytu;

                    // deklaracja i utworzenie tablicy rozliczenia kredytu
                    float[,] RozliczenieKredytu = new float[liczbaLatKredytu * liczbaRatWRoku + 1, 3];

                    // przypisanie stanu początkowego zmiennym
                    Zadłużenie = wartośćKredytu;
                    RataKapitałowa = wartośćKredytu / (liczbaLatKredytu * liczbaRatWRoku);
                    RataOdsetkowa = 0.0F;
                    RataŁączna = 0.0F;
                    KosztKredytu = 0.0F;

                    // inicjalizacja zmiennej tablicowej
                    for (int i = 0; i < RozliczenieKredytu.GetLength(0); i++)
                    {
                        // wyznaczenie stanu "zero" dla zmiennych i przypisanie ich do kolumn
                        RozliczenieKredytu[i, 0] = RataŁączna;     // rata łączna
                        RozliczenieKredytu[i, 1] = RataOdsetkowa;  // rata odsetkowa
                        RozliczenieKredytu[i, 2] = Zadłużenie;     // pozostałe zadłużenie

                        // dopisanie do tablicy kolejnego stanu zmiennych
                        RataOdsetkowa = (oprocentowanieKredytu / liczbaRatWRoku) * ((i + 1) * RataKapitałowa);// rata odsetkowa
                        RataŁączna = RataKapitałowa + RataOdsetkowa;                                               // rata łączna
                        Zadłużenie = Zadłużenie - RataKapitałowa;                                                  // zadłużenie Z po wpłaceniu raty R
                        KosztKredytu += RataOdsetkowa;                                                                // sumowanie kosztu kożystania z kredytu
                    }

                    // wyprowadzenie do formularza Kredyty informacji o koszcie obsługi tego kredytu
                    KosztyKredytu.Text = string.Format("{0,8:F1}", KosztKredytu);

                    // wyprowadzenie do formularza Kredyty informacji o pozostałej kwocie do spłaty
                    KońcoweZadłużenie.Text = string.Format("{0,8:F1}", RozliczenieKredytu[liczbaLatKredytu * liczbaRatWRoku, 2]);

                    // formatowanie parametrów ogólnych wykresu
                    Wykres.Titles.Add("Spłata kredytu w ratach malejących");         // tytuł wykresu
                    Wykres.Location = new Point(30, 40);                             // polożenie górnego lewego narożnika kontrolki Chart
                    Wykres.Width = (int)(this.Width * 0.7F);                         // szerokosć wykresu
                    Wykres.Height = (int)(this.Height * 0.7F);                       // wyskośc wykresu
                    Wykres.BackColor = Color.Bisque;                                 // kolor tła
                    Wykres.Legends.FindByName("Legend1").Docking = Docking.Bottom;   // zdefiniowanie położenia legendy

                    // formatowanie parametrów dla pierwszej serii danych - Series[0] "Rata Łączna"

                    Wykres.Series.Add("Seria 0");

                    Wykres.Series[0].Name = "Rata łączna";                   // nazwa serii danych Series[0]
                    Wykres.Series[0].ChartType = SeriesChartType.Line;       // rodzaj wykresu - liniowy
                    Wykres.Series[0].Color = Color.Blue;                     // kolor linii - niebieski
                    Wykres.Series[0].BorderDashStyle = ChartDashStyle.Dash;  // styl linii - kreskowy
                    Wykres.Series[0].BorderWidth = 2;                        // grubość linii

                    // deklaracja tablic pomocniczych opisujących położenie punktów na wykresie

                    int[] NumeryRatKredytu = new int[RozliczenieKredytu.GetLength(0) - 1];  // tablica 1
                    for (int j = 0; j < RozliczenieKredytu.GetLength(0) - 1; j++)  // zmienna tablicowa
                        NumeryRatKredytu[j] = j + 1;                                  // zaladowanie danych

                    float[] PunktyWykresu = new float[RozliczenieKredytu.GetLength(0) - 1]; // tablica 2
                    for (int k = 0; k < RozliczenieKredytu.GetLength(0) - 1; k++)  // zmienna tablicowa
                        PunktyWykresu[k] = RozliczenieKredytu[k + 1, 0];              // załadowanie danych

                    // powiązanie tablic z wektorami punktów z kontrolką Chart
                    Wykres.Series[0].Points.DataBindXY(NumeryRatKredytu, PunktyWykresu);



                    // dodanie do kontrolki Chart 2-giej serii danych
                    Wykres.Series.Add("Seria 1");

                    // formatowanie parametrów dla 2-giej serii danych - Series[1] "Rata Odsetkowa"
                    Wykres.Series[1].Name = "Rata odsetkowa";                // nazwa serii danych Series[1]
                    Wykres.Series[1].ChartType = SeriesChartType.Line;       // rodzaj wykresu - liniowy
                    Wykres.Series[1].Color = Color.Green;                    // kolor linii - zielony
                    Wykres.Series[1].BorderDashStyle = ChartDashStyle.Dot;   // styl linii - kropkowy
                    Wykres.Series[1].BorderWidth = 2;                        // grubość linii

                    // wprowadzenie danych opisujących położenie punktów linii wykresu dla - Series[1]
                    for (int l = 0; l < RozliczenieKredytu.GetLength(0) - 1; l++)
                        PunktyWykresu[l] = RozliczenieKredytu[l + 1, 1]; ;

                    // powiązanie tablic z wektorami punktów z kontrolką Chart
                    Wykres.Series[1].Points.DataBindXY(NumeryRatKredytu, PunktyWykresu);



                    // dodanie do kontrolki Chart 3-ciej serii danych
                    Wykres.Series.Add("Seria 2");

                    // formatowanie parametrów dla 2-giej serii danych - Series[2] "Rata Kapitałowa"
                    Wykres.Series[2].Name = "Rata kapitałowa";               // nazwa serii danych Series[2]
                    Wykres.Series[2].ChartType = SeriesChartType.Line;       // rodzaj wykresu - liniowy
                    Wykres.Series[2].Color = Color.Red;                      // kolor linii - czerwony
                    Wykres.Series[2].BorderDashStyle = ChartDashStyle.Solid; // styl linii - ciągły
                    Wykres.Series[2].BorderWidth = 2;                        // grubość linii

                    // wprowadzenie danych opisujących położenie punktów linii wykresu dla - Series[2]
                    for (int m = 0; m < RozliczenieKredytu.GetLength(0) - 1; m++)
                        PunktyWykresu[m] = RataKapitałowa;

                    // powiązanie tablic z wektorami punktów z kontrolką Chart
                    Wykres.Series[2].Points.DataBindXY(NumeryRatKredytu, PunktyWykresu);

                    // wyłączenie przycisku graficznej wizualizacni do momentu zresetowania formularza Kredyty
                    // GraficznaPrezentacjaRat.Enabled = false;

                    // przejście do zakładki z wykresem
                    this.tabControl1.SelectedTab = ZakładkaGraficznaPrezentacjaKredytu;
                }
                else


                    // obsługa wykresu dla rat stałych
                    if (Stałe.Checked)
                    {

                        // deklaracja zmiennych lokalnych
                        float Zadłużenie;
                        float RataOdsetkowa;
                        float RataKapitałowa;
                        float RataŁączna;
                        float KosztKredytu;
                        float Nawias;

                        // deklaracja i utworzenie tablicy rozliczenia kredytu
                        float[,] RozliczenieKredytu = new float[liczbaLatKredytu * liczbaRatWRoku + 1, 3];

                        // przypisanie stanu początkowego zmiennym
                        Zadłużenie = wartośćKredytu;
                        RataOdsetkowa = 0.0F;
                        Nawias = (float)Math.Pow((1 + (oprocentowanieKredytu / liczbaRatWRoku)), (liczbaLatKredytu * liczbaRatWRoku));
                        RataŁączna = wartośćKredytu * (((oprocentowanieKredytu / liczbaRatWRoku) * Nawias) / (Nawias - 1));
                        KosztKredytu = 0.0F;
                        RataKapitałowa = 0.0F;

                        // inicjalizacja zmiennej tablicowej
                        for (int i = 0; i < RozliczenieKredytu.GetLength(0); i++)
                        {
                            // wyznaczenie stanu "zero" dla zmiennych i przypisanie ich do kolumn
                            RozliczenieKredytu[i, 0] = RataŁączna;     // rata łączna
                            RozliczenieKredytu[i, 1] = RataOdsetkowa;  // rata odsetkowa
                            RozliczenieKredytu[i, 2] = Zadłużenie;     // pozostałe zadłużenie

                            // dopisanie do tablicy kolejnego stanu zmiennych
                            RataOdsetkowa = Zadłużenie * (oprocentowanieKredytu / liczbaRatWRoku);  // rata odsetkowa
                            RataKapitałowa = RataŁączna - RataOdsetkowa;                               // rata kapitałowa
                            Zadłużenie = Zadłużenie - RataKapitałowa;                                  // zadłużenie Z po wpłaceniu raty R
                            KosztKredytu += RataOdsetkowa;                                                // sumowanie kosztu kożystania z kredytu
                        }

                        // wyprowadzenie do formularza Kredyty informacji o koszcie obsługi tego kredytu
                        KosztyKredytu.Text = string.Format("{0,8:F1}", KosztKredytu);

                        // wyprowadzenie do formularza Kredyty informacji o pozostałej kwocie do spłaty
                        KońcoweZadłużenie.Text = string.Format("{0,8:F1}", RozliczenieKredytu[liczbaLatKredytu * liczbaRatWRoku, 2]);

                        // formatowanie parametrów ogólnych wykresu
                        Wykres.Titles.Add("Spłata kredytu w ratach malejących");         // tytuł wykresu
                        Wykres.Location = new Point(30, 40);                             // polożenie górnego lewego narożnika kontrolki Chart
                        Wykres.Width = (int)(this.Width * 0.7F);                         // szerokosć wykresu
                        Wykres.Height = (int)(this.Height * 0.7F);                       // wyskośc wykresu
                        Wykres.BackColor = Color.Bisque;                                 // kolor tła
                        Wykres.Legends.FindByName("Legend1").Docking = Docking.Bottom;   // zdefiniowanie położenia legendy

                        // formatowanie parametrów dla pierwszej serii danych - Series[0] "Rata Łączna"

                        Wykres.Series.Add("Seria 0");

                        Wykres.Series[0].Name = "Rata łączna";                   // nazwa serii danych Series[0]
                        Wykres.Series[0].ChartType = SeriesChartType.Line;       // rodzaj wykresu - liniowy
                        Wykres.Series[0].Color = Color.Blue;                     // kolor linii - niebieski
                        Wykres.Series[0].BorderDashStyle = ChartDashStyle.Dash;  // styl linii - kreskowy
                        Wykres.Series[0].BorderWidth = 1;                        // grubość linii

                        // deklaracja tablic pomocniczych opisujących położenie punktów na wykresie

                        int[] NumeryRatKredytu = new int[RozliczenieKredytu.GetLength(0) - 1];  // tablica 1
                        for (int j = 0; j < RozliczenieKredytu.GetLength(0) - 1; j++)  // zmienna tablicowa
                            NumeryRatKredytu[j] = j + 1;                                  // zaladowanie danych

                        float[] PunktyWykresu = new float[RozliczenieKredytu.GetLength(0) - 1]; // tablica 2
                        for (int k = 0; k < RozliczenieKredytu.GetLength(0) - 1; k++)  // zmienna tablicowa
                            PunktyWykresu[k] = RozliczenieKredytu[k + 1, 0];              // załadowanie danych

                        // powiązanie tablic z wektorami punktów z kontrolką Chart
                        Wykres.Series[0].Points.DataBindXY(NumeryRatKredytu, PunktyWykresu);



                        // dodanie do kontrolki Chart 2-giej serii danych
                        Wykres.Series.Add("Seria 1");

                        // formatowanie parametrów dla 2-giej serii danych - Series[1] "Rata Odsetkowa"
                        Wykres.Series[1].Name = "Rata odsetkowa";                // nazwa serii danych Series[1]
                        Wykres.Series[1].ChartType = SeriesChartType.Line;       // rodzaj wykresu - liniowy
                        Wykres.Series[1].Color = Color.Green;                    // kolor linii - zielony
                        Wykres.Series[1].BorderDashStyle = ChartDashStyle.Dot;   // styl linii - kropkowy
                        Wykres.Series[1].BorderWidth = 1;                        // grubość linii

                        // wprowadzenie danych opisujących położenie punktów linii wykresu dla - Series[1]
                        for (int l = 0; l < RozliczenieKredytu.GetLength(0) - 1; l++)
                            PunktyWykresu[l] = RozliczenieKredytu[l + 1, 1]; ;

                        // powiązanie tablic z wektorami punktów z kontrolką Chart
                        Wykres.Series[1].Points.DataBindXY(NumeryRatKredytu, PunktyWykresu);



                        // dodanie do kontrolki Chart 3-ciej serii danych
                        Wykres.Series.Add("Seria 2");

                        // formatowanie parametrów dla 2-giej serii danych - Series[2] "Rata Kapitałowa"
                        Wykres.Series[2].Name = "Rata kapitałowa";               // nazwa serii danych Series[2]
                        Wykres.Series[2].ChartType = SeriesChartType.Line;       // rodzaj wykresu - liniowy
                        Wykres.Series[2].Color = Color.Red;                      // kolor linii - czerwony
                        Wykres.Series[2].BorderDashStyle = ChartDashStyle.Solid; // styl linii - ciągły
                        Wykres.Series[2].BorderWidth = 1;                        // grubość linii

                        // wprowadzenie danych opisujących położenie punktów linii wykresu dla - Series[2]
                        for (int m = 0; m < RozliczenieKredytu.GetLength(0) - 1; m++)
                            PunktyWykresu[m] = RozliczenieKredytu[m + 1, 0] - RozliczenieKredytu[m + 1, 1];

                        // powiązanie tablic z wektorami punktów z kontrolką Chart
                        Wykres.Series[2].Points.DataBindXY(NumeryRatKredytu, PunktyWykresu);

                        // wyłączenie przycisku graficznej wizualizacni do momentu zresetowania formularza Kredyty
                        // GraficznaPrezentacjaRat.Enabled = false;



                        // przejście do zakładki z wykresem
                        this.tabControl1.SelectedTab = ZakładkaGraficznaPrezentacjaKredytu;
                    }
        }

        // ograniczenie możliwości wprowadzania danych jedynie do cyfr i znaku (,) 
        // dla kontrolki pobierającej KwotęKredytu
        private void KwotaKredytu_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8;
        }

        // ograniczenie możliwości wprowadzania danych jedynie do cyfr i znaku (,) 
        // dla kontrolki pobierającej IlośćLatKredytowania
        private void OkresSpłaty_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar);
        }

        // obsługa przycisku "krzyżyk" - zamknięcie formularza / programu
        private void Kredyty_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Czy Napewno chcesz opuścić Program",
                               "Zakończyć Program?",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes)
                    Environment.Exit(1);
                else
                    e.Cancel = true; // program nie zostanie zamknięty, użytkownik się rozmyślił
            }

        }

        // Zmiana koloru tła wykresu z poziomu Menu
        private void ZmianaKoloruTłaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog WybranyKolorTła = new ColorDialog();
            if (WybranyKolorTła.ShowDialog() == DialogResult.OK)
            {
                Wykres.BackColor = WybranyKolorTła.Color;
            }
        }

        // Zmiana koloru linii dla danych Series[0]
        private void Linia1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog WybranyKolorLinii = new ColorDialog();
            if (WybranyKolorLinii.ShowDialog() == DialogResult.OK)
            {
                Wykres.Series[0].Color = WybranyKolorLinii.Color;
            }
        }

        // zmiana koloru linii dla danych Series[1]
        private void Linia2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog WybranyKolorLinii = new ColorDialog();
            if (WybranyKolorLinii.ShowDialog() == DialogResult.OK)
            {
                Wykres.Series[1].Color = WybranyKolorLinii.Color;
            }
        }

        // zmiana koloru linii dla danych Series[2]
        private void Linia3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog WybranyKolorLinii = new ColorDialog();
            if (WybranyKolorLinii.ShowDialog() == DialogResult.OK)
            {
                Wykres.Series[2].Color = WybranyKolorLinii.Color;
            }
        }

        // zwiększenie grubości linii o 1 piksel na każde kliknięcie
        // zwiększana będzie grubośc linii wybranej przez uzytkownika
        private void ZwiększenieGrubościToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((RataŁ.Checked == true) && (Wykres.Series[0].BorderWidth < 6))
            {
                Wykres.Series[0].BorderWidth++;
            }
            else
                if ((RataO.Checked == true) && (Wykres.Series[1].BorderWidth < 6))
                {
                    {
                        Wykres.Series[1].BorderWidth++;
                    }
                }
                else
                    if ((RataK.Checked == true) && (Wykres.Series[2].BorderWidth < 6))
                    {
                        {
                            Wykres.Series[2].BorderWidth++;
                        }
                    }
                    else
                        if ((RataŁ.Checked == false) && (RataO.Checked == false) && (RataK.Checked == false))
                        {
                            MessageBox.Show("Musisz zaznaczyć którą linią wykresu chcesz manipulować",
                                            "Wykryto brak części informacji",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Exclamation);
                        }
        }

        // zmniejszenie grubości linii o 1 piksel na każde kliknięcie
        // zmniejszana będzie grubośc linii wybranej przez uzytkownika
        private void ZmniejszenieGrubościToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((RataŁ.Checked == true) && (Wykres.Series[0].BorderWidth > 1))
            {
                Wykres.Series[0].BorderWidth--;
            }
            else
                if ((RataO.Checked == true) && (Wykres.Series[1].BorderWidth > 1))
                {
                    Wykres.Series[1].BorderWidth--;
                }
                else
                    if ((RataK.Checked == true) && (Wykres.Series[2].BorderWidth > 1))
                    {
                        Wykres.Series[2].BorderWidth--;
                    }
                    else
                        if ((RataŁ.Checked == false) && (RataO.Checked == false) && (RataK.Checked == false))
                        {
                            MessageBox.Show("Musisz zaznaczyć którą linią wykresu chcesz manipulować",
                                            "Wykryto brak części informacji",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Exclamation);
                        }
        }

        // zmiana wybranej linii wykresu na "ciągła"
        private void LiniaCiągłaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RataŁ.Checked == true)
            {
                Wykres.Series[0].BorderDashStyle = ChartDashStyle.Solid;
            }
            else
                if (RataO.Checked == true)
                {
                    Wykres.Series[1].BorderDashStyle = ChartDashStyle.Solid;
                }
                else
                    if (RataK.Checked == true)
                    {
                        Wykres.Series[2].BorderDashStyle = ChartDashStyle.Solid;
                    }
                    else
                    {
                        MessageBox.Show("Musisz zaznaczyć którą linią wykresu chcesz manipulować",
                                        "Wykryto brak części informacji",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation);
                    }
        }

        // zmiana wybranej linii wykresu na "kreskowa"
        private void LiniaKreskowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RataŁ.Checked == true)
            {
                Wykres.Series[0].BorderDashStyle = ChartDashStyle.Dash;
            }
            else
                if (RataO.Checked == true)
                {
                    Wykres.Series[1].BorderDashStyle = ChartDashStyle.Dash;
                }
                else
                    if (RataK.Checked == true)
                    {
                        Wykres.Series[2].BorderDashStyle = ChartDashStyle.Dash;
                    }
                    else
                    {
                        MessageBox.Show("Musisz zaznaczyć którą linią wykresu chcesz manipulować",
                                        "Wykryto brak części informacji",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation);
                    }
        }

        // zmiana wybranej linii wykresu na "Kropkowa"
        private void LiniaKropkowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RataŁ.Checked == true)
            {
                Wykres.Series[0].BorderDashStyle = ChartDashStyle.Dot;
            }
            else
                if (RataO.Checked == true)
                {
                    Wykres.Series[1].BorderDashStyle = ChartDashStyle.Dot;
                }
                else
                    if (RataK.Checked == true)
                    {
                        Wykres.Series[2].BorderDashStyle = ChartDashStyle.Dot;
                    }
                    else
                    {
                        MessageBox.Show("Musisz zaznaczyć którą linią wykresu chcesz manipulować",
                                        "Wykryto brak części informacji",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation);
                    }
        }

        // zmiana wybranej linii wykresu na "Kreskowo - Kropkowa"
        private void LiniaKreskowokropkowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RataŁ.Checked == true)
            {
                Wykres.Series[0].BorderDashStyle = ChartDashStyle.DashDot;
            }
            else
                if (RataO.Checked == true)
                {
                    Wykres.Series[1].BorderDashStyle = ChartDashStyle.DashDot;
                }
                else
                    if (RataK.Checked == true)
                    {
                        Wykres.Series[2].BorderDashStyle = ChartDashStyle.DashDot;
                    }
                    else
                    {
                        MessageBox.Show("Musisz zaznaczyć którą linią wykresu chcesz manipulować",
                                        "Wykryto brak części informacji",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation);
                    }
        }

        // zmiana wybranej linii wykresu na "Kreskowo - Kropkowo - Kropkową"
        private void KreskowoKropkowoKropkowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RataŁ.Checked == true)
            {
                Wykres.Series[0].BorderDashStyle = ChartDashStyle.DashDotDot;
            }
            else
                if (RataO.Checked == true)
                {
                    Wykres.Series[1].BorderDashStyle = ChartDashStyle.DashDotDot;
                }
                else
                    if (RataK.Checked == true)
                    {
                        Wykres.Series[2].BorderDashStyle = ChartDashStyle.DashDotDot;
                    }
                    else
                    {
                        MessageBox.Show("Musisz zaznaczyć którą linią wykresu chcesz manipulować",
                                        "Wykryto brak części informacji",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation);
                    }
        }

        // przejście do formularza LOKATY z poziomu Menu
        private void ZamknięcieFormularzaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult Pytanie = MessageBox.Show("Przejść do formularza Lokaty?",
                "Zamknąć formularz KREDYTY?",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            switch (Pytanie)
            {
                case DialogResult.OK:
                    MessageBox.Show("Zamykam formularz KREDYTY i przechodzę do LOKATY",
                "Zamykanie formularza?", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // ukrycie formularza kredyty
                    this.Hide();

                    // odszukaniew kolekcji aktywnych formularzy formularza Lokaty
                    foreach (Form Formularz in Application.OpenForms)
                    {
                        if (Formularz.Name == "Lokaty")
                        {
                            // odsłonięcie formularza lokaty
                            Formularz.Show();
                            // zakończenie obsługi zdarzenia click przycisku
                            return;
                        }
                    }
                    // utworzenie egzemplarza formularza Lokaty
                    Lokaty EgzLokaty = new Lokaty();
                    // wyświetlenie pormularza Lokaty
                    EgzLokaty.Show();
                    break;
                case DialogResult.Cancel:
                    MessageBox.Show("Decyzja użytkownika - zaniechanie akcji",
                "Zamykanie formularza?", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        // obsługa przycisku reset
        private void Reset1_Click(object sender, EventArgs e)
        {
            // przejście do pierwszej zakładki frmularza Kredyty
            this.tabControl1.SelectedTab = ZakładkaRozliczenieSpłatyKredytu;

            WybórLiniiWykresu.Visible = false;   // ukrycie GroupBox z wyborem serii danych
            RataŁ.Checked = false;               // wyłączenie selekcji
            RataO.Checked = false;               // wyłączenie selekcji
            RataK.Checked = false;               // wyłączenie selekcji
            Tabela.Visible = false;              // ukrycie tabeli DataGridView
            Tabela.Rows.Clear();                 // wyczyszczenie wierszy
            Tabela.Refresh();                    // odświeżenie zawartości w pamięci
            Wykres.Visible = false;              // ukrycie kontrolki Chart
            FormatowanieWykresu.Visible = false; // ukrycie menu formatowania wykresu

            // reset pozostałych elementów
            RazNaRok.Checked = false;
            CoPółRoku.Checked = false;
            CoKwartał.Checked = false;
            CoMiesiąc.Checked = true;
            Stałe.Checked = false;
            Malejące.Checked = true;
            Rosnące.Checked = false;
            StopaProcentowa.SelectedIndex = -1;
            KwotaKredytu.Text = "";
            OkresSpłaty.Text = "";
            KosztyKredytu.Text = "";
            KońcoweZadłużenie.Text = "";
            InfoWysokośćRatyKapitałowej.Text = "";
            WRInfo.Visible = false;
            InfoWysokośćRatyKapitałowej.Visible = false;
            GraficznaPrezentacjaRat.Enabled = true;
        }


        // metoda pobierająca dane z pliku "comaseparated"
        private string[,] ŁadowanieCSV(string Plik)
        {
            // pobieranie zawartości pliku
            string CałyPlik = System.IO.File.ReadAllText(Plik);

            // dzielenie pliku na linie
            CałyPlik = CałyPlik.Replace('\n', '\r');
            string[] Linie = CałyPlik.Split(new char[] { '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            // sprawdzenie ile rzędów ma plik
            int IlośćRzędów = Linie.Length;
            int IlośćKolumn = Linie[0].Split('.').Length;

            // zadeklarowanie i utworzenie tabeli o wymaganych parametrach
            string[,] Wartości = new string[IlośćRzędów, IlośćKolumn];

            // załadowanie danych do tabeli
            for (int r = 0; r < IlośćRzędów; r++)
            {
                string[] Rozdziel = Linie[r].Split('.');
                for (int k = 0; k < IlośćKolumn; k++)
                {
                    Wartości[r, k] = Rozdziel[k];
                }
            }

            // zwrócenie wartości.
            return Wartości;
        }

        // Obsługa wczytania wskazanego pliku CSV do formularza KREDYTY (separator - (.))
        private void WczytajRozliczenieZPlikuToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // wywołanie systemowego okna dialogowego pozwalającego na zlokalizowanie i wskazanie pliku z rozszeżeniem *.csv
            OpenFileDialog WczytajPlik = new OpenFileDialog
            {
                Filter = "csv files (*.csv)|*.CSV|All files (*.*)|*.*",
                //WczytajPlik.FilterIndex = 2;
                RestoreDirectory = true
            };

            // po wybraniu i zatwierdzeniu pliku wykonaj pobranie danych
            if (WczytajPlik.ShowDialog() == DialogResult.OK)
            {

                // Pobieranie danych
                string[,] TabelaWczytanychDanych = ŁadowanieCSV(WczytajPlik.FileName);
                int Wiersze = TabelaWczytanychDanych.GetUpperBound(0) + 1;
                int Kolumny = TabelaWczytanychDanych.GetUpperBound(1) + 1;

                // narazie zakladam, ze w pierwszym rzędzie są tylko nazwy kolumn
                // puźniej sprubuje przerobic to tak, żeby importował i eksportował wyniki wraz
                // z podstawowymi parametrami kredytu...

                // wyczyszczenie kontrolki Tabela (na wszelki wypadek)
                Tabela.Columns.Clear();

                // wczytanie z pierwszego rzędu pliku .csv nazw kolumn i utworzenie ich w DGV Tabela
                for (int k = 0; k < Kolumny; k++)
                    Tabela.Columns.Add(TabelaWczytanychDanych[0, k], TabelaWczytanychDanych[0, k]);

                // wczytanie dodatkowych danych do kontrolek formularza KREDYTY z ostatniej linii pliku
                KwotaKredytu.Text = TabelaWczytanychDanych[Wiersze - 1, 0];
                OkresSpłaty.Text = TabelaWczytanychDanych[Wiersze - 1, 1];
                StopaProcentowa.SelectedText = TabelaWczytanychDanych[Wiersze - 1, 2];
                KońcoweZadłużenie.Text = TabelaWczytanychDanych[Wiersze - 1, 3];


                // wczytanie danych z pozostałych wierszy do kontrolki DGV Tabela
                for (int r = 1; r < Wiersze - 1; r++)
                {
                    Tabela.Rows.Add();
                    for (int k = 0; k < Kolumny; k++)
                    {
                        Tabela.Rows[r - 1].Cells[k].Value = TabelaWczytanychDanych[r, k];
                    }
                }

                // odsłonięcie kontrolki Tabela na 2 zakładce
                Tabela.Visible = true;
                // przejście do 2 zakłądki formularza KREDYTY
                this.tabControl1.SelectedTab = ZakładkaTabelaryczneRozliczenieKredytu;
            }
        }



        private void ZapiszRozliczenieDoPlikuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // wywołanie okienka systemowego zapisu pliku
            SaveFileDialog ZapiszPlik = new SaveFileDialog
            {
                Filter = "csv files (*.csv)|*.csv",           // zdefiowanie filtra systemowego dla plików typu .csv
                Title = "Zapis do pliku w formacie .csv",     // ustawienie nagłówka okna systemowego

                // wyświetli ostrzeżenie dla użytkownika, jeśli zdefiniowana ścieżka nie istnieje
                CheckPathExists = true,
                // określenie domyślnej ścieżki zapisu pliku
                InitialDirectory = Application.StartupPath
            };

            ZapiszPlik.ShowDialog();


            // jeśli zdefiniowana nazwa pliku nie jest pusta, otwiera go do zapisu
            if (ZapiszPlik.FileName != "")
            {
                StreamWriter Zapisuję = new StreamWriter(ZapiszPlik.FileName);

                // tworzę plik tekstowy z danymi rozdzielanymi (.)
                string Tymczasowy = string.Empty;

                // dodanie wiersza nagłówkowego z kontrolki DGV Tabela
                foreach (DataGridViewColumn column in Tabela.Columns)
                {
                    Tymczasowy += column.HeaderText + '.';
                }

                // dodanie nowej linii
                Tymczasowy += "\r\n";

                // dodanie do pliku danych z kolejnych rzędów
                foreach (DataGridViewRow row in Tabela.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        Tymczasowy += cell.Value.ToString() + '.';
                    }
                    // odanie kolejnej linii
                    Tymczasowy += "\r\n";
                }
                
                // przeniesienie do pliku danych wsadowych do obliczenia kredytu
                Tymczasowy += KwotaKredytu.Text + '.';
                Tymczasowy += OkresSpłaty.Text + '.';
                Tymczasowy += StopaProcentowa.SelectedItem.ToString() + '.';
                Tymczasowy += KońcoweZadłużenie.Text + '.';

                // zapis danych do pliku dyskowego
                Zapisuję.Write(Tymczasowy);

                // zamknięcie procedury zapisu do pliku
                Zapisuję.Close();
            }
        }
    }
}
 

  //Tworzenie nowego TabPage
            //TabPage test = new TabPage();
            //test.Name = "Unikalna_Nazwa"+(tabControl1.TabPages.Count+1).ToString();
            ////

            ////Tworzenie elementów zawartych w tym tabpage
            //System.Windows.Forms.DataGridView dataGridView1;
            //dataGridView1 = new System.Windows.Forms.DataGridView();

            //dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            //dataGridView1.Location = new System.Drawing.Point(16, 6);
            //dataGridView1.Name = "dataGridView1";
            //dataGridView1.Size = new System.Drawing.Size(513, 392);
            //dataGridView1.TabIndex = 0;
            //test.Controls.Add(dataGridView1);
            ////
            

            ////Dodawanie TabPage do tabcontrol
            //tabControl1.TabPages.Add(test);
            //