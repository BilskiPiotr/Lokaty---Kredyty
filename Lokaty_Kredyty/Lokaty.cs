using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Lokaty_Kredyty
{
    public partial class Lokaty : Form
    {
        public Lokaty()
        {
            InitializeComponent();
        }


        // sprawdzenie i wczytanie do zmiennych danych wejściowych
        bool PobierzDaneWejściowe(out float WartośćLokaty, out float OprocentowanieLokaty, out int OkresLokaty)
        {
            // ustawienie domyślnych wartości dla danych wejściowych do porównania z errorProvider1
            WartośćLokaty = -1.0F;
            OprocentowanieLokaty = 0.0F;
            OkresLokaty = 0;


            // sprawdzenie czy zostało coś wpisane do okienka Kwota Lokaty
            if (string.IsNullOrEmpty(KwotaLokaty.Text))
            {
                // klient nie wpisał kwoty - zgłoszenie błędu 
                errorProvider1.SetError(KwotaLokaty,
                    "ERROR: musisz podać wysokość lokaty K");
                return false;
            }

            // wczytanie wysokości lokaty ze sprawdzeniem poprawności zapisu 
            if (!float.TryParse(KwotaLokaty.Text, out WartośćLokaty))
            {
                // do wprowadzenia kwoty użyto jakims cudem niedozwolonego znaku - zgłoszenie błędu
                errorProvider1.SetError(KwotaLokaty,
                    "ERROR: w zapisie kwoty użyto niedozwolonego znaku");
                return false;
                // w innym wypadku wczytanie kwoty do zmiennej WartośćLokaty
            }

            // sprawdzenie, czy klient wybrał stopę procentową
            if (Nud_Digit1.Value == 0 && Nud_Digit2.Value == 0 && Nud_Digit3.Value == 0)
            {
                // jesli nie wybrał - zgłoszenie błędu
                errorProvider1.SetError(Lb_Procent,
                    "ERROR: musisz wybrać stopę procentową!");
                return false;
            }
            else
            {
                OprocentowanieLokaty = (float)((Nud_Digit1.Value + Nud_Digit2.Value / 10 + Nud_Digit3.Value / 100))/100;
                errorProvider1.Dispose();
            }


            // sprawdzenie, czy wpisano cokolwiek do kontrolki określającej okres lokaty
            if (string.IsNullOrEmpty(CzasNaliczaniaLokaty.Text))
            {
                // jeśli nic nie wpisano - zapalenie kontrolki błędu 
                errorProvider1.SetError(CzasNaliczaniaLokaty,
                    "ERROR: musisz podać liczbę lat lokaty");
                return false;
            }

            // wczytanie liczby lat lokaty ze sprawdzeniem poprawności zapisu 
            if (!int.TryParse(CzasNaliczaniaLokaty.Text, out OkresLokaty))
            {
                // jesli jakimś cudem użyto niepoprawnego znaku - zgłoszenie błedu
                errorProvider1.SetError(CzasNaliczaniaLokaty,
                    "ERROR: wystąpił niedozwolony znak w zapisie liczby lat lokaty");
                return false;
                // w innymwypadku wczytanie liczby lat lokaty do zmiennej OkresLokaty
            }
            return true;
        }


        private void PrzejdźDoKredyty_Click(object sender, EventArgs e)
        {
            // ukrycie formularza Lokaty
            this.Hide();
            // odszukaniew kolekcji aktywnych formularzy formularza Kredyty
            foreach (Form Formularz in Application.OpenForms)
            {
                if (Formularz.Name == "Kredyty")
                {
                    // odsłonięcie formularza Kredyty
                    Formularz.Show();
                    // zakończenie obsługi zdarzenia click przycisku
                    return;
                }
            }
            // utworzenie egzemplarza formularza Kredyty
            Kredyty EgzKredyty = new Kredyty();
            // wyświetlenie pormularza Kredyty
            EgzKredyty.Show();
        }

        // metoda zamykająca program jeśli użytkownik wybieże z menu opcję "Exit"
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // ograniczenie wprowadzania danych do pola Kwota Lokaty do samych cyfr i znaku BackSpace
        private void KwotaLokaty_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8;
        }

        // ograniczenie wprowadzania danych do pola OkresLokaty do samych cyfr i znaku BackSpace
        private void OkresLokaty_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8;
        }

        // przeliczenie stanu konta na koniec okresu lokaty
        private void ObliczStanKonta_Click(object sender, EventArgs e)
        {
            // deklaracje lokalne
            float PrzyszłyStanKonta;
            string formater;

            // zgaszenie zapalonej kontrolki błędu
            errorProvider1.Clear();
            
            // pobranie sprawdznych danych wejściowych
            if (!PobierzDaneWejściowe(out float WartośćLokaty, out float OprocentowanieLokaty, out int OkresLokaty))
            return ;

            // obliczenie przyszłego stanu konta
            PrzyszłyStanKonta = WartośćLokaty * (float)Math.Pow(1 + OprocentowanieLokaty, OkresLokaty);

            formater = "00.00";

            // wizualizacja wyniku obliczeń w kontrolce PrzyszlyStanKonta
            PSK_wyświetl.Text = PrzyszłyStanKonta.ToString(formater) + " zł";
        }

        // ustawianie koloru linii wykresu
        private void _WybierzKolorLiniiWykresu_Click(object sender, EventArgs e)
        {
            ColorDialog WybranyKolorLinii = new ColorDialog();
            if (WybranyKolorLinii.ShowDialog() == DialogResult.OK)
            {
                KolorLiniiWykresu.BackColor = WybranyKolorLinii.Color;
                WykresLokata.Series[0].Color = WybranyKolorLinii.Color;

            }
        }

        // ustawianie koloru tła wykresu
        private void WybierzKolorTłaWykresu_Click(object sender, EventArgs e)
        {
            ColorDialog WybranyKolorTła = new ColorDialog();
            if (WybranyKolorTła.ShowDialog() == DialogResult.OK)
            {
                KolorTłaWykresu.BackColor = WybranyKolorTła.Color;
                WykresLokata.BackColor = WybranyKolorTła.Color;
            }
        }

        // Rrozliczenie zadeklarowanej lokaty
        void RozliczLokatę(ref float[,] TablicaRozliczeniaLokaty, float WartośćLokaty, float OprocentowanieLokaty, int OkresLokaty)
        {
            // deklaracje lokalne
            float StanNaPoczątkuOkresu;
            float StanNaKońcuOkresu;
            float OdsetkiZaDanyOkres;

            // ustalenie początkowego stanu obliczeń dla terminu zerowego
            StanNaPoczątkuOkresu = 0.0F;
            OdsetkiZaDanyOkres = 0.0F;
            StanNaKońcuOkresu = WartośćLokaty;

            // wpisanie rozliczenia lokaty w tablicy TablicaRozliczeniaLokaty
            for (int i = 0; i < TablicaRozliczeniaLokaty.GetLength(0); i++)
            {
                TablicaRozliczeniaLokaty[i, 0] = StanNaPoczątkuOkresu;
                TablicaRozliczeniaLokaty[i, 1] = OdsetkiZaDanyOkres;
                TablicaRozliczeniaLokaty[i, 2] = StanNaKońcuOkresu;

                // ustawienie wartości początkowych zmiennych
                StanNaPoczątkuOkresu = StanNaKońcuOkresu;
                OdsetkiZaDanyOkres = OprocentowanieLokaty * StanNaPoczątkuOkresu;
                StanNaKońcuOkresu = StanNaPoczątkuOkresu + OdsetkiZaDanyOkres;
            }
        }

        // Metoda opisująca użycie przycisku krzyżyk do zamknięcia programu
        private void Lokaty_FormClosing(object sender, FormClosingEventArgs e)
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

        private void TabelaryczneRozliczenieLokaty_Click(object sender, EventArgs e)
        {
            // czyszczenie kontrolki DataGridView z wierszy
            while (TabelarycznaPrezentacjaLokaty.Rows.Count > 0)
            {
                TabelarycznaPrezentacjaLokaty.Rows.RemoveAt(0);
            }

                // pobranie danych wejściowych dla obsługi zdarzenia Click
                if (!PobierzDaneWejściowe(out float WartośćLokaty, out float OprocentowanieLokaty, out int OkresLokaty))
                    return;

                // wykryto błąd przy pobieraniu danych i obsługa zdarzenia Click musi zostać przerwana

                // deklaracja tablicy rozliczenia lokaty
                float[,] TablicaRozliczeniaLokaty = new float[OkresLokaty + 1, 3];

                // wywołanie metody wypełniającej TablicaRozliczeniaLokaty
                RozliczLokatę(ref TablicaRozliczeniaLokaty, WartośćLokaty, OprocentowanieLokaty, OkresLokaty);

                // odsłonięcie kontrolki DataGridView
                TabelarycznaPrezentacjaLokaty.Visible = true;

                // ukrycie kontrolki Chart
                WykresLokata.Visible = false;

                // "przepisanie" (utworzenie) tablicy rozliczenia lokaty
                for (int i = 0; i < TablicaRozliczeniaLokaty.GetLength(0); i++)
                {
                    // dodanie nowego wiersza do kolekcji wierszy kontrolki DataGridView
                    TabelarycznaPrezentacjaLokaty.Rows.Add();

                    // wypełnienie dodanego wiersza danymi pobranymi z tablicy TablicaRozliczeńLokaty
                    TabelarycznaPrezentacjaLokaty.Rows[i].Cells[0].Value = i;
                    TabelarycznaPrezentacjaLokaty.Rows[i].Cells[1].Value = string.Format("{0,8:F2}", TablicaRozliczeniaLokaty[i, 0]);
                    TabelarycznaPrezentacjaLokaty.Rows[i].Cells[2].Value = string.Format("{0,8:F2}", TablicaRozliczeniaLokaty[i, 1]);
                    TabelarycznaPrezentacjaLokaty.Rows[i].Cells[3].Value = string.Format("{0,8:F2}", TablicaRozliczeniaLokaty[i, 2]);

                }
        }

        private void StylLiniiWykresu_SelectedIndexChanged(object sender, EventArgs e)
        {
            // przypisanie zmienej tymczasowej wartości wybranego indexu kontrolki ComboBox
            int SelectedIndex = StylLiniiWykresu.SelectedIndex;
            Object SelectedItem = StylLiniiWykresu.SelectedItem;

            // określenie typu linii w zależności od wyboru użytkownika
            switch (SelectedIndex)
            {
                case 0:
                    WykresLokata.Series[0].BorderDashStyle = ChartDashStyle.Dash;
                    break;
                case 1:
                    WykresLokata.Series[0].BorderDashStyle = ChartDashStyle.DashDot;
                    break;
                case 2:
                    WykresLokata.Series[0].BorderDashStyle = ChartDashStyle.DashDotDot;
                    break;
                case 3:
                    WykresLokata.Series[0].BorderDashStyle = ChartDashStyle.Dot;
                    break;
                case 4:
                    WykresLokata.Series[0].BorderDashStyle = ChartDashStyle.Solid;
                    break;
            }

        }


        private void GraficznaPrezentacjaLokaty_Click(object sender, EventArgs e)
        {
            // Zresetowanie obszaru wykresu do wartości default (skasowanie istniejących w pamięci)
            WykresLokata.ResetAutoValues();
            WykresLokata.Titles.Clear();
            WykresLokata.Series.Clear();

            // pobranie danych wejściowych dla obsługi zdarzenia Click
            if (!PobierzDaneWejściowe(out float WartośćLokaty, out float OprocentowanieLokaty, out int OkresLokaty))
            // w przypadku wykrycia jakiegoś błędu obsługa zostaje przerwana
            return;

            // deklaracja tablicy rozliczenia lokaty
            float[,] TablicaRozliczeniaLokaty = new float[OkresLokaty + 1, 3];

            // wywołanie metody wypełniającej TablicaRozliczeniaLokaty
            RozliczLokatę(ref TablicaRozliczeniaLokaty, WartośćLokaty, OprocentowanieLokaty, OkresLokaty);

            // odsłonięcie kontrolki Chart
            WykresLokata.Visible = true;

            // ukrycie kontrolki DataGridView
            TabelarycznaPrezentacjaLokaty.Visible = false;

            // dodanie nowej serii do wykresu
            WykresLokata.Series.Add("Seria 0");

            // zdefiniowanie tytułu wykresu
            WykresLokata.Titles.Add("Zmiana wartości stanu konta w zadeklarowanym okresie lokaty");

            // ustawienie parametrów wykresu
            WykresLokata.BackColor = Color.Aqua;                             // kolor tła wykresu

            // formatowanie linii wykresu
            WykresLokata.Series[0].Name = "Stan Konta";                      // legenda
            WykresLokata.Series[0].ChartType = SeriesChartType.Line;         // wykres liniowy
            WykresLokata.Series[0].BorderColor = Color.Red;                  // kolor linii
            WykresLokata.Series[0].BorderDashStyle = ChartDashStyle.Solid;   // linia ciągła
            WykresLokata.Series[0].BorderWidth = 1;                          // grubość linii 1 piksel

            // deklaracja pomocniczych tablic dla określenia współrzędnych punktów wykresu
            int[] NumeryOkresówLokaty = new int[TablicaRozliczeniaLokaty.GetLength(0)];
            for (int i = 0; i < TablicaRozliczeniaLokaty.GetLength(0); i++)
                NumeryOkresówLokaty[i] = i;

            float[] StanKontaKN = new float[TablicaRozliczeniaLokaty.GetLength(0)];
            for (int i = 0; i < TablicaRozliczeniaLokaty.GetLength(0); i++)
                StanKontaKN[i] = TablicaRozliczeniaLokaty[i, 2];

            // podpięcie tablic opisujących punkty na wykresie do kontrolki Chart
            WykresLokata.Series[0].Points.DataBindXY(NumeryOkresówLokaty, StanKontaKN);
        }

        private void RodzajWykresu_SelectedIndexChanged(object sender, EventArgs e)
        {
            // przypisanie zmienej tymczasowej wartości wybranego indexu kontrolki ComboBox
            int SelectedIndex1 = RodzajWykresu.SelectedIndex;
            Object SelectedItem1 = RodzajWykresu.SelectedItem;

            // określenie typu wykresu w zależności od wyboru użytkownika przy użyciu ComboBox
            switch (SelectedIndex1)
            {
                case 0:
                    WykresLokata.Series[0].ChartType = SeriesChartType.Line;
                    break;
                case 1:
                    WykresLokata.Series[0].ChartType = SeriesChartType.Bar;
                    break;
                case 2:
                    WykresLokata.Series[0].ChartType = SeriesChartType.Bubble;
                    break;
                case 3:
                    WykresLokata.Series[0].ChartType = SeriesChartType.Column;
                    break;
                case 4:
                    WykresLokata.Series[0].ChartType = SeriesChartType.Radar;
                    break;
                case 5:
                    WykresLokata.Series[0].ChartType = SeriesChartType.RangeBar;
                    break;
                case 6:
                    WykresLokata.Series[0].ChartType = SeriesChartType.StepLine;
                    break;
            }
        }

        // określenie typu wykresu przy użyciu elementów Menu
        private void LineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WykresLokata.Series[0].ChartType = SeriesChartType.Line;
        }

        private void BarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WykresLokata.Series[0].ChartType = SeriesChartType.Bar;
        }

        private void BubbleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WykresLokata.Series[0].ChartType = SeriesChartType.Bubble;
        }

        private void ColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WykresLokata.Series[0].ChartType = SeriesChartType.Column;
        }

        private void RadarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WykresLokata.Series[0].ChartType = SeriesChartType.Radar;
        }

        private void RangeBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WykresLokata.Series[0].ChartType = SeriesChartType.RangeBar;
        }

        private void StepLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WykresLokata.Series[0].ChartType = SeriesChartType.StepLine;
        }


        // wywołanie metody obsługi sówaka  w celu określenia krokowego grubości linii wykresu
        private void GrubośćLiniiWykresu_Scroll(object sender, EventArgs e)
        {
            int GruboscLinii;                                // wywołanie zmienej pomocniczej
            GruboscLinii = GrubośćLiniiWykresu.Value;     // przypisanie wyboru użytkownika do zmienej pomocniczej

            // przepisanie wyboru urzytkownika do definicji grubości linii wykresu w kontrolce chart
            if (GruboscLinii == 1)
            {
                WykresLokata.Series[0].BorderWidth = 1;
            }
            else
                if (GruboscLinii == 2)
                {
                    WykresLokata.Series[0].BorderWidth = 2;
                }
                else
                    if (GruboscLinii == 3)
                    {
                        WykresLokata.Series[0].BorderWidth = 3;
                    }
                    else
                        if (GruboscLinii == 4)
                        {
                            WykresLokata.Series[0].BorderWidth = 4;
                        }
                        else
                            if (GruboscLinii == 5)
                            {
                                WykresLokata.Series[0].BorderWidth = 5;
                            }
        }

        private void Lokaty_Load(object sender, EventArgs e)
        {
            Nud_Digit1.BringToFront();
            Nud_Digit2.BringToFront();
            Nud_Digit3.BringToFront();
            Lb_Separator.SendToBack();
            //FillOprocentowanie();
        }


        private void FillOprocentowanie()
        {
            //Oprocentowanie.DisplayMember = "Text";
            //Oprocentowanie.ValueMember = "Value";

            //List<object> items = new List<object>();

            //items.Add(new { Text = "report A", Value = "reportA" });
            //items.Add(new { Text = "report A", Value = "reportA" });
            //items.Add(new { Text = "report A", Value = "reportA" });
            //items.Add(new { Text = "report A", Value = "reportA" });
            //items.Add(new { Text = "report A", Value = "reportA" });
            //items.Add(new { Text = "report A", Value = "reportA" });
            //items.Add(new { Text = "report A", Value = "reportA" });
            //items.Add(new { Text = "report A", Value = "reportA" });
            //items.Add(new { Text = "report A", Value = "reportA" });
            //items.Add(new { Text = "report A", Value = "reportA" });


            //Oprocentowanie.DataSource = items;
        }
    }
}
