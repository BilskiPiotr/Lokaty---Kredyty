using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Lokaty_Kredyty
{
    public partial class PB_Lokaty : Form
    {
        public PB_Lokaty()
        {
            InitializeComponent();
        }


        // sprawdzenie i wczytanie do zmiennych danych wejściowych
        bool PB_PobierzDaneWejściowe(out float PB_WartośćLokaty, out float PB_OprocentowanieLokaty, out int PB_OkresLokaty)
        {
            // ustawienie domyślnych wartości dla danych wejściowych do porównania z errorProvider1
            PB_WartośćLokaty = -1.0F;
            PB_OprocentowanieLokaty = 0.0F;
            PB_OkresLokaty = 0;


            // sprawdzenie czy zostało coś wpisane do okienka Kwota Lokaty
            if (string.IsNullOrEmpty(PB_KwotaLokaty.Text))
            {
                // klient nie wpisał kwoty - zgłoszenie błędu 
                errorProvider1.SetError(PB_KwotaLokaty,
                    "ERROR: musisz podać wysokość lokaty K");
                return false;
            }

            // wczytanie wysokości lokaty ze sprawdzeniem poprawności zapisu 
            if (!float.TryParse(PB_KwotaLokaty.Text, out PB_WartośćLokaty))
            {
                // do wprowadzenia kwoty użyto jakims cudem niedozwolonego znaku - zgłoszenie błędu
                errorProvider1.SetError(PB_KwotaLokaty,
                    "ERROR: w zapisie kwoty użyto niedozwolonego znaku");
                return false;
                // w innym wypadku wczytanie kwoty do zmiennej PB_WartośćLokaty
            }

            // sprawdzenie, czy klient wybrał stopę procentową
            if (PB_Oprocentowanie.SelectedIndex < 0)
            {
                // jesli nie wybrał - zgłoszenie błędu
                errorProvider1.SetError(PB_Oprocentowanie,
                    "ERROR: musisz wybrać stopę procentową!");
                return false;
            }

            // pobranie wartości stopy procentowej i wprowadzenie jej do zmiennej PB_OprocentowanieLokaty
            if (!float.TryParse(PB_Oprocentowanie.SelectedItem.ToString(), out PB_OprocentowanieLokaty))
            {
                // jesli jakiimś cudem w strumieniu danych pojawi sie niedozwolony znak - zgłoszenie błędu
                errorProvider1.SetError(PB_Oprocentowanie,
                    "ERROR: niedozwolony znak w zapisie stopy procentowej!");
                return false;
                // w innym wypadku wczytanie oprocentowanie do zmiennej PB_OprocentowanieLokaty
            }

            // sprawdzenie, czy wpisano cokolwiek do kontrolki określającej okres lokaty
            if (string.IsNullOrEmpty(PB_CzasNaliczaniaLokaty.Text))
            {
                // jeśli nic nie wpisano - zapalenie kontrolki błędu 
                errorProvider1.SetError(PB_CzasNaliczaniaLokaty,
                    "ERROR: musisz podać liczbę lat lokaty");
                return false;
            }

            // wczytanie liczby lat lokaty ze sprawdzeniem poprawności zapisu 
            if (!int.TryParse(PB_CzasNaliczaniaLokaty.Text, out PB_OkresLokaty))
            {
                // jesli jakimś cudem użyto niepoprawnego znaku - zgłoszenie błedu
                errorProvider1.SetError(PB_CzasNaliczaniaLokaty,
                    "ERROR: wystąpił niedozwolony znak w zapisie liczby lat lokaty");
                return false;
                // w innymwypadku wczytanie liczby lat lokaty do zmiennej PB_OkresLokaty
            }
            return true;
        }


        private void PB_PrzejdźDoKredyty_Click(object sender, EventArgs e)
        {
            // ukrycie formularza Lokaty
            this.Hide();
            // odszukaniew kolekcji aktywnych formularzy formularza PB_Kredyty
            foreach (Form Formularz in Application.OpenForms)
            {
                if (Formularz.Name == "PB_Kredyty")
                {
                    // odsłonięcie formularza PB_Kredyty
                    Formularz.Show();
                    // zakończenie obsługi zdarzenia click przycisku
                    return;
                }
            }
            // utworzenie egzemplarza formularza PB_Kredyty
            PB_Kredyty EgzKredyty = new PB_Kredyty();
            // wyświetlenie pormularza PB_Kredyty
            EgzKredyty.Show();
        }

        // metoda zamykająca program jeśli użytkownik wybieże z menu opcję "Exit"
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // ograniczenie wprowadzania danych do pola Kwota Lokaty do samych cyfr i znaku BackSpace
        private void PB_KwotaLokaty_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8;
        }

        // ograniczenie wprowadzania danych do pola OkresLokaty do samych cyfr i znaku BackSpace
        private void PB_OkresLokaty_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8;
        }

        // przeliczenie stanu konta na koniec okresu lokaty
        private void PB_ObliczStanKonta_Click(object sender, EventArgs e)
        {
            // deklaracje lokalne
            float PB_WartośćLokaty, PB_OprocentowanieLokaty, PB_PrzyszłyStanKonta;
            int PB_OkresLokaty;
            string PB_formater;

            // zgaszenie zapalonej kontrolki błędu
            errorProvider1.Clear();
            
            // pobranie sprawdznych danych wejściowych
            if (!PB_PobierzDaneWejściowe(out PB_WartośćLokaty, out PB_OprocentowanieLokaty, out PB_OkresLokaty))
            return ;

            // obliczenie przyszłego stanu konta
            PB_PrzyszłyStanKonta = PB_WartośćLokaty * (float)Math.Pow(1 + PB_OprocentowanieLokaty, PB_OkresLokaty);

            PB_formater = "00.00";

            // wizualizacja wyniku obliczeń w kontrolce PB_PrzyszlyStanKonta
            PB_PSK_wyświetl.Text = PB_PrzyszłyStanKonta.ToString(PB_formater) + " zł";
        }

        // ustawianie koloru linii wykresu
        private void _PB_WybierzKolorLiniiWykresu_Click(object sender, EventArgs e)
        {
            ColorDialog PB_WybranyKolorLinii = new ColorDialog();
            if (PB_WybranyKolorLinii.ShowDialog() == DialogResult.OK)
            {
                PB_KolorLiniiWykresu.BackColor = PB_WybranyKolorLinii.Color;
                PB_WykresLokata.Series[0].Color = PB_WybranyKolorLinii.Color;

            }
        }

        // ustawianie koloru tła wykresu
        private void PB_WybierzKolorTłaWykresu_Click(object sender, EventArgs e)
        {
            ColorDialog PB_WybranyKolorTła = new ColorDialog();
            if (PB_WybranyKolorTła.ShowDialog() == DialogResult.OK)
            {
                PB_KolorTłaWykresu.BackColor = PB_WybranyKolorTła.Color;
                PB_WykresLokata.BackColor = PB_WybranyKolorTła.Color;
            }
        }

        // Rrozliczenie zadeklarowanej lokaty
        void RozliczLokatę(ref float[,] TablicaRozliczeniaLokaty, float PB_WartośćLokaty, float PB_OprocentowanieLokaty, int PB_OkresLokaty)
        {
            // deklaracje lokalne
            float StanNaPoczątkuOkresu;
            float StanNaKońcuOkresu;
            float OdsetkiZaDanyOkres;

            // ustalenie początkowego stanu obliczeń dla terminu zerowego
            StanNaPoczątkuOkresu = 0.0F;
            OdsetkiZaDanyOkres = 0.0F;
            StanNaKońcuOkresu = PB_WartośćLokaty;

            // wpisanie rozliczenia lokaty w tablicy TablicaRozliczeniaLokaty
            for (int PB_i = 0; PB_i < TablicaRozliczeniaLokaty.GetLength(0); PB_i++)
            {
                TablicaRozliczeniaLokaty[PB_i, 0] = StanNaPoczątkuOkresu;
                TablicaRozliczeniaLokaty[PB_i, 1] = OdsetkiZaDanyOkres;
                TablicaRozliczeniaLokaty[PB_i, 2] = StanNaKońcuOkresu;

                // ustawienie wartości początkowych zmiennych
                StanNaPoczątkuOkresu = StanNaKońcuOkresu;
                OdsetkiZaDanyOkres = PB_OprocentowanieLokaty * StanNaPoczątkuOkresu;
                StanNaKońcuOkresu = StanNaPoczątkuOkresu + OdsetkiZaDanyOkres;
            }
        }

        // Metoda opisująca użycie przycisku krzyżyk do zamknięcia programu
        private void PB_Lokaty_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // wywołanie okienka MesageBox z komunikatem
                if (MessageBox.Show("Czy Napewno chcesz opuścić Program",
                               "Zakończyć Program?",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes)
                    Environment.Exit(1);
                else
                    e.Cancel = true; // rezygnacja z zamknięcia programu
            }
        }

        private void PB_TabelaryczneRozliczenieLokaty_Click(object sender, EventArgs e)
        {
            // czyszczenie kontrolki DataGridView z wierszy
            while (PB_TabelarycznaPrezentacjaLokaty.Rows.Count > 0)
            {
                PB_TabelarycznaPrezentacjaLokaty.Rows.RemoveAt(0);
            }


            {
                // deklaracje lokalne
                float PB_WartośćLokaty;
                float PB_OprocentowanieLokaty;
                int PB_OkresLokaty;

                // pobranie danych wejściowych dla obsługi zdarzenia Click
                if (!PB_PobierzDaneWejściowe(out PB_WartośćLokaty, out PB_OprocentowanieLokaty, out PB_OkresLokaty))
                    return;

                // wykryto błąd przy pobieraniu danych i obsługa zdarzenia Click musi zostać przerwana

                // deklaracja tablicy rozliczenia lokaty
                float[,] TablicaRozliczeniaLokaty = new float[PB_OkresLokaty + 1, 3];

                // wywołanie metody wypełniającej TablicaRozliczeniaLokaty
                RozliczLokatę(ref TablicaRozliczeniaLokaty, PB_WartośćLokaty, PB_OprocentowanieLokaty, PB_OkresLokaty);

                // odsłonięcie kontrolki DataGridView
                PB_TabelarycznaPrezentacjaLokaty.Visible = true;

                // ukrycie kontrolki Chart
                PB_WykresLokata.Visible = false;

                // "przepisanie" (utworzenie) tablicy rozliczenia lokaty
                for (int PB_i = 0; PB_i < TablicaRozliczeniaLokaty.GetLength(0); PB_i++)
                {
                    // dodanie nowego wiersza do kolekcji wierszy kontrolki DataGridView
                    PB_TabelarycznaPrezentacjaLokaty.Rows.Add();

                    // wypełnienie dodanego wiersza danymi pobranymi z tablicy TablicaRozliczeńLokaty
                    PB_TabelarycznaPrezentacjaLokaty.Rows[PB_i].Cells[0].Value = PB_i;
                    PB_TabelarycznaPrezentacjaLokaty.Rows[PB_i].Cells[1].Value = string.Format("{0,8:F2}", TablicaRozliczeniaLokaty[PB_i, 0]);
                    PB_TabelarycznaPrezentacjaLokaty.Rows[PB_i].Cells[2].Value = string.Format("{0,8:F2}", TablicaRozliczeniaLokaty[PB_i, 1]);
                    PB_TabelarycznaPrezentacjaLokaty.Rows[PB_i].Cells[3].Value = string.Format("{0,8:F2}", TablicaRozliczeniaLokaty[PB_i, 2]);

                }
            }
        }

        private void PB_StylLiniiWykresu_SelectedIndexChanged(object sender, EventArgs e)
        {
            // przypisanie zmienej tymczasowej wartości wybranego indexu kontrolki ComboBox
            int PB_SelectedIndex = PB_StylLiniiWykresu.SelectedIndex;
            Object PB_SelectedItem = PB_StylLiniiWykresu.SelectedItem;

            // określenie typu linii w zależności od wyboru użytkownika
            switch (PB_SelectedIndex)
            {
                case 0:
                    PB_WykresLokata.Series[0].BorderDashStyle = ChartDashStyle.Dash;
                    break;
                case 1:
                    PB_WykresLokata.Series[0].BorderDashStyle = ChartDashStyle.DashDot;
                    break;
                case 2:
                    PB_WykresLokata.Series[0].BorderDashStyle = ChartDashStyle.DashDotDot;
                    break;
                case 3:
                    PB_WykresLokata.Series[0].BorderDashStyle = ChartDashStyle.Dot;
                    break;
                case 4:
                    PB_WykresLokata.Series[0].BorderDashStyle = ChartDashStyle.Solid;
                    break;
            }

        }


        private void PB_GraficznaPrezentacjaLokaty_Click(object sender, EventArgs e)
        {
            // Zresetowanie obszaru wykresu do wartości default (skasowanie istniejących w pamięci)
            PB_WykresLokata.ResetAutoValues();
            PB_WykresLokata.Titles.Clear();
            PB_WykresLokata.Series.Clear();

            // deklaracje lokalne
            float PB_WartośćLokaty;
            float PB_OprocentowanieLokaty;
            int PB_OkresLokaty;

            // pobranie danych wejściowych dla obsługi zdarzenia Click
            if (!PB_PobierzDaneWejściowe(out PB_WartośćLokaty, out PB_OprocentowanieLokaty, out PB_OkresLokaty))
            // w przypadku wykrycia jakiegoś błędu obsługa zostaje przerwana
            return;

            // deklaracja tablicy rozliczenia lokaty
            float[,] TablicaRozliczeniaLokaty = new float[PB_OkresLokaty + 1, 3];

            // wywołanie metody wypełniającej TablicaRozliczeniaLokaty
            RozliczLokatę(ref TablicaRozliczeniaLokaty, PB_WartośćLokaty, PB_OprocentowanieLokaty, PB_OkresLokaty);

            // odsłonięcie kontrolki Chart
            PB_WykresLokata.Visible = true;

            // ukrycie kontrolki DataGridView
            PB_TabelarycznaPrezentacjaLokaty.Visible = false;

            // dodanie nowej serii do wykresu
            PB_WykresLokata.Series.Add("Seria 0");

            // zdefiniowanie tytułu wykresu
            PB_WykresLokata.Titles.Add("Zmiana wartości stanu konta w zadeklarowanym okresie lokaty");

            // ustawienie parametrów wykresu
            PB_WykresLokata.BackColor = Color.Aqua;                             // kolor tła wykresu

            // formatowanie linii wykresu
            PB_WykresLokata.Series[0].Name = "Stan Konta";                      // legenda
            PB_WykresLokata.Series[0].ChartType = SeriesChartType.Line;         // wykres liniowy
            PB_WykresLokata.Series[0].BorderColor = Color.Red;                  // kolor linii
            PB_WykresLokata.Series[0].BorderDashStyle = ChartDashStyle.Solid;   // linia ciągła
            PB_WykresLokata.Series[0].BorderWidth = 1;                          // grubość linii 1 piksel

            // deklaracja pomocniczych tablic dla określenia współrzędnych punktów wykresu
            int[] NumeryOkresówLokaty = new int[TablicaRozliczeniaLokaty.GetLength(0)];
            for (int i = 0; i < TablicaRozliczeniaLokaty.GetLength(0); i++)
                NumeryOkresówLokaty[i] = i;

            float[] StanKontaKN = new float[TablicaRozliczeniaLokaty.GetLength(0)];
            for (int i = 0; i < TablicaRozliczeniaLokaty.GetLength(0); i++)
                StanKontaKN[i] = TablicaRozliczeniaLokaty[i, 2];

            // podpięcie tablic opisujących punkty na wykresie do kontrolki Chart
            PB_WykresLokata.Series[0].Points.DataBindXY(NumeryOkresówLokaty, StanKontaKN);
        }

        private void PB_RodzajWykresu_SelectedIndexChanged(object sender, EventArgs e)
        {
            // przypisanie zmienej tymczasowej wartości wybranego indexu kontrolki ComboBox
            int PB_SelectedIndex1 = PB_RodzajWykresu.SelectedIndex;
            Object PB_SelectedItem1 = PB_RodzajWykresu.SelectedItem;

            // określenie typu wykresu w zależności od wyboru użytkownika przy użyciu ComboBox
            switch (PB_SelectedIndex1)
            {
                case 0:
                    PB_WykresLokata.Series[0].ChartType = SeriesChartType.Line;
                    break;
                case 1:
                    PB_WykresLokata.Series[0].ChartType = SeriesChartType.Bar;
                    break;
                case 2:
                    PB_WykresLokata.Series[0].ChartType = SeriesChartType.Bubble;
                    break;
                case 3:
                    PB_WykresLokata.Series[0].ChartType = SeriesChartType.Column;
                    break;
                case 4:
                    PB_WykresLokata.Series[0].ChartType = SeriesChartType.Radar;
                    break;
                case 5:
                    PB_WykresLokata.Series[0].ChartType = SeriesChartType.RangeBar;
                    break;
                case 6:
                    PB_WykresLokata.Series[0].ChartType = SeriesChartType.StepLine;
                    break;
            }
        }

        // określenie typu wykresu przy użyciu elementów Menu
        private void lineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PB_WykresLokata.Series[0].ChartType = SeriesChartType.Line;
        }

        private void barToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PB_WykresLokata.Series[0].ChartType = SeriesChartType.Bar;
        }

        private void bubbleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PB_WykresLokata.Series[0].ChartType = SeriesChartType.Bubble;
        }

        private void columnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PB_WykresLokata.Series[0].ChartType = SeriesChartType.Column;
        }

        private void radarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PB_WykresLokata.Series[0].ChartType = SeriesChartType.Radar;
        }

        private void rangeBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PB_WykresLokata.Series[0].ChartType = SeriesChartType.RangeBar;
        }

        private void stepLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PB_WykresLokata.Series[0].ChartType = SeriesChartType.StepLine;
        }


        // wywołanie metody obsługi sówaka  w celu określenia krokowego grubości linii wykresu
        private void PB_GrubośćLiniiWykresu_Scroll(object sender, EventArgs e)
        {
            int PB_GruboscLinii;                                // wywołanie zmienej pomocniczej
            PB_GruboscLinii = PB_GrubośćLiniiWykresu.Value;     // przypisanie wyboru użytkownika do zmienej pomocniczej

            // przepisanie wyboru urzytkownika do definicji grubości linii wykresu w kontrolce chart
            if (PB_GruboscLinii == 1)
            {
                PB_WykresLokata.Series[0].BorderWidth = 1;
            }
            else
                if (PB_GruboscLinii == 2)
                {
                    PB_WykresLokata.Series[0].BorderWidth = 2;
                }
                else
                    if (PB_GruboscLinii == 3)
                    {
                        PB_WykresLokata.Series[0].BorderWidth = 3;
                    }
                    else
                        if (PB_GruboscLinii == 4)
                        {
                            PB_WykresLokata.Series[0].BorderWidth = 4;
                        }
                        else
                            if (PB_GruboscLinii == 5)
                            {
                                PB_WykresLokata.Series[0].BorderWidth = 5;
                            }
        }
    }
}
