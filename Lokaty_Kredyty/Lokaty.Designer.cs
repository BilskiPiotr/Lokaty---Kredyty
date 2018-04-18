namespace Lokaty_Kredyty
{
    partial class Lokaty
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.MenuLokaty = new System.Windows.Forms.MenuStrip();
            this.LokatyFile = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LokatyZTW = new System.Windows.Forms.ToolStripMenuItem();
            this.lineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bubbleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.columnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.radarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rangeBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stepLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InfoNL = new System.Windows.Forms.Label();
            this.InfoKwota = new System.Windows.Forms.Label();
            this.KwotaLokaty = new System.Windows.Forms.TextBox();
            this.InfoOprocentowanie = new System.Windows.Forms.Label();
            this.InfoOkres = new System.Windows.Forms.Label();
            this.CzasNaliczaniaLokaty = new System.Windows.Forms.TextBox();
            this._WybierzKolorLiniiWykresu = new System.Windows.Forms.Button();
            this.InfoKolorLinii = new System.Windows.Forms.Label();
            this.KolorLiniiWykresu = new System.Windows.Forms.TextBox();
            this.WybierzKolorTłaWykresu = new System.Windows.Forms.Button();
            this.InfoKolorWykresu = new System.Windows.Forms.Label();
            this.KolorTłaWykresu = new System.Windows.Forms.TextBox();
            this.InfoGrubośćLinii = new System.Windows.Forms.Label();
            this.InfoStylLinii = new System.Windows.Forms.Label();
            this.StylLiniiWykresu = new System.Windows.Forms.ComboBox();
            this.InfoRodzajWykresu = new System.Windows.Forms.Label();
            this.RodzajWykresu = new System.Windows.Forms.ComboBox();
            this.InfoSK = new System.Windows.Forms.Label();
            this.PSK_wyświetl = new System.Windows.Forms.TextBox();
            this.ObliczStanKonta = new System.Windows.Forms.Button();
            this.TabelaryczneRozliczenieLokaty = new System.Windows.Forms.Button();
            this.GraficznaPrezentacjaLokaty = new System.Windows.Forms.Button();
            this.PrzejdźDoKredyty = new System.Windows.Forms.Button();
            this.Obrazek = new System.Windows.Forms.PictureBox();
            this.TabelarycznaPrezentacjaLokaty = new System.Windows.Forms.DataGridView();
            this.DGW_OkresLokaty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGWStanNaPoczątku = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGWOdsetki = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGWStanKontaNaKońcu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WykresLokata = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.GrubośćLiniiWykresu = new System.Windows.Forms.TrackBar();
            this.InfoSN = new System.Windows.Forms.Label();
            this.Nud_Digit1 = new System.Windows.Forms.NumericUpDown();
            this.Nud_Digit2 = new System.Windows.Forms.NumericUpDown();
            this.Nud_Digit3 = new System.Windows.Forms.NumericUpDown();
            this.Lb_Procent = new System.Windows.Forms.Label();
            this.Lb_Separator = new System.Windows.Forms.Label();
            this.MenuLokaty.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Obrazek)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabelarycznaPrezentacjaLokaty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WykresLokata)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrubośćLiniiWykresu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Nud_Digit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Nud_Digit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Nud_Digit3)).BeginInit();
            this.SuspendLayout();
            // 
            // MenuLokaty
            // 
            this.MenuLokaty.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LokatyFile,
            this.LokatyZTW});
            this.MenuLokaty.Location = new System.Drawing.Point(0, 0);
            this.MenuLokaty.Name = "MenuLokaty";
            this.MenuLokaty.Size = new System.Drawing.Size(784, 24);
            this.MenuLokaty.TabIndex = 0;
            this.MenuLokaty.Text = "menuStrip1";
            // 
            // LokatyFile
            // 
            this.LokatyFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.LokatyFile.Name = "LokatyFile";
            this.LokatyFile.Size = new System.Drawing.Size(37, 20);
            this.LokatyFile.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // LokatyZTW
            // 
            this.LokatyZTW.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lineToolStripMenuItem,
            this.barToolStripMenuItem,
            this.bubbleToolStripMenuItem,
            this.columnToolStripMenuItem,
            this.radarToolStripMenuItem,
            this.rangeBarToolStripMenuItem,
            this.stepLineToolStripMenuItem});
            this.LokatyZTW.Name = "LokatyZTW";
            this.LokatyZTW.Size = new System.Drawing.Size(132, 20);
            this.LokatyZTW.Text = "Zmiana typu wykresu";
            // 
            // lineToolStripMenuItem
            // 
            this.lineToolStripMenuItem.Name = "lineToolStripMenuItem";
            this.lineToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.lineToolStripMenuItem.Text = "Line";
            this.lineToolStripMenuItem.Click += new System.EventHandler(this.LineToolStripMenuItem_Click);
            // 
            // barToolStripMenuItem
            // 
            this.barToolStripMenuItem.Name = "barToolStripMenuItem";
            this.barToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.barToolStripMenuItem.Text = "Bar";
            this.barToolStripMenuItem.Click += new System.EventHandler(this.BarToolStripMenuItem_Click);
            // 
            // bubbleToolStripMenuItem
            // 
            this.bubbleToolStripMenuItem.Name = "bubbleToolStripMenuItem";
            this.bubbleToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.bubbleToolStripMenuItem.Text = "Bubble";
            this.bubbleToolStripMenuItem.Click += new System.EventHandler(this.BubbleToolStripMenuItem_Click);
            // 
            // columnToolStripMenuItem
            // 
            this.columnToolStripMenuItem.Name = "columnToolStripMenuItem";
            this.columnToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.columnToolStripMenuItem.Text = "Column";
            this.columnToolStripMenuItem.Click += new System.EventHandler(this.ColumnToolStripMenuItem_Click);
            // 
            // radarToolStripMenuItem
            // 
            this.radarToolStripMenuItem.Name = "radarToolStripMenuItem";
            this.radarToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.radarToolStripMenuItem.Text = "Radar";
            this.radarToolStripMenuItem.Click += new System.EventHandler(this.RadarToolStripMenuItem_Click);
            // 
            // rangeBarToolStripMenuItem
            // 
            this.rangeBarToolStripMenuItem.Name = "rangeBarToolStripMenuItem";
            this.rangeBarToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.rangeBarToolStripMenuItem.Text = "RangeBar";
            this.rangeBarToolStripMenuItem.Click += new System.EventHandler(this.RangeBarToolStripMenuItem_Click);
            // 
            // stepLineToolStripMenuItem
            // 
            this.stepLineToolStripMenuItem.Name = "stepLineToolStripMenuItem";
            this.stepLineToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.stepLineToolStripMenuItem.Text = "StepLine";
            this.stepLineToolStripMenuItem.Click += new System.EventHandler(this.StepLineToolStripMenuItem_Click);
            // 
            // InfoNL
            // 
            this.InfoNL.AutoSize = true;
            this.InfoNL.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.InfoNL.Location = new System.Drawing.Point(125, 34);
            this.InfoNL.Name = "InfoNL";
            this.InfoNL.Size = new System.Drawing.Size(503, 16);
            this.InfoNL.TabIndex = 1;
            this.InfoNL.Text = "Obliczanie i wizualizacja przyszłego stanu konta wg. zadanych parametrów";
            // 
            // InfoKwota
            // 
            this.InfoKwota.AutoSize = true;
            this.InfoKwota.Location = new System.Drawing.Point(12, 48);
            this.InfoKwota.Name = "InfoKwota";
            this.InfoKwota.Size = new System.Drawing.Size(68, 13);
            this.InfoKwota.TabIndex = 2;
            this.InfoKwota.Text = "Kwota lokaty";
            // 
            // KwotaLokaty
            // 
            this.KwotaLokaty.Location = new System.Drawing.Point(12, 64);
            this.KwotaLokaty.Name = "KwotaLokaty";
            this.KwotaLokaty.Size = new System.Drawing.Size(106, 20);
            this.KwotaLokaty.TabIndex = 3;
            this.KwotaLokaty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KwotaLokaty_KeyPress);
            // 
            // InfoOprocentowanie
            // 
            this.InfoOprocentowanie.AutoSize = true;
            this.InfoOprocentowanie.Location = new System.Drawing.Point(12, 87);
            this.InfoOprocentowanie.Name = "InfoOprocentowanie";
            this.InfoOprocentowanie.Size = new System.Drawing.Size(94, 13);
            this.InfoOprocentowanie.TabIndex = 4;
            this.InfoOprocentowanie.Text = "Stopa procentowa";
            // 
            // InfoOkres
            // 
            this.InfoOkres.AutoSize = true;
            this.InfoOkres.Location = new System.Drawing.Point(12, 127);
            this.InfoOkres.Name = "InfoOkres";
            this.InfoOkres.Size = new System.Drawing.Size(66, 13);
            this.InfoOkres.TabIndex = 6;
            this.InfoOkres.Text = "Okres lokaty";
            // 
            // CzasNaliczaniaLokaty
            // 
            this.CzasNaliczaniaLokaty.Location = new System.Drawing.Point(12, 143);
            this.CzasNaliczaniaLokaty.Name = "CzasNaliczaniaLokaty";
            this.CzasNaliczaniaLokaty.Size = new System.Drawing.Size(106, 20);
            this.CzasNaliczaniaLokaty.TabIndex = 7;
            this.CzasNaliczaniaLokaty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OkresLokaty_KeyPress);
            // 
            // _WybierzKolorLiniiWykresu
            // 
            this._WybierzKolorLiniiWykresu.Location = new System.Drawing.Point(12, 169);
            this._WybierzKolorLiniiWykresu.Name = "_WybierzKolorLiniiWykresu";
            this._WybierzKolorLiniiWykresu.Size = new System.Drawing.Size(106, 23);
            this._WybierzKolorLiniiWykresu.TabIndex = 8;
            this._WybierzKolorLiniiWykresu.Text = "Kolor linii wykresu";
            this._WybierzKolorLiniiWykresu.UseVisualStyleBackColor = true;
            this._WybierzKolorLiniiWykresu.Click += new System.EventHandler(this._WybierzKolorLiniiWykresu_Click);
            // 
            // InfoKolorLinii
            // 
            this.InfoKolorLinii.AutoSize = true;
            this.InfoKolorLinii.Location = new System.Drawing.Point(12, 195);
            this.InfoKolorLinii.Name = "InfoKolorLinii";
            this.InfoKolorLinii.Size = new System.Drawing.Size(90, 13);
            this.InfoKolorLinii.TabIndex = 9;
            this.InfoKolorLinii.Text = "Kolor linii wykresu";
            // 
            // KolorLiniiWykresu
            // 
            this.KolorLiniiWykresu.BackColor = System.Drawing.Color.Blue;
            this.KolorLiniiWykresu.Enabled = false;
            this.KolorLiniiWykresu.Location = new System.Drawing.Point(12, 211);
            this.KolorLiniiWykresu.Name = "KolorLiniiWykresu";
            this.KolorLiniiWykresu.Size = new System.Drawing.Size(106, 20);
            this.KolorLiniiWykresu.TabIndex = 10;
            // 
            // WybierzKolorTłaWykresu
            // 
            this.WybierzKolorTłaWykresu.Location = new System.Drawing.Point(12, 237);
            this.WybierzKolorTłaWykresu.Name = "WybierzKolorTłaWykresu";
            this.WybierzKolorTłaWykresu.Size = new System.Drawing.Size(106, 23);
            this.WybierzKolorTłaWykresu.TabIndex = 11;
            this.WybierzKolorTłaWykresu.Text = "Kolor tła wykresu";
            this.WybierzKolorTłaWykresu.UseVisualStyleBackColor = true;
            this.WybierzKolorTłaWykresu.Click += new System.EventHandler(this.WybierzKolorTłaWykresu_Click);
            // 
            // InfoKolorWykresu
            // 
            this.InfoKolorWykresu.AutoSize = true;
            this.InfoKolorWykresu.Location = new System.Drawing.Point(12, 263);
            this.InfoKolorWykresu.Name = "InfoKolorWykresu";
            this.InfoKolorWykresu.Size = new System.Drawing.Size(89, 13);
            this.InfoKolorWykresu.TabIndex = 12;
            this.InfoKolorWykresu.Text = "Kolor tła wykresu";
            // 
            // KolorTłaWykresu
            // 
            this.KolorTłaWykresu.BackColor = System.Drawing.Color.Aqua;
            this.KolorTłaWykresu.Enabled = false;
            this.KolorTłaWykresu.Location = new System.Drawing.Point(12, 279);
            this.KolorTłaWykresu.Name = "KolorTłaWykresu";
            this.KolorTłaWykresu.Size = new System.Drawing.Size(106, 20);
            this.KolorTłaWykresu.TabIndex = 13;
            // 
            // InfoGrubośćLinii
            // 
            this.InfoGrubośćLinii.AutoSize = true;
            this.InfoGrubośćLinii.Location = new System.Drawing.Point(19, 302);
            this.InfoGrubośćLinii.Name = "InfoGrubośćLinii";
            this.InfoGrubośćLinii.Size = new System.Drawing.Size(68, 13);
            this.InfoGrubośćLinii.TabIndex = 14;
            this.InfoGrubośćLinii.Text = "Grubość Linii";
            // 
            // InfoStylLinii
            // 
            this.InfoStylLinii.AutoSize = true;
            this.InfoStylLinii.Location = new System.Drawing.Point(12, 373);
            this.InfoStylLinii.Name = "InfoStylLinii";
            this.InfoStylLinii.Size = new System.Drawing.Size(42, 13);
            this.InfoStylLinii.TabIndex = 16;
            this.InfoStylLinii.Text = "StylLinii";
            // 
            // StylLiniiWykresu
            // 
            this.StylLiniiWykresu.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.StylLiniiWykresu.FormattingEnabled = true;
            this.StylLiniiWykresu.Items.AddRange(new object[] {
            "- - - - - - - - - - - -",
            "- ˑ - ˑ - ˑ - ˑ - ˑ - ˑ -",
            "- ˑˑ - ˑˑ - ˑˑ - ˑˑ - ˑ",
            "ˑˑˑˑˑˑˑˑˑˑˑˑˑˑˑˑˑˑˑˑˑˑˑ",
            "——————————"});
            this.StylLiniiWykresu.Location = new System.Drawing.Point(12, 389);
            this.StylLiniiWykresu.Name = "StylLiniiWykresu";
            this.StylLiniiWykresu.Size = new System.Drawing.Size(106, 24);
            this.StylLiniiWykresu.TabIndex = 17;
            this.StylLiniiWykresu.Text = "————————————————————";
            this.StylLiniiWykresu.SelectedIndexChanged += new System.EventHandler(this.StylLiniiWykresu_SelectedIndexChanged);
            // 
            // InfoRodzajWykresu
            // 
            this.InfoRodzajWykresu.AutoSize = true;
            this.InfoRodzajWykresu.Location = new System.Drawing.Point(12, 413);
            this.InfoRodzajWykresu.Name = "InfoRodzajWykresu";
            this.InfoRodzajWykresu.Size = new System.Drawing.Size(82, 13);
            this.InfoRodzajWykresu.TabIndex = 18;
            this.InfoRodzajWykresu.Text = "Rodzaj wykresu";
            // 
            // RodzajWykresu
            // 
            this.RodzajWykresu.FormattingEnabled = true;
            this.RodzajWykresu.Items.AddRange(new object[] {
            "Line",
            "Bar",
            "Bubble",
            "Column",
            "Radar",
            "RangeBar",
            "StepLine"});
            this.RodzajWykresu.Location = new System.Drawing.Point(12, 430);
            this.RodzajWykresu.Name = "RodzajWykresu";
            this.RodzajWykresu.Size = new System.Drawing.Size(106, 21);
            this.RodzajWykresu.TabIndex = 19;
            this.RodzajWykresu.Text = "Line";
            this.RodzajWykresu.SelectedIndexChanged += new System.EventHandler(this.RodzajWykresu_SelectedIndexChanged);
            // 
            // InfoSK
            // 
            this.InfoSK.Location = new System.Drawing.Point(641, 48);
            this.InfoSK.Name = "InfoSK";
            this.InfoSK.Size = new System.Drawing.Size(116, 29);
            this.InfoSK.TabIndex = 20;
            this.InfoSK.Text = "Stan konta po okresie lokaty";
            this.InfoSK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PSK_wyświetl
            // 
            this.PSK_wyświetl.Enabled = false;
            this.PSK_wyświetl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.PSK_wyświetl.Location = new System.Drawing.Point(642, 80);
            this.PSK_wyświetl.Name = "PSK_wyświetl";
            this.PSK_wyświetl.Size = new System.Drawing.Size(115, 20);
            this.PSK_wyświetl.TabIndex = 21;
            // 
            // ObliczStanKonta
            // 
            this.ObliczStanKonta.Location = new System.Drawing.Point(642, 115);
            this.ObliczStanKonta.Name = "ObliczStanKonta";
            this.ObliczStanKonta.Size = new System.Drawing.Size(115, 37);
            this.ObliczStanKonta.TabIndex = 22;
            this.ObliczStanKonta.Text = "Oblicz stan konta";
            this.ObliczStanKonta.UseVisualStyleBackColor = true;
            this.ObliczStanKonta.Click += new System.EventHandler(this.ObliczStanKonta_Click);
            // 
            // TabelaryczneRozliczenieLokaty
            // 
            this.TabelaryczneRozliczenieLokaty.Location = new System.Drawing.Point(642, 158);
            this.TabelaryczneRozliczenieLokaty.Name = "TabelaryczneRozliczenieLokaty";
            this.TabelaryczneRozliczenieLokaty.Size = new System.Drawing.Size(115, 47);
            this.TabelaryczneRozliczenieLokaty.TabIndex = 23;
            this.TabelaryczneRozliczenieLokaty.Text = "Tabelaryczna prezentacja wartości lokaty";
            this.TabelaryczneRozliczenieLokaty.UseVisualStyleBackColor = true;
            this.TabelaryczneRozliczenieLokaty.Click += new System.EventHandler(this.TabelaryczneRozliczenieLokaty_Click);
            // 
            // GraficznaPrezentacjaLokaty
            // 
            this.GraficznaPrezentacjaLokaty.Location = new System.Drawing.Point(642, 211);
            this.GraficznaPrezentacjaLokaty.Name = "GraficznaPrezentacjaLokaty";
            this.GraficznaPrezentacjaLokaty.Size = new System.Drawing.Size(115, 61);
            this.GraficznaPrezentacjaLokaty.TabIndex = 24;
            this.GraficznaPrezentacjaLokaty.Text = "Graficzna prezentacja przebiegu zmian artości lokaty";
            this.GraficznaPrezentacjaLokaty.UseVisualStyleBackColor = true;
            this.GraficznaPrezentacjaLokaty.Click += new System.EventHandler(this.GraficznaPrezentacjaLokaty_Click);
            // 
            // PrzejdźDoKredyty
            // 
            this.PrzejdźDoKredyty.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.PrzejdźDoKredyty.Location = new System.Drawing.Point(642, 354);
            this.PrzejdźDoKredyty.Name = "PrzejdźDoKredyty";
            this.PrzejdźDoKredyty.Size = new System.Drawing.Size(115, 50);
            this.PrzejdźDoKredyty.TabIndex = 25;
            this.PrzejdźDoKredyty.Text = "Przejdź do formularza KREDYTY";
            this.PrzejdźDoKredyty.UseVisualStyleBackColor = true;
            this.PrzejdźDoKredyty.Click += new System.EventHandler(this.PrzejdźDoKredyty_Click);
            // 
            // Obrazek
            // 
            this.Obrazek.Image = global::Lokaty_Kredyty.Properties.Resources.sztabki_zlota1;
            this.Obrazek.Location = new System.Drawing.Point(152, 74);
            this.Obrazek.Name = "Obrazek";
            this.Obrazek.Size = new System.Drawing.Size(437, 330);
            this.Obrazek.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Obrazek.TabIndex = 26;
            this.Obrazek.TabStop = false;
            // 
            // TabelarycznaPrezentacjaLokaty
            // 
            this.TabelarycznaPrezentacjaLokaty.AllowUserToAddRows = false;
            this.TabelarycznaPrezentacjaLokaty.AllowUserToDeleteRows = false;
            this.TabelarycznaPrezentacjaLokaty.AllowUserToResizeColumns = false;
            this.TabelarycznaPrezentacjaLokaty.AllowUserToResizeRows = false;
            this.TabelarycznaPrezentacjaLokaty.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.TabelarycznaPrezentacjaLokaty.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TabelarycznaPrezentacjaLokaty.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DGW_OkresLokaty,
            this.DGWStanNaPoczątku,
            this.DGWOdsetki,
            this.DGWStanKontaNaKońcu});
            this.TabelarycznaPrezentacjaLokaty.Location = new System.Drawing.Point(152, 64);
            this.TabelarycznaPrezentacjaLokaty.Name = "TabelarycznaPrezentacjaLokaty";
            this.TabelarycznaPrezentacjaLokaty.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TabelarycznaPrezentacjaLokaty.Size = new System.Drawing.Size(453, 365);
            this.TabelarycznaPrezentacjaLokaty.TabIndex = 27;
            this.TabelarycznaPrezentacjaLokaty.Visible = false;
            // 
            // DGW_OkresLokaty
            // 
            this.DGW_OkresLokaty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.DGW_OkresLokaty.HeaderText = "Okres Lokaty";
            this.DGW_OkresLokaty.Name = "DGW_OkresLokaty";
            this.DGW_OkresLokaty.ReadOnly = true;
            this.DGW_OkresLokaty.Width = 87;
            // 
            // DGWStanNaPoczątku
            // 
            this.DGWStanNaPoczątku.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.DGWStanNaPoczątku.HeaderText = "Stan na początku";
            this.DGWStanNaPoczątku.Name = "DGWStanNaPoczątku";
            this.DGWStanNaPoczątku.ReadOnly = true;
            this.DGWStanNaPoczątku.Width = 106;
            // 
            // DGWOdsetki
            // 
            this.DGWOdsetki.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.DGWOdsetki.HeaderText = "Naliczone odsetki";
            this.DGWOdsetki.Name = "DGWOdsetki";
            this.DGWOdsetki.ReadOnly = true;
            this.DGWOdsetki.Width = 106;
            // 
            // DGWStanKontaNaKońcu
            // 
            this.DGWStanKontaNaKońcu.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.DGWStanKontaNaKońcu.HeaderText = "Stan konta na końcu okresu";
            this.DGWStanKontaNaKońcu.Name = "DGWStanKontaNaKońcu";
            this.DGWStanKontaNaKońcu.ReadOnly = true;
            this.DGWStanKontaNaKońcu.Width = 123;
            // 
            // WykresLokata
            // 
            chartArea2.Name = "ChartArea1";
            this.WykresLokata.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.WykresLokata.Legends.Add(legend2);
            this.WykresLokata.Location = new System.Drawing.Point(152, 64);
            this.WykresLokata.Name = "WykresLokata";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.WykresLokata.Series.Add(series2);
            this.WykresLokata.Size = new System.Drawing.Size(453, 386);
            this.WykresLokata.TabIndex = 28;
            this.WykresLokata.Text = "chart1";
            this.WykresLokata.Visible = false;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // GrubośćLiniiWykresu
            // 
            this.GrubośćLiniiWykresu.Location = new System.Drawing.Point(12, 318);
            this.GrubośćLiniiWykresu.Maximum = 5;
            this.GrubośćLiniiWykresu.Minimum = 1;
            this.GrubośćLiniiWykresu.Name = "GrubośćLiniiWykresu";
            this.GrubośćLiniiWykresu.Size = new System.Drawing.Size(104, 45);
            this.GrubośćLiniiWykresu.TabIndex = 29;
            this.GrubośćLiniiWykresu.Value = 1;
            this.GrubośćLiniiWykresu.Scroll += new System.EventHandler(this.GrubośćLiniiWykresu_Scroll);
            // 
            // InfoSN
            // 
            this.InfoSN.AutoSize = true;
            this.InfoSN.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.InfoSN.Location = new System.Drawing.Point(19, 347);
            this.InfoSN.Name = "InfoSN";
            this.InfoSN.Size = new System.Drawing.Size(91, 13);
            this.InfoSN.TabIndex = 30;
            this.InfoSN.Text = "1    2     3    4     5";
            // 
            // Nud_Digit1
            // 
            this.Nud_Digit1.Location = new System.Drawing.Point(12, 103);
            this.Nud_Digit1.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.Nud_Digit1.Name = "Nud_Digit1";
            this.Nud_Digit1.Size = new System.Drawing.Size(28, 20);
            this.Nud_Digit1.TabIndex = 31;
            this.Nud_Digit1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Nud_Digit2
            // 
            this.Nud_Digit2.Location = new System.Drawing.Point(50, 103);
            this.Nud_Digit2.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.Nud_Digit2.Name = "Nud_Digit2";
            this.Nud_Digit2.Size = new System.Drawing.Size(28, 20);
            this.Nud_Digit2.TabIndex = 32;
            this.Nud_Digit2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Nud_Digit3
            // 
            this.Nud_Digit3.Location = new System.Drawing.Point(76, 103);
            this.Nud_Digit3.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.Nud_Digit3.Name = "Nud_Digit3";
            this.Nud_Digit3.Size = new System.Drawing.Size(28, 20);
            this.Nud_Digit3.TabIndex = 33;
            this.Nud_Digit3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Lb_Procent
            // 
            this.Lb_Procent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_Procent.Location = new System.Drawing.Point(102, 103);
            this.Lb_Procent.Name = "Lb_Procent";
            this.Lb_Procent.Size = new System.Drawing.Size(15, 23);
            this.Lb_Procent.TabIndex = 34;
            this.Lb_Procent.Text = "%";
            // 
            // Lb_Separator
            // 
            this.Lb_Separator.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_Separator.Location = new System.Drawing.Point(37, 97);
            this.Lb_Separator.Name = "Lb_Separator";
            this.Lb_Separator.Size = new System.Drawing.Size(11, 24);
            this.Lb_Separator.TabIndex = 35;
            this.Lb_Separator.Text = ",";
            // 
            // Lokaty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 462);
            this.Controls.Add(this.Lb_Separator);
            this.Controls.Add(this.Lb_Procent);
            this.Controls.Add(this.Nud_Digit3);
            this.Controls.Add(this.Nud_Digit2);
            this.Controls.Add(this.Nud_Digit1);
            this.Controls.Add(this.InfoSN);
            this.Controls.Add(this.InfoGrubośćLinii);
            this.Controls.Add(this.GrubośćLiniiWykresu);
            this.Controls.Add(this.WykresLokata);
            this.Controls.Add(this.TabelarycznaPrezentacjaLokaty);
            this.Controls.Add(this.Obrazek);
            this.Controls.Add(this.PrzejdźDoKredyty);
            this.Controls.Add(this.GraficznaPrezentacjaLokaty);
            this.Controls.Add(this.TabelaryczneRozliczenieLokaty);
            this.Controls.Add(this.ObliczStanKonta);
            this.Controls.Add(this.PSK_wyświetl);
            this.Controls.Add(this.InfoSK);
            this.Controls.Add(this.RodzajWykresu);
            this.Controls.Add(this.InfoRodzajWykresu);
            this.Controls.Add(this.StylLiniiWykresu);
            this.Controls.Add(this.InfoStylLinii);
            this.Controls.Add(this.KolorTłaWykresu);
            this.Controls.Add(this.InfoKolorWykresu);
            this.Controls.Add(this.WybierzKolorTłaWykresu);
            this.Controls.Add(this.KolorLiniiWykresu);
            this.Controls.Add(this.InfoKolorLinii);
            this.Controls.Add(this._WybierzKolorLiniiWykresu);
            this.Controls.Add(this.CzasNaliczaniaLokaty);
            this.Controls.Add(this.InfoOkres);
            this.Controls.Add(this.InfoOprocentowanie);
            this.Controls.Add(this.KwotaLokaty);
            this.Controls.Add(this.InfoKwota);
            this.Controls.Add(this.InfoNL);
            this.Controls.Add(this.MenuLokaty);
            this.MainMenuStrip = this.MenuLokaty;
            this.Name = "Lokaty";
            this.Text = "Lokata Kapitałowa";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Lokaty_FormClosing);
            this.Load += new System.EventHandler(this.Lokaty_Load);
            this.MenuLokaty.ResumeLayout(false);
            this.MenuLokaty.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Obrazek)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabelarycznaPrezentacjaLokaty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WykresLokata)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrubośćLiniiWykresu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Nud_Digit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Nud_Digit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Nud_Digit3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuLokaty;
        private System.Windows.Forms.ToolStripMenuItem LokatyFile;
        private System.Windows.Forms.ToolStripMenuItem LokatyZTW;
        private System.Windows.Forms.Label InfoNL;
        private System.Windows.Forms.Label InfoKwota;
        private System.Windows.Forms.TextBox KwotaLokaty;
        private System.Windows.Forms.Label InfoOprocentowanie;
        private System.Windows.Forms.Label InfoOkres;
        private System.Windows.Forms.TextBox CzasNaliczaniaLokaty;
        private System.Windows.Forms.Button _WybierzKolorLiniiWykresu;
        private System.Windows.Forms.Label InfoKolorLinii;
        private System.Windows.Forms.TextBox KolorLiniiWykresu;
        private System.Windows.Forms.Button WybierzKolorTłaWykresu;
        private System.Windows.Forms.Label InfoKolorWykresu;
        private System.Windows.Forms.TextBox KolorTłaWykresu;
        private System.Windows.Forms.Label InfoGrubośćLinii;
        private System.Windows.Forms.Label InfoStylLinii;
        private System.Windows.Forms.ComboBox StylLiniiWykresu;
        private System.Windows.Forms.Label InfoRodzajWykresu;
        private System.Windows.Forms.ComboBox RodzajWykresu;
        private System.Windows.Forms.Label InfoSK;
        private System.Windows.Forms.TextBox PSK_wyświetl;
        private System.Windows.Forms.Button ObliczStanKonta;
        private System.Windows.Forms.Button TabelaryczneRozliczenieLokaty;
        private System.Windows.Forms.Button GraficznaPrezentacjaLokaty;
        private System.Windows.Forms.Button PrzejdźDoKredyty;
        private System.Windows.Forms.PictureBox Obrazek;
        private System.Windows.Forms.DataGridView TabelarycznaPrezentacjaLokaty;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGW_OkresLokaty;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGWStanNaPoczątku;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGWOdsetki;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGWStanKontaNaKońcu;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.DataVisualization.Charting.Chart WykresLokata;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ToolStripMenuItem lineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem barToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bubbleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem columnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem radarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rangeBarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stepLineToolStripMenuItem;
        private System.Windows.Forms.TrackBar GrubośćLiniiWykresu;
        private System.Windows.Forms.Label InfoSN;
        private System.Windows.Forms.NumericUpDown Nud_Digit3;
        private System.Windows.Forms.NumericUpDown Nud_Digit2;
        private System.Windows.Forms.NumericUpDown Nud_Digit1;
        private System.Windows.Forms.Label Lb_Separator;
        private System.Windows.Forms.Label Lb_Procent;
    }
}