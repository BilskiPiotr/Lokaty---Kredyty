using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Lokaty_Kredyty
{
    public partial class PB_Kredyty : Form
    {
        public PB_Kredyty()
        {
            InitializeComponent();
        }

        // sprawdzenie i wczytanie do zmiennych danych wejściowych
        bool PB_PobierzDaneWejściowe(out float PB_WartośćKredytu, out float PB_OprocentowanieKredytu, out int PB_LiczbaLatKredytu, out int PB_LiczbaRatWRoku)
        {
            // ustawienie domyślnych wartości dla danych wejściowych do porównania z errorProvider1
            PB_WartośćKredytu = -1.0F;
            PB_OprocentowanieKredytu = 0.0F;
            PB_LiczbaLatKredytu = 0;
            PB_LiczbaRatWRoku = 0;


            // sprawdzenie czy użytkownik wprowadził kwotę kredytu
            if (string.IsNullOrEmpty(PB_KwotaKredytu.Text))
            {
                // klient nie wpisał kwoty - zgłoszenie błędu 
                errorProvider1.SetError(PB_KwotaKredytu,
                    "ERROR: musisz podać kwotę kredytu");
                return false;
            }
            errorProvider1.Dispose();

            // wczytanie wysokości Kredytu ze sprawdzeniem poprawności zapisu 
            if (!float.TryParse(PB_KwotaKredytu.Text, out PB_WartośćKredytu))
            {
                // do wprowadzenia kwoty użyto jakims cudem niedozwolonego znaku - zgłoszenie błędu
                errorProvider1.SetError(PB_KwotaKredytu,
                    "ERROR: w zapisie kwoty użyto niedozwolonego znaku");
                return false;
                // w innym wypadku wczytanie kwoty do zmiennej PB_WartośćKredytu
            }
            errorProvider1.Dispose();

            // sprawdzenie, czy klient wybrał stopę procentową
            if (PB_StopaProcentowa.SelectedIndex < 0)
            {
                // jesli nie wybrał - zgłoszenie błędu
                errorProvider1.SetError(PB_StopaProcentowa,
                    "ERROR: musisz wybrać stopę procentową!");
                return false;
            }
            errorProvider1.Dispose();

            // pobranie wartości stopy procentowej i wprowadzenie jej do zmiennej PB_OprocentowanieKredytu
            if (!float.TryParse(PB_StopaProcentowa.SelectedItem.ToString(), out PB_OprocentowanieKredytu))
            {
                // jesli jakiimś cudem w strumieniu danych pojawi sie niedozwolony znak - zgłoszenie błędu
                errorProvider1.SetError(PB_StopaProcentowa,
                    "ERROR: niedozwolony znak w zapisie stopy procentowej!");
                return false;
                // w innym wypadku wczytanie oprocentowanie do zmiennej PB_OprocentowanieKredytu
            }
            errorProvider1.Dispose();

            // sprawdzenie, czy wpisano cokolwiek do kontrolki określającej okres kredytowania
            if (string.IsNullOrEmpty(PB_OkresSpłaty.Text))
            {
                // jeśli nic nie wpisano - zapalenie kontrolki błędu 
                errorProvider1.SetError(PB_OkresSpłaty,
                    "ERROR: musisz podać czas kredytowania w latach");
                return false;
            }
            errorProvider1.Dispose();

            // wczytanie liczby lat kredytowania ze sprawdzeniem poprawności zapisu 
            if (!int.TryParse(PB_OkresSpłaty.Text, out PB_LiczbaLatKredytu))
            {
                // jesli jakimś cudem użyto niepoprawnego znaku - zgłoszenie błedu
                errorProvider1.SetError(PB_OkresSpłaty,
                    "ERROR: wystąpił niedozwolony znak w zapisie liczby lat kredytowania");
                return false;
                // w innymwypadku wczytanie liczby lat kredytowania do zmiennej PB_OkresKredytu
            }
            errorProvider1.Dispose();

            // sprawdzenie ile razy rocznie będzie spłacana rata kredytu
            if (PB_CoMiesiąc.Checked)
            {
                PB_LiczbaRatWRoku = 12;
            }
            else
                if (PB_CoKwartał.Checked)
                {
                    PB_LiczbaRatWRoku = 4;
                }
                else
                    if (PB_CoPółRoku.Checked)
                    {
                        PB_LiczbaRatWRoku = 2;
                    }
                    else
                        if (PB_RazNaRok.Checked)
                        {
                            PB_LiczbaRatWRoku = 1;
                        }
            return true;
        }

        // obsługa przycisku przejscia do formularza LOKATY
        private void PB_PrzejdźDoLokaty_Click(object sender, EventArgs e)
        {
            // ukrycie formularza Lokaty
            this.Hide();
            // odszukaniew kolekcji aktywnych formularzy formularza PB_Lokaty
            foreach (Form Formularz in Application.OpenForms)
            {
                if (Formularz.Name == "PB_Lokaty")
                {
                    // odsłonięcie formularza PB_Lokaty
                    Formularz.Show();
                    // zakończenie obsługi zdarzenia click przycisku
                    return;
                }
            }
            // utworzenie egzemplarza formularza PB_Lokaty
            PB_Lokaty EgzLokaty = new PB_Lokaty();
            // wyświetlenie pormularza PB_Lokaty
            EgzLokaty.Show();
        }

        // zakończenie programu
        private void koniecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        // Obsługa Przycisku poleceń TABELARYCZNE ROZLICZENIE SPŁATY KREDYTU
        private void PB_TabelarycznaPrezentacjaRat_Click(object sender, EventArgs e)
        {
            // wyczyszczenie możliwej zawartości z kontrolki DataGridView
            while (PB_Tabela.Rows.Count > 0)
            {
                PB_Tabela.Rows.RemoveAt(0);
            }

            // odsłonięcie ukrytych kontrolek
            PB_Tabela.Visible = true;
            PB_WRInfo.Visible = true;
            PB_InfoWysokośćRatyKapitałowej.Visible = true;

            // deklaracja zmiennych
            float PB_WartośćKredytu;
            float PB_OprocentowanieKredytu;
            int PB_LiczbaLatKredytu;
            int PB_LiczbaRatWRoku;

            // pobranie danych wejściowych z formularza KREDYTY
            if (!PB_PobierzDaneWejściowe(out PB_WartośćKredytu, out PB_OprocentowanieKredytu, out PB_LiczbaLatKredytu, out PB_LiczbaRatWRoku))
                //zostałą zapalona kontrolka errorProvider, wiedz koniec obsługi zdażenia
                return;

            // obsługa tabelaryczna dla rat malejących
            if (PB_Malejące.Checked)
            {

                // deklaracje lokalne
                float PB_Zadłużnie;
                float PB_RataOdsetkowa;
                float PB_RataKapitałowa;
                float PB_RataŁączna;
                float PB_KosztKredytu;

                // deklaracja tablicy rozliczenia spłaty kredytu
                float[,] RozliczenieKredyu = new float[PB_LiczbaLatKredytu * PB_LiczbaRatWRoku + 1, 3];

                // przypisanie damych do tablicy dla terminu "zero"
                RozliczenieKredyu[0, 0] = 0.0F;                 // pb łączna
                RozliczenieKredyu[0, 1] = 0.0F;                 // rata odsetkowa
                RozliczenieKredyu[0, 2] = PB_WartośćKredytu;    // zadłużenie

                // ustalenie wartości zmiennych dla terminu "zero"
                PB_Zadłużnie = PB_WartośćKredytu;
                PB_RataKapitałowa = PB_WartośćKredytu / (PB_LiczbaLatKredytu * PB_LiczbaRatWRoku);
                PB_RataOdsetkowa = 0.0F;
                PB_RataŁączna = 0.0F;
                PB_KosztKredytu = 0.0F;

                // inicjalizacja zmiennej tablicowej
                for (int PB_i = 0; PB_i < RozliczenieKredyu.GetLength(0); PB_i++)
                {
                    // wpisanie pierwszego stanu do tablicy rozliczenia kredytu
                    RozliczenieKredyu[PB_i, 0] = PB_RataŁączna;     // rata łączna 
                    RozliczenieKredyu[PB_i, 1] = PB_RataOdsetkowa;  // rata odsetkowa 
                    RozliczenieKredyu[PB_i, 2] = PB_Zadłużnie;      // zadłużenie

                    // wyznaczenie wartości zmiennych dla kolejneg okresu spłaty kredytu
                    PB_RataOdsetkowa = PB_Zadłużnie * PB_OprocentowanieKredytu / PB_LiczbaRatWRoku; // rata odsetkowa
                    PB_RataŁączna = PB_RataKapitałowa + PB_RataOdsetkowa;                           // raty łącznej
                    PB_Zadłużnie = PB_Zadłużnie - PB_RataKapitałowa;                                // zadłużenia po wpłaceniu kolejnej raty kapitałowej
                    PB_KosztKredytu += PB_RataOdsetkowa;                                            // sumowanie kosztu korzystania z kredytu
                }

                // wyprowadzenie do kontrolku formularza informacji o całkowitym koszcie kredytu
                PB_KosztyKredytu.Text = string.Format("{0,8:F1}", PB_KosztKredytu);

                // wypełnienie kontrolki DataGridView wynikami obliczeń dla kolejnych okresów
                // inicjalizacja zmiennej tablicowej
                for (int PB_i = 0; PB_i < RozliczenieKredyu.GetLength(0); PB_i++)
                {
                    PB_Tabela.Rows.Add();                               // dodanie nowego wiersza do kontrolki DataGridView
                    PB_Tabela.Rows[PB_i].Cells[0].Value = PB_i;                                                    // numer raty
                    PB_Tabela.Rows[PB_i].Cells[1].Value = string.Format("{0,8:F2}", RozliczenieKredyu[PB_i, 0]);   // rata łączna
                    PB_Tabela.Rows[PB_i].Cells[2].Value = string.Format("{0,8:F2}", RozliczenieKredyu[PB_i, 1]);   // rata odsetkowa
                    PB_Tabela.Rows[PB_i].Cells[3].Value = string.Format("{0,8:f2}", RozliczenieKredyu[PB_i, 2]);   // zadłużenie
                }

                // wyprowadzenie do formularza informacji o wysokości raty kapitałowej
                PB_InfoWysokośćRatyKapitałowej.Text = string.Format("{0,8:F2}", PB_RataKapitałowa);

                // wyprowadzenie do formularza informacji o saldzie kredytu do spłacenia
                PB_KońcoweZadłużenie.Text = string.Format("{0,8:F1}", RozliczenieKredyu[PB_LiczbaLatKredytu * PB_LiczbaRatWRoku, 2]);

                // przejście do zakładki rozliczenia tabelarycznego
                this.tabControl1.SelectedTab = PB_ZakładkaTabelaryczneRozliczenieKredytu;
            }
            else

                // obsługa tabelaryczna dla rat rosnących
                if (PB_Rosnące.Checked)
                {

                    // deklaracje lokalne
                    float PB_Zadłużnie;
                    float PB_RataOdsetkowa;
                    float PB_RataKapitałowa;
                    float PB_RataŁączna;
                    float PB_KosztKredytu;

                    // deklaracja tablicy rozliczenia spłaty kredytu
                    float[,] RozliczenieKredyu = new float[PB_LiczbaLatKredytu * PB_LiczbaRatWRoku + 1, 3];

                    // przypisanie damych do tablicy dla terminu "zero"
                    RozliczenieKredyu[0, 0] = 0.0F;                 // pb łączna
                    RozliczenieKredyu[0, 1] = 0.0F;                 // rata odsetkowa
                    RozliczenieKredyu[0, 2] = PB_WartośćKredytu;    // zadłużenie

                    // ustalenie wartości zmiennych dla terminu "zero"
                    PB_Zadłużnie = PB_WartośćKredytu;
                    PB_RataKapitałowa = PB_WartośćKredytu / (PB_LiczbaLatKredytu * PB_LiczbaRatWRoku);
                    PB_RataOdsetkowa = 0.0F;
                    PB_RataŁączna = 0.0F;
                    PB_KosztKredytu = 0.0F;

                    // inicjalizacja zmiennej tablicowej
                    for (int PB_i = 0; PB_i < RozliczenieKredyu.GetLength(0); PB_i++)
                    {
                        // wpisanie pierwszego stanu do tablicy rozliczenia kredytu
                        RozliczenieKredyu[PB_i, 0] = PB_RataŁączna;     // rata łączna 
                        RozliczenieKredyu[PB_i, 1] = PB_RataOdsetkowa;  // rata odsetkowa 
                        RozliczenieKredyu[PB_i, 2] = PB_Zadłużnie;      // zadłużenie

                        // wyznaczenie wartości zmiennych dla kolejneg okresu spłaty kredytu
                        PB_RataOdsetkowa = (PB_OprocentowanieKredytu / PB_LiczbaRatWRoku) * ((PB_i + 1) * PB_RataKapitałowa);   // rata odsetkowa
                        PB_RataŁączna = PB_RataKapitałowa + PB_RataOdsetkowa;                                                   // raty łącznej
                        PB_Zadłużnie = PB_Zadłużnie - PB_RataKapitałowa;                                                        // zadłużenia po wpłaceniu kolejnej raty kapitałowej
                        PB_KosztKredytu += PB_RataOdsetkowa;                                                                    // sumowanie kosztu korzystania z kredytu
                    }

                    // wyprowadzenie do kontrolku formularza informacji o całkowitym koszcie kredytu
                    PB_KosztyKredytu.Text = string.Format("{0,8:F1}", (PB_KosztKredytu - PB_RataOdsetkowa));

                    // wypełnienie kontrolki DataGridView wynikami obliczeń dla kolejnych okresów
                    // inicjalizacja zmiennej tablicowej
                    for (int PB_i = 0; PB_i < RozliczenieKredyu.GetLength(0); PB_i++)
                    {
                        PB_Tabela.Rows.Add();                                   // dodanie nowego wiersza do kontrolki DataGridView
                        PB_Tabela.Rows[PB_i].Cells[0].Value = PB_i;                                                    // numer raty
                        PB_Tabela.Rows[PB_i].Cells[1].Value = string.Format("{0,8:F2}", RozliczenieKredyu[PB_i, 0]);   // rata łączna
                        PB_Tabela.Rows[PB_i].Cells[2].Value = string.Format("{0,8:F2}", RozliczenieKredyu[PB_i, 1]);   // rata odsetkowa
                        PB_Tabela.Rows[PB_i].Cells[3].Value = string.Format("{0,8:f2}", RozliczenieKredyu[PB_i, 2]);   // zadłużenie
                    }

                    // wyprowadzenie do formularza informacji o wysokości raty kapitałowej
                    PB_InfoWysokośćRatyKapitałowej.Text = string.Format("{0,8:F2}", PB_RataKapitałowa);

                    // wyprowadzenie do formularza informacji o saldzie kredytu do spłacenia
                    PB_KońcoweZadłużenie.Text = string.Format("{0,8:F1}", RozliczenieKredyu[PB_LiczbaLatKredytu * PB_LiczbaRatWRoku, 2]);

                    // przejście do zakładki rozliczenia tabelarycznego
                    this.tabControl1.SelectedTab = PB_ZakładkaTabelaryczneRozliczenieKredytu;
                }
                else

                    // obsługa tabelaryczna dla rat stałych
                    if (PB_Stałe.Checked)
                    {

                        // deklaracje lokalne
                        float PB_Zadłużnie;
                        float PB_RataOdsetkowa;
                        float PB_RataKapitałowa;
                        float PB_RataŁączna;
                        float PB_KosztKredytu;
                        float PB_Nawias;

                        // deklaracja tablicy rozliczenia spłaty kredytu
                        float[,] RozliczenieKredyu = new float[PB_LiczbaLatKredytu * PB_LiczbaRatWRoku + 1, 3];

                        // przypisanie damych do tablicy dla terminu "zero"
                        RozliczenieKredyu[0, 0] = 0.0F;                 // pb łączna
                        RozliczenieKredyu[0, 1] = 0.0F;                 // rata odsetkowa
                        RozliczenieKredyu[0, 2] = PB_WartośćKredytu;    // zadłużenie

                        // ustalenie wartości zmiennych dla terminu "zero"
                        PB_Zadłużnie = PB_WartośćKredytu;
                        PB_RataOdsetkowa = 0.0F;
                        PB_Nawias = (float)Math.Pow((1 + (PB_OprocentowanieKredytu / PB_LiczbaRatWRoku)), (PB_LiczbaLatKredytu * PB_LiczbaRatWRoku));
                        PB_RataŁączna = PB_WartośćKredytu * (((PB_OprocentowanieKredytu / PB_LiczbaRatWRoku) * PB_Nawias) / (PB_Nawias - 1));
                        PB_KosztKredytu = 0.0F;
                        PB_RataKapitałowa = 0.0F;


                        // inicjalizacja zmiennej tablicowej
                        for (int PB_i = 0; PB_i < RozliczenieKredyu.GetLength(0); PB_i++)
                        {
                            // wpisanie pierwszego stanu do tablicy rozliczenia kredytu
                            RozliczenieKredyu[PB_i, 0] = PB_RataŁączna;     // rata łączna 
                            RozliczenieKredyu[PB_i, 1] = PB_RataOdsetkowa;  // rata odsetkowa 
                            RozliczenieKredyu[PB_i, 2] = PB_Zadłużnie;      // zadłużenie

                            // wyznaczenie wartości zmiennych dla kolejneg okresu spłaty kredytu
                            PB_RataOdsetkowa = PB_Zadłużnie * (PB_OprocentowanieKredytu / PB_LiczbaRatWRoku);    // rata odsetkowa
                            PB_RataKapitałowa = PB_RataŁączna - PB_RataOdsetkowa;  // rata kapitałowa                             
                            PB_Zadłużnie = PB_Zadłużnie - PB_RataKapitałowa;                                    // zadłużenia po wpłaceniu kolejnej raty kapitałowej
                            PB_KosztKredytu += PB_RataOdsetkowa;                                                // sumowanie kosztu korzystania z kredytu
                        }

                        // wyprowadzenie do kontrolku formularza informacji o całkowitym koszcie kredytu
                        PB_KosztyKredytu.Text = string.Format("{0,8:F1}", (PB_KosztKredytu));

                        // wypełnienie kontrolki DataGridView wynikami obliczeń dla kolejnych okresów
                        // inicjalizacja zmiennej tablicowej
                        for (int PB_i = 0; PB_i < RozliczenieKredyu.GetLength(0); PB_i++)
                        {
                            PB_Tabela.Rows.Add();                               // dodanie nowego wiersza do kontrolki DataGridView
                            PB_Tabela.Rows[PB_i].Cells[0].Value = PB_i;                                                    // numer raty
                            PB_Tabela.Rows[PB_i].Cells[1].Value = string.Format("{0,8:F2}", RozliczenieKredyu[PB_i, 0]);   // rata łączna
                            PB_Tabela.Rows[PB_i].Cells[2].Value = string.Format("{0,8:F2}", RozliczenieKredyu[PB_i, 1]);   // rata odsetkowa
                            PB_Tabela.Rows[PB_i].Cells[3].Value = string.Format("{0,8:f2}", RozliczenieKredyu[PB_i, 2]);   // zadłużenie
                        }

                        // wyprowadzenie do formularza informacji o wysokości raty kapitałowej
                        PB_InfoWysokośćRatyKapitałowej.Text = string.Format("{0,8:F2}", PB_RataKapitałowa);

                        // wyprowadzenie do formularza informacji o saldzie kredytu do spłacenia
                        PB_KońcoweZadłużenie.Text = string.Format("{0,8:F1}", RozliczenieKredyu[PB_LiczbaLatKredytu * PB_LiczbaRatWRoku, 2]);

                        // przejście do zakładki rozliczenia tabelarycznego
                        this.tabControl1.SelectedTab = PB_ZakładkaTabelaryczneRozliczenieKredytu;
                    }
        }

        // obsługa przycisku poleceń PREZENTACJA GRAFICZNA KREDYTU
        private void PB_GraficznaPrezentacjaRat_Click(object sender, EventArgs e)
        {
            // zresetowanie wszystkich elementów kontrolki Chart
            PB_Wykres.ResetAutoValues();
            PB_Wykres.Titles.Clear();
            PB_Wykres.Series.Clear();

            // odsłonięcie kontrolki Chart
            PB_Wykres.Visible = true;
            PB_FormatowanieWykresu.Visible = true;

            // deklaracja zmiennych
            float PB_WartośćKredytu;
            float PB_OprocentowanieKredytu;
            int PB_LiczbaLatKredytu;
            int PB_LiczbaRatWRoku;

            // pobranie danych wejściowych z formularza KREDYTY
            if (!PB_PobierzDaneWejściowe(out PB_WartośćKredytu, out PB_OprocentowanieKredytu, out PB_LiczbaLatKredytu, out PB_LiczbaRatWRoku))
                //zostałą zapalona kontrolka errorProvider, wiedz koniec obsługi zdażenia
                return;

            // odsłonięcie pola GroupBox umożliwiającego manipulowanie liniami wykresu
            PB_WybórLiniiWykresu.Visible = true;

            // obsługa wykresu dla rat malejących
            if (PB_Malejące.Checked)
            {
                // deklaracja zmiennych lokalnych
                float PB_Zadłużenie;
                float PB_RataOdsetkowa;
                float PB_RataKapitałowa;
                float PB_RataŁączna;
                float PB_KosztKredytu;

                // deklaracja i utworzenie tablicy rozliczenia kredytu
                float[,] PB_RozliczenieKredytu = new float[PB_LiczbaLatKredytu * PB_LiczbaRatWRoku + 1, 3];

                // przypisanie stanu początkowego zmiennym
                PB_Zadłużenie = PB_WartośćKredytu;
                PB_RataKapitałowa = PB_WartośćKredytu / (PB_LiczbaLatKredytu * PB_LiczbaRatWRoku);
                PB_RataOdsetkowa = 0.0F;
                PB_RataŁączna = 0.0F;
                PB_KosztKredytu = 0.0F;

                // inicjalizacja zmiennej tablicowej
                for (int PB_i = 0; PB_i < PB_RozliczenieKredytu.GetLength(0); PB_i++)
                {
                    // wyznaczenie stanu "zero" dla zmiennych i przypisanie ich do kolumn
                    PB_RozliczenieKredytu[PB_i, 0] = PB_RataŁączna;     // rata łączna
                    PB_RozliczenieKredytu[PB_i, 1] = PB_RataOdsetkowa;  // rata odsetkowa
                    PB_RozliczenieKredytu[PB_i, 2] = PB_Zadłużenie;     // pozostałe zadłużenie

                    // dopisanie do tablicy kolejnego stanu zmiennych
                    PB_RataOdsetkowa = PB_Zadłużenie * (PB_OprocentowanieKredytu / PB_LiczbaRatWRoku);  // rata odsetkowa
                    PB_RataŁączna = PB_RataKapitałowa + PB_RataOdsetkowa;                               // rata łączna
                    PB_Zadłużenie = PB_Zadłużenie - PB_RataKapitałowa;                                  // zadłużenie Z po wpłaceniu raty R
                    PB_KosztKredytu += PB_RataOdsetkowa;                                                // sumowanie kosztu kożystania z kredytu
                }

                // wyprowadzenie do formularza Kredyty informacji o koszcie obsługi tego kredytu
                PB_KosztyKredytu.Text = string.Format("{0,8:F1}", PB_KosztKredytu);

                // wyprowadzenie do formularza Kredyty informacji o pozostałej kwocie do spłaty
                PB_KońcoweZadłużenie.Text = string.Format("{0,8:F1}", PB_RozliczenieKredytu[PB_LiczbaLatKredytu * PB_LiczbaRatWRoku, 2]);

                // formatowanie parametrów ogólnych wykresu
                PB_Wykres.Titles.Add("Spłata kredytu w ratach malejących");         // tytuł wykresu
                PB_Wykres.Location = new Point(30, 40);                             // polożenie górnego lewego narożnika kontrolki Chart
                PB_Wykres.Width = (int)(this.Width * 0.7F);                         // szerokosć wykresu
                PB_Wykres.Height = (int)(this.Height * 0.7F);                       // wyskośc wykresu
                PB_Wykres.BackColor = Color.Bisque;                                 // kolor tła
                PB_Wykres.Legends.FindByName("Legend1").Docking = Docking.Bottom;   // zdefiniowanie położenia legendy

                // formatowanie parametrów dla pierwszej serii danych - Series[0] "Rata Łączna"

                PB_Wykres.Series.Add("Seria 0");

                PB_Wykres.Series[0].Name = "Rata łączna";                   // nazwa serii danych Series[0]
                PB_Wykres.Series[0].ChartType = SeriesChartType.Line;       // rodzaj wykresu - liniowy
                PB_Wykres.Series[0].Color = Color.Blue;                     // kolor linii - niebieski
                PB_Wykres.Series[0].BorderDashStyle = ChartDashStyle.Dash;  // styl linii - kreskowy
                PB_Wykres.Series[0].BorderWidth = 2;                        // grubość linii

                // deklaracja tablic pomocniczych opisujących położenie punktów na wykresie

                int[] NumeryRatKredytu = new int[PB_RozliczenieKredytu.GetLength(0) - 1];  // tablica 1
                for (int PB_j = 0; PB_j < PB_RozliczenieKredytu.GetLength(0) - 1; PB_j++)  // zmienna tablicowa
                    NumeryRatKredytu[PB_j] = PB_j + 1;                                  // zaladowanie danych

                float[] PunktyWykresu = new float[PB_RozliczenieKredytu.GetLength(0) - 1]; // tablica 2
                for (int PB_k = 0; PB_k < PB_RozliczenieKredytu.GetLength(0) - 1; PB_k++)  // zmienna tablicowa
                    PunktyWykresu[PB_k] = PB_RozliczenieKredytu[PB_k + 1, 0];              // załadowanie danych

                // powiązanie tablic z wektorami punktów z kontrolką Chart
                PB_Wykres.Series[0].Points.DataBindXY(NumeryRatKredytu, PunktyWykresu);



                // dodanie do kontrolki Chart 2-giej serii danych
                PB_Wykres.Series.Add("Seria 1");

                // formatowanie parametrów dla 2-giej serii danych - Series[1] "Rata Odsetkowa"
                PB_Wykres.Series[1].Name = "Rata odsetkowa";                // nazwa serii danych Series[1]
                PB_Wykres.Series[1].ChartType = SeriesChartType.Line;       // rodzaj wykresu - liniowy
                PB_Wykres.Series[1].Color = Color.Green;                    // kolor linii - zielony
                PB_Wykres.Series[1].BorderDashStyle = ChartDashStyle.Dot;   // styl linii - kropkowy
                PB_Wykres.Series[1].BorderWidth = 2;                        // grubość linii

                // wprowadzenie danych opisujących położenie punktów linii wykresu dla - Series[1]
                for (int PB_l = 0; PB_l < PB_RozliczenieKredytu.GetLength(0) - 1; PB_l++)
                    PunktyWykresu[PB_l] = PB_RozliczenieKredytu[PB_l + 1, 1]; ;

                // powiązanie tablic z wektorami punktów z kontrolką Chart
                PB_Wykres.Series[1].Points.DataBindXY(NumeryRatKredytu, PunktyWykresu);



                // dodanie do kontrolki Chart 3-ciej serii danych
                PB_Wykres.Series.Add("Seria 2");

                // formatowanie parametrów dla 2-giej serii danych - Series[2] "Rata Kapitałowa"
                PB_Wykres.Series[2].Name = "Rata kapitałowa";               // nazwa serii danych Series[2]
                PB_Wykres.Series[2].ChartType = SeriesChartType.Line;       // rodzaj wykresu - liniowy
                PB_Wykres.Series[2].Color = Color.Red;                      // kolor linii - czerwony
                PB_Wykres.Series[2].BorderDashStyle = ChartDashStyle.Solid; // styl linii - ciągły
                PB_Wykres.Series[2].BorderWidth = 2;                        // grubość linii

                // wprowadzenie danych opisujących położenie punktów linii wykresu dla - Series[2]
                for (int PB_m = 0; PB_m < PB_RozliczenieKredytu.GetLength(0) - 1; PB_m++)
                    PunktyWykresu[PB_m] = PB_RataKapitałowa;

                // powiązanie tablic z wektorami punktów z kontrolką Chart
                PB_Wykres.Series[2].Points.DataBindXY(NumeryRatKredytu, PunktyWykresu);

                // wyłączenie przycisku graficznej wizualizacni do momentu zresetowania formularza Kredyty
                // PB_GraficznaPrezentacjaRat.Enabled = false;

                // przejście do zakładki z wykresem
                this.tabControl1.SelectedTab = PB_ZakładkaGraficznaPrezentacjaKredytu;
            }


            else
                // obsługa wykresu dla rat rosnących
                if (PB_Rosnące.Checked)
                {
                    // deklaracja zmiennych lokalnych
                    float PB_Zadłużenie;
                    float PB_RataOdsetkowa;
                    float PB_RataKapitałowa;
                    float PB_RataŁączna;
                    float PB_KosztKredytu;

                    // deklaracja i utworzenie tablicy rozliczenia kredytu
                    float[,] PB_RozliczenieKredytu = new float[PB_LiczbaLatKredytu * PB_LiczbaRatWRoku + 1, 3];

                    // przypisanie stanu początkowego zmiennym
                    PB_Zadłużenie = PB_WartośćKredytu;
                    PB_RataKapitałowa = PB_WartośćKredytu / (PB_LiczbaLatKredytu * PB_LiczbaRatWRoku);
                    PB_RataOdsetkowa = 0.0F;
                    PB_RataŁączna = 0.0F;
                    PB_KosztKredytu = 0.0F;

                    // inicjalizacja zmiennej tablicowej
                    for (int PB_i = 0; PB_i < PB_RozliczenieKredytu.GetLength(0); PB_i++)
                    {
                        // wyznaczenie stanu "zero" dla zmiennych i przypisanie ich do kolumn
                        PB_RozliczenieKredytu[PB_i, 0] = PB_RataŁączna;     // rata łączna
                        PB_RozliczenieKredytu[PB_i, 1] = PB_RataOdsetkowa;  // rata odsetkowa
                        PB_RozliczenieKredytu[PB_i, 2] = PB_Zadłużenie;     // pozostałe zadłużenie

                        // dopisanie do tablicy kolejnego stanu zmiennych
                        PB_RataOdsetkowa = (PB_OprocentowanieKredytu / PB_LiczbaRatWRoku) * ((PB_i + 1) * PB_RataKapitałowa);// rata odsetkowa
                        PB_RataŁączna = PB_RataKapitałowa + PB_RataOdsetkowa;                                               // rata łączna
                        PB_Zadłużenie = PB_Zadłużenie - PB_RataKapitałowa;                                                  // zadłużenie Z po wpłaceniu raty R
                        PB_KosztKredytu += PB_RataOdsetkowa;                                                                // sumowanie kosztu kożystania z kredytu
                    }

                    // wyprowadzenie do formularza Kredyty informacji o koszcie obsługi tego kredytu
                    PB_KosztyKredytu.Text = string.Format("{0,8:F1}", PB_KosztKredytu);

                    // wyprowadzenie do formularza Kredyty informacji o pozostałej kwocie do spłaty
                    PB_KońcoweZadłużenie.Text = string.Format("{0,8:F1}", PB_RozliczenieKredytu[PB_LiczbaLatKredytu * PB_LiczbaRatWRoku, 2]);

                    // formatowanie parametrów ogólnych wykresu
                    PB_Wykres.Titles.Add("Spłata kredytu w ratach malejących");         // tytuł wykresu
                    PB_Wykres.Location = new Point(30, 40);                             // polożenie górnego lewego narożnika kontrolki Chart
                    PB_Wykres.Width = (int)(this.Width * 0.7F);                         // szerokosć wykresu
                    PB_Wykres.Height = (int)(this.Height * 0.7F);                       // wyskośc wykresu
                    PB_Wykres.BackColor = Color.Bisque;                                 // kolor tła
                    PB_Wykres.Legends.FindByName("Legend1").Docking = Docking.Bottom;   // zdefiniowanie położenia legendy

                    // formatowanie parametrów dla pierwszej serii danych - Series[0] "Rata Łączna"

                    PB_Wykres.Series.Add("Seria 0");

                    PB_Wykres.Series[0].Name = "Rata łączna";                   // nazwa serii danych Series[0]
                    PB_Wykres.Series[0].ChartType = SeriesChartType.Line;       // rodzaj wykresu - liniowy
                    PB_Wykres.Series[0].Color = Color.Blue;                     // kolor linii - niebieski
                    PB_Wykres.Series[0].BorderDashStyle = ChartDashStyle.Dash;  // styl linii - kreskowy
                    PB_Wykres.Series[0].BorderWidth = 2;                        // grubość linii

                    // deklaracja tablic pomocniczych opisujących położenie punktów na wykresie

                    int[] NumeryRatKredytu = new int[PB_RozliczenieKredytu.GetLength(0) - 1];  // tablica 1
                    for (int PB_j = 0; PB_j < PB_RozliczenieKredytu.GetLength(0) - 1; PB_j++)  // zmienna tablicowa
                        NumeryRatKredytu[PB_j] = PB_j + 1;                                  // zaladowanie danych

                    float[] PunktyWykresu = new float[PB_RozliczenieKredytu.GetLength(0) - 1]; // tablica 2
                    for (int PB_k = 0; PB_k < PB_RozliczenieKredytu.GetLength(0) - 1; PB_k++)  // zmienna tablicowa
                        PunktyWykresu[PB_k] = PB_RozliczenieKredytu[PB_k + 1, 0];              // załadowanie danych

                    // powiązanie tablic z wektorami punktów z kontrolką Chart
                    PB_Wykres.Series[0].Points.DataBindXY(NumeryRatKredytu, PunktyWykresu);



                    // dodanie do kontrolki Chart 2-giej serii danych
                    PB_Wykres.Series.Add("Seria 1");

                    // formatowanie parametrów dla 2-giej serii danych - Series[1] "Rata Odsetkowa"
                    PB_Wykres.Series[1].Name = "Rata odsetkowa";                // nazwa serii danych Series[1]
                    PB_Wykres.Series[1].ChartType = SeriesChartType.Line;       // rodzaj wykresu - liniowy
                    PB_Wykres.Series[1].Color = Color.Green;                    // kolor linii - zielony
                    PB_Wykres.Series[1].BorderDashStyle = ChartDashStyle.Dot;   // styl linii - kropkowy
                    PB_Wykres.Series[1].BorderWidth = 2;                        // grubość linii

                    // wprowadzenie danych opisujących położenie punktów linii wykresu dla - Series[1]
                    for (int PB_l = 0; PB_l < PB_RozliczenieKredytu.GetLength(0) - 1; PB_l++)
                        PunktyWykresu[PB_l] = PB_RozliczenieKredytu[PB_l + 1, 1]; ;

                    // powiązanie tablic z wektorami punktów z kontrolką Chart
                    PB_Wykres.Series[1].Points.DataBindXY(NumeryRatKredytu, PunktyWykresu);



                    // dodanie do kontrolki Chart 3-ciej serii danych
                    PB_Wykres.Series.Add("Seria 2");

                    // formatowanie parametrów dla 2-giej serii danych - Series[2] "Rata Kapitałowa"
                    PB_Wykres.Series[2].Name = "Rata kapitałowa";               // nazwa serii danych Series[2]
                    PB_Wykres.Series[2].ChartType = SeriesChartType.Line;       // rodzaj wykresu - liniowy
                    PB_Wykres.Series[2].Color = Color.Red;                      // kolor linii - czerwony
                    PB_Wykres.Series[2].BorderDashStyle = ChartDashStyle.Solid; // styl linii - ciągły
                    PB_Wykres.Series[2].BorderWidth = 2;                        // grubość linii

                    // wprowadzenie danych opisujących położenie punktów linii wykresu dla - Series[2]
                    for (int PB_m = 0; PB_m < PB_RozliczenieKredytu.GetLength(0) - 1; PB_m++)
                        PunktyWykresu[PB_m] = PB_RataKapitałowa;

                    // powiązanie tablic z wektorami punktów z kontrolką Chart
                    PB_Wykres.Series[2].Points.DataBindXY(NumeryRatKredytu, PunktyWykresu);

                    // wyłączenie przycisku graficznej wizualizacni do momentu zresetowania formularza Kredyty
                    // PB_GraficznaPrezentacjaRat.Enabled = false;

                    // przejście do zakładki z wykresem
                    this.tabControl1.SelectedTab = PB_ZakładkaGraficznaPrezentacjaKredytu;
                }
                else


                    // obsługa wykresu dla rat stałych
                    if (PB_Stałe.Checked)
                    {

                        // deklaracja zmiennych lokalnych
                        float PB_Zadłużenie;
                        float PB_RataOdsetkowa;
                        float PB_RataKapitałowa;
                        float PB_RataŁączna;
                        float PB_KosztKredytu;
                        float PB_Nawias;

                        // deklaracja i utworzenie tablicy rozliczenia kredytu
                        float[,] PB_RozliczenieKredytu = new float[PB_LiczbaLatKredytu * PB_LiczbaRatWRoku + 1, 3];

                        // przypisanie stanu początkowego zmiennym
                        PB_Zadłużenie = PB_WartośćKredytu;
                        PB_RataOdsetkowa = 0.0F;
                        PB_Nawias = (float)Math.Pow((1 + (PB_OprocentowanieKredytu / PB_LiczbaRatWRoku)), (PB_LiczbaLatKredytu * PB_LiczbaRatWRoku));
                        PB_RataŁączna = PB_WartośćKredytu * (((PB_OprocentowanieKredytu / PB_LiczbaRatWRoku) * PB_Nawias) / (PB_Nawias - 1));
                        PB_KosztKredytu = 0.0F;
                        PB_RataKapitałowa = 0.0F;

                        // inicjalizacja zmiennej tablicowej
                        for (int PB_i = 0; PB_i < PB_RozliczenieKredytu.GetLength(0); PB_i++)
                        {
                            // wyznaczenie stanu "zero" dla zmiennych i przypisanie ich do kolumn
                            PB_RozliczenieKredytu[PB_i, 0] = PB_RataŁączna;     // rata łączna
                            PB_RozliczenieKredytu[PB_i, 1] = PB_RataOdsetkowa;  // rata odsetkowa
                            PB_RozliczenieKredytu[PB_i, 2] = PB_Zadłużenie;     // pozostałe zadłużenie

                            // dopisanie do tablicy kolejnego stanu zmiennych
                            PB_RataOdsetkowa = PB_Zadłużenie * (PB_OprocentowanieKredytu / PB_LiczbaRatWRoku);  // rata odsetkowa
                            PB_RataKapitałowa = PB_RataŁączna - PB_RataOdsetkowa;                               // rata kapitałowa
                            PB_Zadłużenie = PB_Zadłużenie - PB_RataKapitałowa;                                  // zadłużenie Z po wpłaceniu raty R
                            PB_KosztKredytu += PB_RataOdsetkowa;                                                // sumowanie kosztu kożystania z kredytu
                        }

                        // wyprowadzenie do formularza Kredyty informacji o koszcie obsługi tego kredytu
                        PB_KosztyKredytu.Text = string.Format("{0,8:F1}", PB_KosztKredytu);

                        // wyprowadzenie do formularza Kredyty informacji o pozostałej kwocie do spłaty
                        PB_KońcoweZadłużenie.Text = string.Format("{0,8:F1}", PB_RozliczenieKredytu[PB_LiczbaLatKredytu * PB_LiczbaRatWRoku, 2]);

                        // formatowanie parametrów ogólnych wykresu
                        PB_Wykres.Titles.Add("Spłata kredytu w ratach malejących");         // tytuł wykresu
                        PB_Wykres.Location = new Point(30, 40);                             // polożenie górnego lewego narożnika kontrolki Chart
                        PB_Wykres.Width = (int)(this.Width * 0.7F);                         // szerokosć wykresu
                        PB_Wykres.Height = (int)(this.Height * 0.7F);                       // wyskośc wykresu
                        PB_Wykres.BackColor = Color.Bisque;                                 // kolor tła
                        PB_Wykres.Legends.FindByName("Legend1").Docking = Docking.Bottom;   // zdefiniowanie położenia legendy

                        // formatowanie parametrów dla pierwszej serii danych - Series[0] "Rata Łączna"

                        PB_Wykres.Series.Add("Seria 0");

                        PB_Wykres.Series[0].Name = "Rata łączna";                   // nazwa serii danych Series[0]
                        PB_Wykres.Series[0].ChartType = SeriesChartType.Line;       // rodzaj wykresu - liniowy
                        PB_Wykres.Series[0].Color = Color.Blue;                     // kolor linii - niebieski
                        PB_Wykres.Series[0].BorderDashStyle = ChartDashStyle.Dash;  // styl linii - kreskowy
                        PB_Wykres.Series[0].BorderWidth = 1;                        // grubość linii

                        // deklaracja tablic pomocniczych opisujących położenie punktów na wykresie

                        int[] NumeryRatKredytu = new int[PB_RozliczenieKredytu.GetLength(0) - 1];  // tablica 1
                        for (int PB_j = 0; PB_j < PB_RozliczenieKredytu.GetLength(0) - 1; PB_j++)  // zmienna tablicowa
                            NumeryRatKredytu[PB_j] = PB_j + 1;                                  // zaladowanie danych

                        float[] PunktyWykresu = new float[PB_RozliczenieKredytu.GetLength(0) - 1]; // tablica 2
                        for (int PB_k = 0; PB_k < PB_RozliczenieKredytu.GetLength(0) - 1; PB_k++)  // zmienna tablicowa
                            PunktyWykresu[PB_k] = PB_RozliczenieKredytu[PB_k + 1, 0];              // załadowanie danych

                        // powiązanie tablic z wektorami punktów z kontrolką Chart
                        PB_Wykres.Series[0].Points.DataBindXY(NumeryRatKredytu, PunktyWykresu);



                        // dodanie do kontrolki Chart 2-giej serii danych
                        PB_Wykres.Series.Add("Seria 1");

                        // formatowanie parametrów dla 2-giej serii danych - Series[1] "Rata Odsetkowa"
                        PB_Wykres.Series[1].Name = "Rata odsetkowa";                // nazwa serii danych Series[1]
                        PB_Wykres.Series[1].ChartType = SeriesChartType.Line;       // rodzaj wykresu - liniowy
                        PB_Wykres.Series[1].Color = Color.Green;                    // kolor linii - zielony
                        PB_Wykres.Series[1].BorderDashStyle = ChartDashStyle.Dot;   // styl linii - kropkowy
                        PB_Wykres.Series[1].BorderWidth = 1;                        // grubość linii

                        // wprowadzenie danych opisujących położenie punktów linii wykresu dla - Series[1]
                        for (int PB_l = 0; PB_l < PB_RozliczenieKredytu.GetLength(0) - 1; PB_l++)
                            PunktyWykresu[PB_l] = PB_RozliczenieKredytu[PB_l + 1, 1]; ;

                        // powiązanie tablic z wektorami punktów z kontrolką Chart
                        PB_Wykres.Series[1].Points.DataBindXY(NumeryRatKredytu, PunktyWykresu);



                        // dodanie do kontrolki Chart 3-ciej serii danych
                        PB_Wykres.Series.Add("Seria 2");

                        // formatowanie parametrów dla 2-giej serii danych - Series[2] "Rata Kapitałowa"
                        PB_Wykres.Series[2].Name = "Rata kapitałowa";               // nazwa serii danych Series[2]
                        PB_Wykres.Series[2].ChartType = SeriesChartType.Line;       // rodzaj wykresu - liniowy
                        PB_Wykres.Series[2].Color = Color.Red;                      // kolor linii - czerwony
                        PB_Wykres.Series[2].BorderDashStyle = ChartDashStyle.Solid; // styl linii - ciągły
                        PB_Wykres.Series[2].BorderWidth = 1;                        // grubość linii

                        // wprowadzenie danych opisujących położenie punktów linii wykresu dla - Series[2]
                        for (int PB_m = 0; PB_m < PB_RozliczenieKredytu.GetLength(0) - 1; PB_m++)
                            PunktyWykresu[PB_m] = PB_RozliczenieKredytu[PB_m + 1, 0] - PB_RozliczenieKredytu[PB_m + 1, 1];

                        // powiązanie tablic z wektorami punktów z kontrolką Chart
                        PB_Wykres.Series[2].Points.DataBindXY(NumeryRatKredytu, PunktyWykresu);

                        // wyłączenie przycisku graficznej wizualizacni do momentu zresetowania formularza Kredyty
                        // PB_GraficznaPrezentacjaRat.Enabled = false;



                        // przejście do zakładki z wykresem
                        this.tabControl1.SelectedTab = PB_ZakładkaGraficznaPrezentacjaKredytu;
                    }
        }

        // ograniczenie możliwości wprowadzania danych jedynie do cyfr i znaku (,) 
        // dla kontrolki pobierającej KwotęKredytu
        private void PB_KwotaKredytu_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8;
        }

        // ograniczenie możliwości wprowadzania danych jedynie do cyfr i znaku (,) 
        // dla kontrolki pobierającej IlośćLatKredytowania
        private void PB_OkresSpłaty_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar);
        }

        // obsługa przycisku "krzyżyk" - zamknięcie formularza / programu
        private void PB_Kredyty_FormClosing(object sender, FormClosingEventArgs e)
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
        private void zmianaKoloruTłaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog PB_WybranyKolorTła = new ColorDialog();
            if (PB_WybranyKolorTła.ShowDialog() == DialogResult.OK)
            {
                PB_Wykres.BackColor = PB_WybranyKolorTła.Color;
            }
        }

        // Zmiana koloru linii dla danych Series[0]
        private void linia1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog PB_WybranyKolorLinii = new ColorDialog();
            if (PB_WybranyKolorLinii.ShowDialog() == DialogResult.OK)
            {
                PB_Wykres.Series[0].Color = PB_WybranyKolorLinii.Color;
            }
        }

        // zmiana koloru linii dla danych Series[1]
        private void linia2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog PB_WybranyKolorLinii = new ColorDialog();
            if (PB_WybranyKolorLinii.ShowDialog() == DialogResult.OK)
            {
                PB_Wykres.Series[1].Color = PB_WybranyKolorLinii.Color;
            }
        }

        // zmiana koloru linii dla danych Series[2]
        private void linia3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog PB_WybranyKolorLinii = new ColorDialog();
            if (PB_WybranyKolorLinii.ShowDialog() == DialogResult.OK)
            {
                PB_Wykres.Series[2].Color = PB_WybranyKolorLinii.Color;
            }
        }

        // zwiększenie grubości linii o 1 piksel na każde kliknięcie
        // zwiększana będzie grubośc linii wybranej przez uzytkownika
        private void zwiększenieGrubościToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((PB_RataŁ.Checked == true) && (PB_Wykres.Series[0].BorderWidth < 6))
            {
                PB_Wykres.Series[0].BorderWidth++;
            }
            else
                if ((PB_RataO.Checked == true) && (PB_Wykres.Series[1].BorderWidth < 6))
                {
                    {
                        PB_Wykres.Series[1].BorderWidth++;
                    }
                }
                else
                    if ((PB_RataK.Checked == true) && (PB_Wykres.Series[2].BorderWidth < 6))
                    {
                        {
                            PB_Wykres.Series[2].BorderWidth++;
                        }
                    }
                    else
                        if ((PB_RataŁ.Checked == false) && (PB_RataO.Checked == false) && (PB_RataK.Checked == false))
                        {
                            MessageBox.Show("Musisz zaznaczyć którą linią wykresu chcesz manipulować",
                                            "Wykryto brak części informacji",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Exclamation);
                        }
        }

        // zmniejszenie grubości linii o 1 piksel na każde kliknięcie
        // zmniejszana będzie grubośc linii wybranej przez uzytkownika
        private void zmniejszenieGrubościToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((PB_RataŁ.Checked == true) && (PB_Wykres.Series[0].BorderWidth > 1))
            {
                PB_Wykres.Series[0].BorderWidth--;
            }
            else
                if ((PB_RataO.Checked == true) && (PB_Wykres.Series[1].BorderWidth > 1))
                {
                    PB_Wykres.Series[1].BorderWidth--;
                }
                else
                    if ((PB_RataK.Checked == true) && (PB_Wykres.Series[2].BorderWidth > 1))
                    {
                        PB_Wykres.Series[2].BorderWidth--;
                    }
                    else
                        if ((PB_RataŁ.Checked == false) && (PB_RataO.Checked == false) && (PB_RataK.Checked == false))
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
            if (PB_RataŁ.Checked == true)
            {
                PB_Wykres.Series[0].BorderDashStyle = ChartDashStyle.Solid;
            }
            else
                if (PB_RataO.Checked == true)
                {
                    PB_Wykres.Series[1].BorderDashStyle = ChartDashStyle.Solid;
                }
                else
                    if (PB_RataK.Checked == true)
                    {
                        PB_Wykres.Series[2].BorderDashStyle = ChartDashStyle.Solid;
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
        private void liniaKreskowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PB_RataŁ.Checked == true)
            {
                PB_Wykres.Series[0].BorderDashStyle = ChartDashStyle.Dash;
            }
            else
                if (PB_RataO.Checked == true)
                {
                    PB_Wykres.Series[1].BorderDashStyle = ChartDashStyle.Dash;
                }
                else
                    if (PB_RataK.Checked == true)
                    {
                        PB_Wykres.Series[2].BorderDashStyle = ChartDashStyle.Dash;
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
        private void liniaKropkowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PB_RataŁ.Checked == true)
            {
                PB_Wykres.Series[0].BorderDashStyle = ChartDashStyle.Dot;
            }
            else
                if (PB_RataO.Checked == true)
                {
                    PB_Wykres.Series[1].BorderDashStyle = ChartDashStyle.Dot;
                }
                else
                    if (PB_RataK.Checked == true)
                    {
                        PB_Wykres.Series[2].BorderDashStyle = ChartDashStyle.Dot;
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
        private void liniaKreskowokropkowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PB_RataŁ.Checked == true)
            {
                PB_Wykres.Series[0].BorderDashStyle = ChartDashStyle.DashDot;
            }
            else
                if (PB_RataO.Checked == true)
                {
                    PB_Wykres.Series[1].BorderDashStyle = ChartDashStyle.DashDot;
                }
                else
                    if (PB_RataK.Checked == true)
                    {
                        PB_Wykres.Series[2].BorderDashStyle = ChartDashStyle.DashDot;
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
        private void kreskowoKropkowoKropkowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PB_RataŁ.Checked == true)
            {
                PB_Wykres.Series[0].BorderDashStyle = ChartDashStyle.DashDotDot;
            }
            else
                if (PB_RataO.Checked == true)
                {
                    PB_Wykres.Series[1].BorderDashStyle = ChartDashStyle.DashDotDot;
                }
                else
                    if (PB_RataK.Checked == true)
                    {
                        PB_Wykres.Series[2].BorderDashStyle = ChartDashStyle.DashDotDot;
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
        private void zamknięcieFormularzaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult PB_Pytanie = MessageBox.Show("Przejść do formularza Lokaty?",
                "Zamknąć formularz KREDYTY?",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            switch (PB_Pytanie)
            {
                case DialogResult.OK:
                    MessageBox.Show("Zamykam formularz KREDYTY i przechodzę do LOKATY",
                "Zamykanie formularza?", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // ukrycie formularza kredyty
                    this.Hide();

                    // odszukaniew kolekcji aktywnych formularzy formularza PB_Lokaty
                    foreach (Form Formularz in Application.OpenForms)
                    {
                        if (Formularz.Name == "PB_Lokaty")
                        {
                            // odsłonięcie formularza PB_lokaty
                            Formularz.Show();
                            // zakończenie obsługi zdarzenia click przycisku
                            return;
                        }
                    }
                    // utworzenie egzemplarza formularza PB_Lokaty
                    PB_Lokaty EgzLokaty = new PB_Lokaty();
                    // wyświetlenie pormularza PB_Lokaty
                    EgzLokaty.Show();
                    break;
                case DialogResult.Cancel:
                    MessageBox.Show("Decyzja użytkownika - zaniechanie akcji",
                "Zamykanie formularza?", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        // obsługa przycisku reset
        private void PB_reset1_Click(object sender, EventArgs e)
        {
            // przejście do pierwszej zakładki frmularza Kredyty
            this.tabControl1.SelectedTab = PB_ZakładkaRozliczenieSpłatyKredytu;

            PB_WybórLiniiWykresu.Visible = false;   // ukrycie GroupBox z wyborem serii danych
            PB_RataŁ.Checked = false;               // wyłączenie selekcji
            PB_RataO.Checked = false;               // wyłączenie selekcji
            PB_RataK.Checked = false;               // wyłączenie selekcji
            PB_Tabela.Visible = false;              // ukrycie tabeli DataGridView
            PB_Tabela.Rows.Clear();                 // wyczyszczenie wierszy
            PB_Tabela.Refresh();                    // odświeżenie zawartości w pamięci
            PB_Wykres.Visible = false;              // ukrycie kontrolki Chart
            PB_FormatowanieWykresu.Visible = false; // ukrycie menu formatowania wykresu

            // reset pozostałych elementów
            PB_RazNaRok.Checked = false;
            PB_CoPółRoku.Checked = false;
            PB_CoKwartał.Checked = false;
            PB_CoMiesiąc.Checked = true;
            PB_Stałe.Checked = false;
            PB_Malejące.Checked = true;
            PB_Rosnące.Checked = false;
            PB_StopaProcentowa.SelectedIndex = -1;
            PB_KwotaKredytu.Text = "";
            PB_OkresSpłaty.Text = "";
            PB_KosztyKredytu.Text = "";
            PB_KońcoweZadłużenie.Text = "";
            PB_InfoWysokośćRatyKapitałowej.Text = "";
            PB_WRInfo.Visible = false;
            PB_InfoWysokośćRatyKapitałowej.Visible = false;
            PB_GraficznaPrezentacjaRat.Enabled = true;
        }


        // metoda pobierająca dane z pliku "comaseparated"
        private string[,] PB_ŁadowanieCSV(string PB_Plik)
        {
            // pobieranie zawartości pliku
            string PB_CałyPlik = System.IO.File.ReadAllText(PB_Plik);

            // dzielenie pliku na linie
            PB_CałyPlik = PB_CałyPlik.Replace('\n', '\r');
            string[] PB_Linie = PB_CałyPlik.Split(new char[] { '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            // sprawdzenie ile rzędów ma plik
            int PB_IlośćRzędów = PB_Linie.Length;
            int PB_IlośćKolumn = PB_Linie[0].Split('.').Length;

            // zadeklarowanie i utworzenie tabeli o wymaganych parametrach
            string[,] PB_Wartości = new string[PB_IlośćRzędów, PB_IlośćKolumn];

            // załadowanie danych do tabeli
            for (int PB_r = 0; PB_r < PB_IlośćRzędów; PB_r++)
            {
                string[] PB_Rozdziel = PB_Linie[PB_r].Split('.');
                for (int PB_k = 0; PB_k < PB_IlośćKolumn; PB_k++)
                {
                    PB_Wartości[PB_r, PB_k] = PB_Rozdziel[PB_k];
                }
            }

            // zwrócenie wartości.
            return PB_Wartości;
        }

        // Obsługa wczytania wskazanego pliku CSV do formularza KREDYTY (separator - (.))
        private void wczytajRozliczenieZPlikuToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // wywołanie systemowego okna dialogowego pozwalającego na zlokalizowanie i wskazanie pliku z rozszeżeniem *.csv
            OpenFileDialog PB_WczytajPlik = new OpenFileDialog();
            PB_WczytajPlik.Filter = "csv files (*.csv)|*.CSV|All files (*.*)|*.*";
            //PB_WczytajPlik.FilterIndex = 2;
            PB_WczytajPlik.RestoreDirectory = true;

            // po wybraniu i zatwierdzeniu pliku wykonaj pobranie danych
            if (PB_WczytajPlik.ShowDialog() == DialogResult.OK)
            {

                // Pobieranie danych
                string[,] PB_TabelaWczytanychDanych = PB_ŁadowanieCSV(PB_WczytajPlik.FileName);
                int PB_Wiersze = PB_TabelaWczytanychDanych.GetUpperBound(0) + 1;
                int PB_Kolumny = PB_TabelaWczytanychDanych.GetUpperBound(1) + 1;

                // narazie zakladam, ze w pierwszym rzędzie są tylko nazwy kolumn
                // puźniej sprubuje przerobic to tak, żeby importował i eksportował wyniki wraz
                // z podstawowymi parametrami kredytu...

                // wyczyszczenie kontrolki PB_Tabela (na wszelki wypadek)
                PB_Tabela.Columns.Clear();

                // wczytanie z pierwszego rzędu pliku .csv nazw kolumn i utworzenie ich w DGV PB_Tabela
                for (int PB_k = 0; PB_k < PB_Kolumny; PB_k++)
                    PB_Tabela.Columns.Add(PB_TabelaWczytanychDanych[0, PB_k], PB_TabelaWczytanychDanych[0, PB_k]);

                // wczytanie dodatkowych danych do kontrolek formularza KREDYTY z ostatniej linii pliku
                PB_KwotaKredytu.Text = PB_TabelaWczytanychDanych[PB_Wiersze - 1, 0];
                PB_OkresSpłaty.Text = PB_TabelaWczytanychDanych[PB_Wiersze - 1, 1];
                PB_StopaProcentowa.SelectedText = PB_TabelaWczytanychDanych[PB_Wiersze - 1, 2];
                PB_KońcoweZadłużenie.Text = PB_TabelaWczytanychDanych[PB_Wiersze - 1, 3];


                // wczytanie danych z pozostałych wierszy do kontrolki DGV PB_Tabela
                for (int PB_r = 1; PB_r < PB_Wiersze - 1; PB_r++)
                {
                    PB_Tabela.Rows.Add();
                    for (int PB_k = 0; PB_k < PB_Kolumny; PB_k++)
                    {
                        PB_Tabela.Rows[PB_r - 1].Cells[PB_k].Value = PB_TabelaWczytanychDanych[PB_r, PB_k];
                    }
                }

                // odsłonięcie kontrolki PB_Tabela na 2 zakładce
                PB_Tabela.Visible = true;
                // przejście do 2 zakłądki formularza KREDYTY
                this.tabControl1.SelectedTab = PB_ZakładkaTabelaryczneRozliczenieKredytu;
            }
        }



        private void zapiszRozliczenieDoPlikuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // wywołanie okienka systemowego zapisu pliku
            SaveFileDialog PB_ZapiszPlik = new SaveFileDialog();

            PB_ZapiszPlik.Filter = "csv files (*.csv)|*.csv";           // zdefiowanie filtra systemowego dla plików typu .csv
            PB_ZapiszPlik.Title = "Zapis do pliku w formacie .csv";     // ustawienie nagłówka okna systemowego

            // wyświetli ostrzeżenie dla użytkownika, jeśli zdefiniowana ścieżka nie istnieje
            PB_ZapiszPlik.CheckPathExists = true;
            // określenie domyślnej ścieżki zapisu pliku
            PB_ZapiszPlik.InitialDirectory = Application.StartupPath;

            PB_ZapiszPlik.ShowDialog();


            // jeśli zdefiniowana nazwa pliku nie jest pusta, otwiera go do zapisu
            if (PB_ZapiszPlik.FileName != "")
            {
                StreamWriter PB_Zapisuję = new StreamWriter(PB_ZapiszPlik.FileName);

                // tworzę plik tekstowy z danymi rozdzielanymi (.)
                string PB_Tymczasowy = string.Empty;

                // dodanie wiersza nagłówkowego z kontrolki DGV PB_Tabela
                foreach (DataGridViewColumn column in PB_Tabela.Columns)
                {
                    PB_Tymczasowy += column.HeaderText + '.';
                }

                // dodanie nowej linii
                PB_Tymczasowy += "\r\n";

                // dodanie do pliku danych z kolejnych rzędów
                foreach (DataGridViewRow row in PB_Tabela.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        PB_Tymczasowy += cell.Value.ToString() + '.';
                    }
                    // odanie kolejnej linii
                    PB_Tymczasowy += "\r\n";
                }
                
                // przeniesienie do pliku danych wsadowych do obliczenia kredytu
                PB_Tymczasowy += PB_KwotaKredytu.Text + '.';
                PB_Tymczasowy += PB_OkresSpłaty.Text + '.';
                PB_Tymczasowy += PB_StopaProcentowa.SelectedItem.ToString() + '.';
                PB_Tymczasowy += PB_KońcoweZadłużenie.Text + '.';

                // zapis danych do pliku dyskowego
                PB_Zapisuję.Write(PB_Tymczasowy);

                // zamknięcie procedury zapisu do pliku
                PB_Zapisuję.Close();
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